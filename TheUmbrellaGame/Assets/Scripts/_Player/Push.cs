using UnityEngine;
using System.Collections;

namespace Player
{
	public class Push : MonoBehaviour
	{

		public float Speed;
	
//--------------------------------------------------- Hasn't been touched in awhile ------------------------------------------
	
		void Update ()
		{
			if (Input.GetButtonDown ("Horizontal")) {
				//	Engine.PlayOneShot(Tank, 1);
			}
			transform.Translate (Vector3.right * Speed * Time.deltaTime * Input.GetAxis ("Horizontal"));//every frame the character move
	
		}

//		void OnCollisionStay (Collision collision)
//		{
////			if (collision.rigidbody)
////				collision.rigidbody.AddForce (Vector3.right * 50);
//		
//		}
	}
}
