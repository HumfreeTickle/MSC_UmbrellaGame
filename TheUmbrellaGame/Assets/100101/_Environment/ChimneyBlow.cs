using UnityEngine;
using System.Collections;

public class ChimneyBlow : MonoBehaviour {

	public float push;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame


		void OnTriggerEnter (Collider other)
		{
			
			if (other.gameObject.tag == "Player") {
			other.GetComponent<Rigidbody> ().AddForce(transform.up * push);
	
	}
}
}