using UnityEngine;
using System.Collections;

namespace NPC
{
	public class TheFollower : MonoBehaviour
	{
		//--- no idea what this is for ??????? ---//


		Transform THEtarget;
		public float maxRange;
		public float minRange;
		public bool inSight;
		public float speed;
		public GameObject dropOFF;
		private Animator animator;

		// Use this for initialization
		void Start ()
		{

			inSight = false;
			THEtarget = GameObject.FindGameObjectWithTag ("Player").transform;
			animator = GetComponent<Animator> ();

		}
	
		// Update is called once per frame
		void Update ()
		{
	
			if ((Vector3.Distance (transform.position, THEtarget.position) < maxRange)
				&& (Vector3.Distance (transform.position, THEtarget.position) > minRange)) {

				Debug.Log ("ShouldSee");
				inSight = true;

			}
			if (Vector3.Distance (transform.position, THEtarget.position) > maxRange) {

				Debug.Log ("ShouldStop");
				inSight = false;


			}

			if (inSight) {

				Debug.Log ("TRUE");
				transform.LookAt (THEtarget);
				transform.Translate (Vector3.forward * Time.deltaTime * speed);
				dropOFF.SetActive (true);
				animator.SetBool ("WillWalk", true);
		

			}
			if (!inSight) {

				Debug.Log ("FALSE");
				dropOFF.SetActive (false);
				animator.SetBool ("WillWalk", false);
			}
	
		}

		void OnTriggerEnter (Collider other)
		{

			if (other.gameObject.tag == "Dropper") {

				Destroy (gameObject);
				Destroy (dropOFF);
			}
		}
	}
}

