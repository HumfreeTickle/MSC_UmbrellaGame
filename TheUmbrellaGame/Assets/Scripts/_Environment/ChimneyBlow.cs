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
			if (GameObject.Find ("Umbrella")) {
				umbrellaAnim = GameObject.Find ("Umbrella").GetComponent<Animator> ();
			}

			audio2 = GetComponent<AudioSource> ();
			whoosh = audio2.clip;

//			if (GetComponent<AudioSource> ()) {
//				audio2 = GetComponent<AudioSource> ();
//				whoosh = audio2.clip;
//			}

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

<<<<<<< HEAD:TheUmbrellaGame/Assets/Scripts/_Environment/ChimneyBlow.cs
<<<<<<< HEAD:TheUmbrellaGame/Assets/100101/_Environment/ChimneyBlow.cs

				audio2.PlayOneShot (whoosh);
				Debug.Log ("Whoosh");

=======
=======
>>>>>>> origin/master:TheUmbrellaGame/Assets/Scripts/_Environment/ChimneyBlow.cs
				audio2.PlayOneShot (whoosh);
				Debug.Log ("Whoosh");
>>>>>>> origin/master:TheUmbrellaGame/Assets/Scripts/_Environment/ChimneyBlow.cs



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