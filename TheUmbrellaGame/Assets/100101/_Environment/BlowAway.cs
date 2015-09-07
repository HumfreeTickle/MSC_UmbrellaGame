using UnityEngine;
using System.Collections;

namespace Environment
{
	public class BlowAway : MonoBehaviour
	{
		public float blow;

		//-------------------------------------- Spins the windmill blade ---------------------------------------------

		void Update ()
		{
			transform.Rotate (new Vector3 (15, 30, 45) * Time.deltaTime * .25f); //
		}

//--------------------------------------- Blows Player Back -------------------------------------------------------

		void OnTriggerEnter (Collider other)
		{

			if (other.gameObject.tag == "Player") {
				other.GetComponent<Rigidbody> ().AddForce (blow * other.GetComponent<Rigidbody> ().velocity);//blow back the umbrella
			}
		}
	}
}