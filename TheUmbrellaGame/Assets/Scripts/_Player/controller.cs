using UnityEngine;
using System.Collections;
using Player.PhysicsStuff;
using Inheritence;
using CameraScripts;
using UnityEngine.Audio;

namespace Player
{
	public class controller : MonoBehaviour
	{
		public DestroyObject destroyStuff = new Inheritence.DestroyObject ();
		private GmaeManage gameManager;
		private Controller cameraController;
//	------------------------------------

		private GameObject handle;
//	------------------------------------
		private Animator umbrellaAnim;
		private Animator rotationAnim;

//	------------------------------------
		private Rigidbody rb;
//	------------------------------------
		public float speed;
		public float turningSpeed;
		public float slowDownSpeed = 1.2f;
		public float physicsClamp = 100f;
		private float defaultUpForce;
//	------------------------------------
		private string controllerTypeVertical;
		private string controllerTypeHorizontal;

//  ------------------------------------
		private upwardForce upForce;
		private bool rotate;
		public float gravityDrop = 50f;
		private bool tooLow;
		public GameObject leftside;
		public GameObject rightside;
		private Color transparentColorStart = Color.white;
		private Color transparentColorEnd = new Color (1, 1, 1, 0.5f);
		public float distanceFromTerrain;
		private bool playSFX;

		void Start ()
		{
			rb = GetComponent<Rigidbody> ();
			handle = GameObject.Find ("handle");
			gameManager = GameObject.Find ("Follow Camera").GetComponent<GmaeManage> ();
			cameraController = GameObject.Find ("Follow Camera").GetComponent<Controller> ();
			upForce = GetComponent<upwardForce> ();
			controllerTypeVertical = gameManager.controllerTypeVertical_L;
			controllerTypeHorizontal = gameManager.controllerTypeHorizontal_L;

			umbrellaAnim = GameObject.Find ("Umbrella").GetComponent<Animator> ();
			rotationAnim = GameObject.Find ("Rotation_Sphere").GetComponent<Animator> ();
			defaultUpForce = upForce.upwardsforce;
			GetComponent<CapsuleCollider> ().radius = 0.5f;

			if (!upForce.isActiveAndEnabled) {
				upForce.enabled = true;
			}

			if (gameManager.controllerType == ControllerType.ConsoleContoller) {
				speed = 50;
				turningSpeed = 70;
			} else if (gameManager.controllerType == ControllerType.Keyboard) {
				speed = 25;
				turningSpeed = 35;
			}
		}
		
		void FixedUpdate ()
		{
			ClampPhysics ();

			if (gameManager.gameState == GameState.Game) {
				handle.GetComponent<CapsuleCollider> ().enabled = true;
				rb.useGravity = true;
				rb.angularDrag = 5;
		
				Movement ();
				Descent ();


			} else if (gameManager.gameState == GameState.GameOver) {
				GetComponent<upwardForce> ().enabled = false;
				Physics.gravity = new Vector3 (0, -50.0f, 0);
				rb.mass = 10000;
				umbrellaAnim.SetBool ("Falling", true);
				rotate = false;


			} else if (gameManager.gameState == GameState.MissionEvent) {
				Stabilize ();
			}
		}
		
		//----------------------------- OTHER FUNCTIONS -------------------------------//
		
		void Movement ()
		{
			rotationAnim.SetBool ("Input_V", rotate);

			if (Input.GetAxis (controllerTypeVertical) > 0.1f) { // Probably should only use forward for this and have back be a kind of breaking system
				rb.AddForce (transform.TransformDirection (Vector3.forward) * Input.GetAxis (controllerTypeVertical) * speed); //Add force in the direction it is facing
				rotate = true;

			} else {
				rotationAnim.SetFloat ("Speed", rb.velocity.magnitude);
				rotate = false;
			}
			
			if (Input.GetAxis (controllerTypeVertical) < 0.1f) { // Probably should only use forward for this and have back be a kind of breaking system
				rb.velocity = Vector3.Lerp (rb.velocity, Vector3.zero, Time.fixedDeltaTime);

			}
				
			if (Mathf.Abs (Input.GetAxis (controllerTypeHorizontal)) > 0) { //This shoould rotate the player rather than move sideways
				rb.AddTorque (transform.up * Input.GetAxis (controllerTypeHorizontal) * turningSpeed);
				cameraController.lastHorizontalInput = Input.GetAxis (controllerTypeHorizontal);
			}

			if (!Input.anyKey) {
				rb.velocity = Vector3.Lerp (rb.velocity, Vector3.zero, Time.fixedDeltaTime * slowDownSpeed);
				rb.angularVelocity = Vector3.Lerp (rb.angularVelocity, Vector3.zero, Time.fixedDeltaTime * slowDownSpeed);
				if (gameManager.controllerType == ControllerType.Keyboard) {
					Input.ResetInputAxes ();
				}
			}
		}
		
