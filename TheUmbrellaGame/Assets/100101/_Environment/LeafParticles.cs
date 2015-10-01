using UnityEngine;
using System.Collections;

namespace Environment
{
	[RequireComponent (typeof (MeshCollider))]

	public class LeafParticles : MonoBehaviour
	{
		private ParticleSystem leafParticle;
		public GameObject leafParticleSystem;
		private Transform particleChild;

		void Start ()
		{
			GetComponent<MeshCollider>().convex = true;
			GetComponent<MeshCollider>().isTrigger = true;
			if(transform.childCount == 0){
				particleChild = Instantiate(leafParticleSystem, transform.position, Quaternion.identity) as Transform;
				particleChild.parent = this.transform;
			}

			leafParticle = this.gameObject.transform.FindChild ("Leaf System").GetComponent<ParticleSystem>();
		}
	
		void OnTriggerEnter (Collider col)
		{
			if (col.gameObject.tag == "Player") {
				leafParticle.Play();
			}
		}

		void OnTriggerExit(Collider col)
		{
			if (col.gameObject.tag == "Player") {
				leafParticle.Stop();
			}
		}
	}
}
