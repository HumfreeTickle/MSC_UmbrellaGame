using UnityEngine;
using System.Collections;

public class Collision : MonoBehaviour
{
	void OnTriggerEnter (Collider col)
	{
		if (col.gameObject.tag == "Player") {
			if (!IsInvoking ("InvokeX")) {
				Invoke ("InvokeX", 0.1f);
			}
		}
	}

	void InvokeX ()
	{
		if (transform.parent.GetComponent<Tutuorial> ().X <= 2) {
			transform.parent.GetComponent<Tutuorial> ().X += 1;
		}else{
			transform.parent.GetComponent<Tutuorial> ().X = 5;
		}
	}
}
