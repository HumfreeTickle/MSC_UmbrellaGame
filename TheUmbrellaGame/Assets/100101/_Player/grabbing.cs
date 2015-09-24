using UnityEngine;
using System.Collections;
using Player.PhysicsStuff;

namespace Player
{
	public class grabbing : MonoBehaviour
	{
		public GameObject pickupObject;
		private Transform originalParent;
		private bool terrainHit;
		public float z;


		void Update ()
		{
			terrainHit = GameObject.Find("main_Sphere").GetComponent<CreateWind>().HitTerrain;

			if (terrainHit) {
				if (pickupObject != null) {
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
		}

		void Pickup ()
		{
			if (pickupObject.GetComponent<BoxCollider> ()) {
				pickupObject.GetComponent<BoxCollider> ().enabled = false;
			} else if (pickupObject.GetComponent<MeshCollider> ()) {
				pickupObject.GetComponent<MeshCollider> ().enabled = false;
			} else if (pickupObject.GetComponent<SphereCollider> ()) {
				pickupObject.GetComponent<SphereCollider> ().enabled = false;
			}

			pickupObject.transform.parent = transform;
			pickupObject.transform.localPosition = Vector3.zero - new Vector3 (0, 0, z);
			if (pickupObject.GetComponent<Rigidbody> ()) {
				Destroy (pickupObject.GetComponent<Rigidbody> ());
			}
			if (pickupObject.transform.FindChild ("Activate").GetComponent<Light> ().enabled) {
				pickupObject.transform.FindChild ("Activate").GetComponent<Light> ().enabled = false;
			}
		}

		void Detachment ()
		{
			if (pickupObject.GetComponent<BoxCollider> ()) {
				pickupObject.GetComponent<BoxCollider> ().enabled = true;
			} else if (pickupObject.GetComponent<MeshCollider> ()) {
				pickupObject.GetComponent<MeshCollider> ().enabled = true;
			} else if (pickupObject.GetComponent<SphereCollider> ()) {
				pickupObject.GetComponent<SphereCollider> ().enabled = true;
			}
	
			transform.DetachChildren ();
			pickupObject.transform.parent = originalParent;
			pickupObject.AddComponent<Rigidbody> ();
			pickupObject = null;
			originalParent = null;
		}

		void OnTriggerEnter (Collider col)
		{
			if (col.gameObject.tag == "Pickup") {
				pickupObject = col.gameObject;
				originalParent = pickupObject.transform.parent;
			}
		}

		void OnTriggerExit (Collider col)
		{
			if (col.gameObject.tag == "Pickup") {
				pickupObject = null;
				originalParent = null;
			}
		}
	}
}