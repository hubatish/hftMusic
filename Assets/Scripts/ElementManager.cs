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

    public List<Element> elementPrefabs = new List<Element>();

    public Element GetElement(int elementNum)
    {
        return elementPrefabs[elementNum];
    }
    
    public Element GetRandomElement()
    {
        return GetElement(Random.Range(0, elementPrefabs.Count));
    }    
}
