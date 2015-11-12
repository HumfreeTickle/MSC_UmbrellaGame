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

		public bool catMission{ private get; set; }


		
		//--------------------------------------------------// 
		private GameObject cmaera;
		private GameObject NPC_dropoff;
		public GameObject NPC_DropOff {
			get {
				return NPC_dropoff;
			}
		}

		public bool catDroppedOff{get; set;}

		private NPC_Interaction npc_Interact;

		//------------------------------------------------------------------------------//
		// Would be awesome if I could workout a way to have this seperate by paragraphs
		// Create a list that fills up when ever there is an enter break "\n"
		// split string
//		public List<string> npc_Message_Array = new List<string> ();
		//------------------------------------------------------------------------------//
		private int cat_X = 0;
		
		public int Cat_X {
			set {
				cat_X = value;
			}
		}
		/// <summary>
		/// Used to animate the NPC_Cat.
		/// </summary>
		/// <value><c>true</c> if jump around; otherwise, <c>false</c>.</value>
		public bool jumpAround_Cat{ private get; set; }

		private Animator npc_Animator;
		private GameObject overHereLight;
		private IEnumerator catCoroutine;

		public IEnumerator CatCoroutine {
			get {
				return catCoroutine;
			}
		}

		private Talk talkCoroutine;
		private MeshRenderer catDropOff;
		private Light cat_Activate;

		void Start ()
		{
			gameManager = GameObject.Find ("Follow Camera").GetComponent<GmaeManage> ();
			cmaera = GameObject.Find ("Follow Camera"); 
		
			NPC_dropoff = GameObject.Find ("NPC_Cat");
			npc_Animator = NPC_dropoff.GetComponent<Animator> ();
			overHereLight = NPC_dropoff.transform.FindChild ("Sphere").transform.FindChild ("Activate").gameObject;//where ever the light is on the NPC_Talk characters. I think it's a child of the 'head'.
			npc_Interact = NPC_dropoff.GetComponent<NPC_Interaction> ();
			catDropOff = GameObject.Find ("Drop-Off Zone (Cat)").GetComponent<MeshRenderer> ();
		
			cat_Activate = GameObject.Find("kitten").transform.FindChild("Activate").GetComponent<Light>();

			talkCoroutine = GameObject.Find ("NPC_TalkBox").GetComponent<Talk> ();
			jumpAround_Cat = true;
			catMission = true;
		}
	
		void Update ()
		{
			if (gameManager.MissionState == MissionController.CatMission) {
				npc_Animator.SetBool ("Play", jumpAround_Cat);
				overHereLight.SetActive(jumpAround_Cat);

				if (catMission) {
					NPC_dropoff.tag = "NPC_talk";
					CatMission ();
				} 
			} 
			else if(gameManager.MissionState == MissionController.BoxesMission) {
				jumpAround_Cat = false;
				NPC_dropoff.tag = "NPC";
				catMission = false;
				npc_Interact.MissionDelegate = null;
			}
		}

		/// <summary>
		/// Called when the player drops off the cat on the 2nd island
		/// Ends the cat mission
		/// </summary>

		void StartCatMission ()
		{
			if (catDroppedOff) {
				jumpAround_Cat = false;
				catMission = true;
				cat_X = 1;
			}
		}

		void CatMission ()
		{
			switch (cat_X) {
			// each switchstatement should call talking and/or cameraMove coroutines.
			// using an int for the switch might not be the best.
				
			case 0:
				catMission = false;
				catDropOff.enabled = true;
				npc_Interact.MissionDelegate = StartCatMission;
				cat_Activate.enabled = true;
				break;
				
			case 1:


				if (talkCoroutine.StartCoroutine)
					break;
				System.Action catDialogue2 = () => {
					cat_X = 2;};
				catCoroutine = talkCoroutine.Talking ("Thank you so much.", catDialogue2);
				StartCoroutine (catCoroutine);
				
				if (gameManager.gameState == GameState.MissionEvent) {
					
					cmaera.GetComponent<GmaeManage> ().Progression = 3;
				}
				break;
				
				
			case 2:
				gameManager.MissionState = MissionController.BoxesMission;
				jumpAround_Cat = false; 
				catMission = false;
				break;
				
			default:
				Debug.LogError ("Cat Mission messed up >:(");
				break;
			}
		}
	}
}