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
		public bool finalMissionRunning;
		
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
		public bool finalMissionStart;

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
		private bool proceed = false; // used to prevent spamming of continue button. As well as allow continue button to work
		
		// Would be awesome if I could workout a way to have this seperate by paragraphs
		// Create a list that fills up when ever there is an enter break "\n"
		// split string
		
		//		public List<string> npc_Message_Array = new List<string> (); //supposed to allow for blocks of text to be entered and seperated out automatically
		private string npc_Message = ""; // holds the current message that needs to be displayed
		//------------------------------------------------------------------------------//
		private int x = 0; // for the case state **I wonder if using number's is the best way to cycle through each case

		public int X {
			set {
				x = value;
			}
		}
		
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
		// other missions
		private NPC_CatMission catMissionStuff;
		private NPC_BoxesMission boxesMissionStuff;
		private HorseMission_BackEnd horseMissionStuff;
		public GameObject currentPerson;
		public bool outside;

		public bool Outside {
			set {
				outside = value;
			}
		}

		private bool toolPickedup;
		
		public bool ToolPickedup {
			set {
				toolPickedup = value;
			}
		}

		private bool playTime;
		private Vector3 moveTo;
		private Vector3 robbingPoint;
		private Vector3 lighthouseLookAt;
		private LightOnRotate lighthouseRotate;
		private GameObject LightHouseKeep_DropOff;

		private GameObject NPC_worker;
		//--------------------------------------------//
		
		void Start ()
		{
			gameManager = GameObject.Find ("Follow Camera").GetComponent<GmaeManage> ();
			cmaera = GameObject.Find ("Follow Camera"); 
			umbrella = cmaera.GetComponent<Controller> ().lookAt; //let's the camera look at different things

			catMissionStuff = GetComponent<NPC_CatMission> ();
			boxesMissionStuff = GetComponent<NPC_BoxesMission> ();
			horseMissionStuff = GetComponent<HorseMission_BackEnd> ();

			bridge_npc = GameObject.Find ("NPC_Bridge");
			priest = GameObject.Find ("Priest");
			church = GameObject.Find ("Church_Roof");
			lightHouse = GameObject.Find ("Lighthouse_LookAt");
			lightHouseKeeper = GameObject.Find ("NPC_LightHouseKeeper");

			moveTo = GameObject.Find ("lighthouseMove").transform.position;
			robbingPoint = GameObject.Find ("Robbing_Point").transform.position;

			NPC_worker = GameObject.Find("NPC_Worker");
			pickupTool = GameObject.Find ("Pickaxe");

			lighthouseLookAt = GameObject.Find ("Lighthouse_Look").transform.position;
			lighthouseRotate = GameObject.Find ("LigthHouse_Glass").GetComponent<LightOnRotate> ();

			LightHouseKeep_DropOff = GameObject.Find("LightHouseKeep_DropOff");

			npc_Talking = GameObject.Find ("NPC_Talking").GetComponent<Text> ();
			npc_TalkingBox = GameObject.Find ("NPC_TalkBox").GetComponent<Image> ();




			//doesn't quite work
			npc_Message.Split (new[] { "/n" }, StringSplitOptions.None);

		}
		
		void Update ()
		{
			playTime = cmaera.GetComponent<Controller> ().PlayTime;

			if (catMissionStuff.CatMissionFinished && boxesMissionStuff.BoxesMisssionFinished && horseMissionStuff.HorseMisssionFinished) {

				talkingSpeed = gameManager.TextSpeed;

				bridge_npc.GetComponent<NavMeshMovement> ().FinalMission = true;

				if (x < 1) {
					npc_Animator = bridge_npc.GetComponent<Animator> ();
					if (!npc_Animator.isActiveAndEnabled) {
						npc_Animator.enabled = true;
					}
//					npc_Animator.SetBool ("Play", jumpAround);
					overHereLight = bridge_npc.transform.FindChild ("Sphere").transform.FindChild ("Activate").GetComponent<Light> ();
					overHereLight.enabled = jumpAround;
					npc_Interact = bridge_npc.GetComponent<NPC_Interaction> ();
					npc_Interact.MissionDelegate = StartFinalMission;

					bridge_npc.tag = "NPC_talk";
				}


				if (finalMissionStart) {
					npc_TalkingBox.color = Vector4.Lerp (npc_TalkingBox.color, new Vector4 (npc_TalkingBox.color.r, npc_TalkingBox.color.g, npc_TalkingBox.color.b, 1f), Time.deltaTime);

					if (!finalMissionRunning) {
						StartCoroutine (Final_Mission ());
					}
				} else {
					npc_TalkingBox.color = Vector4.Lerp (npc_TalkingBox.color, new Vector4 (npc_TalkingBox.color.r, npc_TalkingBox.color.g, npc_TalkingBox.color.b, 0f), Time.deltaTime);
				}

				if (pickupTool.transform.parent == GameObject.Find ("handle").transform) {
					toolPickedup = true;
					NPC_worker.transform.FindChild("Hand_R").GetComponent<Animator>().SetBool("MIssing", true);
				}else{
					toolPickedup = false;
				}
			}
		}
		
		void StartFinalMission () // Allows the mission to actually start. Nothing happens if it isn't here
		{
			if (!finalMissionStart) {
				finalMissionStart = true;
			}

			// move to the priest
			if (currentPerson == priest && !outside) {
				x = 3;
			}

			// go outside
			else if (currentPerson == priest && outside) {
				x = 5;
			}

			// lighthouse keeper
			else if (currentPerson == lightHouseKeeper && !toolPickedup) {
				x = 7;
			}
			
			// when you picked up the tool
			else if (currentPerson == lightHouseKeeper && toolPickedup && !lighthouseRotate.LightHerUp) {
				x = 10;
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
					cmaera.GetComponent<GmaeManage> ().gameState = GameState.MissionEvent;
					currentPerson = bridge_npc;
					
					jumpAround = false;
					npc_Animator = bridge_npc.GetComponent<Animator> ();
					
					if (!npc_Animator.isActiveAndEnabled) {
						npc_Animator.enabled = true;
					}
	
					while (i <= npc_Message.Length) {
						npc_Message = "I think something is wrong with the bridge. Can you go find the priest in the church and see what's what?";
						npc_Talking.text = (npc_Message.Substring (0, i));
						npc_TalkingBox.enabled = true;

						if (i == 44) {
							cameraSet = church;
							cmaera.GetComponent<Controller> ().lookAt = cameraSet;
						}

						i += 1;

						if (i < 44) {
							yield return new WaitForSeconds (talkingSpeed);
						} else if (i == 45) {
							yield return new WaitForSeconds (1);
						} else {
							yield return new WaitForSeconds (talkingSpeed);

						}
					}
					
					while (i >= npc_Message.Length) {
						if (Input.GetButtonDown ("Talk")) {
							if (gameManager.gameState == GameState.MissionEvent) {
								proceed = true;
							}
						}
						
						if (proceed) {
							cameraSet = umbrella;
							cmaera.GetComponent<Controller> ().lookAt = cameraSet;
							// Delegate needs to be changed to priest

							npc_Interact = priest.GetComponent<NPC_Interaction> (); 
							npc_Interact.MissionDelegate = StartFinalMission;

							priest.tag = "NPC_talk";

							npc_Message = "";
							npc_TalkingBox.enabled = false;
							npc_Talking.text = npc_Message;
							bridge_npc.tag = "NPC";

							yield return new WaitForSeconds (0.5f);
							if (playTime) {
								i = 0;
								x = 2;
								proceed = false;
								finalMissionStart = false;
								finalMissionRunning = false;

								currentPerson = priest;
								cmaera.GetComponent<GmaeManage> ().gameState = GameState.Game;
							}

							yield return null;

						}
						yield return null;
					}
					yield break;


				// break in the action
				// to allow the player to move to the priest.
					
				case 3:
				//-------- talking code ------------//
					cmaera.GetComponent<GmaeManage> ().gameState = GameState.MissionEvent;
					npc_TalkingBox.enabled = true;

					while (i <= npc_Message.Length) {

						npc_Message = "The bridge is broken? I better check this out. Please come with me outside.";
						npc_Talking.text = (npc_Message.Substring (0, i));
						i += 1;
						if (i < 22) {
							yield return new WaitForSeconds (talkingSpeed);
						} else if (i == 22) {
							yield return new WaitForSeconds (1);
						} else {
							yield return new WaitForSeconds (talkingSpeed);
							
						}
						
					}
				
					while (i >= npc_Message.Length) {
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
								
							gameManager.gameState = GameState.Game;
							npc_Animator = priest.GetComponent<Animator> ();
							npc_Animator.SetBool ("GoOutside", true);

							overHereLight = priest.transform.FindChild("Sphere").transform.GetChild(0).GetComponent<Light>();
							overHereLight.enabled = true;

							finalMissionStart = false;
							finalMissionRunning = false;

							proceed = false;
							yield return null;

						}
						yield return null;

					}						

					yield break;

				//again another break in the action to allow the priest to get outside and the player
					
				case 5:

					//-------- talking code ------------//
					cmaera.GetComponent<GmaeManage> ().gameState = GameState.MissionEvent;
					npc_TalkingBox.enabled = true;
//					npc_Animator.SetBool ("Outside", false);
					overHereLight.enabled = false;

					overHereLight = lightHouseKeeper.transform.FindChild("Sphere").transform.GetChild(0).GetComponent<Light>();
					overHereLight.enabled = true;
					
					while (i <= npc_Message.Length) {
						npc_Message = "This is bad. The lighthouse keeper is the only one who can fix this.";
						npc_Talking.text = (npc_Message.Substring (0, i));
						i += 1;
						if (i == 13) {
							cameraSet = lightHouseKeeper;
							cmaera.GetComponent<Controller> ().lookAt = cameraSet;
							cmaera.GetComponent<Controller> ().MoveYerself = false;
							yield return null;

						}

						if (!cmaera.GetComponent<Controller> ().MoveYerself) {
							cmaera.transform.position = Vector3.Lerp (cmaera.transform.position, moveTo, Time.deltaTime / 2);
						}

						if (i < 14) {
							yield return new WaitForSeconds (talkingSpeed);
						} else if (i == 14) {
							yield return new WaitForSeconds (1);
						} else {
							yield return new WaitForSeconds (talkingSpeed);
							
						}
					}
					//---------------------------------//

					// priest points out the light house and tells the player to go see the keeper up on the balcony
					// warns that he is a very grumpy man
					while (i >= npc_Message.Length) {
						if (Input.GetButtonDown ("Talk")) {
							if (gameManager.gameState == GameState.MissionEvent) {
								proceed = true;
							}
						}
						if (proceed) {
							npc_Message = "";
							npc_Talking.text = npc_Message;
							npc_TalkingBox.enabled = false;
							cmaera.GetComponent<Controller> ().MoveYerself = true;

							cameraSet = umbrella;
							cmaera.GetComponent<Controller> ().lookAt = cameraSet;

							yield return new WaitForSeconds (0.5f);

							
							priest.tag = "NPC";
							if (playTime) {

								i = 0;
								x = 6;
								// Delegate needs to be changed to lighthouse keeper

								gameManager.gameState = GameState.Game;

								finalMissionStart = false;
								finalMissionRunning = false;
							
								currentPerson = lightHouseKeeper;
								lightHouseKeeper.tag = "NPC_talk";

								npc_Interact = lightHouseKeeper.GetComponent<NPC_Interaction> (); 
								npc_Interact.MissionDelegate = StartFinalMission;

								proceed = false;
							}
						}

						yield return null;

					}
					break;

				// break in the action

				case 7:
					//-------- talking code ------------//
					cmaera.GetComponent<GmaeManage> ().gameState = GameState.MissionEvent;
					npc_TalkingBox.enabled = true;
					overHereLight.enabled = false;

					overHereLight = NPC_worker.transform.FindChild("Sphere").transform.GetChild(0).GetComponent<Light>();
					overHereLight.enabled = true;

					while (i <= npc_Message.Length) {
						npc_Message = "What do you want? Fix the bridge ayyy! Alrigh' but you gotta do something for me first.";
						npc_Talking.text = (npc_Message.Substring (0, i));
						i += 1;
						if (i < 18) {
							yield return new WaitForSeconds (talkingSpeed);
						} else if (i == 18) {
							yield return new WaitForSeconds (1);
						} else if (i > 18 && i < 39) {
							yield return new WaitForSeconds (talkingSpeed);
							
						} else if (i == 39) {
							yield return new WaitForSeconds (1);
						} else {
							yield return new WaitForSeconds (talkingSpeed);

						}
					}

					while (i >= npc_Message.Length) {
						if (Input.GetButtonDown ("Talk")) {
							if (gameManager.gameState == GameState.MissionEvent) {
								proceed = true;
							}
						}
						if (proceed) {

							i = 0;
							x = 8;
							proceed = false;

							yield return null;
						
						}

						yield return null;

					}
					//---------------------------------//

					break;


				case 8:	
					while (i <= npc_Message.Length) {
						npc_Message = "See that guy over there. He has a tool I need, but he won't give it to me. I need you to get it from him.";
						npc_Talking.text = (npc_Message.Substring (0, i));
						if (i == 25) {
							pickupTool.tag = "Pickup";
							cameraSet = pickupTool;
							cmaera.GetComponent<Controller> ().lookAt = cameraSet;
							cmaera.GetComponent<Controller> ().MoveYerself = false;
							yield return null;

						}

						if (!cmaera.GetComponent<Controller> ().MoveYerself) {
							cmaera.transform.position = Vector3.Lerp (cmaera.transform.position, robbingPoint, Time.deltaTime / 2);
						}

						i += 1;

						if (i < 26) {
							yield return new WaitForSeconds (talkingSpeed);
						} else if (i == 26) {
							yield return new WaitForSeconds (1);
						} else {
							yield return new WaitForSeconds (talkingSpeed);
							
						}
					}

					while (i >= npc_Message.Length) {
						if (Input.GetButtonDown ("Talk")) {
							if (gameManager.gameState == GameState.MissionEvent) {
								proceed = true;
							}
						}
						if (proceed) {
							npc_Message = "";
							npc_Talking.text = npc_Message;
							npc_TalkingBox.enabled = false;

							cameraSet = umbrella;
							cmaera.GetComponent<Controller> ().lookAt = cameraSet;

							yield return new WaitForSeconds (0.5f);

							if (playTime) {
								finalMissionStart = false;
							
								i = 0;
								x = 9;

								gameManager.gameState = GameState.Game;
								
								finalMissionStart = false;
								finalMissionRunning = false;
								
								proceed = false;
								yield return null;
							}

							yield return null;
							
						}
						yield return null;

					}
					//---------------------------------//

					yield break;

				// break to steal the tool


				case 10:
					//-------- talking code ------------//
					cmaera.GetComponent<GmaeManage> ().gameState = GameState.MissionEvent;
					npc_TalkingBox.enabled = true;
					npc_Animator = lightHouseKeeper.GetComponent<Animator> ();

					while (i <= npc_Message.Length) {
						npc_Message = "Give me a minute.. gotta fix some things.";
						pickupTool.transform.parent = lightHouseKeeper.transform.FindChild ("Hand_R").transform;
						pickupTool.transform.localPosition = new Vector3 (0, 1, 0);
						pickupTool.tag = "Untagged";

						npc_Talking.text = (npc_Message.Substring (0, i));
						if (i == 19) {
							npc_Animator.SetBool ("FixIt", true);
							cameraSet = lightHouse;
							cmaera.GetComponent<Controller> ().lookAt = cameraSet;
							cmaera.GetComponent<Controller> ().MoveYerself = false;
							yield return null;
						}

						if (i == 30) {
							lighthouseRotate.LightHerUp = true;
						}

						if (!cmaera.GetComponent<Controller> ().MoveYerself) {
							cmaera.transform.position = Vector3.Lerp (cmaera.transform.position, lighthouseLookAt, Time.deltaTime / 2);
						}

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
							npc_Talking.text = npc_Message;
							npc_TalkingBox.enabled = false;

							npc_Animator.SetBool ("FixIt", false);

							cmaera.GetComponent<Controller> ().MoveYerself = true;

							cameraSet = umbrella;
							cmaera.GetComponent<Controller> ().lookAt = cameraSet;

							yield return new WaitForSeconds (0.5f);

							if (playTime) {
								i = 0;
								x = 11;

								finalMissionRunning = false;

								proceed = false;
								yield return null;
							}

							yield return null;
						}
						yield return null;
					}
					break;

				case 11:
					//-------- talking code ------------//
					npc_TalkingBox.enabled = true;
					lightHouseKeeper.tag = "Pickup";
					pickupTool.SetActive(false);

					if (npc_Animator.isActiveAndEnabled) {
						npc_Animator.enabled = false;
					}

					while (i <= npc_Message.Length) {
	
						npc_Message = "Alrigh' bring me over to the bridge there would ya. I hope a dainty umbrella like yerself can carry me.";
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
							npc_Talking.text = npc_Message;
							npc_TalkingBox.enabled = false;
							if (LightHouseKeep_DropOff.GetComponent<MeshRenderer> ()) {
								LightHouseKeep_DropOff.GetComponent<MeshRenderer> ().enabled = true;
							}

							gameManager.gameState = GameState.Game;
							
							finalMissionStart = false;
							finalMissionRunning = false;

							proceed = false;
						}

						yield return null;
					}


					yield break;

				//break in action for transport

				case 12:
					// drop off repairman
					// animation for repairs
					// bridge lowers

					Debug.Log("Part 12");


					yield break;


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

