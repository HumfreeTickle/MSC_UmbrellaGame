using UnityEngine;
using System.Collections;

/// <summary>
/// I think this is useless
/// Moved to StayAchieve
/// </summary>
public class CornField : MonoBehaviour
{

	private Achievements achieves;
	private float _timer;

	void Start ()
	{
		achieves = GameObject.Find ("Follow Camera").GetComponent<Achievements> ();

	}
	
	void OnTriggerStay ()
	{
		_timer += Time.deltaTime;
		if (_timer > 2) {
			if (!achieves.coroutineInMotion) {
				if (achieves.achievements.Contains ("Oh so corny")) {
					StartCoroutine (achieves.Notification (achieves.achievements [4]));
				}
			}	
		}
	}

	void OnTriggerExit(){
		_timer = 0;
	}
}
