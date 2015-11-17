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
		public GameObject lineOne;
		public GameObject lineTwo;
		private Color transparentStart = Color.white;

		public bool lightsOn{ private get; set; }

		private Transform handle;
		private GameObject cameraSet;
		private GmaeManage gameManager;
		private Transform moveTo;
		private _MoveCamera cmaeraMove;
		private IEnumerator cameraMoveCoroutine;
		private bool moveCmarea = true;

		void Start ()
		{
			tutorial = GameObject.Find ("Tutorial").GetComponent<Tutuorial> ();
			npc_TutorialMission = GameObject.Find ("Missions").GetComponent<NPC_TutorialMission> ();
			caughtPiece = transform.parent.transform.FindChild ("Caught_Wood").gameObject;

			activeLight = caughtPiece.transform.FindChild ("Activate").GetComponent<Light> (); //finds the light attahed to the caughtpiece
			windParticles = activeLight.gameObject.transform.GetChild (0).gameObject; // not sure what this is used for

			handle = GameObject.Find ("handle").transform;

//			umbrella = GameObject.Find ("main_Sphere");
//			cmaera = GameObject.Find ("Follow Camera").GetComponent<Controller> ();
			gameManager = GameObject.Find ("Follow Camera").GetComponent<GmaeManage> ();

			moveTo = GameObject.Find ("MoveTo").transform;
			GetComponent<AudioSource> ().enabled = false;

			cmaeraMove = GameObject.Find ("Follow Camera").GetComponent<_MoveCamera> ();

		}

		void Update ()
		{

			if (rotation) {
				onRotation ();
				if (moveCmarea) {
					CameraMove ();
				}
			}

			if (caughtPiece.transform.parent == handle.transform) {
				activeLight.enabled = false;
				lightsOn = false;
				rotation = true;//turn on the windmill

				tutorial.objectTag = "";
				this.tag = "Untagged";

				npc_TutorialMission.npc_Tutorial.tag = "NPC_talk";
				npc_TutorialMission.tut_X = 2;
				npc_TutorialMission.jumpAround_Tut = true;
			} else {
				if (lightsOn) {
					activeLight.enabled = true;
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

			GetComponent<AudioSource> ().enabled = true;
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

						windPushEffect = Instantiate (windEffect, transform.position, Quaternion.identity) as GameObject;
						windPushEffect.transform.parent = transform;
						windPushEffect.transform.LookAt (col.transform);
					} else {
						return;
					}
				}
			}
		}

		void CameraMove ()
		{
			if (!cmaeraMove.StartCoroutineCamera) {
				System.Action endCoroutine = () => {
					if (gameManager.GameState == GameState.MissionEvent) {
						gameManager.GameState = GameState.Game;
					}
					moveCmarea = false;};
				
				cameraMoveCoroutine = cmaeraMove.cameraMove (this.gameObject,endCoroutine, moveTo, 2f );
				StartCoroutine (cameraMoveCoroutine);
			}
		}
	}
}