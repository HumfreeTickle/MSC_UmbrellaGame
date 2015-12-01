using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

namespace Environment
{
	[RequireComponent (typeof(MeshCollider))]

	public class LeafParticles : MonoBehaviour
	{
		private ParticleSystem leafParticle;
		private AudioSource leafSound;
		public GameObject leafParticleSystem;
		private GameObject particleChild;
		private GameObject treeTrunk;

		void OnValidate ()
		{

		}

		void Start ()
		{
			if (this.gameObject.transform.childCount > 0) {
				leafParticle = this.gameObject.transform.GetChild (0).GetComponent<ParticleSystem> ();	
				leafSound = this.gameObject.transform.GetChild (0).GetComponent<AudioSource> ();
			} else {
				leafParticle = null;
				leafSound = null;
			}
		}
	
		void OnTriggerEnter (Collider col)
		{
			if (col.gameObject.tag == "Player") {
				if (leafParticle != null || leafSound != null) {
					leafParticle.Play ();
					leafSound.PlayOneShot (leafSound.clip);
				}
			}
		}

		void OnTriggerExit (Collider col)
		{
			if (col.gameObject.tag == "Player") {
				if (leafParticle != null || leafSound != null) {

					leafParticle.Stop ();
				}
			}
		}
	}
}
