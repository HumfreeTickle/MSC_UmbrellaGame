﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Player.PhysicsStuff;
using Inheritence;

namespace Player
{
	public class grabbing : MonoBehaviour
	{
		public GameObject pickupObject;
		private Transform originalParent;
		public float throwingSpeed;
		public GameObject waterParticles;
		private GameObject instanWaterParticles;
		private Rigidbody rb;
		private Achievements achieves;
		private Tutuorial tutorial;
		private DestroyObject destroy = new Inheritence.DestroyObject ();
//		public bool interactTutorial = true; //stops the R1 tutorial from constantly activating

		private Collider umbrella;
		private bool thrown;
		public float pickupSize = 1.2f;

		void Start ()
		{
			tutorial = GameObject.Find ("Tutorial").GetComponent<Tutuorial> ();
			achieves = GameObject.Find ("Follow Camera").GetComponent<Achievements> ();
			rb = GameObject.Find ("main_Sphere").GetComponent<Rigidbody> ();
			umbrella = GameObject.Find ("main_Sphere").GetComponent<Collider> ();
		}

		void Update ()
		{
			if (pickupObject != null) {
				if (pickupObject.GetComponent<Collider> ()) {
					Physics.IgnoreCollision (umbrella, pickupObject.GetComponent<Collider> ());
				}
			}

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

		void Pickup ()
		{
			pickupObject.transform.parent = transform;
			pickupObject.transform.localPosition = Vector3.zero - new Vector3 (0, 0, 2);
			pickupObject.transform.rotation = Quaternion.identity;
			pickupObject.tag = "Player";
			pickupObject.layer = 15;
			ChangeLayer (pickupObject.transform, 15);

			tutorial.objectTag = "";


			if (pickupObject.GetComponent<Rigidbody> ()) {
				pickupObject.GetComponent<Rigidbody> ().isKinematic = true;
				if (pickupObject.GetComponent<Rigidbody> ().useGravity) {
					pickupObject.GetComponent<Rigidbody> ().useGravity = false;
				}
			} else {
				pickupObject.AddComponent<Rigidbody> ();				
				pickupObject.GetComponent<Rigidbody> ().isKinematic = true;
				if (pickupObject.GetComponent<Rigidbody> ().useGravity) {
					pickupObject.GetComponent<Rigidbody> ().useGravity = false;
				}
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
			if (pickupObject == null) {
				pickupObject = this.transform.GetChild (0).gameObject;
			} 
			ChangeLayer (pickupObject.transform, 0);	
			transform.DetachChildren ();

			if (originalParent != null) {
				pickupObject.transform.parent = originalParent;
			}

			pickupObject.tag = "Pickup";
			pickupObject.layer = 0;

			if (pickupObject.GetComponent<Rigidbody> ()) {
				pickupObject.GetComponent<Rigidbody> ().isKinematic = false;
				if (!pickupObject.GetComponent<Rigidbody> ().useGravity) {
					pickupObject.GetComponent<Rigidbody> ().useGravity = true;
				}

			} 

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
				Debug.Log (col.name + " : Enter");
				pickupObject = col.gameObject;
				originalParent = pickupObject.transform.parent;
//				pickupObject.layer = 15;

				if (col.GetComponent<BoxCollider> ()) {
					if (col.GetComponent<BoxCollider> ().isTrigger) {
						if (col.GetComponent<BoxCollider> ().size.magnitude < 1000) {
							col.GetComponent<BoxCollider> ().size = col.GetComponent<BoxCollider> ().size * pickupSize;
						} else {
							Debug.LogError ("Some shit went down : Box Collider");
							while (col.GetComponent<BoxCollider> ().size.magnitude > 10) {
								col.GetComponent<BoxCollider> ().size = col.GetComponent<BoxCollider> ().size / pickupSize;
							}
						}
					}

				} else if (col.GetComponent<SphereCollider> ()) {
					if (col.GetComponent<SphereCollider> ().isTrigger) {
						if (col.GetComponent<SphereCollider> ().radius < 100) {
							col.GetComponent<SphereCollider> ().radius = col.GetComponent<SphereCollider> ().radius * pickupSize;
						} else {
							Debug.LogError ("Some shit went down : Sphere Collider");
							while (col.GetComponent<SphereCollider> ().radius > 10) {
								col.GetComponent<SphereCollider> ().radius = col.GetComponent<SphereCollider> ().radius / pickupSize;
							}
						}
					}
				}
			}
		}

		void OnTriggerStay (Collider col)
		{

//			if (interactTutorial) {
				
			if (col.gameObject.tag == "Pickup") {
//								
//				if (col.GetComponent<BoxCollider> ()) {
//					if (col.GetComponent<BoxCollider> ().isTrigger) {
//						if (col.GetComponent<BoxCollider> ().size.magnitude < 1000) {
//							col.GetComponent<BoxCollider> ().size = col.GetComponent<BoxCollider> ().size * pickupSize;
//						} else {
//							Debug.LogError ("Some shit went down");
//							while (col.GetComponent<BoxCollider> ().size.magnitude > 10) {
//								col.GetComponent<BoxCollider> ().size = col.GetComponent<BoxCollider> ().size / pickupSize;
//							}
//						}
//					}
//						
//				} else if (col.GetComponent<SphereCollider> ()) {
//					if (col.GetComponent<BoxCollider> ().isTrigger) {
//						col.GetComponent<SphereCollider> ().radius = col.GetComponent<SphereCollider> ().radius * pickupSize;
//					}
//				}

				if (tutorial.objectTag == "") {
					tutorial.objectTag = "Interaction";
				}
//					if (interactTutorial) {
//						interactTutorial = false;
//					}

				pickupObject = col.gameObject;
				originalParent = pickupObject.transform.parent;
				for (int child = 0; child< pickupObject.transform.childCount; child++) {
					pickupObject.transform.GetChild (child).gameObject.layer = 15;
				}
			}
//			} 

			if (col.gameObject.tag == "River") {
				if (rb.velocity.magnitude > 10) {
					if (!IsInvoking ("WaterEffects")) {
						Invoke ("WaterEffects", 0.02f);
					}
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
				Debug.Log (col.name + " : Exit");

				if (pickupObject != null) {

					if (col.GetComponent<BoxCollider> ()) {
						if (col.GetComponent<BoxCollider> ().isTrigger) {
							col.GetComponent<BoxCollider> ().size = col.GetComponent<BoxCollider> ().size / pickupSize;
						}
						
					} else if (col.GetComponent<SphereCollider> ()) {
						col.GetComponent<SphereCollider> ().radius = col.GetComponent<SphereCollider> ().radius / pickupSize;
					}

					
					for (int child = 0; child< pickupObject.transform.childCount; child++) {
						pickupObject.transform.GetChild (child).gameObject.layer = 0;
					}
					pickupObject = null;
					originalParent = null;
				}

			}

			if (col.gameObject.tag == "Interaction" 
				|| col.gameObject.tag == "NPC_talk" 
				|| col.gameObject.tag == "NPC" 
				|| col.gameObject.tag == "Pickup") {
				if (tutorial.objectTag != "") {
					tutorial.objectTag = "";
				}
//				if (!interactTutorial) {
//					interactTutorial = true;
//				}
			}
		}

		void ChangeLayer (Transform obj, int layerNumber)
		{
			for (int child = 0; child < obj.childCount; child++) { //goes through each child object one at a time
				obj.gameObject.layer = layerNumber;

				if (obj.GetChild (child).transform.childCount > 0) {
					ChangeLayer (obj.GetChild (child), layerNumber);
				} else {
					obj.GetChild (child).gameObject.layer = layerNumber;
				}
			}
		}

		void WaterEffects ()
		{
			instanWaterParticles = Instantiate (waterParticles, transform.position, Quaternion.identity) as GameObject;
			instanWaterParticles.GetComponent<ParticleSystem> ().Play ();
			destroy.DestroyOnTimer (instanWaterParticles, 0.5f);
		}

//		void TurnOnColliders (Transform obj)
//		{
//			for (int child = 0; child< obj.childCount; child++) { //goes through each child object one at a time
//				if (obj.GetComponent<BoxCollider> ()) {
//					obj.GetComponent<BoxCollider> ().enabled = true;
//				} else if (obj.GetComponent<MeshCollider> ()) {
//					obj.GetComponent<MeshCollider> ().enabled = true;
//				} else if (obj.GetComponent<SphereCollider> ()) {
//					obj.GetComponent<SphereCollider> ().enabled = true;
//				}
//
//				if (obj.GetChild (child).transform.childCount > 0) {
//					TurnOnColliders (obj.GetChild (child));
//				} else {
//					if (obj.GetComponent<BoxCollider> ()) {
//						obj.GetComponent<BoxCollider> ().enabled = true;
//					} else if (obj.GetChild (child).GetComponent<MeshCollider> ()) {
//						obj.GetChild (child).GetComponent<MeshCollider> ().enabled = true;
//					} else if (obj.GetChild (child).GetComponent<SphereCollider> ()) {
//						obj.GetChild (child).GetComponent<SphereCollider> ().enabled = true;
//					}
//				}
//			}
//		}
	}
}
