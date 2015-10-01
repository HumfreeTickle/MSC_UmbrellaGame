using UnityEngine;
using System.Collections;
using NPC;

public class HackedLights : MonoBehaviour
{

	private NPC_BoxesMission boxesGuy;
	private bool lighting;

	void Start ()
	{
		boxesGuy = GameObject.Find ("Missions").GetComponent<NPC_BoxesMission> ();
	}
	
	void Update ()
	{
//		if (transform.parent.tag == "Pickup") {
//			lighting = boxesGuy.lightsON;
//		}

		if (transform.parent.tag == "Pickup") {
			GetComponent<Light> ().enabled = lighting;
		} else {
			GetComponent<Light> ().enabled = false;

		}
	}
}
