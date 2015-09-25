using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CameraScripts;
using UnityEngine.UI;
using System;

namespace NPC
{
	public class NPC_CatMission : MonoBehaviour
	{
		private GmaeManage gameManager;

		//------------- Talking variables -----------------//
		private float talkingSpeed;
		public bool proceed = false;
		private bool playTime; //used for a camera state change

		//-------------- Tutorial Conditions ---------------//
		private bool catMissionFinished;

		public bool CatMissionFinished {
			get {
				return catMissionFinished;
			}

			set {
				catMissionFinished = value;
			}
		}

		private bool catMissionStart;

		public bool CatMissionStart {
			set {
				catMissionStart = value;
			}
		}

		private bool catMissionRunning;

		public bool CatMissionRunning {
			get {
				return catMissionRunning;
			}

			set {
				catMissionRunning = value;
			}
		}

		private bool catDroppedOff;

		public bool CatDroppedOff {
			set {
				catDroppedOff = value;
			}

		}
		
		//--------------------------------------------------// 
		public GameObject particales;
		private GameObject cat;
		private GameObject umbrella;
		private GameObject cmaera;
		private GameObject cameraSet;
		private GameObject dropOff;

		public GameObject DropOff {
			get {
				return dropOff;
			}
		}

		private NPC_TutorialMission tutorialMissionStuff;
		
		//-------------- Talking Stuff ---------------//
		private Text npc_Talking;
		private Image npc_TalkingBox;
		private NPC_Interaction npc_Interact;

		//------------------------------------------------------------------------------//
		// Would be awesome if I could workout a way to have this seperate by paragraphs
		// Create a list that fills up when ever there is an enter break "\n"
		// split string
//		public List<string> npc_Message_Array = new List<string> ();
		public string npc_Message = "";
		//------------------------------------------------------------------------------//
		private int x = 0;
		
		public int Cat_X {
			set {
				x = value;
			}
		}

		private bool jumpAround = true;
		
		/// <summary>
		/// Used to animate the NPC_Cat.
		/// </summary>
		/// <value><c>true</c> if jump around; otherwise, <c>false</c>.</value>
		public bool JumpAround_Cat {
			set {
				jumpAround = value;
			}
		}

		private Animator npc_Animator;
		private Light overHereLight;
		private IEnumerator catCoroutine;
		private bool playParticles = true;

		void Start ()
		{
			gameManager = GameObject.Find ("Follow Camera").GetComponent<GmaeManage> ();

			cat = GameObject.Find ("kitten"); //kitten to look at
			cmaera = GameObject.Find ("Follow Camera"); 
			umbrella = cmaera.GetComponent<Controller> ().lookAt; //let's the camera look at different things
			tutorialMissionStuff = GetComponent<NPC_TutorialMission> ();
			//This can probably be moved into the new inheritence class(NPC_Class)
			npc_Talking = GameObject.Find ("NPC_Talking").GetComponent<Text> ();
			npc_TalkingBox = GameObject.Find ("NPC_TalkBox").GetComponent<Image> ();

			dropOff = GameObject.Find ("NPC_Cat");
			npc_Animator = dropOff.GetComponent<Animator> ();
			overHereLight = dropOff.transform.FindChild ("Sphere").transform.FindChild ("Activate").GetComponent<Light> ();//where ever the light is on the NPC_Talk characters. I think it's a child of the 'head'.
			overHereLight.enabled = false;

			cameraSet = cmaera.GetComponent<Controller> ().lookAt;
			npc_Interact = dropOff.GetComponent<NPC_Interaction> ();

			catCoroutine = Cat_Mission ();
			//doesn't quite work
//			npc_Message.Split (new[] { "/n" }, StringSplitOptions.None);
		}
	
		void Update ()
		{	
			if (tutorialMissionStuff.TutorialMisssionFinished) {
				talkingSpeed = gameManager.TextSpeed;

				npc_Animator.SetBool ("Play", jumpAround);
				overHereLight.enabled = jumpAround;
				playTime = cmaera.GetComponent<Controller> ().PlayTime;
				npc_Interact.MissionDelegate = StartCatMission;

				if (catMissionStart) {
					npc_TalkingBox.color = Vector4.Lerp (npc_TalkingBox.color, new Vector4 (npc_TalkingBox.color.r, npc_TalkingBox.color.g, npc_TalkingBox.color.b, 0.5f), Time.deltaTime);
					if (!catMissionRunning) {
						StartCoroutine (Cat_Mission ());
					}
				} else {
					npc_TalkingBox.color = Vector4.Lerp (npc_TalkingBox.color, new Vector4 (npc_TalkingBox.color.r, npc_TalkingBox.color.g, npc_TalkingBox.color.b, 0f), Time.deltaTime);
				}
			}
		}

