using UnityEngine;
using System.Collections;
using CameraScripts;
using UnityEngine.UI;

namespace NPC
{
	public class NPC_TutorialMission : MonoBehaviour
	{

		//------------- Talking variables -----------------//
		public float talkingSpeed;
		public float TalkingSpeed {

			get {
				return talkingSpeed / 10;
			}
		}
		public float delay;

		//-------------- Tutorial Conditions ---------------//
		private bool tutorialMission;

		private bool tutorialRunning;
		public bool TutorialRunning{
			get{
				return tutorialRunning;
			}
			set{
				tutorialRunning = value;
			}
		}

		//--------------------------------------------------// 

		private GameObject windmill;
		private GameObject cat;
		private GameObject umbrella;
		private GameObject cmaera;
		private GameObject cameraSet;

		//-------------- Talking Stuff ---------------//
		private Text npc_Talking;
		private Image npc_TalkingBox;
		private NPC_Interaction npc_Interact;
		// Would be awesome if I could workout a way to have this seperate by paragraphs
		public string npc_Message = "Can you please help me restart the windmill?";
		public int x = 0;
		public int X{
			set{
				x = value;
			}
		}

		//--------------------------------------------//

		void Start ()
		{
			windmill = GameObject.Find ("Cylinder");
			cat = GameObject.Find("kitten");
			cmaera = GameObject.Find ("Follow Camera");
			umbrella = cmaera.GetComponent<Controller> ().umbrella;

			npc_Talking = GameObject.Find ("NPC_Talking").GetComponent<Text> ();
			npc_Talking.fontSize = 30;
			npc_TalkingBox = GameObject.Find ("NPC_TalkBox").GetComponent<Image> ();

			cameraSet = cmaera.GetComponent<Controller> ().umbrella;
			npc_Interact = this.GetComponent<NPC_Interaction>();
			npc_Interact.misssionDelegate = StartMission;

		}
	
		void Update ()
		{
			if (tutorialMission) {
				npc_TalkingBox.color = Vector4.Lerp (npc_TalkingBox.color, new Vector4 (npc_TalkingBox.color.r, npc_TalkingBox.color.g, npc_TalkingBox.color.b, 0.5f), Time.deltaTime);
				if (!tutorialRunning) {
					StartCoroutine (Tutotial_Mission ());
				}
			} else {
				npc_TalkingBox.color = Vector4.Lerp (npc_TalkingBox.color, new Vector4 (npc_TalkingBox.color.r, npc_TalkingBox.color.g, npc_TalkingBox.color.b, 0f), Time.deltaTime);
			}
		}


		void StartMission(){
			tutorialMission = true;
		}
//------------------------------------------ Mission Coroutine ------------------------------------------------//
		IEnumerator Tutotial_Mission ()
		{
			int i = 0;
			                                     
			while (x < 2) {
				tutorialRunning = true;
				cmaera.GetComponent<GmaeManage> ().gameState = GameState.Talking;
				npc_TalkingBox.enabled = true;

				switch (x) {

				case 0:
					npc_Message = "Can you please help me restart the windmill?";
					npc_Talking.text = (npc_Message.Substring (0, i));
					i += 1;

					while (i >= npc_Message.Length + 1) {
						yield return new WaitForSeconds (delay);

						i = 0;
						x += 1;
					}
					break;

				case 1:
					npc_Message = "Not sure how you could do it but maybe if you get a closer look.";
					npc_Talking.text = (npc_Message.Substring (0, i));
					cameraSet = windmill;
					windmill.tag = "Interaction";

					cmaera.GetComponent<Controller> ().umbrella = cameraSet;
					i += 1;

					if (i >= npc_Message.Length + 1) {

						yield return new WaitForSeconds (delay);
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

			while (x > 2 && x < 6) {
				tutorialRunning = true;
				cmaera.GetComponent<GmaeManage> ().gameState = GameState.Talking;
				npc_TalkingBox.enabled = true;
				
				switch (x) {
					
				case 3:
					npc_Message = "Wow. Thank you so much";
					npc_Talking.text = (npc_Message.Substring (0, i));
					i += 1;
					
					while (i >= npc_Message.Length + 1) {
						yield return new WaitForSeconds (delay);
						
						i = 0;
						x += 1;
					}
					break;
					
				case 4:
					npc_Message = "Can I ask you for one more favour. My friends cat is stuck in a tree";
					npc_Talking.text = (npc_Message.Substring (0, i));
					cameraSet = cat;
					cat.tag = "Interaction";
					
					cmaera.GetComponent<Controller> ().umbrella = cameraSet;
					i += 1;
					
					if (i >= npc_Message.Length + 1) {
						
						yield return new WaitForSeconds (delay);
						i = 0;
						x += 1;
					}
					break;

				case 5:
					npc_Message = "Can you grab it and bring it to my friend on the next island?";
					npc_Talking.text = (npc_Message.Substring (0, i));
					
					cmaera.GetComponent<Controller> ().umbrella = cameraSet;
					i += 1;
					
					if (i >= npc_Message.Length + 1) {
						
						yield return new WaitForSeconds (delay);
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
			cmaera.GetComponent<Controller> ().umbrella = cameraSet;
			// This should only be changed when the mission is fully done
			// So after the return from the windmill
			tutorialMission = false; 
			yield return new WaitForSeconds(1);

			cmaera.GetComponent<GmaeManage> ().gameState = GameState.Game;

			StopCoroutine (Tutotial_Mission ());

			yield return null;
		}
	}
}
