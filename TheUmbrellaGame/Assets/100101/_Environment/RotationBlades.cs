using UnityEngine;
using System.Collections;
using NPC;

namespace Environment
{
	public class RotationBlades : MonoBehaviour
	{
		private bool rotation;//this is the blades turning
		private Light activeLight;//the halo pointing where to interact with the windmill
		private GameObject caughtPieceofWood;
		private NPC_TutorialMission npc_TutorialMission;
		private Tutuorial tutorial;
		public float blowforce;//the force that will be applied to the blowback from the windmill
		public float speed;
//		public GameObject windEffect;
//		private GameObject windPushEffect;

		private bool turning;
		private bool tutorialRunning;
		private NPCManage npcManager = new NPCManage ();
		public GameObject lineOne;
		public GameObject lineTwo;
		private Color transparentStart = Color.white;
		private bool lightsOn;

		public bool LightsOn {
			set {
				lightsOn = value;
			}
		}

		private Transform handle;

		void Start ()
		{
			tutorial = GameObject.Find ("Tutorial").GetComponent<Tutuorial> ();
			npc_TutorialMission = GameObject.Find ("Missions").GetComponent<NPC_TutorialMission> ();
			caughtPieceofWood = transform.FindChild ("Caught_Wood").gameObject;
			activeLight = caughtPieceofWood.transform.FindChild ("Activate").GetComponent<Light> ();
			handle = GameObject.Find ("handle").transform;
		}

		void Update ()
		{
			tutorialRunning = npc_TutorialMission.TutorialRunning;
			if (tutorialRunning) {
				if (lightsOn) {
					activeLight.enabled = true;
				}

				if (rotation) {
					onRotation ();
				}
			}
		}

		void onRotation ()
		{
			transform.Rotate (0, -1 * speed * Time.deltaTime, 0);//the direction and speed at which the windmill will move
			lineOne.SetActive (true);
			lineTwo.SetActive (true);
			transparentStart = Color.Lerp (transparentStart, new Color (1, 1, 1, 0), Time.deltaTime / 2);

			lineOne.GetComponent<LineRenderer> ().material.SetColor ("_Color", transparentStart);
			lineTwo.GetComponent<LineRenderer> ().material.SetColor ("_Color", transparentStart);

			activeLight.enabled = false;
			activeLight.gameObject.transform.GetChild (0).gameObject.SetActive (true);
			tutorial.ObjectTag = "";
			this.tag = "Untagged";
			//						col.GetComponent<Rigidbody> ().AddForce (col.transform.forward * -1 * blowforce);//blow back the umbrella
			//						windPushEffect = Instantiate(windEffect, transform.position, Quaternion.identity) as GameObject;
			//						windPushEffect.transform.parent = this.transform;
			//----------------------------------//
			npcManager.WindmillMission = true;
			//----------------------------------//
			npc_TutorialMission.NPC_Tutorial.tag = "NPC_talk";
			npc_TutorialMission.Tut_X = 3;
			npc_TutorialMission.TutorialRunning = false;
			npc_TutorialMission.JumpAround_Tut = true;
			
		}
		
		void OnTriggerEnter (Collider col)
		{
			if (rotation) {
				if (col.tag == "Player") {
					if (col.GetComponent<Rigidbody> ()) {
						col.GetComponent<Rigidbody> ().AddForce (col.GetComponent<Rigidbody> ().velocity * -1 * blowforce);//blow back the umbrella
					}
				}
			}
		}

		void OnTriggerStay (Collider col)
		{
//			if (rotation) {
//				if (col.tag == "Player") {
//					if (col.GetComponent<Rigidbody> ()) {
//						col.GetComponent<Rigidbody> ().AddForce (col.GetComponent<Rigidbody>().velocity * blowforce);//blow back the umbrella
//					}
//				}
//			}
//
//			if (col.gameObject.name == caughtPieceofWood.name) {
//				print ("still here");
//			}

		}

		void OnTriggerExit (Collider col)
		{
			if (col.tag == "Player") {//if the umbrella interacts with the windmill
				if (handle.FindChild (caughtPieceofWood.name)) {
					//Could I have it create a ray in the direction of the umbrella that changes the amount of force based on distance??

//				if (Input.GetButtonDown ("Interact")) {
//					if (tutorialRunning) {
						rotation = true;//turn on the windmill
//						activeLight.enabled = false;
//						activeLight.gameObject.transform.GetChild (0).gameObject.SetActive (true);
//						tutorial.ObjectTag = "";
//						this.tag = "Untagged";
////						col.GetComponent<Rigidbody> ().AddForce (col.transform.forward * -1 * blowforce);//blow back the umbrella
////						windPushEffect = Instantiate(windEffect, transform.position, Quaternion.identity) as GameObject;
////						windPushEffect.transform.parent = this.transform;
//						//----------------------------------//
//						npcManager.WindmillMission = true;
//						//----------------------------------//
//						npc_TutorialMission.NPC_Tutorial.tag = "NPC_talk";
//						npc_TutorialMission.Tut_X = 3;
//						npc_TutorialMission.TutorialRunning = false;
//						npc_TutorialMission.JumpAround_Tut = true;
//					}
				}
			}
		}
	}
}