		/// <summary>
		/// Called when the player drops off the cat on the 2nd island
		/// Ends the cat mission
		/// </summary>
		void StartCatMission ()
		{
			if (catDroppedOff && !catMissionFinished) {
				x = 3;
			}
		}

		IEnumerator Cat_Mission ()
		{
			int i = 0;
			catMissionRunning = true;

			while (x < 4) {
				switch (x) {
					
				case 0:

					cmaera.GetComponent<GmaeManage> ().gameState = GameState.MissionEvent;
					npc_TalkingBox.enabled = true;

					while (i <= npc_Message.Length) {
						
						npc_Message = "Can I ask you for one more favour? My friend's cat is stuck in a tree.";
						npc_Talking.text = (npc_Message.Substring (0, i));
						cameraSet = cat;
						cat.transform.Find ("Activate").GetComponent<Light> ().enabled = true;
						cat.tag = "Pickup";
						
						cmaera.GetComponent<Controller> ().lookAt = cameraSet;
						i += 1;
						yield return new WaitForSeconds (talkingSpeed);
						
					}
					while (i >= npc_Message.Length) {
						if (Input.GetButtonDown ("Talk")) {
							proceed = true;
						}
						
						if (proceed) {
							i = 0;
							x = 1;
							proceed = false;
						}
						yield return null;
						
					}
					break; //end of case 0
					
				case 1:
					while (i <= npc_Message.Length) {
						
						npc_Message = "Can you grab it and bring it to my friend on the next island?";
						npc_Talking.text = (npc_Message.Substring (0, i));
						cameraSet = umbrella;
						
						cmaera.GetComponent<Controller> ().lookAt = cameraSet;
						i += 1;
						yield return new WaitForSeconds (talkingSpeed);
						
					}
					while (i >= npc_Message.Length) {
						if (playTime) {
							if (Input.GetButtonDown ("Talk")) {
								proceed = true;
							}
							if (proceed) {
								npc_Message = " ";
								npc_TalkingBox.enabled = false;
								npc_Talking.text = npc_Message;
								cmaera.GetComponent<GmaeManage> ().gameState = GameState.Game;
								catMissionStart = false; 

								i = 0;
								x = 2;
								proceed = false;
//								StopCoroutine (catCoroutine);
							}
						}
						yield return null;
						
					}
					break; //end of case 1


				case 3:
					cmaera.GetComponent<GmaeManage> ().gameState = GameState.MissionEvent;
					npc_TalkingBox.enabled = true;
					jumpAround = false;

					while (i <= npc_Message.Length) {
							
						cmaera.GetComponent<GmaeManage> ().Progression = 3;
						if (playParticles) {
							Instantiate (particales, umbrella.transform.position + new Vector3 (0, 1f, 0), Quaternion.identity);
							playParticles = false;
						}

						npc_Message = "Thank you so much.";
						npc_Talking.text = (npc_Message.Substring (0, i));
						i += 1;
						yield return new WaitForSeconds (talkingSpeed);
							
					}
					while (i >= npc_Message.Length) {
						if (!catMissionFinished) {
							if (Input.GetButtonDown ("Talk")) {
								proceed = true;
							}
							if (proceed) {
								npc_Message = "";
								npc_TalkingBox.enabled = false;
								npc_Talking.text = npc_Message;
								cmaera.GetComponent<GmaeManage> ().gameState = GameState.Game;
								i = 0;
								x = 4;
								catMissionStart = false; //might have to be moved to case 5
								catMissionFinished = true;
								proceed = false;
							}
						}

						yield return null;
							
					}
					break; //end of case 3

				case 4:
					dropOff.tag = "NPC";
					StopCoroutine (catCoroutine);
					break;
				default:
					yield return null;

					break;
				}

				yield return new WaitForSeconds (talkingSpeed / 10);

			}
			yield break;

		}
	}
}