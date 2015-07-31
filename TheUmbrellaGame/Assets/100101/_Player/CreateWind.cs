using UnityEngine;
using System.Collections;
using CameraScripts;

namespace Player.PhysicsStuff
{
	// should wind creation be an inheritence class that is called ???
	public class CreateWind : MonoBehaviour
	{
		private GmaeManage GameManager;
		private GameState gameState;
		public GameObject windSystem;
		private GameObject instatiatedWind;
		private Vector3 spawnDistance;
		private float charge;
		public float maxTerrainDistance = 1.0f;

		private Material umbrellaColour;
		private Vector3 baseUmbrella = new Vector3 (0f, -5f, 0f);

		private GameObject umbrella;
		private Rigidbody umbrellaRb;
		public float bounceBack;

		void Start ()
		{
			GameManager = GameObject.Find ("Follow Camera").GetComponent<GmaeManage> ();
			gameState = GameManager.gameState;
			umbrella = GameObject.Find ("Umbrella");
			umbrellaRb = this.gameObject.GetComponent<Rigidbody>();

			umbrellaColour = umbrella.transform.GetChild(0).GetComponent<Renderer> ().material;
			charge = GameManager.WindCharge;
		}

		void Update ()
		{
			charge = GameManager.WindCharge;

			bounceBack = Mathf.Clamp(bounceBack, Mathf.NegativeInfinity, 0);
			gameState = GameManager.gameState;

			if (gameState != GameState.Pause || gameState != GameState.GameOver) {
				if (Input.GetButtonDown ("CrateWind") && charge >= 1) {
					SummonWind ();
					GameManager.WindCharge = Mathf.Clamp( Mathf.Lerp (GameManager.WindCharge, 0, Time.fixedDeltaTime * 10), 2, 100);
				}

//---------------- TURN OFF UPWARDFORCE ---------------------
				if (charge <= 10) {
					GetComponent<upwardForce> ().enabled = false;
				} 

//---------------------------- COLOUR CHANGING ------------------------------------------------------------------------

				Color newUmbrellaColour = Vector4.Lerp (umbrellaColour.color, new Vector4 (charge / 100, charge / 100, charge / 100, 1), Time.deltaTime * (charge + 1)); 

				umbrellaColour.SetColor ("_Color", newUmbrellaColour);


//---------------------------- RAYCASTING STUFF -----------------------------------------------------------------------

				Vector3 downRayDown = Vector3.down;
				RaycastHit hit;

		
				if (Physics.Raycast (transform.position + baseUmbrella, downRayDown, out hit, Mathf.Infinity)) {

					//------------- DEBUGGING -----------------------------
					Debug.DrawRay (transform.position + baseUmbrella, downRayDown, Color.green, Mathf.Infinity, false);
//					print (hit.collider);
//					print ("Distance: " + hit.distance);

					//------------- CONDITIONS ----------------------------
					if (hit.collider.tag == "Terrain" && hit.distance < maxTerrainDistance) {
						GameManager.WindCharge = Mathf.Clamp( Mathf.Lerp (charge, 100, Time.time / (hit.distance * 100)), 2, 100);
					} 
				} else {
					GameManager.WindCharge = Mathf.Lerp (charge, 0, Time.deltaTime);

					umbrellaRb.AddForce(transform.TransformDirection(this.gameObject.transform.forward) * bounceBack);
					//not really working. Need to try something else
				}
			}
		}


//----------------------------- OTHER FUNCTIONS ------------------------------------------------------------------------
	

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
