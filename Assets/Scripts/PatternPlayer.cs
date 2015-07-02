using UnityEngine;
using System.Collections;

public class PatternPlayer : MonoBehaviour {
    protected Element left;
    protected Element right;
    protected Element up;

    private float m_direction = 0.0f;

    public float distBetweenElements = 0.75f;

    //Order of events with singleton start shouldn't matter cause this doesn't actually start scene in game
    protected void Start()
    {
        left = SpawnRandomElement(Vector3.left);
        right = SpawnRandomElement(Vector3.right);
        up = SpawnRandomElement(Vector3.up);
    }

    public void OnMove(BirdScript.MessageMove data)
    {
        m_direction = data.dir;
        //Debug.Log("OnMove: " + data.dir);
        if (data.dir == -1)
        {
            SpawnElementInDirection(left,Vector3.left);
        }
        if (data.dir == 1)
        {
            SpawnElementInDirection(right,Vector3.right);
        }
    }

    public void OnJump(BirdScript.MessageJump data)
    {
        if (data.jump)
        {
            SpawnElementInDirection(up,Vector3.up);
        }
    }

    public float destroyTime = 1.5f;

    /// <summary>
    /// Spawn an element at random from ElementManager
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    Element SpawnRandomElement(Vector3 direction)
    {
        Element e = ((Element)GameObject.Instantiate(ElementManager.Instance.GetRandomElement(), transform.position + direction * distBetweenElements, Quaternion.identity));
        SpriteRenderer renderer = e.GetComponent<SpriteRenderer>();
        renderer.color = new Color(renderer.color.r,renderer.color.g,renderer.color.b,0.4f);
        return e;
    }

    Element SpawnElementInDirection(Element prefab,Vector3 direction)
    {
        //Uh.. show the element and that
        Element e = ((Element)GameObject.Instantiate(prefab, transform.position+direction*distBetweenElements, Quaternion.identity));
        GameObject.Destroy(e.gameObject, destroyTime);
        SpriteRenderer renderer = e.GetComponent<SpriteRenderer>();
        renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, 1f);

        //Actually play the game
        PatternCoordinator.Instance.AttemptElementPress(e);
        return e;
    }

    protected void OnDestroy()
    {
        if (left != null)
        {
            GameObject.Destroy(left.gameObject);
        }
        if (right != null)
        {
            GameObject.Destroy(right.gameObject);
        }
        if (up != null)
        {
            GameObject.Destroy(up.gameObject);
        }
    }

}
