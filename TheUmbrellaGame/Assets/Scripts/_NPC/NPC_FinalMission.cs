using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CameraScripts;
using UnityEngine.UI;
using System;
using Enivironment;
using Player;

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

		public GameObject overHereLight{ get; set; }

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
			"Alrigh' but you gotta do something for me first."
		};
		private string[] LightHouseKeeper_Dialogue2 = 
		{
			"See that guy over there.",
			"He has a tool I need, but he won't give it to me.",
			"I need you to get it from him."
		};
		private string[] LightHouseKeeper_Dialogue3 = 
		{
			"Give me a minute.. ",
			"Gotta fix some things.",
		};
		private string[] LightHouseKeeper_Dialogue4 = 
		{
			"Alrigh' bring me over to the bridge there would ya.",
			"I hope a dainty umbrella like yerself can carry me."
		};
		private string[] Priest_Dialogue3 = 
		{
			"Thanks",
			"You know for an umbrella, you're alright.",
		};
		public GameObject[] lookAtObjects = new GameObject[4];
		private GameObject lookAt;
		public Transform[] moveToObjects = new Transform[3];
		private Transform moveTo;
		private _MoveCamera cmaeraMove;
		private IEnumerator cameraMoveCoroutine;
		private bool moveCmarea;
		private GameObject umbrella;
		private GameObject handle;
