using UnityEngine;
using System.Collections;

namespace CameraScripts
{
	public class Controller : MonoBehaviour
	{
		private GmaeManage GameManager;
		private GameState gameState;
		private Camera camrea;
		private float newCameraFOV;

		//----------------- UmbrellaStuff ---------------//
		public GameObject lookAt;
		private Transform lookAtTr;

		/// <summary>
		/// The transform of what ever you want the camera to be pointed at
		/// </summary>
		/// <value>The look at transform.</value>
		public Transform lookAtTransform {
			get {
				return lookAtTr;
			}

			set {
				lookAtTr = value;
			}
		}

		public bool playTime;

		/// <summary>
		/// Used when you need to lerp between two camera points and need a state change when it has arrived
		/// </summary>
		/// <value><c>true</c> if play time; otherwise, <c>false</c>.</value>
		public bool PlayTime {
			get {
				return playTime;
			}
		}

		public float threshold;
		private Rigidbody lookAtRb;
		//-----------------------------------------------//

		public float speed;
		public float rotateSpeed;
		private float xAway = -10;
		private float yAway = 20;
		private float zAway = -3;
		private GameObject whatAmIHitting;

		// The distance in the x-z plane to the target
		public float distance = 10.0f;
		public float side = 2f;
		// the height we want the camera to be above the target
		public float height = 5.0f;
		public float maxHeight = 10f;

		// How much we want to damp the movement
		public float heightDamping = 2.0f;
		public float rotationDamping = 3.0f;
		public Material transparent;
		public Material backupMaterial;
		private Tutuorial tutorialObject;
		private bool moveYerself = true;
		public bool MoveYerself{
			get{
				return moveYerself;
			}

			set{
				moveYerself = value;
			}
		}

		private float overSteer = 0f;
		public float maxOverSteer = 10;
		private float lastHorizontalInput;

		public float LastHorizontalInput {
			set {
				lastHorizontalInput = value;
			}
		}

		void Start ()
		{
			GameManager = GetComponent<GmaeManage> ();
			gameState = GameManager.gameState;
			camrea = GetComponent<Camera> ();

			lookAt = GameObject.Find ("main_Sphere");
			lookAtTr = lookAt.transform;
			lookAtRb = lookAt.GetComponent<Rigidbody> ();
			height = 3f;
			distance = 2.5f;
		}
		
		void Update ()
		{
			gameState = GameManager.gameState;
			RayCastView ();

			if (gameState == GameState.Pause) {
				if (GameManager.controllerType != ControllerType.Keyboard) {
					RotateYaw ();
					RotatePitch ();
				}
			}

			if (gameState == GameState.MissionEvent) {
				lookAtTr = lookAt.transform;
			}
		}
		//-------------------------------------------- Other Function Calls -------------------------------------------------------//
		
		void FixedUpdate ()
		{
			if (!lookAt) {
				return;
			}
			if (gameState == GameState.Intro) {
				if (Vector3.Distance (transform.position, lookAtTr.position) > 15) {
					Input.ResetInputAxes ();
				}

				height = 3f;
				distance = 2.5f;

			} else {
				height = maxHeight;
				distance = 2.8f;
			}


			if (gameState != GameState.GameOver) {

				//					 Calculate gameState == GameState.Pausethe current rotation angles (only need quaternion for movement)
				float wantedRotationAngle = lookAtTr.eulerAngles.y;
					
				float wantedHeight = lookAtTr.position.y + height;
					
				float currentRotationAngle = transform.eulerAngles.y;
					
				float currentHeight = transform.position.y;

				if (gameState == GameState.Game || gameState == GameState.Intro) {

					// Damp the rotation around the y-axis
					currentRotationAngle = Mathf.LerpAngle (currentRotationAngle, wantedRotationAngle, rotationDamping * Time.fixedDeltaTime);
					
					// Damp the height
					currentHeight = Mathf.Lerp (currentHeight, wantedHeight, heightDamping * Time.fixedDeltaTime);
					
					// Convert the angle into a euler axis rotation using Quaternions
					Quaternion currentRotation = Quaternion.Euler (0, currentRotationAngle + overSteer, 0);
					
					// Set the position of the camera on the x-z plane behind the target

					if (lookAtRb.velocity.magnitude > 10) {
						rotationDamping = 5;
						overSteer = Mathf.Lerp (overSteer, maxOverSteer * Mathf.Round (lastHorizontalInput), Time.fixedDeltaTime);
						maxHeight = Mathf.Lerp (maxHeight, 5, Time.fixedDeltaTime);

						newCameraFOV = camrea.fieldOfView + (lookAtRb.velocity.magnitude * Time.fixedDeltaTime);
						camrea.fieldOfView = Mathf.Lerp (camrea.fieldOfView, Mathf.Clamp (newCameraFOV, 60, 90), Time.fixedDeltaTime * speed);
					} else {
						if (Quaternion.Angle (transform.rotation, currentRotation) < 35) {
							overSteer = Mathf.Lerp (overSteer, 0, Time.fixedDeltaTime);
							rotationDamping = 10;

						}
						maxHeight = Mathf.Lerp (maxHeight, 10, Time.fixedDeltaTime);
						camrea.fieldOfView = Mathf.Lerp (camrea.fieldOfView, 60, Time.fixedDeltaTime);
					}
					
					transform.position = Vector3.Lerp (transform.position, lookAtTr.position, Time.fixedDeltaTime * speed);
					transform.position -= currentRotation * Vector3.forward * distance;
					
					// Set the height of the camera
					transform.position = new Vector3 (transform.position.x, currentHeight, transform.position.z);
					
					// Set the LookAt property to remain fixed on the target
					transform.LookAt (lookAtTr);


				} else if (gameState == GameState.MissionEvent) {

					camrea.fieldOfView = Mathf.Lerp (camrea.fieldOfView, 60, Time.fixedDeltaTime);

					// Damp the height
					currentHeight = Mathf.Lerp (currentHeight, wantedHeight, Time.fixedDeltaTime);

					// Set the height of the camera
					if (moveYerself) {
						transform.position = new Vector3 (transform.position.x, currentHeight, transform.position.z);
					}

					lookAtTr = lookAt.transform;

					Quaternion rotation = Quaternion.LookRotation (lookAtTr.position - transform.position);
					transform.rotation = Quaternion.Slerp (transform.rotation, rotation, Time.deltaTime * (speed / 10));

					if (Quaternion.Angle (transform.rotation, rotation) < threshold) {
						playTime = true;
					} else {
						playTime = false;
					}

				}
			}
				//-------------------------------------------- Camera Changes on Death -------------------------------------------------------//
				
				else if (gameState == GameState.GameOver) {

				if (GameManager.Timer > 2) {
					transform.position = transform.position + new Vector3 (xAway, yAway, zAway);
					transform.LookAt (lookAtTr);
				} else {
					transform.LookAt (lookAtTr);
				}

			}
		}
		
