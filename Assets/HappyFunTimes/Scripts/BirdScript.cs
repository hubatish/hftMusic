using HappyFunTimes;
using UnityEngine;
using System.Collections;

public class BirdScript : MonoBehaviour {

    public Transform left;
    public Transform right;
    public Transform up;

    public Transform nameTransform;

    // this is the base color of the avatar.
    // we need to know it because we need to know what color
    // the avatar will become after its hsv has been adjusted.
    public Color baseColor;

    private float m_direction = 0.0f;
    private GUIStyle m_guiStyle = new GUIStyle();
    private GUIContent m_guiName = new GUIContent("");
    private Rect m_nameRect = new Rect(0,0,0,0);
    private string m_playerName;

    // Manages the connection between this object and the phone.
    private NetPlayer m_netPlayer;

    // Message when player presses or release jump button
    private class MessageJump : MessageCmdData
    {
        public bool jump = false;
    }

    // Message when player pressed left or right
    private class MessageMove : MessageCmdData
    {
        public int dir = 0;  // will be -1, 0, or +1
    }

    // Message to send to phone to tell it the color of the avatar
    // Note that it sends an hue, saturation, value **adjustment**
    // meaning that RGB values are first converted to HSV where H, S, and V
    // are each in the 0 to 1 range. Then this adjustment is added to those 3
    // values. Finally they are converted back to RGB.
    // The min/max values are a hue range. Anything outside that range will
    // not be adjusted.
    private class MessageSetColor : MessageCmdData
    {
        public MessageSetColor() { }  // for deserialization
        public MessageSetColor(float _h, float _s, float _v, float _min, float _max)
        {
            h = _h;
            s = _s;
            v = _v;
            rangeMin = _min;
            rangeMax = _max;
        }
        public float h; // hue
        public float s; // saturation
        public float v; // value
        public float rangeMin;
        public float rangeMax;
    }

    void Init() {
    }

    // Use this for initialization
    void Start ()
    {
        Init();
    }

    // Called when player connects with their phone
    void InitializeNetPlayer(SpawnInfo spawnInfo)
    {
        Init();

        m_netPlayer = spawnInfo.netPlayer;
        m_netPlayer.OnDisconnect += Remove;
        m_netPlayer.OnNameChange += ChangeName;

        // Setup events for the different messages.
        m_netPlayer.RegisterCmdHandler<MessageMove>("move", OnMove);
        m_netPlayer.RegisterCmdHandler<MessageJump>("jump", OnJump);

        MoveToRandomSpawnPoint();

        SetName(m_netPlayer.Name);

        // Pick a random amount to adjust the hue and saturation
        float hue = Random.value;
        float sat = (float)Random.Range(0, 3) * -0.25f;
        MessageSetColor color = new MessageSetColor(
            hue,
            sat,
            0.0f,
            hue * 0.5f,            //m_material.GetFloat("_HSVRangeMin"),
            1f);            ///m_material.GetFloat("_HSVRangeMax"));
        SetColor(color);

        // Send it to the phone
        m_netPlayer.SendCmd("setColor", color);
    }

    void Update()
    {
    }

    void MoveToRandomSpawnPoint()
    {
        // Pick a random spawn point
        int ndx = Random.Range(0, LevelSettings.settings.spawnPoints.Length - 1);
        transform.localPosition = LevelSettings.settings.spawnPoints[ndx].localPosition;
    }

    void SetName(string name)
    {
        m_playerName = name;
        gameObject.name = "Player-" + m_playerName;
        m_guiName = new GUIContent(m_playerName);
        Vector2 size = m_guiStyle.CalcSize(m_guiName);
        m_nameRect.width  = size.x + 12;
        m_nameRect.height = size.y + 5;
    }

    void SetColor(MessageSetColor color) {
        Color[] pix = new Color[1];
        Vector4 hsva = ColorUtils.ColorToHSVA(baseColor);
        hsva.x += color.h;
        hsva.y += color.s;
        hsva.w += color.v;
        pix[0] = ColorUtils.HSVAToColor(hsva);
        Texture2D tex = new Texture2D(1, 1);
        tex.SetPixels(pix);
        tex.Apply();
        m_guiStyle.normal.background = tex;
        //m_material.SetVector("_HSVAAdjust", new Vector4(color.h, color.s, color.v, 0.0f));
        //m_material.SetFloat("_HSVRangeMin", color.rangeMin);
        //m_material.SetFloat("_HSVRangeMax", color.rangeMax);
    }

    void Remove(object sender, System.EventArgs e)
    {
        Destroy(gameObject);
    }

    // Update is called once per frame
    void FixedUpdate () {
    }

    void OnGUI()
    {
        Vector2 size = m_guiStyle.CalcSize(m_guiName);
        Vector3 coords = Camera.main.WorldToScreenPoint(nameTransform.position);
        m_nameRect.x = coords.x - size.x * 0.5f - 5f;
        m_nameRect.y = Screen.height - coords.y;
        m_guiStyle.normal.textColor = Color.black;
        m_guiStyle.contentOffset = new Vector2(4, 2);
        GUI.Box(m_nameRect, m_playerName, m_guiStyle);
    }

    void ChangeName(object sender, System.EventArgs e)
    {
        SetName(m_netPlayer.Name);
    }

    void OnMove(MessageMove data)
    {
        m_direction = data.dir;
        //Debug.Log("OnMove: " + data.dir);
        if (data.dir == -1)
        {
            SpawnPrefab(left);
        }
        if (data.dir == 1)
        {
            SpawnPrefab(right);
        }
    }

    void OnJump(MessageJump data)
    {
        //m_jumpJustPressed = data.jump && !m_jumpPressed;
        //m_jumpPressed = data.jump;
        //Debug.Log("OnJump: " + data.jump);
        if (data.jump)
        {
            SpawnPrefab(up);
        }
    }

    public float destroyTime = 1.5f;

    void SpawnPrefab(Transform prefab)
    {
        GameObject g = ((Transform) GameObject.Instantiate(prefab, transform.position, Quaternion.identity)).gameObject;
        GameObject.Destroy(g, destroyTime);
    }
}
