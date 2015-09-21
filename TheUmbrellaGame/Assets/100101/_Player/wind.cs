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
<<<<<<< HEAD
=======
		public GameState gameState;
>>>>>>> origin/master

		void Awake ()
		{

			umbrellaObject = GameObject.Find ("Umbrella").transform;
			umbrellaModel = umbrellaObject.GetComponent<Animator> ();
		}

		void Update ()
		{
			transform.LookAt (GameObject.Find ("main_Sphere").transform);
			destroyObject.DestroyOnTimer (this.gameObject, 3f);
		}

		//----------------------------- OTHER FUNCTIONS ------------------------------------------------------------------------


		void OnParticleCollision (GameObject umbrella)
		{
			if (umbrella.name == "main_Sphere") {
<<<<<<< HEAD
				umbrella.GetComponent<Rigidbody> ().AddForce (Vector3.up * windForce);
				if(!goGoAnimation){
					StartCoroutine(AnimationControl());
				}
=======
				if(gameState == GameState.Game){
					umbrella.GetComponent<Rigidbody> ().AddForce (Vector3.up * windForce * 10);
				}else if(gameState == GameState.Intro){
					umbrella.GetComponent<Rigidbody> ().AddForce (Vector3.up * 300);
				}
				if(!goGoAnimation){
					StartCoroutine(AnimationControl());
				}
			}
		}

		IEnumerator AnimationControl(){
			goGoAnimation = true;
			umbrellaModel.SetBool ("Hit", true);
			if(gameState == GameState.Intro){
				umbrellaModel.SetBool("GameStart", true);
				GameObject.Find ("Follow Camera").GetComponent<GmaeManage>().gameState = GameState.Game;
>>>>>>> origin/master
			}
			yield return new WaitForSeconds(0.5f);
			umbrellaModel.SetBool ("Hit", false);
			goGoAnimation = false;
		}

		IEnumerator AnimationControl(){
			goGoAnimation = true;
			umbrellaModel.SetBool ("Hit", true);
			yield return new WaitForSeconds(0.5f);
			umbrellaModel.SetBool ("Hit", false);
			goGoAnimation = false;
		}
	}
}