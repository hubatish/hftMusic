using UnityEngine;
using System.Collections;

/// <summary>
/// Check if a player has pressed a beat since last input
///     ..uh, why am I using so many listeners?
/// </summary>
public class CloseBeatsChecker : Singleton<CloseBeatsChecker> {

    //Did a player press a correct element during this or the previous beat?
    protected bool beatOn = false;

    //Don't start counting/timing beats until a player presses a button
    protected bool noBeatsYet = true;

    protected BeatManager beatManager
    {
        get
        {
            if (_beatManager == null)
            {
                _beatManager = gameObject.GetComponent<BeatManager>();
            }
            return _beatManager;
        }
    }
    private BeatManager _beatManager;

	// Use this for initialization
	void Start () {
        beatManager.AddListenerLater(CheckPressed, 1);
        PatternCoordinator.Instance.AddPatternEndAction(delegate () 
        {
            noBeatsYet = true;
            beatOn = false;
        });
	}

    protected void CheckPressed()
    {
        if(!beatOn && !noBeatsYet)
        {
            //No player pressed correct element
            PatternCoordinator.Instance.FailPattern();
        }
        else
        {
            //someone pressed a beat, all's good! for now...
            beatOn = false;
        }
    }

    //A player pressed a button!
    public void PlayerPressed()
    {
        beatOn = true;
        noBeatsYet = false;
    }
}
