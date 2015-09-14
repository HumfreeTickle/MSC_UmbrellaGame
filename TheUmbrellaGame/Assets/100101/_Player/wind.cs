using UnityEngine;
using System.Collections;
using Inheritence;

namespace Player
{
	public class wind : MonoBehaviour
	{
		private DestroyObject destroyObject = new Inheritence.DestroyObject ();
		public float windForce;
		public Transform umbrellaObject;
		private Animator umbrellaModel;
		private static bool goGoAnimation;

		void Awake ()
		{

			umbrellaObject = GameObject.Find ("Umbrella").transform;
			umbrellaModel = umbrellaObject.GetComponent<Animator> ();
		}

		void Update ()
		{
			transform.LookAt (GameObject.Find ("main_Sphere").transform);

			destroyObject.DestroyOnTimer (this.gameObject, 3f);
			print(goGoAnimation);

		}

		//----------------------------- OTHER FUNCTIONS ------------------------------------------------------------------------


		void OnParticleCollision (GameObject umbrella)
		{
			if (umbrella.name == "main_Sphere") {
				umbrella.GetComponent<Rigidbody> ().AddForce (Vector3.up * windForce);
				if(!goGoAnimation){
					StartCoroutine(AnimationControl());
				}
			}
		}

		IEnumerator AnimationControl(){
			goGoAnimation = true;
//			umbrellaModel.speed = 1;
			umbrellaModel.SetBool ("Hit", true);
			yield return new WaitForSeconds(0.5f);
			umbrellaModel.SetBool ("Hit", false);
//			umbrellaModel.speed = -1;
			goGoAnimation = false;

//			StopCoroutine("AnimationControl");

		}
	}
}