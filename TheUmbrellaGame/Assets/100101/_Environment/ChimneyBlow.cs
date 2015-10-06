using UnityEngine;
using System.Collections;

namespace Enivironment
{
	public class ChimneyBlow : MonoBehaviour
	{
		public float push;
		private float savedPush;
		private Animator umbrellaAnim;

		void Start ()
		{
			savedPush = push;
			umbrellaAnim = GameObject.Find ("Umbrella").GetComponent<Animator> ();
		}

		void OnTriggerStay (Collider other)
		{
			if (other.gameObject.tag == "Player") {
				if (other.GetComponent<Rigidbody> ()) {
					other.GetComponent<Rigidbody> ().AddForce (transform.forward * push);
				}
				umbrellaAnim.SetBool ("Hit", true);

				push -= 1;
				push = Mathf.Clamp (push, 0, savedPush);
			}
		}

		void OnTriggerExit (Collider other)
		{
			if (other.gameObject.tag == "Player") {
				umbrellaAnim.SetBool ("Hit", false);

				push = savedPush;
				
			}
		}
	}
}