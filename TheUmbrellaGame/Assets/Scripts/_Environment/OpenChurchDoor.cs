using UnityEngine;
using System.Collections;

public class OpenChurchDoor : MonoBehaviour {
	public Animator Opener;

	void OnTriggerEnter(Collider other){

		if(other.gameObject.tag == "NPC"){
			Opener.SetBool("Open", true);
		}
	}
}

