using UnityEngine;
using System.Collections;

public class StayAchieve : MonoBehaviour
{

	private Achievements achieves;
	private float _timer;
	public string achievementName;

	void Start ()
	{
		achieves = GameObject.Find ("Follow Camera").GetComponent<Achievements> ();
		
	}
	
	void OnTriggerStay (Collider col)
	{
		if (col.gameObject.tag == "Player") {
			_timer += Time.deltaTime;
			if (_timer > 2) {
				if (!achieves.CoroutineInMotion) {
					if (achieves.achievements.Contains (achievementName)) {
						StartCoroutine (achieves.Notification (achieves.achievements[achieves.achievements.IndexOf(achievementName)]));
					}
				}	
			}
		}
	}
	
	void OnTriggerExit (Collider col)
	{
		if (col.gameObject.tag == "Player") {
			_timer = 0;
		}
	}
}
