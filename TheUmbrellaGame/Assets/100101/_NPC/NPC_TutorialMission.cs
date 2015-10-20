using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CameraScripts;
using UnityEngine.UI;
using System;
using Player.PhysicsStuff;
using Environment;

namespace NPC
{
	public class NPC_TutorialMission : MonoBehaviour
	{

		private GmaeManage gameManager;

		//------------- Talking variables -----------------//

		private float talkingSpeed;

		//-------------- Tutorial Conditions ---------------//
		private bool tutorialMission;
		private bool tutorialRunning;

		/// <summary>
		/// States whether the mission has been complete or not
		/// </summary>
		/// <value><c>true</c> if tutorial running; otherwise, <c>false</c>.</value>
		public bool TutorialRunning {
			get {
				return tutorialRunning;
			}
			set {
				tutorialRunning = value;
			}
		}

		//--------------------------------------------------// 
		public GameObject particales;
		private GameObject windmill;
		private GameObject umbrella;
		private GameObject cmaera;
		private GameObject cameraSet;
		private GameObject npc_Tutorial;

		public GameObject NPC_Tutorial {
			get {
				return npc_Tutorial;
			}
		}

		//ends the mission
		public bool tutorialMissionFinished = false;
		/// <summary>
		/// Allows other scripts to know when the tutorial mission has been completed
		/// </summary>
		/// <value><c>true</c> if misssion finished; otherwise, <c>false</c>.</value>
		public bool TutorialMisssionFinished {
			get {
				return tutorialMissionFinished;
			}
			set {
				tutorialMissionFinished = value;
			}
		}

		//-------------- Talking Stuff ---------------//
		private Text npc_Talking; //text box
		private Image npc_TalkingBox; //background image
		private NPC_Interaction npc_Interact; //used to call the mission when the player talks to an NPC
		private bool proceed = false; // used to prevent spamming of continue button. As well as allow continue button to work

		//---------------- Stuff to keep the font size relative to the screen --------------//
//		private float _oldHeight;
//		private float _oldWidth;
//		public float Ratio = 30;
		//------------------------------------------------------------------------------//
		// Would be awesome if I could workout a way to have this seperate by paragraphs
		// Create a list that fills up when ever there is an enter break "\n"
		// split string

//		public List<string> npc_Message_Array = new List<string> (); //supposed to allow for blocks of text to be entered and seperated out automatically
		private string npc_Message = ""; // holds the current message that needs to be displayed
		//------------------------------------------------------------------------------//
		private int x = 0; // for the case state **I wonder if using number's is the best way to cycle through each case
		/// <summary>
		/// allows other scripts to increase the case state
		/// </summary>
		/// <value>The tut_ x.</value>
		public int Tut_X {
			set {
				x = value;
			}
		}
		 
		private bool jumpAround = true;

		/// <summary>
		/// Used to animate the NPC_Tut.
		/// </summary>
		/// <value><c>true</c> if jump around; otherwise, <c>false</c>.</value>
		public bool JumpAround_Tut {
			set {
				jumpAround = value;
			}
		}

		private bool playTime;
		private bool playParticles = true;
		private Animator npc_Animator;
		private Light overHereLight;
		private IEnumerator tutorialCoroutine;
		private bool proceedTalk;
		//--------------------------------------------//

		void Start ()
		{
			gameManager = GameObject.Find ("Follow Camera").GetComponent<GmaeManage> ();
			windmill = GameObject.Find ("Cylinder"); //windmill part to look at
			cmaera = GameObject.Find ("Follow Camera"); 
			umbrella = cmaera.GetComponent<Controller> ().lookAt; //let's the camera look at different things
			npc_Tutorial = GameObject.Find ("NPC_Tutorial");
			//This can probably be moved into the new inheritence class(NPC_Class)
			npc_Talking = GameObject.Find ("NPC_Talking").GetComponent<Text> ();
			npc_TalkingBox = GameObject.Find ("NPC_TalkBox").GetComponent<Image> ();
			npc_Animator = npc_Tutorial.GetComponent<Animator> ();
			if (!npc_Animator.isActiveAndEnabled) {
				npc_Animator.enabled = true;
			}
			overHereLight = npc_Tutorial.transform.FindChild ("Sphere").transform.FindChild ("Activate").GetComponent<Light> ();//where ever the light is on the NPC_Talk characters. 
			cameraSet = cmaera.GetComponent<Controller> ().lookAt;
			npc_Interact = npc_Tutorial.GetComponent<NPC_Interaction> (); // 
			npc_Interact.MissionDelegate = StartTutorialMission; // changes the delegate so talking activates that mission.

			tutorialCoroutine = Tutotial_Mission ();

			//doesn't quite work
			npc_Message.Split (new[] { "/n" }, StringSplitOptions.None);
		}
	
		void Update ()
		{	

			npc_Animator.SetBool ("PLay", jumpAround);
			overHereLight.enabled = jumpAround;
			playTime = cmaera.GetComponent<Controller> ().PlayTime;
			talkingSpeed = gameManager.TextSpeed;
			npc_TalkingBox.transform.GetChild (0).GetComponent<Animator> ().SetBool ("proceed", proceedTalk);

			if (tutorialMission) {

				npc_TalkingBox.color = Vector4.Lerp (npc_TalkingBox.color, new Vector4 (npc_TalkingBox.color.r, npc_TalkingBox.color.g, npc_TalkingBox.color.b, 0.5f), Time.deltaTime);
				if (!tutorialRunning) {
					StartCoroutine (Tutotial_Mission ());
				}
			} else {
				npc_TalkingBox.color = Vector4.Lerp (npc_TalkingBox.color, new Vector4 (npc_TalkingBox.color.r, npc_TalkingBox.color.g, npc_TalkingBox.color.b, 0f), Time.deltaTime);
			}
		}

