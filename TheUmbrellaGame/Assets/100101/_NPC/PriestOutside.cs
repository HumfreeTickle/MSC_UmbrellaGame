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
			gameObject.transform.position = new Vector3(-3f, -.7f, 87.9f);
			Debug.Log ("Should Exit");
		}
	}

}
