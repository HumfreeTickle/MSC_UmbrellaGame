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
		public bool interactTutorial = true; //stops the R1 tutorial from constantly activating

		private bool thrown;
		public bool pickup;

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
								pickup = true;
							}
						}
					}
				}
				if (transform.childCount > 0) {
					if (Input.GetButtonDown ("Interact")) {
						if (!IsInvoking ("Detachment")) {
							Invoke ("Detachment", 0);
							pickup = false;

						}
					}
				}
			}

			if (pickupObject != null) {
				if (pickup) {
					TurnOffColliders (pickupObject.transform);
				} else {
					TurnOnColliders (pickupObject.transform);
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
			pickupObject.transform.rotation = Quaternion.identity;
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

			thrown = false;
		}

		void Detachment ()
		{
			TurnOnColliders (pickupObject.transform);

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
			if (!thrown) {
				pickupObject.GetComponent<Rigidbody> ().AddForce (rb.velocity * throwingSpeed);
				thrown = true;
			}
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

			if (interactTutorial) {
				
				if (col.gameObject.tag == "Pickup") {
					tutorial.ObjectTag = "Interaction";
					interactTutorial = false;
				}
			} 
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

			if (col.gameObject.tag == "Interaction" || col.gameObject.tag == "NPC_talk" || col.gameObject.tag == "NPC" || col.gameObject.tag == "Pickup") {
				tutorial.ObjectTag = "";
				interactTutorial = true;
			}
		}

		void TurnOffColliders (Transform obj)
		{
			for (int child = 0; child < obj.childCount; child++) { //goes through each child object one at a time

				if (obj.GetComponent<BoxCollider> ()) {
					obj.GetComponent<BoxCollider> ().enabled = false;
				} else if (obj.GetComponent<MeshCollider> ()) {
					obj.GetComponent<MeshCollider> ().enabled = false;
				} else if (obj.GetComponent<SphereCollider> ()) {
					obj.GetComponent<SphereCollider> ().enabled = false;
				}

				if (obj.GetChild (child).transform.childCount > 0) {
					TurnOffColliders (obj.GetChild (child));
				} else {
					if (obj.GetChild (child).GetComponent<BoxCollider> ()) {
						obj.GetChild (child).GetComponent<BoxCollider> ().enabled = false;
					} else if (obj.GetChild (child).GetComponent<MeshCollider> ()) {
						obj.GetChild (child).GetComponent<MeshCollider> ().enabled = false;
					} else if (obj.GetChild (child).GetComponent<SphereCollider> ()) {
						obj.GetChild (child).GetComponent<SphereCollider> ().enabled = false;
					}
				}
			}
		}

		void TurnOnColliders (Transform obj)
		{
			for (int child = 0; child< obj.childCount; child++) { //goes through each child object one at a time
				if (obj.GetComponent<BoxCollider> ()) {
					obj.GetComponent<BoxCollider> ().enabled = true;
				} else if (obj.GetComponent<MeshCollider> ()) {
					obj.GetComponent<MeshCollider> ().enabled = true;
				} else if (obj.GetComponent<SphereCollider> ()) {
					obj.GetComponent<SphereCollider> ().enabled = true;
				}

				if (obj.GetChild (child).transform.childCount > 0) {
					TurnOnColliders (obj.GetChild (child));
				} else {
					if (obj.GetComponent<BoxCollider> ()) {
						obj.GetComponent<BoxCollider> ().enabled = true;
					} else if (obj.GetChild (child).GetComponent<MeshCollider> ()) {
						obj.GetChild (child).GetComponent<MeshCollider> ().enabled = true;
					} else if (obj.GetChild (child).GetComponent<SphereCollider> ()) {
						obj.GetChild (child).GetComponent<SphereCollider> ().enabled = true;
					}
				}
			}
		}
	}
}
