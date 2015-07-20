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
            newElement.SetTransparent();
            spawnedElements.Add(newElement);
        }
        SetCurrentPosition(0);
    }

    public void ClearPattern()
    {
        for(int i = spawnedElements.Count - 1; i >= 0; i--)
        {
            GameObject.Destroy(spawnedElements[i].gameObject);
            spawnedElements.RemoveAt(i);
        }
    }

    public void ResetPattern()
    {
        ClearPattern();
        GeneratePattern(defaultNumElements);
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
        }
    }
    private int _currentPosition;
    private int prevPosition = 0;

    private void SetCurrentPosition(int value)
    {
        currentPosition = value;
        indicator.position = GetVector3FromPosition(value) + Vector3.left * distBetweenElements;
        spawnedElements[_currentPosition].SetVisible();
        if (prevPosition > spawnedElements.Count - 1)
        {
            prevPosition = 0;
        }
        spawnedElements[prevPosition].SetTransparent();
        prevPosition = _currentPosition;
    }

    public Transform indicator;

    public void AdvancePosition()
    {
        int position = currentPosition+1;
        if (position > spawnedElements.Count-1)
        {
            //won the pattern!
            Debug.Log("won the pattern!");
            ResetPattern();
            CallWinActions();
        }
        else
        {
            SetCurrentPosition(position);
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
        else
        {
            //failure, restart progress in pattern
            FailPattern();
        }
        return false;
    }

    public void FailPattern()
    {
        SetCurrentPosition(0);
        CallLoseActions();
    }

    protected List<Action> winActions = new List<Action>();
    protected List<Action> loseActions = new List<Action>();

    protected void CallWinActions()
    {
        foreach (var action in winActions)
        {
            action();
        }
    }

    protected void CallLoseActions()
    {
        foreach(var action in loseActions)
        {
            action();
        }
    }

    public void AddPatternEndAction(Action action)
    {
        winActions.Add(action);
        loseActions.Add(action);
    }

    public void AddPatternLoseAction(Action action)
    {
        loseActions.Add(action);
    }

    public void AddPatternWinAction(Action action)
    {
        winActions.Add(action);
    }
}
