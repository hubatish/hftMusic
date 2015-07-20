using UnityEngine;
using System.Collections;

public class ScoreManager : MonoBehaviour {

    public int score
    {
        get
        {
            return _score;
        }
        set
        {
            _score = value;
            scoreText.text = _score.ToString();
        }
    }

    private int _score;

    [SerializeField]
    private TextMesh scoreText;

	// Use this for initialization
	void Start () {
        PatternCoordinator.Instance.AddPatternLoseAction(delegate()
        {
            score = 0;
        });
        PatternCoordinator.Instance.AddPatternWinAction(delegate ()
        {
            score += 1;
        });
	}


}
