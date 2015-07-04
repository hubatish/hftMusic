using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Manage all the elements
/// </summary>
public class ElementManager : MonoBehaviour
{
    public static ElementManager Instance;

    protected void Awake()
    {
        Instance = this;
    }

    protected void Start()
    {
        PlayerListener.Instance.SubscribeToPlayerJoin(OnPlayerSpawn);
    }

    //All possible elements
    [SerializeField]
    protected List<Element> elementPrefabs = new List<Element>();

    //Only use some of the elements - vary it for the number of players
    [SerializeField]
    protected List<Element> availableElements = new List<Element>();

    public Element GetElement(int elementNum)
    {
        return availableElements[elementNum];
    }
    
    public Element GetRandomElement()
    {
        return GetElement(Random.Range(0, availableElements.Count));
    }

    private int nextUnusedElement = 0;
    public Element GetUnusedElement()
    {
        if (nextUnusedElement > availableElements.Count - 1)
        {
            nextUnusedElement = 0;
        }
        Element returnValue = availableElements[nextUnusedElement];
        nextUnusedElement += 1;
        Debug.Log("current used element "+nextUnusedElement + " elements count "+(availableElements.Count));
        return returnValue;
    }

    /// <summary>
    /// Set available elements to pull from all elements
    /// </summary>
    /// <param name="numPlayers"></param>
    public void OnPlayerSpawn(int numPlayers)
    {
        int numElements = numPlayers*2;
        if (numElements == 0)
        {
            numElements += 1;
        }
        Debug.Log("available elements: " + numElements);
        availableElements = elementPrefabs.GetRange(0, numElements);
        PatternCoordinator.Instance.ResetPattern();
    }
}