//--------------------------------------------------------------------------------------------------------------≠//
		
		void Start ()
		{

			gameManager = GameObject.Find ("Follow Camera").GetComponent<GmaeManage> ();

			if (gameManager.GameState != GameState.NullState) {
				bridge_npc = GameObject.Find ("NPC_Bridge");
				priest = GameObject.Find ("Priest");
				lightHouseKeeper = GameObject.Find ("NPC_LightHouseKeeper");
				NPC_worker = GameObject.Find ("NPC_Worker");
				pickupTool = GameObject.Find ("Pickaxe");
				lighthouseRotate = GameObject.Find ("LigthHouse_Glass").GetComponent<LightOnRotate> ();
				LightHouseKeep_DropOff = GameObject.Find ("LightHouseKeep_DropOff");

				if (LightHouseKeep_DropOff.activeSelf) {
					LightHouseKeep_DropOff.SetActive (false);
				}

				bridgeDrop = GameObject.Find ("Walkway-Bridge_C-Basic").GetComponent<DropTheBridge> ();
				overHereLight = bridge_npc.transform.FindChild ("Sphere").transform.FindChild ("Activate").gameObject;
				talkCoroutine = GameObject.Find ("NPC_TalkBox").GetComponent<Talk> ();

				if (lookAtObjects.Length != 4) {
					lookAtObjects = new GameObject[4];
				}
				lookAtObjects [0] = GameObject.Find ("Church_Roof");
				lookAtObjects [1] = GameObject.Find ("Lighthouse_LookAt");
				lookAtObjects [2] = GameObject.Find ("Pickaxe");
				lookAtObjects [3] = GameObject.Find ("Walkway-Bridge_C-Basic");
		

				if (moveToObjects.Length != 3) {
					moveToObjects = new Transform[3];
				}

				moveToObjects [0] = GameObject.Find ("lighthouseMove").transform;
				moveToObjects [1] = GameObject.Find ("Robbing_Point").transform;
				moveToObjects [2] = GameObject.Find ("Lighthouse_Move").transform;

				umbrella = GameObject.Find ("main_Sphere");

				npc_Interact = bridge_npc.GetComponent<NPC_Interaction> ();
				npc_Interact.MissionDelegate = StartFinalMission;

				npc_Animator = bridge_npc.GetComponent<Animator> ();

				jumpAround_Final = true;
				currentPerson = bridge_npc;
				cmaera = GameObject.Find ("Follow Camera");
				cmaeraMove = cmaera.GetComponent<_MoveCamera> ();

				handle = GameObject.Find ("handle");
			}
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

				// Sets the mission in motion
				if (final_X == 0) {
					bridge_npc.tag = "NPC_talk";
				} else {
					bridge_npc.tag = "NPC";
				}

				if (pickupTool.transform.parent == handle.transform || pickupTool.transform.parent == lightHouseKeeper.transform.FindChild ("Hand_R").transform) {
					toolPickedup = true;
					jumpAround_Final = true;

					if (final_X == 8) { 
						lightHouseKeeper.tag = "NPC_talk";
					}

					NPC_worker.transform.FindChild ("Hand_R").GetComponent<Animator> ().SetBool ("MIssing", true);
					pickupTool.GetComponent<AudioSource> ().enabled = false;

				} else {
					toolPickedup = false;
				}

				if (finalMission) {
					Final_Mission ();
				}

				if (moveCmarea) {
					CameraMove (lookAt, moveTo);
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
				final_X = 9;
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
				if (talkCoroutine.StartCoroutineTalk) {
					break;
				}
				
				// used to change the switch case once dialogue has ended
				// the action equals a function with no parameters,
				// through the lambda expersion, this then call x = 1;
				System.Action dialogue1 = (/*parameters*/) /*lambda*/ => {
					final_X = 1; /*code to be run*/
				};

				// assigns and calls the talking coroutine 
				finalCoroutine = talkCoroutine.Talking (bridgeNPC_Dialouge, dialogue1);
				StartCoroutine (finalCoroutine);

				bridge_npc.tag = "NPC";

				moveCmarea = true;

				lookAt = lookAtObjects [0];

		
				break;
				
			// Movement between NPC_Bridge and priest
			case 1:
				moveCmarea = false;

				currentPerson = priest;
				if (!npc_Animator.isActiveAndEnabled) {
					npc_Animator.enabled = true;
				}
				jumpAround_Final = true;
				
				priest.tag = "NPC_talk";
				finalMission = false;
				break;
				
			case 2:
				
				if (talkCoroutine.StartCoroutineTalk)
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
				if (talkCoroutine.StartCoroutineTalk) {
					break;
				} else {
					moveCmarea = true;
					
					lookAt = lookAtObjects [1];
					moveTo = moveToObjects [0];
				}
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
				if (talkCoroutine.StartCoroutineTalk)
					break;
				System.Action dialogue4 = () => {
					final_X = 7;
					moveCmarea = true;
				};
				finalCoroutine = talkCoroutine.Talking (LightHouseKeeper_Dialogue1, dialogue4);
				StartCoroutine (finalCoroutine);
				lightHouseKeeper.tag = "NPC";


				break;

			case 7:
				if (talkCoroutine.StartCoroutineTalk) {
					break;
				} else {
					lookAt = lookAtObjects [2];
					moveTo = moveToObjects [1];
				}

				System.Action stealing = () => {
					final_X = 8;
					pickupTool.tag = "Pickup";
				};

				finalCoroutine = talkCoroutine.Talking (LightHouseKeeper_Dialogue2, stealing);
				StartCoroutine (finalCoroutine);

				break;


			// Stealing the pickaxe
			case 8:
				pickupTool.transform.FindChild ("Activate").GetComponent<Light> ().enabled = true;
				finalMission = false;
				break;

			case 9:
				if (talkCoroutine.StartCoroutineTalk) {
					break;
				} else {
					moveCmarea = true;
					lookAt = lookAtObjects [1];
					moveTo = moveToObjects [2];

				}
				System.Action lightHerUp = () => {
					final_X = 10;
					lighthouseRotate.lightHerUp = true;
				};

				finalCoroutine = talkCoroutine.Talking (LightHouseKeeper_Dialogue3, lightHerUp);
				StartCoroutine (finalCoroutine);

				lightHouseKeeper.tag = "NPC";
				pickupTool.transform.position = lightHouseKeeper.transform.FindChild ("Hand_R").transform.position;
				pickupTool.transform.parent = lightHouseKeeper.transform.FindChild ("Hand_R").transform;
				pickupTool.transform.rotation = Quaternion.identity;
				handle.GetComponent<grabbing> ().pickupObject = null;

				npc_Animator.SetBool ("FixIt", true);


				break;

			case 10:
				if (talkCoroutine.StartCoroutineTalk)
					break;
				System.Action lightup = () => {
					final_X = 11;
					if (npc_Animator.isActiveAndEnabled) {
						npc_Animator.SetBool ("FixIt", false);
						npc_Animator.enabled = false;
					}
					lightHouseKeeper.tag = "Pickup";

				};

				finalCoroutine = talkCoroutine.Talking (LightHouseKeeper_Dialogue4, lightup);
				StartCoroutine (finalCoroutine);
				moveCmarea = false;

				pickupTool.SetActive (false);

				break;

			//Bringing the lighthousekeeper over to the bridge
			case 11:
				currentPerson = null; 
				lightHouseKeeper.GetComponent<SphereCollider> ().radius = 0.5f;

				priest.tag = "NPC_talk";

				overHereLight = priest.transform.FindChild ("Sphere").transform.FindChild ("Activate").gameObject;
				overHereLight.SetActive (true);
				LightHouseKeep_DropOff.SetActive (true);

				finalMission = false;
				break;

			case 12:
				overHereLight.SetActive (false);

				if (talkCoroutine.StartCoroutineTalk)
					break;
				System.Action dialogue6 = () => {
					final_X = 13;
					bridgeDrop.drop = true;};
				finalCoroutine = talkCoroutine.Talking (Priest_Dialogue3, dialogue6);
				StartCoroutine (finalCoroutine);
				priest.tag = "NPC";

				break;

			case 13:
				moveCmarea = true;
				lookAt = lookAtObjects [3];

				finalMission = false;

				break;

			default:
				Debug.LogError ("Final Mission messed up >:(");
				break;
			}
		}

		void CameraMove (GameObject lookAt, Transform moveTo = null)
		{
			if (!cmaeraMove.StartCoroutineCamera) {
				System.Action endCoroutine = () => {
					cmaera.GetComponent<Controller> ().lookAt = umbrella; // changes the camera's focus

					if (moveCmarea) {

						moveCmarea = false;
					}
					if (final_X == 13 && !finalMission) {
						gameManager.Progression = 6;
					}

				
					lookAt = null;
					moveTo = null;
				};

				if (moveTo != null) {
					cameraMoveCoroutine = cmaeraMove.cameraMove (lookAt, endCoroutine, moveTo, 5f);
				} else {
					cameraMoveCoroutine = cmaeraMove.cameraMove (lookAt, endCoroutine, null, 5f);
				}
				StartCoroutine (cameraMoveCoroutine);
			}
		}
	}//end
}

