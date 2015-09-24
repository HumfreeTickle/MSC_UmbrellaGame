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
		private GameObject boxesGuy;
		private GameObject cmaera;
		private GameObject umbrella;


		//------------- Talking variables -----------------//
		
		private float talkingSpeed;
		public bool boxesMissionRunning;
		
		/// <summary>
		/// States whether the mission has been complete or not
		/// </summary>
		/// <value><c>true</c> if tutorial running; otherwise, <c>false</c>.</value>
		public bool BoxesMissionRunning {
			get {
				return boxesMissionRunning;
			}
			set {
				boxesMissionRunning = value;
			}
		}
		
		//--------------------------------------------------// 
		public GameObject particales;
		public bool boxesMissionStart;
		
		public bool BoxesMissionStart {
			set {
				boxesMissionStart = value;
			}
		}

		//ends the mission
		public bool boxesMissionFinished = false;
		/// <summary>
		/// Allows other scripts to know when the tutorial mission has been completed
		/// </summary>
		/// <value><c>true</c> if misssion finished; otherwise, <c>false</c>.</value>
		public bool BoxesMisssionFinished {
			get {
				return boxesMissionFinished;
			}
			set {
				boxesMissionFinished = value;
			}
		}

		public bool boxesDropped;

		public bool BoxesDropped {
			get {
				return boxesDropped;
			}

			set {
				boxesDropped = value;
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
		public int x = 0; // for the case state **I wonder if using number's is the best way to cycle through each case
		/// <summary>
		/// allows other scripts to increase the case state
		/// </summary>
		/// <value>The tut_ x.</value>

		public int Boxes_X {
			set {
				x = value;
			}
		}
		
		public bool jumpAround = true;
		
		/// <summary>
		/// Used to animate the NPC_Tut.
		/// </summary>
		/// <value><c>true</c> if jump around; otherwise, <c>false</c>.</value>
		public bool JumpAround_Boxes {
			set {
				jumpAround = value;
			}
		}
		
		private bool playParticles = true;
		private Animator npc_Animator;
		private Light overHereLight;
		private IEnumerator boxesCoroutine;
		private NPC_CatMission catMissionStuff;
		private GameObject boxes;


		//--------------------------------------------//
		
		void Start ()
		{
			gameManager = GameObject.Find ("Follow Camera").GetComponent<GmaeManage> ();
			cmaera = GameObject.Find ("Follow Camera"); 
			umbrella = cmaera.GetComponent<Controller> ().lookAt; //let's the camera look at different things
			catMissionStuff = GetComponent<NPC_CatMission> ();
			boxesGuy = GameObject.Find ("NPC_Boxes");
			npc_Talking = GameObject.Find ("NPC_Talking").GetComponent<Text> ();
			npc_TalkingBox = GameObject.Find ("NPC_TalkBox").GetComponent<Image> ();
			npc_Animator = boxesGuy.GetComponent<Animator> ();
			if (!npc_Animator.isActiveAndEnabled) {
				npc_Animator.enabled = true;
			}
			overHereLight = boxesGuy.transform.FindChild ("Sphere").transform.FindChild ("Activate").GetComponent<Light> ();//where ever the light is on the NPC_Talk characters. 
			overHereLight.enabled = false;

			npc_Interact = boxesGuy.GetComponent<NPC_Interaction> (); // 
			boxesCoroutine = Boxes_Mission ();

			boxes = GameObject.Find ("CratePickupCollection");
			
			//doesn't quite work
			npc_Message.Split (new[] { "/n" }, StringSplitOptions.None);
		}
		
		void Update ()
		{
			if (catMissionStuff.CatMissionFinished) {
				boxesGuy.tag = "NPC_talk";
				talkingSpeed = gameManager.TextSpeed;

				npc_Animator.SetBool ("Play", jumpAround);
				overHereLight.enabled = jumpAround;
				npc_Interact.MissionDelegate = StartBoxesMission;
				
				if (boxesMissionStart) {
					npc_TalkingBox.color = Vector4.Lerp (npc_TalkingBox.color, new Vector4 (npc_TalkingBox.color.r, npc_TalkingBox.color.g, npc_TalkingBox.color.b, 0.5f), Time.deltaTime);
					if (!boxesMissionRunning) {
						StartCoroutine (Boxes_Mission ());
					}
				} else {
					npc_TalkingBox.color = Vector4.Lerp (npc_TalkingBox.color, new Vector4 (npc_TalkingBox.color.r, npc_TalkingBox.color.g, npc_TalkingBox.color.b, 0f), Time.deltaTime);
				}
			}
		}
		
		void StartBoxesMission ()//Allows the mission to actually start. Nothing happens if it isn't here
		{
			if (!boxesMissionStart) {
				boxesMissionStart = true;
			}
		

			if (boxesDropped) {
				x = 3;
				boxesMissionRunning = false;
			}
		}

		void TurnOnLights (GameObject obj)
		{
			for (int child = 0; child < obj.transform.childCount; child++) {
				if (obj.transform.GetChild (child).transform.childCount > 0) {
					TurnOnLights (obj.transform.GetChild (child).gameObject);
				} else {
					if (obj.GetComponent<Light> ()) {
						if (obj.GetComponent<Light> ().isActiveAndEnabled) {
							obj.GetComponent<Light> ().enabled = true;
						}
					}
				}
			}
		}
		
		//------------------------------------------ Mission Coroutine ------------------------------------------------//
		IEnumerator Boxes_Mission ()
		{
			boxesMissionRunning = true;
			int i = 0;
			
			while (x < 6) {// only allows the first 2 cases to playout
				switch (x) {
					
				case 0:
					jumpAround = false;
					cmaera.GetComponent<GmaeManage> ().gameState = GameState.Event;
					npc_TalkingBox.enabled = true;
					while (i <= npc_Message.Length) {
						//npc_Message[x] //grabs the part of the list the text is attributed to
						npc_Message = "My boxes!!! Someone has stolen them and haphazardly placed them around the town.";
						npc_Talking.text = (npc_Message.Substring (0, i));
						i += 1;
						yield return new WaitForSeconds (talkingSpeed);
					}
					//-------------------------------- all this stuff --------------------------------//
					while (i >= npc_Message.Length + 1) {
						if (Input.GetButtonDown ("Talk")) {
							proceed = true;
						}
						
						if (proceed) {
							TurnOnLights (boxes);

							i = 0;
							x = 1;
							proceed = false;
						}
						yield return null;
					}
					
					break;
					
					
				case 1:
					while (i <= npc_Message.Length) {
						npc_Message = "Can you please retrive them from their unusual locals?";
						npc_Talking.text = (npc_Message.Substring (0, i));
						
						i += 1;
						yield return new WaitForSeconds (talkingSpeed);
					}
					
					while (i >= npc_Message.Length) {
						if (Input.GetButtonDown ("Talk")) {
							proceed = true;
						}
						
						if (proceed) {
							if (!boxesDropped) {
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
					cmaera.GetComponent<GmaeManage> ().gameState = GameState.Event;
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
						if (boxesMissionFinished) {
							if (Input.GetButtonDown ("Talk")) {
								proceed = true;
							}
							if (proceed) {
								npc_Message = "";
								npc_Talking.text = npc_Message;
								npc_TalkingBox.enabled = false;

								i = 0;
								x = 4;

								cmaera.GetComponent<GmaeManage> ().gameState = GameState.Game;
								boxesMissionStart = false;
								boxesMissionFinished = true;
								proceed = false;
							}
						}
						yield return null;
						
					}
					break;
					
				case 4:
					boxesGuy.tag = "NPC";
					StopCoroutine (boxesCoroutine);
					x = 5;
					break;
					
				case 5:
					
					break;
					
				default:
					Debug.Log ("Default");
					yield return new WaitForSeconds (10);

					break;
				}
				yield return new WaitForSeconds (talkingSpeed / 10);
			}
			yield return null;
		}
		
	}//end
}
