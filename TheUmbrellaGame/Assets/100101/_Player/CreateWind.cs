using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CameraScripts;

namespace Player.PhysicsStuff
{
	// should wind creation be an inheritence class that is called ???
	public class CreateWind : MonoBehaviour
	{
		/// <summary>
		/// Turns on/off the barrriers around the islands
		/// </summary>
		private GmaeManage GameManager;
		private GameState gameState;
		public GameObject windSystem;
		private GameObject instatiatedWind;
		private Vector3 spawnDistance;
		//-----------------------------------//
		private float charge;
		public float progression;
		public float maxTerrainDistance = 1.0f;
		//-----------------------------------//
		public Material umbrellaColour;
		public Color blackTint = Color.black;
		public List<Color> originalColours;
		private bool gameStart;
		//-----------------------------------//
		private Vector3 baseUmbrella = new Vector3 (0f, -5f, 0f);
		private GameObject umbrella;
		private GameObject canopyColours;
		private Rigidbody umbrellaRb;
		//-----------------------------------//
		public bool barriers;
		public float bounceBack;
		private RaycastHit hit;

		public RaycastHit RaycastingInfo {
			get {
				return hit;
			}
		}

		void Start ()
		{
			GameManager = GameObject.Find ("Follow Camera").GetComponent<GmaeManage> ();
			gameState = GameManager.gameState;
			umbrella = GameObject.Find ("Umbrella");
			umbrellaRb = this.gameObject.GetComponent<Rigidbody> ();
			charge = GameManager.UmbrellaCharge;
			canopyColours = umbrella.transform.FindChild ("Canopy_Colours").gameObject;
			ChangeColours (canopyColours.transform);
		}

		void Update ()
		{
			charge = GameManager.UmbrellaCharge;
			progression = GameManager.Progression;

			bounceBack = Mathf.Clamp (bounceBack, 0, Mathf.Infinity);
			gameState = GameManager.gameState;

			if (gameState != GameState.Pause || gameState != GameState.GameOver) {
				if (Input.GetButtonDown ("CrateWind") && charge >= 1) {
					if (this.transform.childCount < 2) {
						if (!IsInvoking ("SummonWind")) {
							Invoke ("SummonWind", 0.1f);
						}
					}
					GameManager.UmbrellaCharge = Mathf.Clamp (Mathf.Lerp (GameManager.UmbrellaCharge, 0, Time.fixedDeltaTime * 10), 2, 100);
				}

//---------------- TURN OFF UPWARDFORCE ---------------------
				if (charge <= 10) {
					GetComponent<upwardForce> ().enabled = false;

				} 
				//------------------------- COLOUR CHANGING ---------------------------//

//				ChangeColours (canopyColours.transform);




//---------------------------- RAYCASTING STUFF -----------------------------------------------------------------------

				Vector3 downRayDown = (Vector3.down * 100);

				if (Physics.Raycast (transform.position + baseUmbrella, downRayDown, out hit, Mathf.Infinity)) {

					//------------- DEBUGGING -----------------------------
//					Debug.DrawRay (transform.position + baseUmbrella, downRayDown, Color.green, 10, false);

					//------------- CONDITIONS ----------------------------
					if (hit.collider.tag == "Terrain" && hit.distance < maxTerrainDistance) {
						GameManager.UmbrellaCharge = Mathf.Clamp (Mathf.Lerp (charge, 100, Time.time / (hit.distance * 100)), 0, 100);
					} 
				} else {
					GameManager.UmbrellaCharge = Mathf.Lerp (charge, 0, Time.deltaTime / (progression*2));// progress :)

					if (barriers) {
						umbrellaRb.AddForce ((-bounceBack) * umbrellaRb.velocity);
						//not really working. Need to try something else
					}
				}
			}
		}


//----------------------------- OTHER FUNCTIONS ------------------------------------------------------------------------
		void ChangeColours (Transform obj)
		{
			for (int child = 0; child< obj.childCount; child++) { //goes through each child object one at a time
				if (obj.GetChild (child).transform.childCount > 0) {
					ChangeColours (obj.GetChild (child));
				} else {
					if (obj.GetChild (child).GetComponent<MeshRenderer> ()) { // checks to see if there is a mesh renderer attached to child
						MeshRenderer umbrellaChild = obj.GetChild (child).GetComponent<MeshRenderer> ();
						//Needs to only do this once
						if (originalColours.Count < 16) {
							originalColours.Add (umbrellaChild.material.color);
						} else {
							gameStart = true;
						}
						if (gameStart && charge <= 10) {
							umbrellaChild.material.color = Color.Lerp (umbrellaChild.material.color, blackTint, Time.deltaTime);
						}
//						else if(gameStart && charge > 10){
//							//NEEDS A WAY TO DIFFERENTIATE EACH SECTION
//						umbrellaChild.material.color = Color.Lerp (umbrellaChild.material.color, originalColours [child], Time.deltaTime);
//						}
					}
				}
			}
		}

		void SummonWind ()
		{
			//-------------------- CREATING THE WIND ----------------------------------
			spawnDistance = transform.position - new Vector3 (0, 10, 0);
		
			instatiatedWind = Instantiate (windSystem, spawnDistance, Quaternion.Euler (Vector3.forward)) as GameObject;
			instatiatedWind.transform.parent = this.transform; 
			instatiatedWind.GetComponent<ParticleSystem> ().enableEmission = true;
			instatiatedWind.GetComponent<wind> ().windForce = charge * 10;

			//--------------------	TURNS OFF PARTICLES AFTER ONE CYCLE -----------------
			if (Input.GetButtonUp ("CrateWind")) {
				instatiatedWind.GetComponent<ParticleSystem> ().enableEmission = false;
			}
		}
	}
}
