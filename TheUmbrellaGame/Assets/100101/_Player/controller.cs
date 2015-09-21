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

		public Rigidbody handle;
//	------------------------------------
		private Animator umbrellaAnim;
		private Animator rotationAnim;

//	------------------------------------
		private Rigidbody rb;
//	------------------------------------
		public float speed;
		public float turningSpeed;
		public float slowDownSpeed = 1.2f;
		private float defaultUpForce;
//	------------------------------------
		private string controllerTypeVertical;
		private string controllerTypeHorizontal;

//  ------------------------------------
		private RaycastHit hit;
		private upwardForce upForce;
		private bool rotate;

		void Start ()
		{
			rb = GetComponent<Rigidbody> ();

			upForce = GetComponent<upwardForce> ();
			controllerTypeVertical = gameManager.ControllerTypeVertical;
			controllerTypeHorizontal = gameManager.ControllerTypesHorizontal;
			umbrellaAnim = GameObject.Find ("Umbrella").GetComponent<Animator> ();
			rotationAnim = GameObject.Find ("Rotation_Sphere").GetComponent<Animator> ();
			defaultUpForce = upForce.upwardsforce;
			GetComponent<CapsuleCollider>().radius = 0.5f;


			if (!upForce.isActiveAndEnabled) {
				upForce.enabled = true;
			}
		}
		
		void FixedUpdate ()
		{

			if (gameManager.gameState == GameState.Game) {
				handle.GetComponent<CapsuleCollider>().enabled = true;
				rb.useGravity = true;
				Movement ();
				hit = GetComponent<CreateWind> ().RaycastingInfo;
				if (hit.collider != null) {
					TheDescent ();
				} else {
					Physics.gravity = new Vector3 (0, -18.36f, 0);
					rb.mass = 1;
					umbrellaAnim.SetBool ("Falling", false);
					GetComponent<CapsuleCollider>().radius = 0.5f;	
				}

			} else if (gameManager.gameState == GameState.GameOver) {
				GetComponent<upwardForce> ().enabled = false;
				Physics.gravity = new Vector3 (0, -50.0f, 0);
				rb.mass = 10000;
				umbrellaAnim.SetBool ("Falling", true);
				rotate = false;

			} else if (gameManager.gameState == GameState.Event) {
				Stabilize ();
			}
		}
		
		//----------------------------- OTHER FUNCTIONS ------------------------------------------------------------------------
		
		void Movement ()
		{
			rotationAnim.SetBool ("Input_V", rotate);

			if (Input.GetAxis (controllerTypeVertical) > 0.1f) { // Probably should only use forward for this and have back be a kind of breaking system
				rb.AddForce (transform.TransformDirection (Vector3.forward) * Input.GetAxis (controllerTypeVertical) * speed); //Add force in the direction it is facing
				rotate = true;

			} else {
				rotationAnim.SetFloat("Speed", rb.velocity.magnitude);
				rotate = false;
			}
			
			if (Input.GetAxis (controllerTypeVertical) < 0.1f) { // Probably should only use forward for this and have back be a kind of breaking system

				rb.velocity = Vector3.Lerp (rb.velocity, Vector3.zero, Time.fixedDeltaTime);
			}
				
			if (Mathf.Abs (Input.GetAxis (controllerTypeHorizontal)) > 0) { //This shoould rotate the player rather than move sideways
				rb.AddTorque (transform.up * Input.GetAxis (controllerTypeHorizontal) * turningSpeed);
			}

			if (!Input.anyKeyDown) {
				rb.velocity = Vector3.Lerp (rb.velocity, Vector3.zero, Time.fixedDeltaTime * slowDownSpeed);
				rb.angularVelocity = Vector3.Lerp (rb.angularVelocity, Vector3.zero, Time.fixedDeltaTime * 10);
			}
		}
		
		void TheDescent () //allow the umbrella to go down
		{
			if (hit.collider.gameObject.tag == "Terrain" && hit.distance < 3) { // prevents the palyer from getting caught in the ground
				upForce.upwardsforce = Mathf.Lerp (upForce.upwardsforce, defaultUpForce * 1.25f, Time.deltaTime * 5);
				upForce.enabled = true;

			} else {
				// ------------ Standard on/off for the descent ---------------
				if (Input.GetAxis ("Vertical_R") <= -0.9f) {
					upForce.enabled = false;
				}else if(Input.GetAxis ("Vertical_R") <= -0.01f){
					upForce.upwardsforce = Mathf.Lerp (upForce.upwardsforce, 0, Time.deltaTime);
				}
				// ------------ brings the umbrella back to equilibrium ---------------
				else {
					upForce.enabled = true;

					upForce.upwardsforce = Mathf.Lerp (upForce.upwardsforce, defaultUpForce, Time.deltaTime);
				}

			}

			if (!upForce.isActiveAndEnabled) {
				Physics.gravity = new Vector3 (0, -50.0f, 0);
				rb.mass = 10000;
				umbrellaAnim.SetBool ("Falling", true);
				GetComponent<CapsuleCollider>().radius = 0.25f;

			} else {
				Physics.gravity = new Vector3 (0, -18.36f, 0);
				rb.mass = 1;
				umbrellaAnim.SetBool ("Falling", false);
				GetComponent<CapsuleCollider>().radius = 0.5f;
			}
		}

		//stops the umbrella from drifting away when she's chatting
		void Stabilize ()
		{
			rb.velocity = Vector3.Lerp (rb.velocity, Vector3.zero, Time.fixedDeltaTime * 10);
			upForce.upwardsforce = Mathf.Lerp (upForce.upwardsforce, defaultUpForce, Time.deltaTime);
			upForce.enabled = true;
		}
	}
}

