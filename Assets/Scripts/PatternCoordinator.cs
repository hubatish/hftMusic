using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// Manage all the elements
/// </summary>
public class PatternCoordinator : MonoBehaviour
{
    public static PatternCoordinator Instance;

    public ElementManager elementManager;
    protected void Start()
    {
        elementManager = gameObject.GetComponent<ElementManager>();
        GeneratePattern(5);
        Instance = this;
    }

    public List<Element> spawnedElements = new List<Element>();
    public float distBetweenElements = 0.75f;

    public void GeneratePattern(int length)
    {
        for (int i = 0; i < length; i++)
        {
            Element newElement = (Element) GameObject.Instantiate(elementManager.GetRandomElement(), 
                                transform.position + distBetweenElements * i * Vector3.down,
                                                                        Quaternion.identity);
            spawnedElements.Add(newElement);
        }
    }

    public void ClearPattern()
    {
        for(int i = spawnedElements.Count - 1; i >= 0; i--)
        {
            GameObject.Destroy(spawnedElements[i].gameObject);
        }
    }


}
