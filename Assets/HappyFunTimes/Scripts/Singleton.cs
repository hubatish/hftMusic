using UnityEngine;

/// <summary>
/// So many singletons! 
/// This file is taken from the internet Unity wiki example singleton file
///     and a project from Kenneth
/// </summary>
public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{

    public static T Instance
    {
        get
        {
            if (_instance == null && instantiateIfNull)
            {
                GameObject g = new GameObject(typeof(T).ToString() + " Singleton");
                _instance = g.AddComponent<T>();
                Debug.Log("Instantiating an instance of " + typeof(T).ToString() + " since no instance exists in the scene.");
            }
            return _instance;
        }
        private set
        {
            _instance = value;
        }
    }
    private static T _instance;

    // Should we create a new GameObject if there is no instance
    protected static bool instantiateIfNull = false;

    [Header("What should be done with duplicate singletons?")]
    [Tooltip("TRUE: the original instance of the singleton will be kept \nFALSE: the new instance will be kept")]
    public bool keepOriginalInstance = true;

    [Header("How should the extra singletons be destroyed?")]
    [Tooltip("TRUE: the entire GameObject will be destroyed \nFALSE: just this component will be destroyed")]
    public bool destroyGameObject = true;

    [Header("Should the GameObject persist between scenes?")]
    public bool persistant;

    protected virtual void Awake()
    {
        // Handle singleton creation/destruction to preserve one instance
        if (Instance == null)
        {
            Instance = this as T;
        }
        else
        {
            if (keepOriginalInstance)
            {
                Debug.Log("Duplicate of " + typeof(T).ToString() + " detected. Deleting extra instance.");
                if (destroyGameObject)
                {
                    Destroy(this.gameObject);
                }
                else
                {
                    Destroy(this);
                }
            }
            else
            {
                Debug.Log("Duplicate of " + typeof(T).ToString() + " detected. Deleting original instance.");
                if (destroyGameObject)
                {
                    Destroy(Instance.gameObject);
                }
                else
                {
                    Destroy(_instance);
                }
                Instance = this as T;
            }
        }
        // Handle persisting between scenes
        if (persistant)
        {
            DontDestroyOnLoad(this.gameObject);
        }
    }
}