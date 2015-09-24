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

		public GameState gameState;
		private Tutuorial gameTutorial;

		void Awake ()
		{

			umbrellaObject = GameObject.Find ("Umbrella").transform;
			umbrellaModel = umbrellaObject.GetComponent<Animator> ();
			gameTutorial = GameObject.Find ("Tutorial").GetComponent<Tutuorial>();
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
				umbrella.GetComponent<Rigidbody> ().AddForce (Vector3.up * windForce);
				if(!goGoAnimation){
					StartCoroutine(AnimationControl());
				}
				if(gameState == GameState.Game){
					umbrella.GetComponent<Rigidbody> ().AddForce (Vector3.up * windForce * 10);
				}else if(gameState == GameState.Intro){
					gameTutorial.AnimatorYeah.SetBool("Wind",false);
					umbrella.GetComponent<Rigidbody> ().AddForce (Vector3.up * 1000);

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
			}
			yield return new WaitForSeconds(0.5f);
			umbrellaModel.SetBool ("Hit", false);
			goGoAnimation = false;
		}
	}
}