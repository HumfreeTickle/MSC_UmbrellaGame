using UnityEngine;
using System.Collections;

namespace NPC
{
	public class RunHome : MonoBehaviour
	{

		public bool onGround;
		public Vector3 RunTo;
		public float Speed;
		private Vector3 CurPosition;
		public GameObject Landing;
		public GameObject newKitten;
	
		void Awake ()
		{
			if (GetComponent<Rigidbody> () != null) {
				Destroy (GetComponent<Rigidbody> ());
			}
		}

		// Update is called once per frame
		void OnTriggerEnter (Collider other)
		{
		
			if (other.gameObject.tag == "Terrain") {
				onGround = true;
			} else {
				onGround = false;
			}
		}

		void OnCollisionEnter (Collision col)
		{
			if (col.gameObject.tag != "Terrain" & col.gameObject.tag != "Player" & col.gameObject.tag != "House") {
				Instantiate (newKitten, new Vector3 (193.4f, 20.4f, 65.8f), Quaternion.Euler (0, 291.2715f, 0));
				transform.position = new Vector3 (-21.34f, 0, 0);
				Destroy (gameObject);
			}

		}

		void Update ()
		{
		
			if (onGround) {
				GetComponent<Animation> ().Play ("Run");
				transform.position = Vector3.Lerp (transform.position, RunTo, Speed * Time.deltaTime);
				Destroy (Landing);
				Debug.Log ("Should Lerp");
			}
		}
	}
}