using UnityEngine;
using System.Collections;

public class BoxesSave : MonoBehaviour
{

	private Rigidbody rb;
	private Vector3 startingPos;
	private Quaternion startingRotation;
	
	void Start ()
	{
		if (GetComponent<Rigidbody> ()) {
			rb = GetComponent<Rigidbody> ();
		}
		startingPos = transform.position;
		startingRotation = transform.rotation;
	}
	
	void Update ()
	{		
		if (GetComponent<Rigidbody> ()) {
			rb = GetComponent<Rigidbody> ();
		}
		
		LayerMask cat = 15;
		RaycastHit hit;
		if (Physics.Raycast (transform.position, Vector3.down, out hit, Mathf.Infinity, cat.value)) {
			//------------- DEBUGGING -----------------------------
			Debug.DrawRay (transform.position, Vector3.down, Color.yellow, 10, false);
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
	
	void Reset ()
	{
		if (GetComponent<Rigidbody> ()) {
			rb.velocity = Vector3.zero;
			transform.position = startingPos;
			transform.rotation = startingRotation;
		}
	}
}
