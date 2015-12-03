using UnityEngine;
using System.Collections;

namespace Player.PhysicsStuff
{
	public class flingObject : MonoBehaviour
	{

		public GameObject player;//this is the object that will be picked up/
		public float power;
		public Vector3 way;
		public bool isAttached;
		private Joint theJoint;
		public float radio = 5.0F;
		public float powerR = 10.0F;
		public GameObject BallFly;
		public float toBreak;
		public float LetGo;
		public bool JointGood;

//-------------------------------------- Sets up the connections -----------------------------------------------------------------

		void OnTriggerEnter (Collider other)
		{
	
			if (other.tag == "Throw") {

				gameObject.AddComponent<FixedJoint> ();//create the fixed joint
				theJoint = gameObject.GetComponent<FixedJoint> ();//the fixed joint is called theJoint
				theJoint.connectedBody = player.GetComponent<Rigidbody> ();//this adds the object as the connecter on the fixed joint
				theJoint.enableCollision = false;//this will stop the 2 objects in the joint colliding any more, preventing more joints being created
				theJoint.enablePreprocessing = true;
				theJoint.breakForce = 3000000f;//needs to be a high value so the joint will attach and stay attached.
				theJoint.breakTorque = 3000000f;
				JointGood = true;

			}


		}

//-------------------------------------- Breaks the bond -----------------------------------------------------------------

		void FixedUpdate ()
		{

			if (Input.GetKeyUp (KeyCode.DownArrow)) {//when the down arrow is released.
				theJoint.breakTorque = 0.001f;//this lowers the breaktorque of the handle making the object break away.
				//theJoint.enableCollision = false;
				player.GetComponent<Rigidbody> ().mass = 1;//increases the mass of the object 
				//player.transform.position += Vector3.forward * 50;
				Debug.Log ("Less Force");
			}
		}

//-------------------------------------- During Flight -----------------------------------------------------------------

		void Update ()
		{
		
			if (theJoint == !isActiveAndEnabled) {//when the joint breaks
			
				player.GetComponent<Rigidbody> ().useGravity = true;//gravity will no be enabled on the object
				player.GetComponent<Rigidbody> ().AddForce (transform.forward * power);//the object will now constantly move forward.
//			player.transform.position += Vector3.forward * Time.deltaTime *5;
				isAttached = true;
				Destroy (theJoint);//the joint is destroyed
//			Debug.Log ("Broken");


			}
		}
//
//-------------------------------------- Possibly useful things but who knows -----------------------------------------------------------------

//			
//			theJoint.enableCollision = false;
//			theJoint.enablePreprocessing = false;
//			JointGood = true;

	}
//		
	//}
	//void LateUpdate(){


//		theJoint.connectedBody = player.GetComponent<Rigidbody>();
//		theJoint.enableCollision = false;
//		theJoint.enablePreprocessing = false;
//		JointGood = true;
//		theJoint.breakForce = 0.011f;
//		theJoint.breakTorque = 0.011f;
//
//}





	//			Debug.Log("AttachNow");
////			    isAttached = true;
//				player = other.gameObject;
//				other.transform.parent = transform;
//			    gameObject.AddComponent<HingeJoint>();
//				player.AddComponent<Rigidbody> ();


	

//}

////			player.AddComponent<Rigidbody> ();
//			player.GetComponent<Rigidbody>().AddExplosionForce(power, transform.position, radio, 5.0F);
////			player.GetComponent<Rigidbody>().useGravity = true;
//
//		}
	
}
