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

		//This is a mess
		//*****
		private bool tutorialMission;
		private bool tutorialRunning;
		private bool tutorialStarted;
		public bool TutorialMission{
			get{
				return tutorialStarted;
			}

			set{
				tutorialStarted = value;
			}
		}
		//*****
		//--------------------------------------------------// 

		private GameObject windmill;
		private GameObject umbrella;
		private GameObject cmaera;
		private GameObject cameraSet;

		//-------------- Talking Stuff ---------------//
		private Text npc_Talking;
		private Image npc_TalkingBox;
		public string npc_Message = "Can you please help me restart the windmill?";
		//--------------------------------------------//

		void Start ()
		{
			windmill = GameObject.Find ("windmill02");
			cmaera = GameObject.Find ("Follow Camera");
			umbrella = cmaera.GetComponent<Controller> ().umbrella;

			npc_Talking = GameObject.Find ("NPC_Talking").GetComponent<Text> ();
			npc_Talking.fontSize = 30;
			npc_TalkingBox = GameObject.Find ("NPC_TalkBox").GetComponent<Image> ();

			cameraSet = cmaera.GetComponent<Controller> ().umbrella;
		}
	
		void Update ()
		{
			tutorialMission = GetComponent<NPC_Interaction> ().TutorialMission;

			if (tutorialMission) {
				npc_TalkingBox.color = Vector4.Lerp (npc_TalkingBox.color, new Vector4 (npc_TalkingBox.color.r, npc_TalkingBox.color.g, npc_TalkingBox.color.b, 0.5f), Time.deltaTime);
				if (!tutorialRunning) {
					StartCoroutine (Tutotial_Mission ());
				}
			} else {
				npc_TalkingBox.color = Vector4.Lerp (npc_TalkingBox.color, new Vector4 (npc_TalkingBox.color.r, npc_TalkingBox.color.g, npc_TalkingBox.color.b, 0f), Time.deltaTime);
			}
		}

		IEnumerator Tutotial_Mission ()
		{
			//<Might have to add some more cases for when the player has finished the windmill section

			int x = 0;
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
						x = 1;
					}
					break;

				case 1:
					npc_Message = "Not sure how you could do it but maybe if you get a closer look.";
					npc_Talking.text = (npc_Message.Substring (0, i));
					cameraSet = windmill;
					cmaera.GetComponent<Controller> ().umbrella = cameraSet;
					i += 1;

					if (i >= npc_Message.Length + 1) {

						yield return new WaitForSeconds (delay);
						npc_Message = "";
						npc_TalkingBox.enabled = false;
						npc_Talking.text = npc_Message;
						i = 0;
						x = 2;
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
			tutorialStarted = true;
			tutorialRunning = false;
			GetComponent<NPC_Interaction> ().TutorialMission = false;

			yield return new WaitForSeconds(1);

			cmaera.GetComponent<GmaeManage> ().gameState = GameState.Game;

			StopCoroutine (Tutotial_Mission ());

			yield return null;
		}
	}
}
