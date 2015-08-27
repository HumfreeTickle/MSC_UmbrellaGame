using UnityEngine;
using System.Collections;

namespace Player
{
	public class grabbing : MonoBehaviour
	{
		public GameObject pickup;
		public Transform originalParent;

		void Update ()
		{
			if (pickup != null) {
				if (Input.GetButtonDown ("Interact")) {
					if (Input.GetButtonDown ("Interact")) {
						if (!IsInvoking ("Pickup")) {
							Invoke ("Pickup", 0);
						}
					}
				}
			}
			if (transform.childCount > 0) {
				if (Input.GetButtonDown ("Interact")) {
					if (!IsInvoking ("Detachment")) {
						Invoke ("Detachment", 0);
					}
				}
			}
		}

		void Pickup ()
		{
			pickup.transform.parent = transform;
			pickup.transform.localPosition = Vector3.zero;
			if(pickup.GetComponent<Rigidbody>()){
				Destroy (pickup.GetComponent<Rigidbody>());
			}
		}

		void Detachment ()
		{
			transform.DetachChildren ();
			pickup.transform.parent = originalParent;
			pickup.AddComponent<Rigidbody>();
		}

		void OnTriggerEnter (Collider col)
		{
			if (col.gameObject.tag == "Interaction") {
				pickup = col.gameObject;
				originalParent = pickup.transform.parent;
			}
		}

		void OnTriggerExit (Collider col)
		{
			if (col.gameObject.tag == "Interaction") {
				pickup = null;
				originalParent = null;
			}
		}
	}
}