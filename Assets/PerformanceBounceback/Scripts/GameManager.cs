﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour {
    public int score = 0;            // official score keeper
    public float timeRemain = 0.0f;        // countdown timer
    public int timeGameLength = 30;  // variable for how long each game is
    public float invokeInterval = 0.5f;
    
    public AudioSource sourceMusic = null;  //sound source for music
    protected const float PITCH_ACCELERATE_MAX = 0.25f;

    public AudioSource sourceOneShot = null;  //sound source for oneshot
    public AudioClip clipStart = null;
    public AudioClip clipStop = null;
    
    void Start() {
        score = 0;
        timeRemain = 0;

        GameRestart();      //for now, remove during actual gameplay?
    }

    public void GameRestart() {
        score = 0;
        timeRemain = timeGameLength; 
        if (sourceMusic) {      //start game sound
            sourceMusic.loop = true;
            sourceMusic.Play();
            sourceMusic.pitch = 1.0f;
        }
        if (sourceOneShot && clipStart) {
            sourceOneShot.PlayOneShot(clipStart);
        }
        InvokeRepeating("ScoreboardRepeating", 0.0f, invokeInterval);     //call scoreboard every 0.5s
    }

    public bool GameRunning() {
        return (timeRemain >= 0);
    }

    // function to increment score, update callback
    public void ScoreIncrement(int numHits=1) {
        lock (thisLock) {
            score += numHits;
            GameManager.ScoreboardUpdate(this);
        }
    }

    private void ScoreboardRepeating() {
        timeRemain -= invokeInterval;
        if (timeRemain >= 0) {
            GameManager.ScoreboardUpdate(this);
            if (sourceMusic) {
                sourceMusic.pitch = 1.0f + PITCH_ACCELERATE_MAX*((timeGameLength - timeRemain)/timeGameLength);
            }
        }
        else {
            //TODO: sound buzzer
            CancelInvoke("ScoreboardRepeating");
            if (sourceMusic) {
                sourceMusic.Stop();
                sourceMusic.pitch = 1.0f;
            }
            if (sourceOneShot && clipStop) {
                sourceOneShot.PlayOneShot(clipStop);
            }
        }
    }

    // ----------------------------------------------------------------
    // methods for callback to any watching scoreboards
    protected static System.Object thisLock = new System.Object (); //lock for matrix update
    protected static Dictionary<string, ArrayList > callbackSet = new Dictionary<string, ArrayList >();
    public delegate void ScoreboardCallback(int scoreNew, int timeRemainNew, string strCombined);

    // actually propagate score to scoreboards
    protected static void ScoreboardUpdate(GameManager gm) {
        GameManager.ScoreboardUpdate(gm.score, (int)Mathf.Ceil(gm.timeRemain), 
                                    string.Format("Score: {0}\nRemaining: {1:#0.0}s", gm.score, gm.timeRemain));
    }

    // actually propagate score to scoreboards
    protected static void ScoreboardUpdate(int scoreNew, int timeRemainNew, string strCombined) {
        lock (thisLock) {
            List<string> listKey;
            listKey = new List<string>(callbackSet.Keys);
            foreach(string funcName in listKey) {
                ScoreboardCallback funcA = (ScoreboardCallback)callbackSet[funcName][0];             
                funcA(scoreNew, timeRemainNew, strCombined);
            }
        }
    }

    // register new callback from scoreboards
    public static void ScoreboardRegister(string strFunction, ScoreboardCallback callbackPost, bool bAddCallback=true) {
        lock (thisLock) {
            ArrayList funcTuple = new ArrayList() {callbackPost};
            if (!callbackSet.ContainsKey(strFunction) && bAddCallback) {
                callbackSet.Add(strFunction, funcTuple);
            }
            else if (!bAddCallback) {        //called again to REMOVE?
                callbackSet.Remove(strFunction);
            }
        }
    }



}