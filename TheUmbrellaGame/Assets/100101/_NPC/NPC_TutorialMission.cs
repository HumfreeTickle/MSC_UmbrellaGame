using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CameraScripts;
using UnityEngine.UI;
using System;

namespace NPC
{
	/// <summary>
	/// need to move the talking parts into an inheritnce class so all NPC's can access it
	/// set up proper gamestate variable
	/// </summary>
	public class NPC_TutorialMission : MonoBehaviour
	{

		private GameState gameState;

		//------------- Talking variables -----------------//
		public float talkingSpeed;

		public float TalkingSpeed {

			get {
				return talkingSpeed / 10;
			}
		}

		public float textSpeed;

		//-------------- Tutorial Conditions ---------------//
		private bool tutorialMission;
		private bool tutorialRunning;

		public bool TutorialRunning {
			get {
				return tutorialRunning;
			}
			set {
				tutorialRunning = value;
			}
		}

		//--------------------------------------------------// 

		private GameObject windmill;
		private GameObject cat;
		private GameObject umbrella;
		private GameObject cmaera;
		public GameObject cameraSet;
		private bool missionFinished = false;

		public bool MisssionFinished {
			set {
				missionFinished = value;
			}
		}

		//-------------- Talking Stuff ---------------//
		private Text npc_Talking;
		private Image npc_TalkingBox;
		private NPC_Interaction npc_Interact;
		private bool proceed;
		private float _oldHeight;
		private float _oldWidth;
		public float Ratio = 30;
		//------------------------------------------------------------------------------//
		// Would be awesome if I could workout a way to have this seperate by paragraphs
		// Create a list that fills up when ever there is an enter break "\n"
		// split string
		public List<string> npc_Message_Array = new List<string> ();
		public string npc_Message = "Can you please help me restart the windmill? /n Yes";
		//------------------------------------------------------------------------------//
		public int x = 0;

		public int X {
			set {
				x = value;
			}
		}

		//--------------------------------------------//

		void Start ()
		{
			windmill = GameObject.Find ("Cylinder"); //windmill part to look at
			cat = GameObject.Find ("kitten"); //kitten to look at
			cmaera = GameObject.Find ("Follow Camera"); 
			umbrella = cmaera.GetComponent<Controller> ().lookAt; //let's the camera look at different things

			//This can probably be moved into the new inheritence class(NPC_Class)
			npc_Talking = GameObject.Find ("NPC_Talking").GetComponent<Text> ();
			npc_TalkingBox = GameObject.Find ("NPC_TalkBox").GetComponent<Image> ();


			cameraSet = cmaera.GetComponent<Controller> ().lookAt;
			npc_Interact = this.GetComponent<NPC_Interaction> ();
			npc_Interact.misssionDelegate = StartTutorialMission;

			//doesn't quite work
			npc_Message.Split (new[] { "/n" }, StringSplitOptions.None);
		}
	
		void Update ()
		{	
			if (_oldWidth != Screen.width || _oldHeight != Screen.height) {
				_oldWidth = Screen.width;
				_oldHeight = Screen.height;
				npc_Talking.fontSize = Mathf.RoundToInt (Mathf.Min (Screen.width, Screen.height) / Ratio);
			}

			if (cmaera.GetComponent<GmaeManage> ().gameState == GameState.Event) {
				if (Input.GetButtonDown ("Talk")) {
					proceed = true;
				}
			}

			if (tutorialMission) {
				npc_TalkingBox.color = Vector4.Lerp (npc_TalkingBox.color, new Vector4 (npc_TalkingBox.color.r, npc_TalkingBox.color.g, npc_TalkingBox.color.b, 0.5f), Time.deltaTime);
				if (!tutorialRunning) {
					StartCoroutine (Tutotial_Mission ());
				}
			} else {
				npc_TalkingBox.color = Vector4.Lerp (npc_TalkingBox.color, new Vector4 (npc_TalkingBox.color.r, npc_TalkingBox.color.g, npc_TalkingBox.color.b, 0f), Time.deltaTime);
			}
		}

