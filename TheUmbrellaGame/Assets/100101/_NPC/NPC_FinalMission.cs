using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CameraScripts;
using UnityEngine.UI;
using System;
using Enivironment;

namespace NPC
{
	public class NPC_FinalMission : MonoBehaviour
	{
		private GmaeManage gameManager;
		private MissionController missionStates;
		private GameObject cmaera;

		//------------- NPC used ------------//
		private GameObject bridge_npc;
		private GameObject priest;
		private GameObject lightHouseKeeper;
		private GameObject pickupTool;
		private DropTheBridge bridgeDrop;
		
		//------------- Talking variables -----------------//
		
		private float talkingSpeed;
		
		//--------------------------------------------------// 
		public GameObject particales;
		public bool finalMission;
		private NPC_Interaction npc_Interact; //used to call the mission when the player talks to an NPC

		
		//------------------------------------------------------------------------------//
		public int final_X{ private get; set; } // for the case state **I wonder if using number's is the best way to cycle through each case
		public bool jumpAround_Final{ private get; set; }
		
		private Animator npc_Animator;
		private GameObject overHereLight;
		private GameObject currentPerson;

		public bool outside{ private get; set; }

		public bool toolPickedup{ private get; set; }

		private bool proceedTalk;
		private bool playTime;
		private LightOnRotate lighthouseRotate;
		private GameObject LightHouseKeep_DropOff;
		private GameObject NPC_worker;
		private NavMeshAgent agent;
		private bool fixeding;
		private IEnumerator finalCoroutine;
		private Talk talkCoroutine;

//-------------------------------------------- Dialougue Section -----------------------------------------------//
		private string[] bridgeNPC_Dialouge = 
		{
			"I think something is wrong with the bridge.",
			"Can you go find the priest in the church and see what's what?"
		};
		private string[] Priest_Dialogue1 = 
		{
			"The bridge is broken?",
			"I better check this out.",
			"Please meet me outside by the bridge."
		};
		private string[] Priest_Dialogue2 = 
		{
			"This is bad.",
			"The lighthouse keeper is the only one who can fix this."
		};
		private string[] LightHouseKeeper_Dialogue1 = 
		{
			"What do you want?",
			"Fix the bridge ayyy!",
			"Alrigh' but you gotta do something for me first.",
			"See that guy over there.",
			"He has a tool I need, but he won't give it to me.",
			"I need you to get it from him."
		};
		private string[] LightHouseKeeper_Dialogue2 = 
		{
			"Give me a minute.. ",
			"Gotta fix some things.",
			"Alrigh' bring me over to the bridge there would ya.",
			"I hope a dainty umbrella like yerself can carry me."
		};
		private string[] Priest_Dialogue3 = 
		{
			"Thanks",
			"You know for an umbrella, you're alright.",
		};

//--------------------------------------------------------------------------------------------------------------≠//
		
		void Start ()
		{
			gameManager = GameObject.Find ("Follow Camera").GetComponent<GmaeManage> ();

			bridge_npc = GameObject.Find ("NPC_Bridge");
			priest = GameObject.Find ("Priest");
			lightHouseKeeper = GameObject.Find ("NPC_LightHouseKeeper");
			NPC_worker = GameObject.Find ("NPC_Worker");
			pickupTool = GameObject.Find ("Pickaxe");
			lighthouseRotate = GameObject.Find ("LigthHouse_Glass").GetComponent<LightOnRotate> ();
			LightHouseKeep_DropOff = GameObject.Find ("LightHouseKeep_DropOff");
			LightHouseKeep_DropOff.SetActive(false);
			bridgeDrop = GameObject.Find ("Walkway-Bridge_C-Basic").GetComponent<DropTheBridge> ();
			overHereLight = bridge_npc.transform.FindChild ("Sphere").transform.FindChild ("Activate").gameObject;
			talkCoroutine = GameObject.Find ("NPC_TalkBox").GetComponent<Talk> ();
			
			npc_Interact = bridge_npc.GetComponent<NPC_Interaction> ();
			npc_Interact.MissionDelegate = StartFinalMission;

			npc_Animator = bridge_npc.GetComponent<Animator> ();

			jumpAround_Final = true;
			currentPerson = bridge_npc;
		}
		
		void Update ()
		{
			if (gameManager.MissionState == MissionController.FinalMission) {

				bridge_npc.GetComponent<NavMeshMovement> ().FinalMission = true;
				if (currentPerson != null) {
					overHereLight = currentPerson.transform.FindChild ("Sphere").transform.FindChild ("Activate").gameObject;
					npc_Interact = currentPerson.GetComponent<NPC_Interaction> ();
					npc_Interact.MissionDelegate = StartFinalMission;

					overHereLight.SetActive (jumpAround_Final);


					if (currentPerson.GetComponent<Animator> ()) {
						npc_Animator = currentPerson.GetComponent<Animator> ();
					}

				}

				if (final_X == 0) {
					bridge_npc.tag = "NPC_talk";
				} else {
					bridge_npc.tag = "NPC";
				}

				if (pickupTool.transform.parent == GameObject.Find ("handle").transform || pickupTool.transform.parent == lightHouseKeeper.transform.FindChild ("Hand_R").transform) {
					toolPickedup = true;
					jumpAround_Final = true;

					lightHouseKeeper.tag = "NPC_talk";
					NPC_worker.transform.FindChild ("Hand_R").GetComponent<Animator> ().SetBool ("MIssing", true);

				} else {
					toolPickedup = false;
				}

				if (finalMission) {
					Final_Mission ();
				}
			}
		
		}
		
