using UnityEngine;
using System.Collections;

namespace Environment
{
	public class CloudBehave : MonoBehaviour
	{

		private Rigidbody cloud;
		public float speedOfCloud;
		public GameObject newCloud;

		[System.NonSerialized]
		public Quaternion cloudRotation = new Quaternion (0.7f, 0, 0, -0.7f);
		private GameObject newlySpawnedCloud;

		void Start ()
		{	
			cloud = GetComponent<Rigidbody> ();
		}

//----------------------------------- Calls and movement stuff -------------------------------------------------------------//

		void FixedUpdate ()
		{
			cloud.AddForce (Vector3.right * -1 * Time.fixedDeltaTime * speedOfCloud);
			createSomeClouds ();
			ignoreOthers ();
		}

//------------------------------------ Function to spawn in Clouds --------------------------------------------------------//

		void createSomeClouds ()
		{
			if (transform.position.x <= -800) {
				newlySpawnedCloud = Instantiate (newCloud, new Vector3 (800, transform.position.y, transform.position.z), cloudRotation) as GameObject;
				newlySpawnedCloud.transform.parent = GameObject.Find("Clouds").transform;

				Destroy (gameObject);
			}
		}

//----------------------------------- An Attempt to get the clouds to ignore each other -----------------------------------//

		void ignoreOthers ()
		{
			Physics.IgnoreCollision (GameObject.FindWithTag ("Clouds").GetComponent<MeshCollider> (), GameObject.FindWithTag ("Clouds").GetComponent<MeshCollider> ());
		}
	}
}
