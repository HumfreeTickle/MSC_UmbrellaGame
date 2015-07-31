using UnityEngine;
using System.Collections;
using Inheritence;


namespace Player
{
	public class wind : MonoBehaviour
	{
		public DestroyObject destroyObject = new Inheritence.DestroyObject();

		public float windForce;
		public Transform umbrellaObject;

		void Awake ()
		{
			umbrellaObject = GameObject.Find ("Umbrella").transform;
		}

		void Update ()
		{
			transform.LookAt (GameObject.Find ("main_Sphere").transform);
			umbrellaFalls ();
			destroyObject.DestroyOnTimer(this.gameObject, 5f);
		}

		//----------------------------- OTHER FUNCTIONS ------------------------------------------------------------------------

		void umbrellaFalls ()
		{ //this was to make it a once off thing
			if (umbrellaObject.position.y <= 20) {
				GetComponent<ParticleSystem> ().enableEmission = true;
			} else if (GetComponent<ParticleSystem> () != null) {
				GetComponent<ParticleSystem> ().enableEmission = false;
			}
		}

		void OnParticleCollision (GameObject umbrella)
		{
			if (umbrella.name == "main_Sphere") {
				umbrella.GetComponent<Rigidbody> ().AddForce (Vector3.up * windForce);
			}
		}
	}
}