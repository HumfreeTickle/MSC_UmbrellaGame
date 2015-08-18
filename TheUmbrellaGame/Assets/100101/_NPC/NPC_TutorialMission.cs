using UnityEngine;
using System.Collections;
using CameraScripts;
using UnityEngine.UI;

namespace NPC
{
	public class NPC_TutorialMission : MonoBehaviour
	{
		public float talkingSpeed;

		private bool tutorialMission;
		private GameObject windmill;
		private GameObject umbrella;
		private GameObject cmaera;
		private GameObject cameraSet;
		private Text npc_Talking;

		public string npc_Message = "Can you please help me restart the windmill?";

		void Start ()
		{
			windmill = GameObject.Find("windmill02");
			umbrella = GameObject.Find ("main_sphere");
			cmaera = GameObject.Find("Follow Camera");
			npc_Talking = GameObject.Find("NPC_Talking").GetComponent<Text>();
			cameraSet = cmaera.GetComponent<Controller>().umbrella;
		}
	
		void Update ()
		{
			tutorialMission = GetComponent<NPC_Interaction>().TutorialMission;
			if(tutorialMission){
				StartCoroutine(Tutotial_Mission());
			}
		}

		IEnumerator Tutotial_Mission(){
			int i = 0;
			while(i <= npc_Message.Length + 1){
				cmaera.GetComponent<GmaeManage>().gameState = GameState.Talking;
				npc_Talking.text = (npc_Message.Substring(0, i));
				i += 1;

				if(i == npc_Message.Length + 1){
					i = 0;
					npc_Message = "Not sure how you could do it but maybe if you get a closer look.";
					cameraSet = windmill;
					print (cameraSet);
				}

				yield return new WaitForSeconds(talkingSpeed);
			}
//			cameraSet = umbrella;
//			cmaera.GetComponent<GmaeManage>().gameState = GameState.Game;
//			print (cameraSet);
//
//
//
//			yield return null;
		}
	}
}