		void Descent ()
		{
			if (gameManager.controllerType == ControllerType.ConsoleContoller) {
				if (Input.GetAxis (gameManager.controllerTypeVertical_R) <= -0.9f) {
					upForce.enabled = false;
					//-----------------------//
					leftside.GetComponent<LineRenderer> ().enabled = true;
					rightside.GetComponent<LineRenderer> ().enabled = true;
					//-----------------------//

				} else if (Input.GetAxis (gameManager.controllerTypeVertical_R) <= -0.1f) {
					upForce.upwardsforce = Mathf.Lerp (upForce.upwardsforce, 0, Time.deltaTime);
				} else { //Ceiling stuff
					if (!GetComponent<CreateWind> ().tooHigh) {
						upForce.enabled = true;
						upForce.upwardsforce = Mathf.Lerp (upForce.upwardsforce, defaultUpForce, Time.deltaTime);
					}
				}

			} else if (gameManager.controllerType == ControllerType.Keyboard) {
				if (Input.GetButton (gameManager.controllerTypeVertical_R) 
					&& Input.GetAxisRaw (gameManager.controllerTypeVertical_R) < -0.1f) {
					upForce.enabled = false;

					//-----------------------//
					leftside.GetComponent<LineRenderer> ().enabled = true;
					rightside.GetComponent<LineRenderer> ().enabled = true;
					//-----------------------//
					
				} else if (Input.GetButtonUp (gameManager.controllerTypeVertical_R)) {
					upForce.upwardsforce = Mathf.Lerp (upForce.upwardsforce, 0, Time.deltaTime);
				} else { //Ceiling stuff
					if (!GetComponent<CreateWind> ().tooHigh) {
						upForce.enabled = true;
						upForce.upwardsforce = Mathf.Lerp (upForce.upwardsforce, defaultUpForce, Time.deltaTime);
					}
				}

			}

			//------------------------------------------------------//
			if (!upForce.isActiveAndEnabled) {
				Physics.gravity = new Vector3 (0, -gravityDrop, 0);
				rb.mass = 10000;
				umbrellaAnim.SetBool ("Falling", true);
				rotationAnim.SetBool ("Input_H", true);
				GetComponent<CapsuleCollider> ().radius = Mathf.Lerp (GetComponent<CapsuleCollider> ().radius, 0.25f, Time.fixedDeltaTime * 2);

				transparentColorStart = Color.white;
				transparentColorEnd = new Color (1, 1, 1, 0.5f);
				leftside.GetComponent<LineRenderer> ().SetColors (transparentColorStart, transparentColorEnd);
				rightside.GetComponent<LineRenderer> ().SetColors (transparentColorStart, transparentColorEnd);

				if (playSFX) {
					playSFX = false;
					leftside.GetComponent<AudioSource> ().PlayOneShot (leftside.GetComponent<AudioSource> ().clip);
				}
			} else {
				Physics.gravity = new Vector3 (0, -18.36f, 0);
				rb.mass = 1;
				umbrellaAnim.SetBool ("Falling", false);
				rotationAnim.SetBool ("Input_H", false);
				
				GetComponent<CapsuleCollider> ().radius = Mathf.Lerp (GetComponent<CapsuleCollider> ().radius, 0.5f, Time.fixedDeltaTime * 2);

				transparentColorStart = Color.Lerp (transparentColorStart, new Color (1, 1, 1, 0), Time.deltaTime * 5);
				transparentColorEnd = Color.Lerp (transparentColorEnd, new Color (1, 1, 1, 0), Time.deltaTime * 5);
				leftside.GetComponent<LineRenderer> ().SetColors (transparentColorStart, transparentColorEnd);
				rightside.GetComponent<LineRenderer> ().SetColors (transparentColorStart, transparentColorEnd);

				if (transparentColorStart.a < 0.1f) {
					leftside.GetComponent<LineRenderer> ().enabled = false;
					rightside.GetComponent<LineRenderer> ().enabled = false;
				}
			}
		}

		void Stabilize ()
		{
			if (Vector3.Distance (rb.velocity, Vector3.zero) < 5) {
				rb.velocity = Vector3.zero;
			} else {
				rb.velocity = Vector3.Lerp (rb.velocity, Vector3.zero, Time.fixedDeltaTime * 10);
			}
			rb.angularDrag = Mathf.Lerp (rb.angularDrag, 100, Time.fixedDeltaTime * 10);
			rb.useGravity = false;
			umbrellaAnim.SetBool ("Falling", false);
			rotationAnim.SetBool ("Input_V", false);
			rotationAnim.SetBool ("Input_H", false);
			rotationAnim.SetFloat ("Speed", 0);
			Physics.gravity = new Vector3 (0, -18.36f, 0);

			upForce.upwardsforce = Mathf.Lerp (upForce.upwardsforce, defaultUpForce, Time.deltaTime);
			upForce.enabled = false;
		}

		void ClampPhysics ()
		{
			rb.velocity = Vector3.ClampMagnitude (rb.velocity, physicsClamp);
			if (rb.velocity.magnitude > physicsClamp) {
				Debug.LogError ("Velocity exceeded");
			}
		}
	}
}

