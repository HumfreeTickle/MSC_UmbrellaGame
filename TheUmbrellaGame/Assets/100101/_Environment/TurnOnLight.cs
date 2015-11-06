using UnityEngine;
using System.Collections;

namespace Environment
{
	[RequireComponent (typeof(Light))]
	public class TurnOnLight : MonoBehaviour
	{
		private GmaeManage gameManager;

		void Awake ()
		{
			if (GetComponent<Light> ().isActiveAndEnabled) {
				GetComponent<Light> ().enabled = false;
			}
			gameManager = GameObject.Find ("Follow Camera").GetComponent<GmaeManage> ();
		}

		void Update ()
		{

			if (gameManager.MissionState == MissionController.FinalMission) {
				GetComponent<Light> ().enabled = true;

			} else {
				GetComponent<Light> ().enabled = false;
			}
		}
	}
}