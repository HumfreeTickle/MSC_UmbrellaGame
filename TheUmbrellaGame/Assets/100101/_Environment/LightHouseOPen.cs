using UnityEngine;
using System.Collections;

public class LightHouseOPen : MonoBehaviour {

	public Animator Door;

	// Use this for initialization

	void OnTriggerEnter(Collider other){

		if(other.gameObject.tag == "NPC" ){

			Debug.Log("Open");
			Door.SetBool ("DoorOpen", true);
		}

	}
	void OnTriggerExit(Collider other){

		if(other.gameObject.tag == "NPC" ){
			
			Door.SetBool ("DoorOpen", false);

    }
	}
}

