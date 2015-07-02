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

    protected ElementManager elementManager;
    protected void Awake()
    {
        elementManager = gameObject.GetComponent<ElementManager>();
        GeneratePattern(defaultNumElements);
        Instance = this;
    }

    public List<Element> spawnedElements = new List<Element>();
    public float distBetweenElements = 0.75f;
    public int defaultNumElements = 5;

    public void GeneratePattern(int length)
    {
        for (int i = 0; i < length; i++)
        {
            Element newElement = (Element) GameObject.Instantiate(elementManager.GetRandomElement(), 
                                                                            GetVector3FromPosition(i),
                                                                                Quaternion.identity);
            spawnedElements.Add(newElement);
        }
        currentPosition = 0;
    }

    public void ClearPattern()
    {
        for(int i = spawnedElements.Count - 1; i >= 0; i--)
        {
            GameObject.Destroy(spawnedElements[i].gameObject);
        }
    }

    protected Vector3 GetVector3FromPosition(int elementNum)
    {
        return transform.position + distBetweenElements * elementNum * Vector3.down;
    }

    //manage current position
    public int currentPosition
    {
        get
        {
            return _currentPosition;
        }
        set
        {
            _currentPosition = value;
            indicator.position = GetVector3FromPosition(value) + Vector3.left*distBetweenElements;
        }
    }
    private int _currentPosition;

    public Transform indicator;

    public void AdvancePosition()
    {
        currentPosition += 1;
        if (currentPosition > spawnedElements.Count-1)
        {
            //won the pattern!
            ClearPattern();
            GeneratePattern(defaultNumElements);
        }
    }

    /// <summary>
    /// An element has been created
    /// Check if it matches the next element in pattern
    /// If so, advance position
    /// </summary>
    /// <param name="e"></param>
    /// <returns></returns>
    public bool AttemptElementPress(Element e)
    {
        if(e.isSame(spawnedElements[currentPosition]))
        {
            AdvancePosition();
            return true; 
        }
        return false;
    }
}
