using UnityEngine;
using System.Collections;


// don't think this is used
namespace Environment
{
	public class whenToFire : MonoBehaviour
	{
		private ParticleSystem partEmit;

		void Start ()
		{
			partEmit = GetComponent<ParticleSystem> ();
		}
	
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