		void StartTutorialMission ()
		{
			tutorialMission = true;
		}

//------------------------------------------ Mission Coroutine ------------------------------------------------//
		IEnumerator Tutotial_Mission ()
		{
			int i = 0;
			                                     
			while (x < 2) {
				tutorialRunning = true;
				cmaera.GetComponent<GmaeManage> ().gameState = GameState.Event;
				npc_TalkingBox.enabled = true;

				switch (x) {

				case 0:
					//npc_Message[x] //grabs the part of the list the text is attributed to
					npc_Message = "Can you please help me restart the windmill?";
					npc_Talking.text = (npc_Message.Substring (0, i));
					i += 1;

				//-------------------------------- all this stuff --------------------------------//
					while (i >= npc_Message.Length + 1) {
						yield return new WaitForSeconds (textSpeed);

						if (proceed) {
							i = 0;
							x += 1;
							proceed = false;
						}
					}

					break;

				case 1:
					npc_Message = "Not sure how you could do it but maybe if you get a closer look.";
					npc_Talking.text = (npc_Message.Substring (0, i));
					cameraSet = windmill;
					windmill.tag = "Interaction";

					cmaera.GetComponent<Controller> ().lookAt = cameraSet;
					i += 1;

					while (i >= npc_Message.Length) {
						yield return new WaitForSeconds (textSpeed);

						if (proceed) {
							npc_Message = "";
							npc_TalkingBox.enabled = false;
							npc_Talking.text = npc_Message;
							i = 0;
							x = 2;
							proceed = false;
						}
					}
					break;
				
				default:
					Debug.Log ("Default");
					break;
				}
				yield return null;
			}

			while (x > 2 && x < 6) {
				tutorialRunning = true;
				cmaera.GetComponent<GmaeManage> ().gameState = GameState.Event;
				npc_TalkingBox.enabled = true;
				
				switch (x) {
					
				case 3:
					npc_Message = "Wow. Thank you so much";
					npc_Talking.text = (npc_Message.Substring (0, i));
					i += 1;
					while (i >= npc_Message.Length + 1) {
						cmaera.GetComponent<GmaeManage> ().Progression = 2;
						yield return new WaitForSeconds (textSpeed);

						if (missionFinished) {
							if (proceed) {
								i = 0;
								x += 1;
								proceed = false;
							}
							//change missionDelegate to catmissionStart
//						npc_Interact.misssionDelegate()
						}
					}
					break;
						
//------------------------------------new script for the cat mission-----------------------------------------//
				case 4:
					npc_Message = "Can I ask you for one more favour. My friends cat is stuck in a tree";
					npc_Talking.text = (npc_Message.Substring (0, i));
					cameraSet = cat;
					cat.transform.Find ("Activate").GetComponent<Light> ().enabled = true;
					cat.tag = "Pickup";
					
					cmaera.GetComponent<Controller> ().lookAt = cameraSet;
					i += 1;
					
					if (i >= npc_Message.Length + 1) {
						
						yield return new WaitForSeconds (textSpeed);
						if (proceed) {
							i = 0;
							x += 1;
							proceed = false;
						}
					}
					break;

				case 5:
					npc_Message = "Can you grab it and bring it to my friend on the next island?";
					npc_Talking.text = (npc_Message.Substring (0, i));
					
					cmaera.GetComponent<Controller> ().lookAt = cameraSet;
					i += 1;
					
					if (i >= npc_Message.Length + 1) {
						yield return new WaitForSeconds (textSpeed);
						if (proceed) {
							npc_Message = "";
							npc_TalkingBox.enabled = false;
							npc_Talking.text = npc_Message;
							i = 0;
							x = 2;
							proceed = false;
						}
					}
					break;
//--------------------------------------------------------------------------------------------------------------------//
				default:
					Debug.Log ("Default");
					
					break;
				}
				yield return new WaitForSeconds (TalkingSpeed);
			}

				cameraSet = umbrella;
				cmaera.GetComponent<Controller> ().lookAt = cameraSet;
			// This should only be changed when the mission is fully done
			// So after the return from the windmill
				tutorialMission = false; 
				yield return new WaitForSeconds (1);

				cmaera.GetComponent<GmaeManage> ().gameState = GameState.Game;

				StopCoroutine (Tutotial_Mission ());

				yield return null;
		}

	}//end
}
