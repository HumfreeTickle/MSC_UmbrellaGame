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
		//------------- Talking variables -----------------//
		public float talkingSpeed;
		
		public float TalkingSpeed {
			
			get {
				return talkingSpeed / 10;
			}
		}
		
		public float textSpeed;
		
		//-------------- Tutorial Conditions ---------------//
		private bool catMission;

		private bool catMissionRunning;
		
		public bool CatMissionRunning {
			get {
				return catMissionRunning;
			}
			set {
				catMissionRunning = value;
			}
		}
		
		//--------------------------------------------------// 
		
		private GameObject cat;
		private GameObject umbrella;
		private GameObject cmaera;
		private GameObject cameraSet;
		public Material umbrellaColour;
		public Transform Blue;
		
		//-------------- Talking Stuff ---------------//
		private Text npc_Talking;
		private Image npc_TalkingBox;
		private NPC_Interaction npc_Interact;

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


		void Start ()
		{
			cat = GameObject.Find ("kitten"); //kitten to look at
			cmaera = GameObject.Find ("Follow Camera"); 
			umbrella = cmaera.GetComponent<Controller> ().lookAt; //let's the camera look at different things
			
			//This can probably be moved into the new inheritence class(NPC_Class)
			npc_Talking = GameObject.Find ("NPC_Talking").GetComponent<Text> ();
			npc_TalkingBox = GameObject.Find ("NPC_TalkBox").GetComponent<Image> ();
			
			
			cameraSet = cmaera.GetComponent<Controller> ().lookAt;
			npc_Interact = this.GetComponent<NPC_Interaction> ();
			npc_Interact.misssionDelegate = StartCatMission;
			
			//doesn't quite work
			npc_Message.Split (new[] { "/n" }, StringSplitOptions.None);
		}
	
		void Update ()
		{
			if(catMission){
				StartCoroutine(Cat_Mission());
			}
		}

		void StartCatMission ()
		{
			catMission = true;
		}

		IEnumerator Cat_Mission ()
		{
			int i = 0;
			
			while (x < 2) {
				catMissionRunning = true;
				cmaera.GetComponent<GmaeManage> ().gameState = GameState.Event;
				npc_TalkingBox.enabled = true;
				
				switch (x) {
					
					//new script for the cat mission
				case 0:
					npc_Message = "Can I ask you for one more favour. My friends cat is stuck in a tree";
					npc_Talking.text = (npc_Message.Substring (0, i));
					cameraSet = cat;
					cat.transform.Find ("Activate").GetComponent<Light> ().enabled = true;
					cat.tag = "Pickup";
					
					cmaera.GetComponent<Controller> ().lookAt = cameraSet;
					i += 1;
					
					if (i >= npc_Message.Length + 1) {
						
						yield return new WaitForSeconds (textSpeed);
						i = 0;
						x += 1;
					}
					break;
					
				case 1:
					npc_Message = "Can you grab it and bring it to my friend on the next island?";
					npc_Talking.text = (npc_Message.Substring (0, i));
					
					cmaera.GetComponent<Controller> ().lookAt = cameraSet;
					i += 1;
					
					if (i >= npc_Message.Length + 1) {
						
						yield return new WaitForSeconds (textSpeed);
						npc_Message = "";
						npc_TalkingBox.enabled = false;
						npc_Talking.text = npc_Message;
						i = 0;
						x += 1;
					}
					break;
					
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
			catMission = false; 
			yield return new WaitForSeconds (1);
			
			cmaera.GetComponent<GmaeManage> ().gameState = GameState.Game;
			
			StopCoroutine (Cat_Mission ());
			
			yield return null;
		}

	}
}
