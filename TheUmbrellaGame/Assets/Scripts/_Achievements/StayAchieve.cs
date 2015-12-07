using UnityEngine;
using System.Collections;

public class StayAchieve : MonoBehaviour
{
	private Achievements achieves;
	private float _timer;
	public string achievementName;
	private AudioClip environmentSFX;
	private AudioSource audio2;
	private float playedAudio;

	void Start ()
	{
		achieves = GameObject.Find ("Follow Camera").GetComponent<Achievements> ();

		if (GetComponent<AudioSource> ()) {
			audio2 = GetComponent<AudioSource> ();
			environmentSFX = audio2.clip;
		} else {
			Debug.LogWarning (gameObject.name + " lacks audio");
		}
	}
	
	void OnTriggerStay (Collider col)
	{
		if (col.gameObject.tag == "Player") {
			if (achieves.achievements.Contains (achievementName)) {
				if (audio2) {
					if (Time.time > playedAudio + environmentSFX.length) {
						audio2.PlayOneShot (environmentSFX);
						playedAudio = Time.time;
					}
				}
				_timer += Time.deltaTime;

				if (_timer > 2) {
					if (!achieves.coroutineInMotion) {
						if (achieves.achievements.Contains (achievementName)) {

							//Starts the message coroutine in Achievements script
							StartCoroutine (achieves.Notification (achieves.achievements [achieves.achievements.IndexOf (achievementName)]));
						}
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
