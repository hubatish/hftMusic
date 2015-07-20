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
            if (true)//(timeThisBeat<timePerBeat / 2f)
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

    [SerializeField]
    protected float startTimePerBeat = 1.0f;

    private float timePerBeat;

    public AudioClip beatSound;
    public AudioSource audioPlayer;

	// Use this for initialization
	void Start () {
        ResetBeats();
        PatternCoordinator.Instance.AddPatternEndAction(ResetBeats);
        audioPlayer = gameObject.GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        timeThisBeat += Time.deltaTime;
        if (timeThisBeat>timePerBeat)
        {
            timeThisBeat = 0f;
            //timePerBeat *= 0.96f;
            currentBeat += 1;
            AdvanceABeat();
        }
	}

    //Onto the next beat, do things!
    protected void AdvanceABeat()
    {
        for(int i = onBeatActions.Count - 1; i>=0;i--)
        {
            if(currentBeat >= onBeatActions[i].beat)
            {
                onBeatActions[i].action();
                //remove the action
                if(!onBeatActions[i].repeat)
                {
                    onBeatActions.RemoveAt(i);
                }
            }
        }
        audioPlayer.PlayOneShot(beatSound);
    }

    public void ResetBeats()
    {
        timePerBeat = startTimePerBeat;
        timeThisBeat = 0;
    }

    public class OnBeatAction
    {
        public Action action;
        public int beat;
        public bool repeat;
        public OnBeatAction(Action action, int beat, bool repeat = true)
        {
            this.action = action;
            this.beat = beat;
            this.repeat = repeat;
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
