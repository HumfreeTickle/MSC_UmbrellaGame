using UnityEngine;
using System.Collections;

public class StayAchieve : MonoBehaviour {

	private Achievements achieves;
	private float _timer;
	
	public string achievementName;
	public int listItem;
	
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
					StartCoroutine (achieves.Notification (achieves.achievements [listItem]));
				}
			}	
		}
	}
	
	void OnTriggerExit(){
		_timer = 0;
	}
}
