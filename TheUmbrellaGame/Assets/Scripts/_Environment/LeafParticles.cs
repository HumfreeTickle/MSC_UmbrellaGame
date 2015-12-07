using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

namespace Environment
{
	[RequireComponent (typeof(MeshCollider))]

	public class LeafParticles : MonoBehaviour
	{
		private ParticleSystem leafParticle;
		private AudioSource gameObjectAudio;
		public GameObject leafParticleSystem;
		private GameObject particleChild;
		private GameObject treeTrunk;

		void Start ()
		{
			if (this.gameObject.transform.childCount > 0) {
				leafParticle = this.gameObject.transform.GetChild (0).GetComponent<ParticleSystem> ();	
				gameObjectAudio = this.gameObject.transform.GetChild (0).GetComponent<AudioSource> ();
			} else {
				leafParticle = null;
				gameObjectAudio = null;
			}
		}
	
		void OnTriggerEnter (Collider col)
		{
			if (col.gameObject.tag == "Player") {
				if (leafParticle != null || gameObjectAudio != null) {
					leafParticle.Play ();
					gameObjectAudio.PlayOneShot (gameObjectAudio.clip);
				}
			}
		}

		void OnTriggerExit (Collider col)
		{
			if (col.gameObject.tag == "Player") {
				if (leafParticle != null || gameObjectAudio != null) {
					leafParticle.Stop ();
				}
			}
		}
	}
}
