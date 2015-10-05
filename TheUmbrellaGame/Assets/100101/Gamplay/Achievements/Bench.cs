﻿using UnityEngine;
using System.Collections;

public class Bench : MonoBehaviour {

	private Achievements achieves;
	private float _timer;

	public string achievementName;
	
	void Start ()
	{
		achieves = GameObject.Find ("Follow Camera").GetComponent<Achievements> ();
		
	}
	
	void OnTriggerStay ()
	{
		_timer += Time.deltaTime;
		if (_timer > 2) {
			if (!achieves.CoroutineInMotion) {
				if (achieves.achievements.Contains (achievementName)) {
					StartCoroutine (achieves.Notification (achieves.achievements [2]));
				}
			}	
		}
	}
	
	void OnTriggerExit(){
		_timer = 0;
	}
}
