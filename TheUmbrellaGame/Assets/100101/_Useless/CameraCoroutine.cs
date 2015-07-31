using UnityEngine;
using System.Collections;

namespace Uesless
{
	public class CameraCoroutine : MonoBehaviour
	{
		private float smoothing = 1f; //speed at which the camera moves to the new location
		public float smoothSpeed;
		public Transform umbrella;
		public float waitTime;	//Time between each coroutine check 

		public float distance = 10;
		public float height = 1;
		private float umbrellaRotation; //Holds the umbrella's y axis rotation
		private float umbrellaHeight; //Holds the umbrella's y axis position

		private float currentRotationAngle; // current rotation along the y axis
		private float currentHeight; // current position along the y axis

		private Quaternion currentRotation;

		void Start ()
		{
			StartCoroutine (MovementCoroutine (umbrella));
		}
		
		IEnumerator MovementCoroutine (Transform target)
		{
			while (true) {
				//Sets all the variables
				umbrellaRotation = umbrella.eulerAngles.y;
				umbrellaHeight = umbrella.position.y;
				currentRotationAngle = transform.eulerAngles.y;
				currentHeight = transform.position.y;

				//Smoothly transitions between the camera and the umbrella
				currentRotationAngle = Mathf.LerpAngle (currentRotationAngle, umbrellaRotation + 2, (smoothing / 1.2f) * Time.deltaTime);
				currentHeight = Mathf.Lerp (currentHeight, umbrellaHeight + height, (smoothing / 2) * Time.deltaTime); // This is jst for rising and falling

				//Converts euler to Quaternion. Not sure how reliable this is
				currentRotation = Quaternion.Euler (0, currentRotationAngle, 0); 

				//Actually moves the camera into position
				transform.position = umbrella.position;
				transform.position -= currentRotation * Vector3.forward * distance;
				transform.position = new Vector3 (transform.position.x, currentHeight, transform.position.z);

				// Always look at the target
				transform.LookAt (target); // Handy little thing

				//Changes how smoothly movement is (Probably shouldn't go higher that 1
				smoothing = Mathf.Clamp (smoothSpeed, -1, 1);
//			print ("Reached the target. ");
				yield return new WaitForSeconds (waitTime);
			}
		}
	}
}

