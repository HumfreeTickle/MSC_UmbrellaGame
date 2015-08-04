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
		public GameObject umbrella;
		private Transform umbrellaTr;
		public Transform umbrellaTrCanopy;
		private Rigidbody umbrellaRb;
		public float speed;
		public float rotateSpeed;
		public float xAway;
		public float yAway;
		public float zAway;
		private GameObject whatAmIHitting;


		// The distance in the x-z plane to the target
		public float distance = 10.0f;
		public float side = 2f;
		// the height we want the camera to be above the target
		public float height = 5.0f;
		// How much we want to damp the movement
		public float heightDamping = 2.0f;
		public float rotationDamping = 3.0f;
//		private Color transparent = new Color (1, 1, 1, 0);
		
		void Start ()
		{
			GameManager = GetComponent<GmaeManage> ();
			gameState = GameManager.gameState;
			camrea = GetComponent<Camera> ();

			umbrellaTr = umbrella.transform;
			umbrellaRb = umbrella.GetComponent<Rigidbody> ();
		}
		
		void Update ()
		{
			gameState = GameManager.gameState;

			if (gameState == GameState.Pause || gameState == GameState.Idle) {
				if (GameManager.controllerType != ControllerType.Keyboard) {
					RotateYaw ();
					RotatePitch ();
				}
			} 

			RayCastView ();
			
			//-------------------------------------------- Other Function Calls -------------------------------------------------------//
			
		}
		
		void FixedUpdate ()
		{
			if (!umbrella) {
				return;
			}
			
			if (gameState != GameState.GameOver) {
				
				if (gameState == GameState.Game || gameState == GameState.Intro) {
					
					// Calculate gameState == GameState.Pausethe current rotation angles (only need quaternion for movement)
					float wantedRotationAngle = umbrellaTr.eulerAngles.y;
					
					float wantedHeight = umbrellaTr.position.y + height;
					
					float currentRotationAngle = transform.eulerAngles.y;
					
					float currentHeight = transform.position.y;
					
					// Damp the rotation around the y-axis
					currentRotationAngle = Mathf.LerpAngle (currentRotationAngle, wantedRotationAngle, rotationDamping * Time.fixedDeltaTime);
					
					// Damp the height
					currentHeight = Mathf.Lerp (currentHeight, wantedHeight, heightDamping * Time.fixedDeltaTime);
					
					// Convert the angle into a euler axis rotation using Quaternions
					Quaternion currentRotation = Quaternion.Euler (0, currentRotationAngle, 0);
					
					// Set the position of the camera on the x-z plane behind the target
					
					if (umbrellaRb.velocity.magnitude > 10) {
						newCameraFOV = camrea.fieldOfView + (umbrellaRb.velocity.magnitude * Time.fixedDeltaTime);
						camrea.fieldOfView = Mathf.Lerp (camrea.fieldOfView, Mathf.Clamp (newCameraFOV, 60, 80), Time.fixedDeltaTime * speed);
					} 
					
					transform.position = Vector3.Lerp (transform.position, umbrellaTr.position, Time.fixedDeltaTime * speed);
					transform.position -= currentRotation * Vector3.forward * distance;
					
					// Set the height of the camera
					transform.position = new Vector3 (transform.position.x, currentHeight, transform.position.z);
					
					// Set the LookAt property to remain fixed on the target
					transform.LookAt (umbrellaTr);
				}
			}
				//-------------------------------------------- Camera Changes on Death -------------------------------------------------------//
				
				else if (gameState == GameState.GameOver) {

				if (GameManager.Timer > 2) {
					transform.position = transform.position + new Vector3 (xAway, yAway, zAway);
				} else {
					transform.LookAt (umbrellaTr);
				}
			}
		}
		
		//-------------------------------------------- Right Analouge Stick Stuff -------------------------------------------------------//
		
		void RotateYaw ()
		{
			if (Mathf.Abs (Input.GetAxis ("Horizontal_R")) > 0) {
				transform.RotateAround (umbrellaTr.position, Vector3.up, Input.GetAxis ("Horizontal_R") * rotateSpeed); 
			}
		}
		
		void RotatePitch ()
		{
			if (Mathf.Abs (Input.GetAxis ("Vertical_R")) > 0) {
				transform.RotateAround (umbrellaTr.position, transform.TransformDirection (Vector3.right), -1 * Input.GetAxis ("Vertical_R") * rotateSpeed); 
			}
		}
		
		
		//-------------------------------------------- Stops Blocked View -------------------------------------------------------//

		void RayCastView ()
		{
			RaycastHit hit;
			Vector3  screenPos = camrea.WorldToScreenPoint(umbrellaTr.position);
			Ray cameraRay = camrea.ScreenPointToRay (screenPos);

			Debug.DrawRay(cameraRay.origin, cameraRay.direction, Color.blue);

			if (Physics.Raycast (cameraRay, out hit)) {
				if (hit.collider.tag != "Player") {

					if (hit.collider.gameObject.GetComponent<MeshRenderer> ()) {
						if (whatAmIHitting != null) {
							whatAmIHitting.GetComponent<MeshRenderer> ().enabled = false;
					
							// ----- Checks to if what is being hit is a different object ----//
							if (whatAmIHitting != hit.collider.gameObject) {

								whatAmIHitting = hit.collider.gameObject;
								print (whatAmIHitting);
							}
						} else {
							whatAmIHitting = hit.collider.gameObject;
						}
					}
				}else if(hit.collider.tag == "Player") {
					// ----- Checks to see if its empty ----//
					if (whatAmIHitting != null) {
						whatAmIHitting.GetComponent<MeshRenderer> ().enabled = true;
					}
				}
			}
		}
	}
}

