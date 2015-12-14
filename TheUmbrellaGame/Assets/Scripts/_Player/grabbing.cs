using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Player.PhysicsStuff;
using Inheritence;

namespace Player
{
	/// <summary>
	/// Allows the player to pickup a gameObject, tagged as "Pickup"
	/// The player can detach the object or apply a force realtive to their own velocity
	/// </summary>
	public class grabbing : MonoBehaviour
	{
		private GmaeManage gameManager;
		public GameObject pickupObject;
		private Transform originalParent;
		public float throwingSpeed;
		public GameObject waterParticles;
		private GameObject instanWaterParticles;
		private GameObject umbrella;
		private Rigidbody rb;
		private Achievements achieves;
		private Tutuorial tutorial;
		private DestroyObject destroy = new Inheritence.DestroyObject ();
		private Collider umbrellaCol;
		private bool thrown;
		public float pickupSize = 1.2f;
		private float pickupObjectRadius = 0;
		private Vector3 pickupObjectSize = Vector3.zero;

		void Start ()
		{
			gameManager = GameObject.Find ("Follow Camera").GetComponent<GmaeManage> ();
			umbrella = GameObject.Find ("main_Sphere");
			if (gameManager.consoleControllerType == ConsoleControllerType.PS3) {
				tutorial = GameObject.Find ("Tutorial_PS3").GetComponent<Tutuorial> ();
			} else if (gameManager.consoleControllerType == ConsoleControllerType.XBox) {
				tutorial = GameObject.Find ("Tutorial_XBox").GetComponent<Tutuorial> ();
			}

			achieves = GameObject.Find ("Follow Camera").GetComponent<Achievements> ();
			rb = umbrella.GetComponent<Rigidbody> ();
			umbrellaCol = umbrella.GetComponent<Collider> ();
		}

		void Update ()
		{
			if (pickupObject != null) {
				if (pickupObject.GetComponent<Collider> ()) {
					Physics.IgnoreCollision (umbrellaCol, pickupObject.GetComponent<Collider> ());
				}
			}

			if (pickupObject != null) {
				if (Input.GetButtonDown (gameManager.controllerInteract)) {
					if (Input.GetButtonDown (gameManager.controllerInteract)) {
						if (!IsInvoking ("Pickup")) {
							Invoke ("Pickup", 0);
						}
					}
				}
			}

			if (transform.childCount > 0) {
				if (Input.GetButtonDown (gameManager.controllerInteract) || gameManager.gameState == GameState.MissionEvent) {
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

			if (pickupObject.GetComponent<BoxCollider> ()) {
				if (pickupObject.GetComponent<BoxCollider> ().isTrigger) {
					pickupObject.GetComponent<BoxCollider> ().size = pickupObjectSize;
				}
				
			} else if (pickupObject.GetComponent<SphereCollider> ()) {
				pickupObject.GetComponent<SphereCollider> ().radius = pickupObjectRadius;
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
				pickupObject = col.gameObject;
				originalParent = pickupObject.transform.parent;

				// used to increase the size of the trigger box so the player can pick it up easier
				// otherwise they can very easily slip outside the trigger area
				if (col.GetComponent<BoxCollider> ()) {
					if (col.GetComponent<BoxCollider> ().isTrigger) {
						if (col.GetComponent<BoxCollider> ().size.magnitude < 1000) {
							if (pickupObjectSize == Vector3.zero) {
								pickupObjectSize = col.GetComponent<BoxCollider> ().size;
							}
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
							if (pickupObjectRadius == 0) {
								pickupObjectRadius = col.GetComponent<SphereCollider> ().radius;
							}
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
			if (col.gameObject.tag == "Pickup") {
				if (tutorial.objectTag == "") {
					tutorial.objectTag = "Pickup";
				}
				pickupObject = col.gameObject;
				originalParent = pickupObject.transform.parent;
				for (int child = 0; child< pickupObject.transform.childCount; child++) {
					pickupObject.transform.GetChild (child).gameObject.layer = 15;
				}
			} else if (col.gameObject.tag == "Interaction") {
				if (tutorial.objectTag == "") {
					tutorial.objectTag = "Interaction";
				}
			} else if (col.gameObject.tag == "River") {
				if (rb.velocity.magnitude > 10) {
					if (!IsInvoking ("WaterEffects")) {
						Invoke ("WaterEffects", 0.02f);
					}
					if (!achieves.coroutineInMotion) {
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
				if (col.GetComponent<BoxCollider> ()) {
					if (col.GetComponent<BoxCollider> ().isTrigger) {
						col.GetComponent<BoxCollider> ().size = col.GetComponent<BoxCollider> ().size / pickupSize;
					}
						
				} else if (col.GetComponent<SphereCollider> ()) {
					col.GetComponent<SphereCollider> ().radius = col.GetComponent<SphereCollider> ().radius / pickupSize;
				}

				if (pickupObject != null) {
					for (int child = 0; child< pickupObject.transform.childCount; child++) {
						pickupObject.transform.GetChild (child).gameObject.layer = 0;
					}

					pickupObject = null;
					originalParent = null;
				} else {
					pickupObjectRadius = 0;
					pickupObjectSize = Vector3.zero;
				}
			}

			if (col.gameObject.tag == "Interaction" 
				|| col.gameObject.tag == "NPC_talk" 
				|| col.gameObject.tag == "NPC" 
				|| col.gameObject.tag == "Pickup") {
				if (tutorial.objectTag != "") {
					tutorial.objectTag = "";
				}
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

		void WaterEffects () // adds the water partile effect when the player moves through water
		{
			instanWaterParticles = Instantiate (waterParticles, transform.position, Quaternion.identity) as GameObject;
			instanWaterParticles.GetComponent<ParticleSystem> ().Play ();
			destroy.DestroyOnTimer (instanWaterParticles, 0.5f);
		}
	}
}