		void StartFinalMission () // Allows the mission to actually start. Nothing happens if it isn't here
		{
			if (!finalMission) {
				finalMission = true;
			}

			// move to the priest
			if (currentPerson == priest && !outside) {
				final_X = 2;
			}

			// go outside
			else if (currentPerson == priest && outside) {
				final_X = 4;
			}

			// lighthouse keeper
			else if (currentPerson == lightHouseKeeper && !toolPickedup) {
				final_X = 6;
			}
			
			// when you picked up the tool
			else if (currentPerson == lightHouseKeeper && toolPickedup && !lighthouseRotate.lightHerUp) {
				final_X = 8;
			}
		}

		void Final_Mission ()
		{
			jumpAround_Final = false;
			
			switch (final_X) {
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
					final_X = 1; /*code to be run*/};

				// assigns and calls the talking coroutine 
				finalCoroutine = talkCoroutine.Talking (bridgeNPC_Dialouge, dialogue1);
				StartCoroutine (finalCoroutine);
				bridge_npc.tag = "NPC";
				break;
				
			// Movement between NPC_Bridge and priest
			case 1:
				currentPerson = priest;

				if (!npc_Animator.isActiveAndEnabled) {
					npc_Animator.enabled = true;
				}
				jumpAround_Final = true;
				
				priest.tag = "NPC_talk";
				finalMission = false;
				break;
				
			case 2:
				
				if (talkCoroutine.StartCoroutine)
					break;
				System.Action dialogue2 = () => {
					final_X = 3;};

				finalCoroutine = talkCoroutine.Talking (Priest_Dialogue1, dialogue2);
				StartCoroutine (finalCoroutine);
				priest.tag = "NPC";

				break;
				
				
			// Movement outside
			case 3:
				npc_Animator.SetBool ("GoOutside", true);
				finalMission = false;
				break;

			case 4:
				if (talkCoroutine.StartCoroutine)
					break;
				System.Action dialogue3 = () => {
					final_X = 5;
					currentPerson = lightHouseKeeper;
				};

				finalCoroutine = talkCoroutine.Talking (Priest_Dialogue2, dialogue3);
				StartCoroutine (finalCoroutine);
				priest.tag = "NPC";
				break;

			// Movement between priest and lighthouse keeper
			case 5:

				if (!npc_Animator.isActiveAndEnabled) {
					npc_Animator.enabled = true;
				}
				jumpAround_Final = true;

				lightHouseKeeper.tag = "NPC_talk";
				finalMission = false;
				break;

			case 6:
				if (talkCoroutine.StartCoroutine)
					break;
				System.Action dialogue4 = () => {
					final_X = 7;
					pickupTool.tag = "Pickup";
				};
				finalCoroutine = talkCoroutine.Talking (LightHouseKeeper_Dialogue1, dialogue4);
				StartCoroutine (finalCoroutine);
				lightHouseKeeper.tag = "NPC";

				break;


			// Stealing the pickaxe
			case 7:
				pickupTool.transform.FindChild ("Activate").GetComponent<Light> ().enabled = true;
				finalMission = false;
				break;

			case 8:
				if (talkCoroutine.StartCoroutine)
					break;
				System.Action dialogue5 = () => {
					final_X = 9;
					lighthouseRotate.lightHerUp = true;
					npc_Animator.SetBool ("FixIt", false);

				};

				finalCoroutine = talkCoroutine.Talking (LightHouseKeeper_Dialogue2, dialogue5);
				StartCoroutine (finalCoroutine);
				lightHouseKeeper.tag = "NPC";
				pickupTool.transform.position = lightHouseKeeper.transform.FindChild ("Hand_R").transform.position;
				pickupTool.transform.parent = lightHouseKeeper.transform.FindChild ("Hand_R").transform;
				pickupTool.transform.rotation = Quaternion.identity;
				npc_Animator.SetBool ("FixIt", true);

				break;

			//Bringing the lighthousekeeper over to the bridge
			case 9:

				currentPerson = null;
				lightHouseKeeper.tag = "Pickup";
				lightHouseKeeper.GetComponent<SphereCollider> ().radius = 0.15f;
				priest.tag = "NPC_talk";
				LightHouseKeep_DropOff.SetActive (true);

				finalMission = false;
				break;

			case 10:
				if (talkCoroutine.StartCoroutine)
					break;
				System.Action dialogue6 = () => {
					final_X = 11;
					bridgeDrop.drop = true;};
				finalCoroutine = talkCoroutine.Talking (Priest_Dialogue3, dialogue6);
				StartCoroutine (finalCoroutine);
				priest.tag = "NPC";


				if (gameManager.gameState == GameState.MissionEvent) {
					gameManager.Progression = 6;
				}
				break;

			case 11:
				finalMission = false;
				break;

			default:
				Debug.LogError ("Final Mission messed up >:(");
				break;
			}
		}
	}//end
}

