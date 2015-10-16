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
		private GameObject horseGuy;
		private GameObject cmaera;
		private GameObject umbrella;
		private float talkingSpeed;
		private bool horseMissionRunning;

		public bool HorseMissionRunning {
			get {
				return horseMissionRunning;
			}
			set {
				horseMissionRunning = value;
			}
		}

		public GameObject particales;
		private bool horsesMissionStart;
	
		public bool HorseMissionStart {
			get {
				return horsesMissionStart;
			}
		
			set {
				horsesMissionStart = value;
			}
		}

		private bool horseMissionFinished = false;

		public bool HorseMisssionFinished {
			get {
				return horseMissionFinished;
			}
			set {
				horseMissionFinished = value;
			}
		}

		private bool horseReturned;
	
		public bool HorseReturned {
			get {
				return horseReturned;
			}
		
			set {
				horseReturned = value;
			}
		
		}

		private Text npc_Talking; //text box
		private Image npc_TalkingBox; //background image
		private NPC_Interaction npc_Interact; //used to call the mission when the player talks to an NPC
		public bool proceed = false; // used to prevent spamming of continue button. As well as allow continue button to work

		//		public List<string> npc_Message_Array = new List<string> (); //supposed to allow for blocks of text to be entered and seperated out automatically
		public string npc_Message = ""; // holds the current message that needs to be displayed

			
		private int x = 0; // for the case state

		public int Horses_X {
			set {
				x = value;
			}
		}
	
		private bool jumpAround = true;
		private bool playParticles = true;
		private Animator npc_Animator;
		private Light overHereLight;
		private IEnumerator horseCoroutine;
		private NPC_CatMission catMissionStuff;
		private Transform horses;

		// Use this for initialization
		void Start ()
		{

			gameManager = GameObject.Find ("Follow Camera").GetComponent<GmaeManage> ();
			cmaera = GameObject.Find ("Follow Camera"); 
			umbrella = cmaera.GetComponent<Controller> ().lookAt; //let's the camera look at different things
			horseGuy = GameObject.Find ("HorseMan");
			horseGuy.GetComponent<NPC_Interaction> ().MissionDelegate = StartHorsesMission;// he will only give you the horse mission.
			npc_Talking = GameObject.Find ("NPC_Talking").GetComponent<Text> ();
			npc_TalkingBox = GameObject.Find ("NPC_TalkBox").GetComponent<Image> ();
			overHereLight = horseGuy.transform.FindChild ("Sphere").transform.FindChild ("Activate").GetComponent<Light> ();
			overHereLight.enabled = false;
			horseCoroutine = Horse_Mission ();
			npc_Interact = horseGuy.GetComponent<NPC_Interaction> ();

			horses = GameObject.Find ("Horse").transform;

			if (GetComponent<Animator> ()) {
				if (!npc_Animator.isActiveAndEnabled) {
					npc_Animator.enabled = true;
				}
			}

		}
	
		// Update is called once per frame
		void Update ()
		{

			//if (catMissionStuff.CatMissionFinished) {
			horseGuy.tag = "NPC_talk";
			talkingSpeed = gameManager.TextSpeed;
				
//				npc_Animator.SetBool ("Play", jumpAround);
			overHereLight.enabled = jumpAround;
			npc_Interact.MissionDelegate = StartHorsesMission;

			if (horsesMissionStart) {
				npc_TalkingBox.color = Vector4.Lerp (npc_TalkingBox.color, new Vector4 (npc_TalkingBox.color.r, npc_TalkingBox.color.g, npc_TalkingBox.color.b, 0.5f), Time.deltaTime);
				if (!horseMissionRunning) {
					StartCoroutine (Horse_Mission ());
				}
			} else {
				npc_TalkingBox.color = Vector4.Lerp (npc_TalkingBox.color, new Vector4 (npc_TalkingBox.color.r, npc_TalkingBox.color.g, npc_TalkingBox.color.b, 0f), Time.deltaTime);
			}
			//	}

		}

		void StartHorsesMission ()
		{

			if (!horsesMissionStart) {
				horsesMissionStart = true;
			}

			if (horseReturned && horseMissionRunning) {
				x = 3;
				horseMissionRunning = false;
			}

		}
	
		void TurnOnLights (Transform obj)
		{
		
		
		}

		IEnumerator Horse_Mission ()
		{

			horseMissionRunning = true;
			int i = 0;

			while (x < 5) {// only allows the first 2 cases to playout
				switch (x) {
				case 0:
					jumpAround = false;
					cmaera.GetComponent<GmaeManage> ().gameState = GameState.MissionEvent;
					npc_TalkingBox.enabled = true;
					while (i <= npc_Message.Length) {
						npc_Message = "My horses have all ran away!! Please round them bup for me. ";
						npc_Talking.text = (npc_Message.Substring (0, i));
						i += 1;
						yield return new WaitForSeconds (talkingSpeed);
					}

					while (i >= npc_Message.Length + 1) {
						if (Input.GetButtonDown ("Talk")) {
							if (gameManager.gameState == GameState.MissionEvent) {
								proceed = true;
							}
						}
						if (proceed) {
							TurnOnLights (horses);
						
							i = 0;
							x = 1;
							proceed = false;
						}

						yield return null;
						
					}
						
					break;

				case 1:
					while (i <= npc_Message.Length) {
						npc_Message = "You will need to give them a few nudges as they tend to get distracted easily!";
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
							if (!horseReturned) {
								npc_Message = "";
								npc_TalkingBox.enabled = false;
								npc_Talking.text = npc_Message;
								i = 0;
								x = 2;
								proceed = false;
								cmaera.GetComponent<GmaeManage> ().gameState = GameState.Game;
							}
						}
						yield return null;
					}
					break;
				case 3:
					
					jumpAround = false;
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
					
					while (i >= npc_Message.Length) {
						if (horseMissionFinished) {
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
								horsesMissionStart = false;
								
								// Temp final stuff
								GameObject.FindWithTag ("Final").GetComponent<Light> ().enabled = true;
								proceed = false;
								yield break;
							}
						}
						yield return null;
						
					}
					break;

				}
			}
		
		}


	}

}

//}