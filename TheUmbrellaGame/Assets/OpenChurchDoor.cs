using UnityEngine;
using System.Collections;

public class OpenChurchDoor : MonoBehaviour {

	public Animator Opener;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other){

		if(other.gameObject.tag == "NPC"){

			Opener.SetBool("Open", true);
		}


	}
}

