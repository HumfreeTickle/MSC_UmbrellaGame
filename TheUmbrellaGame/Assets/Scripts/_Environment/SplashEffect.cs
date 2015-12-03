using UnityEngine;
using System.Collections;
using Inheritence;

namespace Environment
{
	public class SplashEffect : MonoBehaviour
	{
		private DestroyObject destroy = new Inheritence.DestroyObject ();
		private GameObject instanWaterParticles;
		public GameObject waterParticles;
	
		void OnTriggerEnter (Collider col)
		{
			if (col.gameObject.tag == "River") {
				instanWaterParticles = Instantiate (waterParticles, transform.position, Quaternion.identity) as GameObject;
				instanWaterParticles.GetComponent<ParticleSystem> ().Play ();
				destroy.DestroyOnTimer (instanWaterParticles, 1f);
			}
		}
	}
}
