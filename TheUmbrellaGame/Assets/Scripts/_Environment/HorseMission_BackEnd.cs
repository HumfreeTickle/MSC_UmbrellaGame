using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CameraScripts;
using UnityEngine.UI;
using System;

namespace NPC
{
	public class HorseMission_BackEnd : MonoBehaviour
	{
		private GmaeManage gameManager;
		private MissionController missionStates;

		public GameObject horseGuy{ get; private set; }

		public bool horsesMission{ get; set; }

		public bool horseReturned{ get; set; }

		private NPC_Interaction npc_Interact; //used to call the mission when the player talks to an NPC
			
		public int horse_X{ private get; set; } // for the case state
	
		public bool jumpAround_Horses{ private get; set; }

		private Animator npc_Animator;
		private GameObject overHereLight;
		private Transform horses;
		private IEnumerator horseCoroutine;
		private MeshRenderer horseDropOff;
		private Talk talkCoroutine;
		private string[] horseMissionDialogue1 = 
		{
			"My horses have all ran away!! Please round them up for me. ",
			"You will need to give them a few nudges as they tend to get distracted easily!",
 		};
		private string[] horseMissionDialogue2 = 
		{
			"Thanks",
			"Here is your final colour! Welcome to our world!", 
			"Hey what's happpeing in the village down below?",
			"There seems to be a big commotion going on!",
			"Can you head down to see what is happening?"
		};

		void Start ()
		{
			gameManager = GameObject.Find ("Follow Camera").GetComponent<GmaeManage> ();

			horseGuy = GameObject.Find ("HorseMan");
			horseGuy.GetComponent<NPC_Interaction> ().missionDelegate = StartHorsesMission;// he will only give you the horse mission.
		
			overHereLight = horseGuy.transform.FindChild ("Sphere").transform.FindChild ("Activate").gameObject;
			overHereLight.SetActive (false);

			npc_Interact = horseGuy.GetComponent<NPC_Interaction> ();
			if (horseGuy.GetComponent<Animator> ()) {
				npc_Animator = horseGuy.GetComponent<Animator> ();
				if (!npc_Animator.isActiveAndEnabled) {
					npc_Animator.enabled = true;
				}
			}
			horseDropOff = GameObject.Find ("HorseDropoff").GetComponent<MeshRenderer> ();
			horses = GameObject.Find ("Horses").transform;

			if (GetComponent<Animator> ()) {
				if (!npc_Animator.isActiveAndEnabled) {
					npc_Animator.enabled = true;
				}
			}

			horse_X = 0;
			talkCoroutine = GameObject.Find ("NPC_TalkBox").GetComponent<Talk> ();
		}
	
		void Update ()
		{
			npc_Animator.SetBool ("Play", jumpAround_Horses);
			overHereLight.SetActive (jumpAround_Horses);

			if (gameManager.missionState == MissionController.HorsesMission) {

				if (horse_X == 0) {
					npc_Interact.missionDelegate = StartHorsesMission;
					horseGuy.tag = "NPC_talk";
					jumpAround_Horses = true;
				}

				if (horsesMission) {
					horseGuy.tag = "NPC"; // sets the NPC to the blank npc tag so the player can no longer talk to him
					HorseMission ();
				} 
			} else if (gameManager.missionState == MissionController.HorsesMission) {
				jumpAround_Horses = false;
				horseGuy.tag = "NPC";
				horsesMission = false;
				npc_Interact.missionDelegate = null;
			}
		}

		void StartHorsesMission ()
		{
			if (!horsesMission) {
				horsesMission = true;
			}
			if (horseReturned) {
				horse_X = 2;

			}

		}
	
		void TurnOnLights (Transform obj)
		{
			for (int child = 0; child < obj.childCount; child++) {
				if (obj.GetChild (child).childCount > 0) {
					TurnOnLights (obj.GetChild (child).transform);
				} else {
					if (obj.GetChild (child).GetComponent<Light> ()) {
						if (!obj.GetChild (child).GetComponent<Light> ().isActiveAndEnabled) {
							obj.GetChild (child).GetComponent<Light> ().enabled = true;
						}
					}
				}
			}
		}

		void HorseMission ()
		{
			jumpAround_Horses = false;

			switch (horse_X) {
			// each switchstatement should call talking and/or cameraMove coroutines.
			// using an int for the switch might not be the best.
				
			case 0:
				
				//prevents multiple calls
				if (talkCoroutine.startCoroutineTalk) {
					break;
				}
				
				// used to change the switch case once dialogue has ended
				// the action equals a function with no parameters,
				// through the lambda expersion, this then call x = 1;
				System.Action dialogue1 = (/*parameters*/) /*lambda*/ => {
					horse_X = 1; /*code to be run*/};
				TurnOnLights (horses);
				horseDropOff.enabled = true;
				// assigns and calls the talking coroutine 
				horseCoroutine = talkCoroutine.Talking (horseMissionDialogue1, dialogue1);
				StartCoroutine (horseCoroutine);
				break;
				
			case 1:
				horsesMission = false;
				break;
				
			case 2:
				
				if (talkCoroutine.startCoroutineTalk)
					break;
				System.Action dialogue2 = () => {
					horse_X = 3;};
				horseCoroutine = talkCoroutine.Talking (horseMissionDialogue2, dialogue2);
				StartCoroutine (horseCoroutine);
				
				if (gameManager.gameState == GameState.MissionEvent) {
					gameManager.Progression = 5;
				}
				
				break;
				
				
			case 3:
				gameManager.missionState = MissionController.FinalMission;
				horsesMission = false;
				break;
				
			default:
				Debug.LogError ("Horse Mission messed up >:(");
				break;
			}
		}

	}
}