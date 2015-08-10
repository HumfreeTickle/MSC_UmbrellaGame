using UnityEngine;
using System.Collections;

namespace Environment
{
	public class RotationBlades : MonoBehaviour
	{
		bool rotation = false;//this is the blades turning
		public GameObject landHere;//the halo pointing where to interact with the windmill
		public GameObject brolly;//the umbrella
		public float blowforce;//the force that will be applied to the blowback from the windmill
		public Vector3 direction;//the direction the umbrella will be pushed in
		public bool turning;
		public float timer;
	
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
					other.GetComponent<Rigidbody> ().AddForce (direction * blowforce);//blow back the umbrella
				}
			}
		}
	}
}