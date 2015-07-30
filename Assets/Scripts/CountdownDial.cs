using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// Countdown, with effect around circle
///     Rotates a needle around xy plane
/// </summary>
public class CountdownDial : MonoBehaviour {

    protected float curTime = 0.0f;
    [SerializeField]
    private float endTime = 0.0f;

    public bool ticking
    {
        get
        {
            return _ticking;
        }
        protected set
        {
            _ticking = value;
            whileTicking.SetActive(_ticking);
            notTicking.SetActive(!_ticking);
        }
    }
    [SerializeField]
    private bool _ticking = false;

    [Tooltip("Show this object while ticking")]
    [SerializeField]
    private GameObject whileTicking;
    [SerializeField]
    private GameObject notTicking;

    // Watch out! This script edits rotation, so please make sure the needle has in-editor rotation
    [SerializeField]
    [Tooltip("Marker for current time position")]
    private Transform needle;

    public Action doneAction = delegate() { };

    protected void DoneTicking()
    {
        ticking = false;
    }

	void Awake () {
        doneAction += DoneTicking;
        ticking = _ticking;
	}

    public void StartTicking(float time)
    {
        endTime = time;
        ticking = true;
    }
	
	void Update () {
        if (!ticking)
        {
            return;
        }
        curTime += Time.deltaTime;
        float percentage = curTime / endTime;
        needle.localRotation = Quaternion.Euler(new Vector3(0, 0, -percentage * 360f));
        if (curTime >= endTime)
        {
            doneAction();
        }
	}
}
