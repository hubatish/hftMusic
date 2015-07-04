using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class BeatManager : MonoBehaviour {
    public int currentBeat = 0;
    public int nearestBeat
    {
        get
        {
            //Round up/down for current beat
            if (timeThisBeat<timePerBeat / 2f)
            {
                return _currentBeat;
            }
            else
            {
                return _currentBeat + 1;
            }
        }
        set
        {
            _currentBeat = value;
        }
    }
    protected int _currentBeat = 0;


    protected float timeThisBeat = 0f;

    public float timePerBeat = 1f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        timeThisBeat += Time.deltaTime;
        if (timeThisBeat>timePerBeat)
        {
            timeThisBeat = 0f;
            timePerBeat *= 0.96f;
            currentBeat += 1;
        }
	}

    public class OnBeatAction
    {
        Action action;
        int beat;
        public OnBeatAction(Action action, int beat)
        {
            this.action = action;
            this.beat = beat;
        }
    }

    public List<OnBeatAction> onBeatActions = new List<OnBeatAction>();

    /// <summary>
    /// Call an action _ beats later
    /// </summary>
    /// <param name="action"></param>
    /// <param name="beats"></param>
    public void AddListenerLater(Action action, int beats)
    {
        onBeatActions.Add(new OnBeatAction(action, currentBeat + beats));
    }
}
