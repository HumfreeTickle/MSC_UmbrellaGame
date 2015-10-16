using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CameraScripts;
using UnityEngine.UI;
using System;

namespace NPC
{
	public class NPC_FinalMission : MonoBehaviour
	{
		private GmaeManage gameManager;
		private MissionController missionStates;

		private GameObject cmaera;
		private GameObject cameraSet;

		private GameObject umbrella;

		private GameObject bridge_npc;
		private GameObject church;
		private GameObject priest;

		private GameObject lightHouse;
		private GameObject lightHouseKeeper;

		private GameObject pickupTool;
		
		//------------- Talking variables -----------------//
		
		private float talkingSpeed;
		private bool finalMissionRunning;
		
		/// <summary>
		/// States whether the mission has been complete or not
		/// </summary>
		/// <value><c>true</c> if tutorial running; otherwise, <c>false</c>.</value>
		public bool FinalMissionRunning {
			get {
				return finalMissionRunning;
			}
			set {
				finalMissionRunning = value;
			}
		}
		
		//--------------------------------------------------// 
		public GameObject particales;
		private bool finalMissionStart;

		public bool FinalMissionStart {
			get {
				return finalMissionStart;
			}
			
			set {
				finalMissionStart = value;
			}
		}
		
		//ends the mission
		private bool finalMissionFinished = false;
		/// <summary>
		/// Allows other scripts to know when the tutorial mission has been completed
		/// </summary>
		/// <value><c>true</c> if misssion finished; otherwise, <c>false</c>.</value>
		public bool FinalMisssionFinished {
			get {
				return finalMissionFinished;
			}
			set {
				finalMissionFinished = value;
			}
		}
		
		//-------------- Talking Stuff ---------------//
		private Text npc_Talking; //text box
		private Image npc_TalkingBox; //background image
		private NPC_Interaction npc_Interact; //used to call the mission when the player talks to an NPC
		public bool proceed = false; // used to prevent spamming of continue button. As well as allow continue button to work
		
		// Would be awesome if I could workout a way to have this seperate by paragraphs
		// Create a list that fills up when ever there is an enter break "\n"
		// split string
		
		//		public List<string> npc_Message_Array = new List<string> (); //supposed to allow for blocks of text to be entered and seperated out automatically
		public string npc_Message = ""; // holds the current message that needs to be displayed
		//------------------------------------------------------------------------------//
		private int x = 0; // for the case state **I wonder if using number's is the best way to cycle through each case
		/// <summary>
		/// allows other scripts to increase the case state
		/// </summary>
		/// <value>The tut_ x.</value>
		
		private bool jumpAround = true;
		
		/// <summary>
		/// Used to animate the NPC_Tut.
		/// </summary>
		/// <value><c>true</c> if jump around; otherwise, <c>false</c>.</value>
		public bool JumpAround_Priest {
			set {
				jumpAround = value;
			}
		}
		
		private bool playParticles = true;
		private Animator npc_Animator;
		private Light overHereLight;
		private IEnumerator finalCoroutine;
		// other missions
		private NPC_CatMission catMissionStuff;
		private NPC_BoxesMission boxesMissionStuff;
		private HorseMission_BackEnd horseMissionStuff;


		
		//--------------------------------------------//
		
		void Start ()
		{
			gameManager = GameObject.Find ("Follow Camera").GetComponent<GmaeManage> ();
			cmaera = GameObject.Find ("Follow Camera"); 
			umbrella = cmaera.GetComponent<Controller> ().lookAt; //let's the camera look at different things

			catMissionStuff = GetComponent<NPC_CatMission> ();
			boxesMissionStuff = GetComponent<NPC_BoxesMission>();
			horseMissionStuff = GetComponent<HorseMission_BackEnd>();

			bridge_npc = GameObject.Find("");
			priest = GameObject.Find ("Priest");
			church = GameObject.Find("");
			lightHouse = GameObject.Find("");
			lightHouseKeeper = GameObject.Find("NPC_LightHouseKeeper");
			pickupTool = GameObject.Find("");

			npc_Talking = GameObject.Find ("NPC_Talking").GetComponent<Text> ();
			npc_TalkingBox = GameObject.Find ("NPC_TalkBox").GetComponent<Image> ();

			npc_Animator = priest.GetComponent<Animator> ();
			if (!npc_Animator.isActiveAndEnabled) {
				npc_Animator.enabled = true;
			}
			overHereLight = priest.transform.FindChild ("Sphere").transform.FindChild ("Activate").GetComponent<Light> ();//where ever the light is on the NPC_Talk characters. 
			overHereLight.enabled = false;

			// gonna need a few of these
			// 1. bridge_npc
			// 2. priest
			// 3. lighthouse keeper
			npc_Interact = priest.GetComponent<NPC_Interaction> (); // 
			finalCoroutine = Final_Mission ();

			//doesn't quite work
			npc_Message.Split (new[] { "/n" }, StringSplitOptions.None);

		}
		
