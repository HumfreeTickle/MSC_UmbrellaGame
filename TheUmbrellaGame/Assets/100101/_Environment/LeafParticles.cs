using UnityEngine;
using System.Collections;

namespace Environment
{
	[RequireComponent (typeof(MeshCollider))]

	public class LeafParticles : MonoBehaviour
	{
		private ParticleSystem leafParticle;
		public GameObject leafParticleSystem;
		private GameObject particleChild;
		private GameObject treeTrunk;

		void OnValidate ()
		{
			gameObject.name = "Tree_Leaves";
			treeTrunk = transform.parent.transform.GetChild(1).gameObject;
			treeTrunk.name = "Tree_Trunk";



			GetComponent<MeshCollider> ().convex = true;
			GetComponent<MeshCollider> ().isTrigger = true;

			if(!treeTrunk.GetComponent<MeshCollider>()){
				treeTrunk.AddComponent<MeshCollider>();
			}
			treeTrunk.GetComponent<MeshCollider> ().convex = true;

			if (transform.childCount == 0) {
				particleChild = Instantiate (leafParticleSystem, this.transform.position, Quaternion.identity) as GameObject;
				particleChild.transform.parent = this.transform;
				leafParticle = this.gameObject.transform.GetChild(0).GetComponent<ParticleSystem> ();

			}else{
				leafParticle = this.gameObject.transform.GetChild(0).GetComponent<ParticleSystem> ();
			}


		}

		void Start ()
		{
			leafParticle = this.gameObject.transform.GetChild(0).GetComponent<ParticleSystem> ();		
		}
	
		void OnTriggerEnter (Collider col)
		{
			if (col.gameObject.tag == "Player") {
				leafParticle.Play ();
			}
		}

		void OnTriggerExit (Collider col)
		{
			if (col.gameObject.tag == "Player") {
				leafParticle.Stop ();
			}
		}
	}
}
