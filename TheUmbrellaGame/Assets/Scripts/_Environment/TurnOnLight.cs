using UnityEngine;
using System.Collections;

namespace Environment
{
	[RequireComponent (typeof(Light))]
	public class TurnOnLight : MonoBehaviour
	{
		private GmaeManage gameManager;
		private MissionController currentMission;

		void Awake ()
		{
			if (GetComponent<Light> ().isActiveAndEnabled) {
				GetComponent<Light> ().enabled = false;
			}
			gameManager = GameObject.Find ("Follow Camera").GetComponent<GmaeManage> ();
		}

		void Update ()
		{
			currentMission = gameManager.MissionState;

			if (currentMission == MissionController.FinalMission /*|| sun.intensity < 0.5f*/) {
				GetComponent<Light> ().enabled = true;

			} else {
				GetComponent<Light> ().enabled = false;
			}
		}
	}
}