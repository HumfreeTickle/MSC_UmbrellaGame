using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CameraScripts;
using UnityEngine.UI;
using System;

namespace NPC
{
	public class NPC_BoxesMission : MonoBehaviour
	{
		private GmaeManage gameManager;
		private MissionController missionStates;

		public GameObject boxesGuy{ get; private set; }

		//--------------------------------------------------// 
		public bool boxesMission{ get; set; }

		public bool boxesDropped{ get; set; }
		
		//-------------- Talking Stuff ---------------//
		private NPC_Interaction npc_Interact; //used to call the mission when the player talks to an NPC

		public int boxes_X{ get; set; }

		public bool jumpAround_Boxes{ private get; set; }

		private Animator npc_Animator;
		private GameObject overHereLight;
		public IEnumerator boxesCoroutine;
		private Transform boxes;
		private Talk talkCoroutine;
		private string[] boxesMissionDialogue1 = 
		{
			"My boxes!!! "
		};
		private string[] boxesMissionDialogue2 = 
		{
			"Someone has stolen them and haphazardly placed them around the town.", 
			"Can you please retrive them from their unusual locales?"
		};
		private string[] boxesMissionDialogue3 = 
		{
			"You are a saint of an umbrella.",
			"Thank you so very much."
		};
		private _MoveCamera cmaeraMove;
		private IEnumerator cameraMoveCoroutine;
		private bool moveCmarea = true;
		private GameObject box;
		
		//--------------------------------------------//
		
		void Start ()
		{
			gameManager = GameObject.Find ("Follow Camera").GetComponent<GmaeManage> ();
			boxesGuy = GameObject.Find ("NPC_Boxes");
		
			npc_Animator = boxesGuy.GetComponent<Animator> ();
			if (!npc_Animator.isActiveAndEnabled) {
				npc_Animator.enabled = true;
			}
			overHereLight = boxesGuy.transform.FindChild ("Sphere").transform.FindChild ("Activate").gameObject;//where ever the light is on the NPC_Talk characters. 
			overHereLight.SetActive (false);

			npc_Interact = boxesGuy.GetComponent<NPC_Interaction> (); // 

			boxes = GameObject.Find ("CratePickupCollection").transform;
			talkCoroutine = GameObject.Find ("NPC_TalkBox").GetComponent<Talk> ();
			boxes_X = 0;
			jumpAround_Boxes = false;

			cmaeraMove = GameObject.Find ("Follow Camera").GetComponent<_MoveCamera> ();
			box = boxes.GetChild (0).gameObject;

		}
		
		void Update ()
		{
			npc_Animator.SetBool ("Play", jumpAround_Boxes);
			overHereLight.SetActive (jumpAround_Boxes);

			if (gameManager.MissionState == MissionController.BoxesMission) {

				if (boxes_X == 0) {
					boxesGuy.tag = "NPC_talk";
					npc_Interact.MissionDelegate = StartBoxesMission;
					jumpAround_Boxes = true;
				}

				if (boxesMission) {
					boxesGuy.tag = "NPC";
					BoxesMission ();
				}

			} else if (gameManager.MissionState == MissionController.HorsesMission) {
				jumpAround_Boxes = false;
				boxesGuy.tag = "NPC";
				boxesMission = false;
				npc_Interact.MissionDelegate = null;
			}
		}
		
		void StartBoxesMission ()//Allows the mission to actually start. Nothing happens if it isn't here
		{
			if (!boxesMission) {
				boxesMission = true;
			}

			if (boxesDropped) {
				boxes_X = 3;
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

		void BoxesMission ()
		{
			jumpAround_Boxes = false;

			
			switch (boxes_X) {
			// each switchstatement should call talking and/or cameraMove coroutines.
			// using an int for the switch might not be the best.
				
			case 0:
				//prevents multiple calls
				if (talkCoroutine.StartCoroutineTalk) {
					break;
				}
	
				// used to change the switch case once dialogue has ended
				// the action equals a function with no parameters,
				// through the lambda expersion, this then call x = 1;
				System.Action endCorutine1 = (/*parameters*/) /*lambda*/ => {
					boxes_X = 1; /*code to be run*/};

				boxesCoroutine = talkCoroutine.Talking (boxesMissionDialogue1, endCorutine1);
				StartCoroutine (boxesCoroutine);
				break;

			case 1:
				//prevents multiple calls
				if (talkCoroutine.StartCoroutineTalk) {
					break;
				}
				
				// used to change the switch case once dialogue has ended
				// the action equals a function with no parameters,
				// through the lambda expersion, this then call x = 1;
				System.Action endCoroutine = (/*parameters*/) /*lambda*/ => {
					boxes_X = 2; /*code to be run*/};

				// assigns and calls the talking coroutine 
				boxesCoroutine = talkCoroutine.Talking (boxesMissionDialogue2, endCoroutine);
				StartCoroutine (boxesCoroutine);
				TurnOnLights (boxes);
				if (moveCmarea) {
					CameraMove ();
				}
				break;
				
			case 2:
				boxesMission = false;
				break;
				
			case 3:
				
				if (talkCoroutine.StartCoroutineTalk)
					break;
				System.Action tutorialDialogue2 = () => {
					boxes_X = 4;};
				boxesCoroutine = talkCoroutine.Talking (boxesMissionDialogue3, tutorialDialogue2);
				StartCoroutine (boxesCoroutine);
				
				if (gameManager.gameState == GameState.MissionEvent) {
					gameManager.Progression = 4;
				}
				
				break;
				
				
			case 4:
				gameManager.MissionState = MissionController.HorsesMission;
				boxesMission = false;
				break;
				
			default:
				Debug.LogError ("Boxes Mission messed up >:(");
				break;
			}
		}

		void CameraMove ()
		{
			if (!cmaeraMove.StartCoroutineCamera) {
				System.Action endCoroutine = () => {
					moveCmarea = false;};
				
				cameraMoveCoroutine = cmaeraMove.cameraMove (box, endCoroutine);
				StartCoroutine (cameraMoveCoroutine);
			}
		}

	}//end
}