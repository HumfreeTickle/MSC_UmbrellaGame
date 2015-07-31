using UnityEngine;
using System.Collections;

namespace Environment
{
	public class BlowAway : MonoBehaviour
	{

		public float blow;
		public Vector3 way;

		//-------------------------------------- Spins the windmill blade ---------------------------------------------

		void Update ()
		{
			transform.Rotate (new Vector3 (15, 30, 45) * Time.deltaTime * .25f); //
//		transform.position = new Vector3(Mathf.PingPong(Time.time, 3), transform.position.y, transform.position.z);
		}

//--------------------------------------- Blows Player Back -------------------------------------------------------

		void OnTriggerEnter (Collider other)
		{

			if (other.gameObject.tag == "Player") {
				other.GetComponent<Rigidbody> ().AddForce (blow * way);//blow back the umbrella
			}
		}
	}
}