		//-------------------------------------------- Right Analouge Stick Stuff -------------------------------------------------------//
		
		void RotateYaw ()
		{
			if (Mathf.Abs (Input.GetAxis ("Horizontal_R")) > 0) {
				transform.RotateAround (lookAtTr.position, Vector3.up, Input.GetAxis ("Horizontal_R") * rotateSpeed); 
			}
		}
		
		void RotatePitch ()
		{
			if (Mathf.Abs (Input.GetAxis ("Vertical_R")) > 0) {
				transform.RotateAround (lookAtTr.position, transform.TransformDirection (Vector3.right), -1 * Input.GetAxis ("Vertical_R") * rotateSpeed); 
			}
		}
		
		
		//-------------------------------------------- Stops Blocked View -------------------------------------------------------//

		/// <summary>
		/// When the camera goes behind an object this replaces the material with a transparent version
		/// Doesn't really work. Especially since alot of stuff is broken up into tiny pieces
		/// :(
		/// </summary>
		void RayCastView ()
		{
			RaycastHit hit;
			Vector3 screenPos = camrea.WorldToScreenPoint (lookAtTr.position);
			Ray cameraRay = camrea.ScreenPointToRay (screenPos);
			Vector3 cameraDir = cameraRay.direction * 100;
			Debug.DrawRay (cameraRay.origin, cameraDir, Color.blue);
			LayerMask pickups = 12;

			
			if (Physics.Raycast (cameraRay, out hit, pickups)) {

				if (gameState != GameState.MissionEvent) {
					if (hit.collider.tag != "Player") {//hits anything other then the player

						if (hit.collider.gameObject.GetComponent<MeshRenderer> ()) {// checks to see if it has a mesh renderer
							if (whatAmIHitting != null) { //

								// ----- Checks to if what is being hit is a different object ----//
								if (whatAmIHitting != hit.collider.gameObject) {
									whatAmIHitting.GetComponent<MeshRenderer> ().material = backupMaterial; // changes the old object back
									whatAmIHitting = hit.collider.gameObject; // assigns the new object

									//--- stores the material that was on the object for use later -------
									backupMaterial = whatAmIHitting.GetComponent<MeshRenderer> ().sharedMaterial;

									//--- changes objects material to a transparent texture -------------
									whatAmIHitting.GetComponent<MeshRenderer> ().sharedMaterial = transparent;
								}

							} else {
								whatAmIHitting = hit.collider.gameObject;
								backupMaterial = whatAmIHitting.GetComponent<MeshRenderer> ().sharedMaterial;
								whatAmIHitting.GetComponent<MeshRenderer> ().sharedMaterial = transparent;
//								if (whatAmIHitting.gameObject.transform.childCount > 0) {
//
//									for (int i = 0; i < whatAmIHitting.gameObject.transform.childCount; i++) {
//										if (whatAmIHitting.gameObject.transform.GetChild (i).GetComponent<MeshRenderer> ()) {
//											whatAmIHitting.gameObject.transform.GetChild (i).GetComponent<MeshRenderer> ().enabled = false;
//										}
//									}
////								foreach (MeshRenderer child in whatAmIHittingChildren) {
////									child.enabled = false;
////								}
//								}
							}
						}

					} else if (hit.collider.tag == "Player") {
						// ----- Checks to see if its empty ----//
						if (whatAmIHitting != null) {
							//--- returns object to original state -----
							whatAmIHitting.GetComponent<MeshRenderer> ().material = backupMaterial;
							if (whatAmIHitting.gameObject.transform.childCount > 0) {
								for (int i = 0; i < whatAmIHitting.gameObject.transform.childCount; i++) {
									if (whatAmIHitting.gameObject.transform.GetChild (i).GetComponent<MeshRenderer> ()) {
										whatAmIHitting.gameObject.transform.GetChild (i).GetComponent<MeshRenderer> ().enabled = true;
									}
								}
								backupMaterial = null;
								whatAmIHitting = null;
							}
						}
					}
				}
			}
		}
	}
}