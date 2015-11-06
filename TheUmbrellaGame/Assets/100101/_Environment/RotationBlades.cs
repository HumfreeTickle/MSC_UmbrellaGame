using UnityEngine;
using System.Collections;
using NPC;
using CameraScripts;

namespace Environment
{
	public class RotationBlades : MonoBehaviour
	{
		public bool rotation;//this is the blades turning
		private Light activeLight;//the halo pointing where to interact with the windmill
		private GameObject caughtPiece;
		private GameObject windParticles;
		private NPC_TutorialMission npc_TutorialMission;
		private Tutuorial tutorial;
		public float blowforce;//the force that will be applied to the blowback from the windmill
		public float speed;
		public GameObject windEffect;
		private GameObject windPushEffect;
		private bool turning;
		private NPCManage npcManager = new NPCManage ();
		public GameObject lineOne;
		public GameObject lineTwo;
		private Color transparentStart = Color.white;
		public bool lightsOn{private get; set;}

		private Transform handle;
		private GameObject umbrella;
		private Controller cmaera;
		private GameObject cameraSet;
		private GmaeManage gameManager;
		private bool running;
		private bool zeldafy = true;
		private Vector3 moveTo;

		void Start ()
		{
			tutorial = GameObject.Find ("Tutorial").GetComponent<Tutuorial> ();
			npc_TutorialMission = GameObject.Find ("Missions").GetComponent<NPC_TutorialMission> ();
			caughtPiece = transform.FindChild ("Caught_Wood").gameObject;

			activeLight = caughtPiece.transform.FindChild ("Activate").GetComponent<Light> (); //finds the light attahed to the caughtpiece
			windParticles = activeLight.gameObject.transform.GetChild (0).gameObject; // not sure what this is used for

			handle = GameObject.Find ("handle").transform;

			umbrella = GameObject.Find ("main_Sphere");
			cmaera = GameObject.Find ("Follow Camera").GetComponent<Controller> ();
			gameManager = GameObject.Find ("Follow Camera").GetComponent<GmaeManage> ();

			moveTo = GameObject.Find ("MoveTo").transform.position;
		}

		void Update ()
		{
			if (lightsOn) {
				activeLight.enabled = true;
			}else{
				activeLight.enabled = false;

			}

			if (rotation) {
				onRotation ();
				StartCoroutine (cameraMove ());
			}

//			if (caughtPiece.transform.parent == handle.transform) {
////				npc_TutorialMission.DropWood = true;
//			}
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
			if (windParticles != null) {
				windParticles.SetActive (true);
			}
		}
		
		void OnTriggerEnter (Collider col)
		{
			if (rotation) {
				if (col.tag == "Player") {
					if (col.GetComponent<Rigidbody> ()) {
						col.GetComponent<Rigidbody> ().AddForce (col.GetComponent<Rigidbody> ().velocity * -1 * blowforce);//blow back the umbrella
					}
					if (windEffect != null) {
						windPushEffect = Instantiate (windEffect, col.transform.position, Quaternion.identity) as GameObject;
						windPushEffect.transform.parent = transform;
					} else {
						return;
					}
				}
			}
		}

		void OnTriggerExit (Collider col)
		{
			if (col.tag == "Player") {//if the umbrella interacts with the windmill
				if (caughtPiece.transform.parent == handle) {
					rotation = true;//turn on the windmill
					running = true;

					tutorial.objectTag = "";
					this.tag = "Untagged";

					//----------------------------------//
					npcManager.WindmillMission = true;
					//----------------------------------//
					npc_TutorialMission.npc_Tutorial.tag = "NPC_talk";
					npc_TutorialMission.tut_X = 2;
					npc_TutorialMission.jumpAround_Tut = true;
				}
			}
		}

// sweet I already worked out a way to move the camera independently from the missions
// just gotta make it generic and move it out of here

		IEnumerator cameraMove ()
		{
			while (running) {
				while (zeldafy) {
					gameManager.GameState = GameState.MissionEvent;
					cameraSet = this.gameObject;
					cmaera.lookAt = cameraSet;
					cmaera.MoveYerself = false;

					while (Vector3.Distance(cmaera.transform.position, moveTo) > 10 && cameraSet ==  this.gameObject) {
						if (!cmaera.MoveYerself) {
							cmaera.transform.position = Vector3.Lerp (cmaera.transform.position, moveTo, Time.deltaTime / 2);

							yield return null;
						}
						zeldafy = false;

						yield return null;

					}
					yield return null;

				}
				yield return new WaitForSeconds (5);
				cameraSet = umbrella;
				cmaera.lookAt = cameraSet;
				cmaera.MoveYerself = true;
				gameManager.GameState = GameState.Game;
				running = false;
			}

			yield break;
//			look back at the windmill when you've made it start :)
		}
	}
}