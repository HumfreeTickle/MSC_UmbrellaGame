using UnityEngine;
using System.Collections;
using Inheritence;

<<<<<<< HEAD
=======

>>>>>>> origin/master
namespace Player
{
	public class wind : MonoBehaviour
	{
		private DestroyObject destroyObject = new Inheritence.DestroyObject ();
		public Transform umbrellaObject;
		private Animator umbrellaModel;
		private static bool goGoAnimation;
		public GameState gameState;
		private Tutuorial gameTutorial;

		void Awake ()
		{
			umbrellaObject = GameObject.Find ("Umbrella").transform;
			umbrellaModel = umbrellaObject.GetComponent<Animator> ();
			gameTutorial = GameObject.Find ("Tutorial").GetComponent<Tutuorial> ();

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

				if (!goGoAnimation) {
					StartCoroutine (AnimationControl ());
				}
				if (gameState == GameState.Intro) {
					gameTutorial.AnimatorYeah.SetBool ("Wind", false);
					umbrella.GetComponent<Rigidbody> ().AddForce (Vector3.up * 1000);
				}
			}
		}

		IEnumerator AnimationControl ()
		{
			goGoAnimation = true;
			umbrellaModel.SetBool ("Hit", true);
			if (gameState == GameState.Intro) {
				umbrellaModel.SetBool ("GameStart", true);
				GameObject.Find ("Follow Camera").GetComponent<GmaeManage> ().MissionState = MissionController.TutorialMission;
				GameObject.Find ("Follow Camera").GetComponent<GmaeManage> ().GameState = GameState.Game;
			}

			yield return new WaitForSeconds (0.5f);
			umbrellaModel.SetBool ("Hit", false);
			goGoAnimation = false;
		}
	}
}