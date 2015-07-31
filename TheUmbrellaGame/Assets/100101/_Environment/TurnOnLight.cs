using UnityEngine;
using System.Collections;

namespace Environment
{
	public class TurnOnLight : MonoBehaviour
	{


		public bool LitUp;

		// Use this for initialization
		void Start ()
		{
	
			LitUp = false;
		}

		void Update ()
		{

			if (LitUp) {

				GetComponent<Light> ().enabled = true;
			}
		}
		// Update is called once per frame

		void OnTriggerEnter (Collider other)
		{

			if (other.gameObject.tag == "Player") {

				LitUp = true;
		
			}
		}
	}
}