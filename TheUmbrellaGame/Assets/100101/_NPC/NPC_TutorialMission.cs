using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace NPC
{
	public class NPC_TutorialMission : MonoBehaviour
	{

		private GmaeManage gameManager;

		//-------------- Tutorial Conditions ---------------//
		private bool tutorialMission;

		//--------------------------------------------------// 
		private GameObject cmaera;

		public GameObject npc_Tutorial{ get; private set; }

		/// <summary>
		/// Allows other scripts to know when the tutorial mission has been completed
		/// </summary>
		/// <value><c>true</c> if misssion finished; otherwise, <c>false</c>.</value>
//		public bool tutorialMissionFinished { get; set; } //ends the mission

		//-------------- Talking Stuff ---------------//
		private NPC_Interaction npc_Interact; //used to call the mission when the player talks to an NPC

		//------------------------------------------------------------------------------//

		/// <summary>
		/// allows other scripts to increase the case state
		/// </summary>
		/// <value>The tut_ x.</value>
		public int tut_X{ private get; set; }

		/// <summary>
		/// Used to animate the NPC_Tut.
		/// </summary>
		/// <value><c>true</c> if jump around; otherwise, <c>false</c>.</value>
		public bool jumpAround_Tut{ private get; set; }//

		private Talk talkCoroutine;
		private Animator npc_Animator;
		private GameObject overHereLight;

		private IEnumerator tutorialCoroutine;
		private string[] tutorialMissionDialogue = 
		{
		 	"Can you please help me restart the windmill?", 
			"Not sure how you could do it but maybe if you get a closer look.",
			"When you've fixed it come back to me."
		};
		private string[] catMissionDialogue = 
		{
			"Wow. Thank you so much",
			"Can I ask you for one more favour? My friend's cat is stuck in a tree.",
			"Don't know how he got over here from the village in the next island.", 
			"Can you grab it and bring it to my friend on the next island?"
		};

		//--------------------------------------------//

		void Start ()
		{
			gameManager = GameObject.Find ("Follow Camera").GetComponent<GmaeManage> ();
			cmaera = GameObject.Find ("Follow Camera");

			npc_Tutorial = GameObject.Find ("NPC_Tutorial"); //reference to the NPC on the first island
			npc_Animator = npc_Tutorial.GetComponent<Animator> ();
			if (!npc_Animator.isActiveAndEnabled) {
				npc_Animator.enabled = true;
			}
			overHereLight = npc_Tutorial.transform.FindChild ("Sphere").transform.FindChild ("Activate").gameObject;//where ever the light is on the NPC_Talk characters. 
			npc_Interact = npc_Tutorial.GetComponent<NPC_Interaction> (); // 
			npc_Interact.MissionDelegate = StartTutorialMission; // changes the delegate so talking activates that mission.

			talkCoroutine = GameObject.Find ("NPC_TalkBox").GetComponent<Talk> ();

			jumpAround_Tut = true;
			tut_X = 0;
		}
	
		void Update ()
		{	
			// Allows the player to know where the mission is.
			npc_Animator.SetBool ("PLay", jumpAround_Tut);
			overHereLight.SetActive(jumpAround_Tut);

			if (gameManager.MissionState == MissionController.TutorialMission || gameManager.MissionState == MissionController.Default) {
				if (tutorialMission) {
					npc_Tutorial.tag = "NPC"; // sets the NPC to the blank npc tag so the player can no longer talk to him
					TutorialMission ();
				} 
			} else /*if (gameManager.MissionState == MissionController.CatMission)*/ {
				jumpAround_Tut = false;
				npc_Tutorial.tag = "NPC";
				tutorialMission = false;
				npc_Interact.MissionDelegate = null;
			}
		}

		void StartTutorialMission ()//Allows the mission to actually start.
		{
			tutorialMission = true;
			jumpAround_Tut = false;
		}

		void TutorialMission ()
		{
			switch (tut_X) {
			// each switchstatement should call talking and/or cameraMove coroutines.
			// using an int for the switch might not be the best.

			case 0:

				//prevents multiple calls
				if (talkCoroutine.StartCoroutine) {
					break;
				}

				// used to change the switch case once dialogue has ended
				// the action equals a function with no parameters,
				// through the lambda expersion, this then call x = 1;
				System.Action dialogue1 = (/*parameters*/) /*lambda*/ => {
					tut_X = 1; /*code to be run*/};

				// assigns and calls the talking coroutine 
				tutorialCoroutine = talkCoroutine.Talking (tutorialMissionDialogue, dialogue1);
				StartCoroutine (tutorialCoroutine);
				break;
				
			case 1:
				tutorialMission = false;
				break;

			case 2:

				if (talkCoroutine.StartCoroutine)
					break;
				System.Action dialogue2 = () => {
					tut_X = 3;};
				tutorialCoroutine = talkCoroutine.Talking (catMissionDialogue, dialogue2);
				StartCoroutine (tutorialCoroutine);

				if (gameManager.gameState == GameState.MissionEvent) {
					cmaera.GetComponent<GmaeManage> ().Progression = 2;
				}

				break;
		

			case 3:
				gameManager.MissionState = MissionController.CatMission;
				tutorialMission = false;
				break;

			default:
				Debug.LogError ("Tutorial Mission messed up >:(");
				break;
			}
		}
	} //end 
}