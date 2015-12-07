using UnityEngine;
using System.Collections;


//not sure what this is for
public class MoveUp : MonoBehaviour {

	public GameObject theDestination;
	public float speed;
	private Rigidbody rb;

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
