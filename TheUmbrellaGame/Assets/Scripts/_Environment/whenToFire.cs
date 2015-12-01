using UnityEngine;
using System.Collections;

namespace Environment
{
	public class whenToFire : MonoBehaviour
	{

		private ParticleSystem partEmit;

		// Use this for initialization
		void Start ()
		{
			partEmit = GetComponent<ParticleSystem> ();
		}
	
		// Update is called once per frame
		void Update ()
		{

			if (Mathf.Abs (Input.GetAxis ("Vertical_L")) > 0.1) {
				partEmit.enableEmission = true;
			} else {
				partEmit.enableEmission = false;
			}
		}
	}
}
