using UnityEngine;
using System.Collections;

public class LightHouseOPen : MonoBehaviour {

	public Animator Door;

	// Use this for initialization
	void Start () {
	

	}
	
	// Update is called once per frame
	void Update () {
	
		}
	void OnTriggerEnter(Collider other){

		if(other.gameObject.tag == "NPC" ){

			Door.SetBool ("DoorOpen", true);
		}

	}
	void OnTriggerExit(Collider other){

		if(other.gameObject.tag == "NPC" ){
			
			Door.SetBool ("DoorOpen", false);
    }
	}
}
