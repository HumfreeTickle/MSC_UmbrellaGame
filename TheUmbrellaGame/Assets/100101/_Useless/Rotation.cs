using UnityEngine;
using System.Collections;

///was a good try but really isn't needed anymore :(

namespace Useless
{
	public class Rotation : MonoBehaviour
	{

//----------------------------------- Nope nope nope nope noooooooooope ------------------------------------------


		public float rotationSpeed;
		public float speed;
		private float posX;
		private float posY;
		private float posZ;
		private bool rotated;
		private Vector3 defaultRotation;
		private Rigidbody umbrellaBody;
		public int switching = 1;
		private Quaternion rotateTo;
		private Vector3 directionOftravel = Vector3.forward;

		void Start ()
		{

			umbrellaBody = GetComponent<Rigidbody> ();
		}

		void Update ()
		{
//		print (Quaternion.Angle (transform.rotation, rotateTo));
//		print (transform.rotation);
//		print (transform.eulerAngles);
			whichDirection ();
			rotated = transform.eulerAngles.x > 330 ? true : false;
			//fallOutOfTheSky ();
			if (Mathf.Abs (umbrellaBody.velocity.x) > 0.1f || Mathf.Abs (umbrellaBody.velocity.z) > 0.1f) {
				correctRotation ();
			}

		}

//	void testRotation ()
//	{
//		if (Input.GetKey (KeyCode.LeftArrow)) {
//			print (Quaternion.Angle (transform.rotation, rotateTo));
//
//			//from position 1 to position 2
//
//
//			// this is the last rotation from position 4 to position 1
//			else if(Quaternion.Angle (transform.rotation, new Quaternion (0.7f, 0f, 0f, -0.7f)) >= 1) {
//				rotateTo = new Quaternion (0.7f, 0f, 0f, -0.7f);
//			}
//			//from position 3 to positon 4
//			else if (Quaternion.Angle (transform.rotation, new Quaternion (0.5f, -0.5f, -0.5f, -0.5f)) >= 1) {
//				rotateTo = new Quaternion (0.5f, -0.5f, -0.5f, -0.5f);
//			} 
//			// from position 2 to position 3
//			else if(Quaternion.Angle (transform.rotation, new Quaternion (0, -0.7f, 0.7f, 0)) >= 1) {
//				rotateTo = new Quaternion (0, -0.7f, 0.7f, 0);
//				}
//
//			
//		}
//	}

		//I think I'm gonna have to use the Quaternion values to check whether to change or not

		void rotateAround ()
		{
		 
			switch (switching) {

			case 1:
				rotateTo = new Quaternion (0.7f, 0, 0, -0.7f); //forward
				break;

			case 2:
				rotateTo = new Quaternion (0, 0, 0, -1); //Changes angle to foward tilt
				directionOftravel = Vector3.forward; //Moves positively along z axis
				break;
	

			case 3:
				rotateTo = new Quaternion (-0.5f, -0.5f, -0.5f, 0.5f); //left
				break;
			case 4:
				rotateTo = new Quaternion (0, -0.7f, 0, 0.7f); //Changes angle to forward/left tilt
				directionOftravel = -1 * Vector3.right; //Moves negatively along x axis
				break;


			case 5:
				rotateTo = new Quaternion (0, -0.7f, -0.7f, 0); //back
				break;
			case 6:
				rotateTo = new Quaternion (0, -1f, 0f, 0f); //Changes angle to backwards tilt 
				directionOftravel = -1 * Vector3.forward;  //Moves negatively along z axis
				break;


			case 7:
				rotateTo = new Quaternion (0.5f, -0.5f, -0.5f, -0.5f); //right
				break;
			case 8:
				rotateTo = new Quaternion (0, -0.7f, 0, -0.7f); //Changes anlge to forward/right tilt
				directionOftravel = Vector3.right; //Moves positiively along x axis
				break;

			default:
				rotateTo = new Quaternion (0.7f, 0, 0, -0.7f); //forward
				break;
			}


			transform.rotation = Quaternion.Slerp (transform.rotation, rotateTo, Time.deltaTime);//rotates umbrella


		}

		void whichDirection ()
		{
			if (Input.GetKeyDown (KeyCode.LeftArrow)) { //when left arrow is down
				//even numbers need to be there so when you press up and a direction it doesn't suddenly revert to an odd number
				print ("Left Arrow Down");
				print (transform.rotation);
				switch (switching) {
				case 1:
					switching = 3;
					break;
				case 2:
					switching = 4;
					break;
				case 3:
					switching = 5;
					break;
				case 4:
					switching = 6;
					break;
				case 5:
					switching = 7;
					break;
				case 6:
					switching = 8;
					break;
				case 7:
					switching = 1;
					break;
				default:
					switching = switching - 1;
					break;
				}
			}

			if (Input.GetKeyDown (KeyCode.RightArrow)) {
				print ("Right Arrow Down");
				switch (switching) {
				case 1:
					switching = 7;
					break;
				case 2:
					switching = 8;
					break;
				case 3:
					switching = 1;
					break;
				case 4:
					switching = 2;
					break;
				case 5:
					switching = 3;
					break;
				case 6:
					switching = 4;
					break;
				case 7:
					switching = 5;
					break;
				case 8:
					switching = 6;
					break;
				default:
					switching = switching - 1;
					break;
				}
			}
			// & switching % 2 == 0 & Input.GetAxis ("Vertical") == 0



//		if (Input.GetKey (KeyCode.UpArrow) & switching % 2 != 0 & Input.GetAxis ("Horizontal") != 0) {
//			switching += 1;
//		} //Cna't remember why this was needed seems to be arbitary

			if (Input.GetKeyDown (KeyCode.UpArrow)) {
				switch (switching) {
				case 1:
					switching = 2;
					break;
				case 3:
					switching = 4;
					break;
				case 5:
					switching = 6;
					break;
				case 7:
					switching = 8;
					break;
				default:
					switching = switching - 1;
					break;
				}
			}

			if (Input.GetKeyDown (KeyCode.LeftArrow) || Input.GetKeyDown (KeyCode.RightArrow)) {
				rotateAround ();	
			}

			if (Input.GetKey (KeyCode.UpArrow)) {
				rotateAround ();
				umbrellaBody.AddForce (directionOftravel * Time.deltaTime * speed);
			}

			transform.rotation = Quaternion.Slerp (transform.rotation, rotateTo, Time.deltaTime); //doesn't complete the full rotation, without it 
		}

		void fallOutOfTheSky ()
		{
			if (rotated) {
				umbrellaBody.useGravity = true;
			} 
			if (umbrellaBody.useGravity == true & !rotated) {
				umbrellaBody.velocity = Vector3.Lerp (umbrellaBody.velocity, new Vector3 (umbrellaBody.velocity.x, 0, umbrellaBody.velocity.z), Time.deltaTime);
				if (umbrellaBody.velocity.y <= 0.1) {
					umbrellaBody.useGravity = false;
				}
			}
		}

		void correctRotation ()
		{
			if (!Input.anyKey) {
				if (switching % 2 == 0) {
					switching -= 1;
				}
				rotateAround ();

				if (umbrellaBody.useGravity == false) {
					umbrellaBody.velocity = Vector3.Lerp (umbrellaBody.velocity, new Vector3 (0, 0, 0), Time.deltaTime);
				}
			}
		}
	}
}
