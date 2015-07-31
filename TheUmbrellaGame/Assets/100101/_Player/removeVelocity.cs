using UnityEngine;
using System.Collections;

namespace Player
{
	public class removeVelocity : MonoBehaviour
	{

		public float slowDownSpeed; //the rate at which velocity is removed
		private Rigidbody rb;

		void Start ()
		{
			rb = GetComponent<Rigidbody> ();
		}

//----------------------------------------- Allows the Umbrella to come to a gentle stop ------------------------------------------

		void FixedUpdate ()
		{
			if (!Input.anyKeyDown) {
				rb.velocity = Vector3.Lerp (rb.velocity, Vector3.zero, Time.fixedDeltaTime * slowDownSpeed);
				rb.angularVelocity = Vector3.Lerp (rb.angularVelocity, Vector3.zero, Time.fixedDeltaTime * 10);
			}
		}
	}
}
