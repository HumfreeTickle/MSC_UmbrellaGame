using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CameraScripts;
using UnityEngine.Audio;

namespace Player.PhysicsStuff
{
	public class CreateWind : MonoBehaviour
	{
		/// <summary>
		/// Turns on/off the barrriers around the islands
		/// </summary>
		private GmaeManage gameManager;
		private GameState gameState;
		public GameObject windSystem;
		private GameObject instatiatedWind;
		private Vector3 spawnDistance;
		//-----------------------------------//
		public float maxTerrainDistance = 1.0f;

		//-----------------------------------//
		private Vector3 baseUmbrella = new Vector3 (0f, -5f, 0f);
		private Vector3 windSource = new Vector3 (0f, 10f, 0f);
		private Rigidbody rb;
		private Animator tutorialAnim;
		private float verticalInput;
		//-----------------------------------//
		private RaycastHit hit;

		public RaycastHit RaycastingInfo {
			get {
				return hit;
			}
		}

		public bool hitTerrain{ get; private set; }

		private bool onceOnly;

		public bool tooHigh{ get; private set; }

		public float upSpeed = 100;
		public AudioMixerSnapshot defaultSnapShot;
		public AudioMixerSnapshot offIslandSnapShot;
		public float snapshotTransitionSpeed;

		void Start ()
		{
			gameManager = GameObject.Find ("Follow Camera").GetComponent<GmaeManage> ();
			gameState = gameManager.gameState;
			rb = gameObject.GetComponent<Rigidbody> ();
			if (gameManager.controllerType == ControllerType.ConsoleContoller) {

				if (gameManager.consoleControllerType == ConsoleControllerType.PS3) {
					tutorialAnim = GameObject.Find ("Tutorial_PS3").GetComponent<Animator> ();
				} else if (gameManager.consoleControllerType == ConsoleControllerType.XBox) {
					tutorialAnim = GameObject.Find ("Tutorial_XBox").GetComponent<Animator> ();
				}
			} else if (gameManager.controllerType == ControllerType.Keyboard) {
				tutorialAnim = GameObject.Find ("Down_tutorial").GetComponent<Animator> ();
			}
		}

		void Update ()
		{
			gameState = gameManager.gameState;

			if (gameState != GameState.Pause || gameState != GameState.GameOver || gameState != GameState.MissionEvent) {
				if (gameManager.controllerType == ControllerType.ConsoleContoller) {
					if (Input.GetAxis (gameManager.controllerTypeVertical_R) >= 0.1f) {
						verticalInput = Input.GetAxis (gameManager.controllerTypeVertical_R);
						Rising ();
						if (this.transform.childCount < 2) {
							if (!IsInvoking ("SummonWind")) {
								Invoke ("SummonWind", 0);
							}
						}
					}
				} else if (gameManager.controllerType == ControllerType.Keyboard) {
					if (Input.GetButton (gameManager.controllerTypeVertical_R) && Input.GetAxisRaw (gameManager.controllerTypeVertical_R) > 0.1f) {
						verticalInput = 1;

						Rising ();
						if (this.transform.childCount < 2) {
							if (!IsInvoking ("SummonWind")) {
								Invoke ("SummonWind", 0);
							}
						}
					}
				}



       //---------------------------- RAYCASTING STUFF ------------------------------------//

				Vector3 downRayDown = (Vector3.down * 100);
				LayerMask clouds = 5;

				if (Physics.Raycast (transform.position + baseUmbrella, downRayDown, out hit, Mathf.Infinity, clouds.value)) {
					//------------- DEBUGGING -----------------------------
//					Debug.DrawRay (transform.position + baseUmbrella, downRayDown, Color.green, 10, false);
					hitTerrain = true;
					//------------- CONDITIONS ----------------------------
					if (hit.collider.tag == "Terrain") {
						rb.drag = 0;
//						GameManager.LAstKnownPosition = new Vector3 (transform.localPosition.x, hit.transform.position.y, transform.localPosition.z);

					} 

					// Activate falling tutorial
					if (gameState == GameState.Game) {

						if (hit.collider.tag == "Terrain" && hit.distance > maxTerrainDistance) {
							if (!onceOnly) {
								tutorialAnim.SetBool ("Fall", true);

								onceOnly = true;
								offIslandSnapShot.TransitionTo (snapshotTransitionSpeed / 2);

							}

						} else {
							defaultSnapShot.TransitionTo (snapshotTransitionSpeed);
							tutorialAnim.SetBool ("Fall", false);
											
						}


						if (hit.collider.tag == "Terrain" && hit.distance > (maxTerrainDistance * 3f)) {
							tooHigh = true;
						} 
							
						if (tooHigh) {
							if (hit.distance > maxTerrainDistance) {
								GetComponent<upwardForce> ().enabled = false;

							} else {
								GetComponent<upwardForce> ().enabled = true;
								tooHigh = false;

							}
						}
					}

				} else {
					hitTerrain = false;
					tooHigh = false;

					offIslandSnapShot.TransitionTo (snapshotTransitionSpeed);

					tutorialAnim.SetBool ("Fall", false);


					// Used to keep the player on the first island
					// Testing made us realise this wasn't very fun
//					if (barriers) {
//						rb.AddForce ((-bounceBack) * rb.velocity);
//					}
				}
			}
		}


//----------------------------- OTHER FUNCTIONS -------------------------------------------//

		void Rising ()
		{
			if (gameState == GameState.Game) {
				rb.GetComponent<Rigidbody> ().AddForce (Vector3.up * verticalInput * upSpeed);
			}

		}

		void SummonWind ()
		{
			if (gameState != GameState.MissionEvent) {
				//-------------------- CREATING THE WIND ----------------------------------
				spawnDistance = transform.position - windSource;
				instatiatedWind = Instantiate (windSystem, spawnDistance, Quaternion.Euler (Vector3.forward)) as GameObject;
				instatiatedWind.transform.parent = this.transform; 
				instatiatedWind.GetComponent<ParticleSystem> ().enableEmission = true;
				instatiatedWind.GetComponent<wind> ().gameState = gameState;
			}
		}
	}
}