		void Update ()
		{
			if (catMissionStuff.CatMissionFinished && boxesMissionStuff.BoxesMisssionFinished && horseMissionStuff.HorseMisssionFinished) {

				bridge_npc.GetComponent<NavMeshMovement>().FinalMission = true;

				priest.tag = "NPC_talk";
				talkingSpeed = gameManager.TextSpeed;

				npc_Interact.MissionDelegate = StartFinalMission;

				if (!finalMissionRunning) {
					StartCoroutine (Final_Mission ());
				}
			}
		}
		
		void StartFinalMission ()//Allows the mission to actually start. Nothing happens if it isn't here
		{
			if (!finalMissionStart) {
				finalMissionStart = true;
			}
			
			
			if (finalMissionRunning) {
				x = 3;
				finalMissionRunning = false;
			}
		}
		
		//------------------------------------------ Mission Coroutine ------------------------------------------------//
		IEnumerator Final_Mission ()
		{
			finalMissionRunning = true;
			int i = 0;
			
			while (x < 14) {
				switch (x) {
					
				case 0:

					// this has to activate the NPC's to go to the chrurch;
					// should probably set a static boolean to true;
					// in NavMesh Movement

					// Also highlights one of the NPC's to talk to
					// once you've talked to him then it moves onto the next case;

//					jumpAround = false;
//					cmaera.GetComponent<GmaeManage> ().gameState = GameState.MissionEvent;
//					npc_TalkingBox.enabled = true;
//					while (i <= npc_Message.Length) {
//						npc_Message = "My boxes!!! Someone has stolen them and haphazardly placed them around the town.";
//						npc_Talking.text = (npc_Message.Substring (0, i));
//						i += 1;
//						yield return new WaitForSeconds (talkingSpeed);
//					}
//					//-------------------------------- all this stuff --------------------------------//
//					while (i >= npc_Message.Length + 1) {
//						if (Input.GetButtonDown ("Talk")) {
//							if (gameManager.gameState == GameState.MissionEvent) {
//								proceed = true;
//							}
//						}
//						
//						if (proceed) {
//							i = 0;
//							x = 1;
//							proceed = false;
//						}
//						yield return null;
//					}
					
					break;
					
					
				case 1:

					// first talking part
					// tells the player to go see the priest
					// probably should move the camera
					cameraSet = church;
					cmaera.GetComponent<Controller> ().lookAt = cameraSet;

					cameraSet = umbrella;
					cmaera.GetComponent<Controller> ().lookAt = cameraSet;

					while (i <= npc_Message.Length) {
						npc_Message = "Can you please retrive them from their unusual locals?";
						npc_Talking.text = (npc_Message.Substring (0, i));
						
						i += 1;
						yield return new WaitForSeconds (talkingSpeed);
					}
					
					while (i >= npc_Message.Length) {
						if (Input.GetButtonDown ("Talk")) {
							if (gameManager.gameState == GameState.MissionEvent) {
								proceed = true;
							}
						}
						
						if (proceed) {
							npc_Message = "";
							npc_TalkingBox.enabled = false;
							npc_Talking.text = npc_Message;
							i = 0;
							x = 2;
							proceed = false;
							cmaera.GetComponent<GmaeManage> ().gameState = GameState.Game;
						}
						yield return null;
					}
					break;
				// break in the action
				// to allow the player to move to the priest.
					
				case 3:

					// priest tells you that the bridge needs to be fixed
					// and to follow him outside

				//-------- talking code ------------//
					cmaera.GetComponent<GmaeManage> ().gameState = GameState.MissionEvent;
					npc_TalkingBox.enabled = true;

					while (i <= npc_Message.Length) {
						
						cmaera.GetComponent<GmaeManage> ().Progression = 4;
						if (playParticles) {
							Instantiate (particales, umbrella.transform.position + new Vector3 (0, 1f, 0), Quaternion.identity);
							playParticles = false;
							
						}
						npc_Message = "You are a saint of an umbrella. Thank you so very much.";
						npc_Talking.text = (npc_Message.Substring (0, i));
						i += 1;
						yield return new WaitForSeconds (talkingSpeed);
						
					}
				//---------------------------------//

					
					while (i >= npc_Message.Length) {
						if (finalMissionFinished) {
							if (Input.GetButtonDown ("Talk")) {
								if (gameManager.gameState == GameState.MissionEvent) {
									proceed = true;
								}
							}
							if (proceed) {
								npc_Message = "";
								npc_Talking.text = npc_Message;
								npc_TalkingBox.enabled = false;
								
								i = 0;
								x = 4;
								
								missionStates = MissionController.BridgeMission;
								gameManager.missionState = missionStates;
								gameManager.gameState = GameState.Game;
								finalMissionStart = false;
								
								// Temp final stuff
								GameObject.FindWithTag ("Final").GetComponent<Light> ().enabled = true;
								proceed = false;
								yield break;
							}
						}
						yield return null;
						
					}
					break;

				//again another break in the action to allow the priest to get outside and the player
					
				case 5:

					//-------- talking code ------------//
					cmaera.GetComponent<GmaeManage> ().gameState = GameState.MissionEvent;
					npc_TalkingBox.enabled = true;
					
					while (i <= npc_Message.Length) {
						
						cmaera.GetComponent<GmaeManage> ().Progression = 4;
						if (playParticles) {
							Instantiate (particales, umbrella.transform.position + new Vector3 (0, 1f, 0), Quaternion.identity);
							playParticles = false;
							
						}
						npc_Message = "You are a saint of an umbrella. Thank you so very much.";
						npc_Talking.text = (npc_Message.Substring (0, i));
						i += 1;
						yield return new WaitForSeconds (talkingSpeed);
						
					}
					//---------------------------------//

					// priest points out the light house and tells the player to go see the keeper up on the balcony
					// warns that he is a very grumpy man
					cameraSet = lightHouse;
					cmaera.GetComponent<Controller> ().lookAt = cameraSet;

					cameraSet = umbrella;
					cmaera.GetComponent<Controller> ().lookAt = cameraSet;

					priest.tag = "NPC";
					StopCoroutine (finalCoroutine);
					x = 5;
					break;

				// break in the action

				case 7:
					//-------- talking code ------------//
					cmaera.GetComponent<GmaeManage> ().gameState = GameState.MissionEvent;
					npc_TalkingBox.enabled = true;
					
					while (i <= npc_Message.Length) {
						
						cmaera.GetComponent<GmaeManage> ().Progression = 4;
						if (playParticles) {
							Instantiate (particales, umbrella.transform.position + new Vector3 (0, 1f, 0), Quaternion.identity);
							playParticles = false;
							
						}
						npc_Message = "What do you want? Fix the bridge ayyy! Hmm.. Alrigh' but you gotta do something for me first.";
						npc_Talking.text = (npc_Message.Substring (0, i));
						i += 1;
						yield return new WaitForSeconds (talkingSpeed);
						
					}
					//---------------------------------//

					//-------- talking code ------------//
					cmaera.GetComponent<GmaeManage> ().gameState = GameState.MissionEvent;
					npc_TalkingBox.enabled = true;
					
					while (i <= npc_Message.Length) {
						
						cmaera.GetComponent<GmaeManage> ().Progression = 4;
						if (playParticles) {
							Instantiate (particales, umbrella.transform.position + new Vector3 (0, 1f, 0), Quaternion.identity);
							playParticles = false;
							
						}
						npc_Message = "See that guy over there. He has a tool I need, but he won't give it to me. I need you to get it from him.";
						npc_Talking.text = (npc_Message.Substring (0, i));
						i += 1;
						yield return new WaitForSeconds (talkingSpeed);
						
					}
					//---------------------------------//
					// talking to the lighthouse keeper
					// needs you to rob a tool off the guy cutting the trees
					// camera focuses on the guy
					cameraSet = pickupTool;
					cmaera.GetComponent<Controller> ().lookAt = cameraSet;

					cameraSet = umbrella;
					cmaera.GetComponent<Controller> ().lookAt = cameraSet;

					break;

				// break to steal the tool

				case 9:
					//-------- talking code ------------//
					cmaera.GetComponent<GmaeManage> ().gameState = GameState.MissionEvent;
					npc_TalkingBox.enabled = true;
					
					while (i <= npc_Message.Length) {
						
						cmaera.GetComponent<GmaeManage> ().Progression = 4;
						if (playParticles) {
							Instantiate (particales, umbrella.transform.position + new Vector3 (0, 1f, 0), Quaternion.identity);
							playParticles = false;
							
						}
						npc_Message = "Give me a minute.. gotta fix some things.";
						npc_Talking.text = (npc_Message.Substring (0, i));
						i += 1;
						yield return new WaitForSeconds (talkingSpeed);
						
					}
					//---------------------------------//
					// return to th lighthouse keeper
					// give him the part
					// animation of him repairing stuff
					// camera moves out like the windmill
					cameraSet = lightHouse;
					cmaera.GetComponent<Controller> ().lookAt = cameraSet;

					cameraSet = umbrella;
					cmaera.GetComponent<Controller> ().lookAt = cameraSet;

					// done in another script
					// lighthouse turns on and starts to spin

					break;

				case 10:
					//-------- talking code ------------//
					cmaera.GetComponent<GmaeManage> ().gameState = GameState.MissionEvent;
					npc_TalkingBox.enabled = true;
					
					while (i <= npc_Message.Length) {
						
						cmaera.GetComponent<GmaeManage> ().Progression = 4;
						if (playParticles) {
							Instantiate (particales, umbrella.transform.position + new Vector3 (0, 1f, 0), Quaternion.identity);
							playParticles = false;
							
						}
						npc_Message = "Alrigh' bring me over to the bridge there would ya. I hope a dainty umbrella like yerself can carry me. The wife likes to cook so might be a bit heavier then I used to be.";
						npc_Talking.text = (npc_Message.Substring (0, i));
						i += 1;
						yield return new WaitForSeconds (talkingSpeed);
						
					}
					//---------------------------------//
					// asks to be carried over to the priest
					// camera moves
					cameraSet = priest;
					cmaera.GetComponent<Controller> ().lookAt = cameraSet;

					cameraSet = umbrella;
					cmaera.GetComponent<Controller> ().lookAt = cameraSet;

					break;

				//break in action for transport

				case 12:
					// drop off repairman
					// animation for repairs
					// bridge lowers
					break;

				case 13:
					//-------- talking code ------------//
					cmaera.GetComponent<GmaeManage> ().gameState = GameState.MissionEvent;
					npc_TalkingBox.enabled = true;
					
					while (i <= npc_Message.Length) {
						
						cmaera.GetComponent<GmaeManage> ().Progression = 4;
						if (playParticles) {
							Instantiate (particales, umbrella.transform.position + new Vector3 (0, 1f, 0), Quaternion.identity);
							playParticles = false;
							
						}
						npc_Message = "Thank you, your talents have surely helped everyone on this fine day.";
						npc_Talking.text = (npc_Message.Substring (0, i));
						i += 1;
						yield return new WaitForSeconds (talkingSpeed);
						
					}
					//---------------------------------//
					// priest thanks you
					// new colour gained

					break;

				case 14:
					yield break;

				default:
					Debug.Log ("Default");
					yield return new WaitForSeconds (10);
					
					break;
				}
				yield return null;
			}
			yield break;
		}
		
	}//end
}

