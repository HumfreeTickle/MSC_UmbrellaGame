using UnityEngine;
using System.Collections;

public class PriestOutside : MonoBehaviour {

	private Animator anim;

	// Use this for initialization
	void Start () {
		anim= GetComponent<Animator>();
	}
	void OnTriggerEnter(Collider other){

		if(other.gameObject.tag == "ChurchExit"){

			anim.enabled=false;
			gameObject.transform.position = new Vector3(-9.5f, 0.15f, 94.8f);
			Debug.Log ("Should Exit");
		}
	}

}