		void StartTutorialMission ()//Allows the mission to actually start. Nothing happens if it isn't here
		{
			tutorialMission = true;
//			npc_Tutorial.GetComponent<NPC_Interaction>().MissionDelegate = null;
		}

//------------------------------------------ Mission Coroutine ------------------------------------------------//
		IEnumerator Tutotial_Mission ()
		{
			tutorialRunning = true;
			int i = 0;
//			proceed = false;
			                                     
			while (x < 5) {// only allows the first 2 cases to playout
				switch (x) {

				case 0:
					jumpAround = false;
					gameManager.gameState = GameState.MissionEvent;
					npc_TalkingBox.enabled = true;
					while (i <= npc_Message.Length) {
						//npc_Message[x] //grabs the part of the list the text is attributed to
						npc_Message = "Can you please help me restart the windmill?";
						npc_Talking.text = (npc_Message.Substring (0, i));
						i += 1;
						yield return new WaitForSeconds (talkingSpeed);
					}
				//-------------------------------- all this stuff --------------------------------//
					while (i >= npc_Message.Length + 1) {
						if (!proceed) {
							if (gameManager.gameState == GameState.MissionEvent) {
								proceedTalk = true;
							}
						}


						if (Input.GetButtonDown ("Talk")) {
							if (gameManager.gameState == GameState.MissionEvent) {
								proceed = true;
							}
						}

						if (proceed) {
							proceedTalk = false;

							i = 0;
							x = 1;
							proceed = false;
						}
						yield return null;
					}

					break;
				
			
				case 1:
					while (i <= npc_Message.Length) {
						npc_Message = "Not sure how you could do it but maybe if you get a closer look.";
						npc_Talking.text = (npc_Message.Substring (0, i));

						if (i == 9) {
							cameraSet = windmill;
							windmill.GetComponent<RotationBlades> ().LightsOn = true;
							cmaera.GetComponent<Controller> ().lookAt = cameraSet;
						}
						i += 1;
						yield return new WaitForSeconds (talkingSpeed);
					}

					while (i >= npc_Message.Length) {
						if (!proceed) {
							if (gameManager.gameState == GameState.MissionEvent) {
								proceedTalk = true;
							}
						}

						windmill.GetComponent<RotationBlades> ().LightsOn = false;
						if (Input.GetButtonDown ("Talk")) {
							if (gameManager.gameState == GameState.MissionEvent) {
								proceed = true;
							}
						}

						if (proceed) {
							proceedTalk = false;

							npc_Message = "";
							npc_TalkingBox.enabled = false;
							npc_Talking.text = npc_Message;
							npc_Tutorial.tag = "NPC";
							cameraSet = umbrella;
							cmaera.GetComponent<Controller> ().lookAt = cameraSet;

							yield return new WaitForSeconds (0.5f);
							if (playTime) {
								proceedTalk = false;

								gameManager.gameState = GameState.Game;
								tutorialMission = false; 
								i = 0;
								x = 2;
								proceed = false;
								StopCoroutine (tutorialCoroutine);
							}
						}
						yield return null;
					}
					break;
					
				case 3:
					jumpAround = false;
					gameManager.gameState = GameState.MissionEvent;
					npc_TalkingBox.enabled = true;

					while (i <= npc_Message.Length) {

						cmaera.GetComponent<GmaeManage> ().Progression = 2;
						if (playParticles) {
							Instantiate (particales, umbrella.transform.position + new Vector3 (0, 1f, 0), Quaternion.identity);
							playParticles = false;

						}
						npc_Message = "Wow. Thank you so much";
						npc_Talking.text = (npc_Message.Substring (0, i));
						i += 1;
						yield return new WaitForSeconds (talkingSpeed);

					}
					while (i >= npc_Message.Length) {
						if (!proceed) {
							if (gameManager.gameState == GameState.MissionEvent) {

								proceedTalk = true;
							}
						}

						if (tutorialMissionFinished) {
							if (Input.GetButtonDown ("Talk")) {
								if (gameManager.gameState == GameState.MissionEvent) {
									proceed = true;
								}
							}
							if (proceed) {

								proceedTalk = false;

								umbrella.GetComponent<CreateWind> ().Barriers = false;
								gameManager.missionState = MissionController.CatMission;
								npc_Tutorial.tag = "NPC";

								i = 0;
								x = 4;

								proceed = false;
							}
						}
						yield return null;

					}
					break;
						
				case 4:
					npc_Message = "";
					proceedTalk = false;

					npc_Talking.text = npc_Message;
					GetComponent<NPC_CatMission> ().CatMissionStart = true;
					npc_Tutorial.tag = "NPC";
					x = 5;
					break;

				case 5:

					break;

				default:
					Debug.Log ("Default");
					break;
				}
				yield return null;
			}
			yield break;

//			yield return null;
		}

	}//end
}
