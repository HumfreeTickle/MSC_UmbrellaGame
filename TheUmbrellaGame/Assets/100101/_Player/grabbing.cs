using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Player.PhysicsStuff;
using Inheritence;

namespace Player
{
	public class grabbing : MonoBehaviour
	{
		private GameObject pickupObject;
		private Transform originalParent;
		private bool terrainHit;
		public float z;
		public float throwingSpeed;
		public GameObject waterParticles;
		private GameObject instanWaterParticles;
		private Rigidbody rb;
		private Achievements achieves;
		private Tutuorial tutorial;
		private DestroyObject destroy = new Inheritence.DestroyObject ();

		void Start ()
		{
			tutorial = GameObject.Find ("Tutorial").GetComponent<Tutuorial> ();
			achieves = GameObject.Find ("Follow Camera").GetComponent<Achievements> ();
			rb = GameObject.Find ("main_Sphere").GetComponent<Rigidbody> ();
		}

		void Update ()
		{
			terrainHit = GameObject.Find ("main_Sphere").GetComponent<CreateWind> ().HitTerrain;

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
			pickupObject.tag = "Player";
			tutorial.ObjectTag = "";
			if (pickupObject.GetComponent<Rigidbody> ()) {
				Destroy (pickupObject.GetComponent<Rigidbody> ());
			}
			if (pickupObject.transform.FindChild ("Activate")) {
				if (pickupObject.transform.FindChild ("Activate").GetComponent<Light> ().enabled) {
					pickupObject.transform.FindChild ("Activate").GetComponent<Light> ().enabled = false;
				}
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
			pickupObject.tag = "Pickup";

			pickupObject.AddComponent<Rigidbody> ();

			pickupObject.GetComponent<Rigidbody> ().AddForce (rb.velocity * throwingSpeed);
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

		void OnTriggerStay (Collider col)
		{
			if (col.gameObject.tag == "River") {
				if (rb.velocity.magnitude > 10) {
					instanWaterParticles = Instantiate (waterParticles, transform.position, Quaternion.identity) as GameObject;
					instanWaterParticles.GetComponent<ParticleSystem> ().Play ();
					destroy.DestroyOnTimer (instanWaterParticles, 1f);
					if (!achieves.CoroutineInMotion) {
						if (achieves.achievements.Contains ("Splish. Splash.")) {
							StartCoroutine (achieves.Notification (achieves.achievements [0]));
						}
					}
				}
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