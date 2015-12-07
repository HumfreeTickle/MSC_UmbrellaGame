using UnityEngine;
using System.Collections;
using Inheritence;

namespace Player
{
	public class wind : MonoBehaviour
	{
		private GmaeManage gameManager;
		private DestroyObject destroyObject = new Inheritence.DestroyObject ();
		public Transform umbrellaObject;
		private Animator umbrellaModel;
		private static bool goGoAnimation;
		public GameState gameState;
		private Tutuorial gameTutorial;

		void Start ()
		{
			gameManager = GameObject.Find ("Follow Camera").GetComponent<GmaeManage> ();
			umbrellaObject = GameObject.Find ("Umbrella").transform;
			umbrellaModel = umbrellaObject.GetComponent<Animator> ();
			if (gameManager.consoleControllerType == ConsoleControllerType.PS3) {
				gameTutorial = GameObject.Find ("Tutorial_PS3").GetComponent<Tutuorial> ();
			} else if (gameManager.consoleControllerType == ConsoleControllerType.XBox) {
				gameTutorial = GameObject.Find ("Tutorial_XBox").GetComponent<Tutuorial> ();
			}
		}

		void Update ()
		{
			transform.LookAt (GameObject.Find ("main_Sphere").transform);
			destroyObject.DestroyOnTimer (this.gameObject, 3f);
		}

		//----------------------------- OTHER FUNCTIONS ----------------------------------------//

		void OnParticleCollision (GameObject umbrella)
		{
			if (umbrella.name == "main_Sphere") {

				if (!goGoAnimation) {
					StartCoroutine (AnimationControl ());
				}
				if (gameState == GameState.Intro) {
					gameTutorial.windAnim.SetBool ("Wind", false);
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
				gameManager.missionState = MissionController.TutorialMission;
				gameManager.gameState = GameState.Game;
			}

			yield return new WaitForSeconds (0.5f);
			umbrellaModel.SetBool ("Hit", false);
			goGoAnimation = false;
		}
	}
}