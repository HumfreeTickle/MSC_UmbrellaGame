using UnityEngine;
using System.Collections;

namespace Environment
{
	public class RotationBlades : MonoBehaviour
	{
		private bool rotation;//this is the blades turning
		public GameObject landHere;//the halo pointing where to interact with the windmill
		public float blowforce;//the force that will be applied to the blowback from the windmill
		private bool turning;
	
		void Update ()
		{
			onRotation ();
		}

		void onRotation ()
		{
			if (rotation) {
				transform.Rotate (0, 5 * Time.deltaTime, 0);//the direction and speed at which the windmill will move
			}
		}

		void OnTriggerStay (Collider other)
		{
			if (other.gameObject.tag == "Player") {//if the umbrella interacts with the windmill
				if (Input.GetButtonDown ("Interact")) {
					rotation = true;//turn on the windmill
					Destroy (landHere);//get rid of the halo
					other.GetComponent<Rigidbody> ().AddForce (other.transform.forward * -1 * blowforce);//blow back the umbrella
				}
			}
		}
	}
}