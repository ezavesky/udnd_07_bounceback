using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour {
	protected Text textScore = null;    // cached text for score manager

	// Use this for initialization
	void Start () {
	    textScore = GetComponentInChildren<Text>();
        GameManager.ScoreboardRegister(gameObject.transform.parent.gameObject.name + " " + gameObject.name, ScoreboardCallback);
	}
	
    public void ScoreboardCallback(int scoreNew, int timeRemainNew, string strCombined) 
    {
        textScore.text = strCombined;       //could do something else, but why?
    }
}
