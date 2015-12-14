using UnityEngine;
using System.Collections;

namespace Environment
{
	//-- No longer used --//

	public class Barriers : MonoBehaviour
	{
		private float _timer;
		public float knockBack;
		public Vector3 directionOfForce;

//---------------------------- While the Umbrella is off the island ---------------------------------------//

		void OnTriggerStay (Collider barrier)
		{
			if (barrier.tag == "Player") {

				_timer += Time.deltaTime;
				barrier.GetComponent<Rigidbody> ().drag = Mathf.Lerp (barrier.GetComponent<Rigidbody> ().drag, 10, Time.fixedDeltaTime / 100);
				if (_timer > 3) {
					barrier.GetComponent<Rigidbody> ().AddForce (knockBack * directionOfForce);
				}
			}
		}

//---------------------------- When it's back -------------------------------------------//
	
		void OnTriggerExit (Collider barrier)
		{
			if (barrier.tag == "Player") {
				barrier.GetComponent<Rigidbody> ().drag = 0;
			}
		}
	}
}