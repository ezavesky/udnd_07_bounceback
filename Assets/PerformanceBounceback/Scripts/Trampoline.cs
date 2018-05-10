using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline : MonoBehaviour {
    protected ParticleSystem pSystem;
    protected GameManager gameManager;

	// Use this for initialization
	void Start () {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        pSystem = GetComponentInChildren<ParticleSystem>();
	}
	
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Throwable"))
        {
            //Score Point
            gameManager.ScoreIncrement();
            //Particle effect
            pSystem.Play();
            Debug.Log("Trampoline Hit");
        }

    }
}
