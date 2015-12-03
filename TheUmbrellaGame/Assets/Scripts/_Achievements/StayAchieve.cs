using UnityEngine;
using System.Collections;

public class StayAchieve : MonoBehaviour
{

	private Achievements achieves;
	private float _timer;
	public string achievementName;
	private AudioClip Rustle;
	private AudioSource audio2;

	void Start ()
	{
		achieves = GameObject.Find ("Follow Camera").GetComponent<Achievements> ();
		audio2 = GetComponent<AudioSource>();
		Rustle = audio2.clip;
		
	}


	
	void OnTriggerEnter (Collider col)
	{
		if (col.gameObject.tag == "Player") {

			audio2.PlayOneShot (Rustle);
			_timer += Time.deltaTime;
			if (_timer > 2) {
				if (!achieves.CoroutineInMotion) {
					if (achieves.achievements.Contains (achievementName)) {
						//Starts the message coroutine in Achievements script
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
