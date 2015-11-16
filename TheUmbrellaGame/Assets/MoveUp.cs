using UnityEngine;
using System.Collections;

public class MoveUp : MonoBehaviour {

	public GameObject theDestination;
	public float speed;
	private Rigidbody rb;


	// Use this for initialization
	void Start () {
	
		rb = GetComponent<Rigidbody>();
		rb.AddForce(transform.up * speed);
	}


	void OnTriggerEnter(Collider other){

		if (other.gameObject.tag == "StartTrigger"){
			Debug.Log ("Should Start");

		}
	}
}
