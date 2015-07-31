using UnityEngine;
using System.Collections;

namespace Player
{
	public class grabbing : MonoBehaviour
	{
		private Rigidbody umbrellaBody;
		public GameObject player;
		public GameObject Landing;
		public bool JumpKey;

		void Update ()
		{
			if (transform.childCount > 0) {

				if (Input.GetButtonDown ("Fire3")) {
					JumpKey = !JumpKey;
				}

				Detachment ();
			}
		}
		
		void Detachment ()
		{
			if (JumpKey == true) {
				transform.DetachChildren ();
				player.AddComponent<Rigidbody> ();
				Landing.SetActive (false);
			}
		}

		void OnTriggerEnter (Collider other)
		{
			if (other.tag == "Collectable") {
				player = other.gameObject;
				other.transform.parent = transform;
				Landing.SetActive (true);
				JumpKey = false;

			}
		}
	}
}