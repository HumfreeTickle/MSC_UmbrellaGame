using UnityEngine;
using System.Collections;

namespace Useless
{
	public class rotateCamera : MonoBehaviour
	{

//----------------------------------- Pretty Sure this all got moved to the controller script ------------------------------------------


		public Transform umbrella;
		public float speed;
		public float xAway;
		public float yAway;
		public float zAway;

		// Use this for initialization
		void Start ()
		{
			transform.position = umbrella.position - transform.TransformDirection (new Vector3 (xAway, yAway, zAway));
			transform.LookAt (umbrella);
		}
	
		// Update is called once per frame
		void Update ()
		{
			transform.position = umbrella.position - transform.TransformDirection (new Vector3 (xAway, yAway, zAway));//Vector3.Scale(umbrella.position, new Vector3(1, 0, 1)).normalized;
//		transform.RotateAround(umbrella.position, Vector3.up, speed); 
			RotateYaw ();
			RotatePitch ();
		}

		void RotateYaw ()
		{
			if (Mathf.Abs (Input.GetAxisRaw ("Horizontal_R")) > 0) {
//			print ("r");
				transform.RotateAround (umbrella.position, Vector3.up, Input.GetAxisRaw ("Horizontal_R") * speed); 
			}
		}

		void RotatePitch ()
		{
			if (Mathf.Abs (Input.GetAxisRaw ("Vertical_R")) > 0) {
				transform.RotateAround (umbrella.position, Vector3.right, Input.GetAxisRaw ("Vertical_R") * speed); 
			}
		}
	}
}