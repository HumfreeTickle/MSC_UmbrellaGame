using UnityEngine;
using System.Collections;

namespace Player
{
	public class grabbing : MonoBehaviour
	{
		private Rigidbody umbrellaBody;
		private GameObject pickup;
		public GameObject Landing;
		public bool JumpKey;

		void Update ()
		{
			if (transform.childCount > 0) {

				if (Input.GetButtonDown ("Interact")) {
					JumpKey = !JumpKey;
				}

				Detachment ();
			}
		}
		
		void Detachment ()
		{
			if (JumpKey == true) {
				transform.DetachChildren ();
				if (!pickup.GetComponent<Rigidbody> ()) {
					pickup.AddComponent<Rigidbody> ();
				}
				Landing.SetActive (false);
			}
		}

//		void OnTriggerStay (Collider other)
//		{
//			if (other.gameObject.tag == "Interaction") {
//				if (Input.GetButtonDown ("Interact")) {
//					pickup = other.gameObject;
//					other.transform.parent = transform;
////					Landing.SetActive (true);
//					JumpKey = false;
//				}
//			}
//		}
	}
}