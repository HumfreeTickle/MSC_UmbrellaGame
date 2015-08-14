using UnityEngine;
using System.Collections;
using Player.PhysicsStuff;
using Inheritence;

namespace Player
{
	public class controller : MonoBehaviour
	{
		public DestroyObject destroyStuff = new Inheritence.DestroyObject ();
		public GmaeManage gameManager;
//	------------------------------------
		public Rigidbody frontsphere;
		public Rigidbody backsphere;
		public Rigidbody leftsphere;
		public Rigidbody rightsphere;
		public Rigidbody handle;

//	------------------------------------
		public Animator umbrellaAnim;
		public ForceMode movementForce;
		public ForceMode backwardForce;
		public ForceMode rotationForce;
//	------------------------------------
		private Rigidbody rb;
		private float lsphereMass;
		private float rsphereMass;
		private float fsphereMass;
		private float bsphereMass;
//	------------------------------------
		//	private float rbMass;
		private float handleMass;
		public float forceAppliedToTilt; // used for tilting purposes
		public float speed;
		public float floating;
		public float turningSpeed;
//	------------------------------------
		private string controllerTypeVertical;
		private string controllerTypeHorizontal;

//  ------------------------------------
		private RaycastHit hit;
		private upwardForce upForce;

		void Start ()
		{
			rb = GetComponent<Rigidbody> ();
			lsphereMass = leftsphere.mass;
			rsphereMass = rightsphere.mass;
			fsphereMass = frontsphere.mass;
			bsphereMass = backsphere.mass;
			handleMass = handle.mass;
			upForce = GetComponent<upwardForce> ();
			controllerTypeVertical = gameManager.ControllerTypeVertical;
			controllerTypeHorizontal = gameManager.ControllerTypesHorizontal;
		}
		
		void FixedUpdate ()
		{
			if (gameManager.gameState != GameState.Game) {
				Input.ResetInputAxes (); // stops the player from making an input before the game begins
			}

			if (gameManager.gameState == GameState.Game) {
				Movement ();
				HorizontalMass ();
				VerticalMass ();
//				if (GetComponent<CreateWind> ().RaycastingInfo != null) {
				hit = GetComponent<CreateWind> ().RaycastingInfo;
				if (hit.collider != null) {
					TheDescent ();
				}
			} else if (gameManager.gameState == GameState.GameOver) {
				GetComponent<upwardForce> ().enabled = false;
			}
		}
		
		//----------------------------- OTHER FUNCTIONS ------------------------------------------------------------------------
		
		void Movement ()
		{

			// need to set up a check as to whether the controller is active when a game state change occurs
			umbrellaAnim.SetFloat ("Input_Vertical", Input.GetAxis (controllerTypeVertical));
			umbrellaAnim.SetFloat ("Input_Horizontal", Input.GetAxis (controllerTypeHorizontal));

			if (Input.GetAxis (controllerTypeVertical) > 0.1f) { // Probably should only use forward for this and have back be a kind of breaking system
				rb.AddForce (transform.TransformDirection (Vector3.forward) * Input.GetAxis (controllerTypeVertical) * speed, movementForce); //Add force in the direction it is facing
			}
			
			if (Input.GetAxis (controllerTypeVertical) < 0.1f) { // Probably should only use forward for this and have back be a kind of breaking system

				rb.velocity = Vector3.Lerp (rb.velocity, Vector3.zero, Time.fixedDeltaTime);
			}
				
			if (Mathf.Abs (Input.GetAxis (controllerTypeHorizontal)) > 0) { //This shoould rotate the player rather than move sideways
				rb.AddTorque (transform.up * Input.GetAxis (controllerTypeHorizontal) * turningSpeed, rotationForce);

			}
		}
		
		void HorizontalMass ()
		{
			if (Input.GetAxisRaw (controllerTypeHorizontal) < 0) {
				leftsphere.mass = lsphereMass + forceAppliedToTilt;
			} else if (Input.GetAxisRaw (controllerTypeHorizontal) > 0) {
				rightsphere.mass = rsphereMass + forceAppliedToTilt;
			} else if (Input.GetAxisRaw (controllerTypeHorizontal) == 0) {
				leftsphere.mass = lsphereMass;
				rightsphere.mass = rsphereMass;
			}
		}
		
		void VerticalMass ()
		{
			if (Input.GetAxisRaw (controllerTypeVertical) > 0) {
				frontsphere.mass = fsphereMass + forceAppliedToTilt;
				handle.mass = handleMass + forceAppliedToTilt / 2;
			} else if (Input.GetAxisRaw (controllerTypeVertical) < 0) {
				backsphere.mass = bsphereMass + forceAppliedToTilt * 2;
			} else if (Input.GetAxisRaw (controllerTypeVertical) == 0) {
				frontsphere.mass = fsphereMass;
				backsphere.mass = bsphereMass;
				handle.mass = handleMass;
			}
		}
		
		void TheDescent ()
		{
			if (hit.collider.gameObject.tag == "Terrain" && hit.distance < 5) {
				upForce.upwardsforce = Mathf.Lerp (upForce.upwardsforce, 44, Time.deltaTime);
				upForce.enabled = true;

			} else {
				upForce.upwardsforce = Mathf.Lerp (upForce.upwardsforce, 34, Time.deltaTime);
				
				if (Input.GetButtonDown ("DropFromSky")) {
					upForce.enabled = !upForce.enabled;
				}

			}
		}
	}
}

