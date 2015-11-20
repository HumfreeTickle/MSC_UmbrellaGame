using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

namespace Enivironment
{
	public class ChimneyBlow : MonoBehaviour
	{
		public float push;
		private float savedPush;
		private Animator umbrellaAnim;
		private AudioClip whoosh;
		private AudioSource audio2;


		void Start ()
		{
			savedPush = push;
			umbrellaAnim = GameObject.Find ("Umbrella").GetComponent<Animator> ();
<<<<<<< HEAD
			audio2 = GetComponent<AudioSource>();
			whoosh = audio2.clip;

=======
			if (GetComponent<AudioSource> ()) {
				audio2 = GetComponent<AudioSource> ();
				whoosh = audio2.clip;
			}
>>>>>>> origin/master
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

		void OnTriggerEnter (Collider other)
		{
			if (other.gameObject.tag == "Player") {
<<<<<<< HEAD

				audio2.PlayOneShot(whoosh);
				Debug.Log("Whoosh");
=======
				audio2.PlayOneShot (whoosh);
>>>>>>> origin/master
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