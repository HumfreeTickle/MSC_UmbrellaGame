﻿using UnityEngine;
using System.Collections;

public class CatSave : MonoBehaviour
{
	private Rigidbody rb;
	private Vector3 startingPos;
	private Quaternion startingRotation;

	void Start ()
	{
		rb = GetComponent<Rigidbody> ();
		startingPos = transform.position;
		startingRotation = transform.rotation;
	}
	
	void Update ()
	{
		rb = GetComponent<Rigidbody> ();

		LayerMask cat = 15;
		RaycastHit hit;
		if (Physics.Raycast (transform.position, Vector3.down, out hit, Mathf.Infinity, cat.value)) {
			//------------- DEBUGGING -----------------------------
//			Debug.DrawRay (transform.position, Vector3.down, Color.yellow, 10, false);
			if (hit.collider.tag != "River") {

				if (IsInvoking ("Reset")) {
					CancelInvoke ("Reset");
				}
			}

		} else {
			if (transform.parent.tag != "Player") {
				if (!IsInvoking ("Reset")) {
					Invoke ("Reset", 3);
				}
			}
		}
	}

	void OnTriggerStay (Collider col)
	{
		if (col.gameObject.tag == "River") {
			if (GetComponent<BoxCollider> ()) {
				GetComponent<BoxCollider> ().isTrigger = true;
			}
			if (transform.parent.tag != "Player") {
				if (!IsInvoking ("Reset")) {
					Invoke ("Reset", 3);
				}
			}
		}
	}

	void OnTriggerExit (Collider col)
	{
		if (col.gameObject.tag == "River") {
			if (GetComponent<BoxCollider> ()) {
				GetComponent<BoxCollider> ().isTrigger = false;
			}
			if (transform.parent.tag != "Player") {
				if (IsInvoking ("Reset")) {
					CancelInvoke ("Reset");
				}
			}
		}
	}

	void Reset ()
	{
		if (GetComponent<Rigidbody> ()) {
			rb.velocity = Vector3.zero;
			rb.isKinematic = true;
			transform.position = startingPos;
			transform.rotation = startingRotation;
		}
	}
}
