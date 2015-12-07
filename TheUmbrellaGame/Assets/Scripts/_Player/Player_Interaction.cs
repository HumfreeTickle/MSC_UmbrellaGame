using UnityEngine;
using System.Collections;

namespace Player
{
	public class Player_Interaction : MonoBehaviour
	{
		private Tutuorial tutorial;
		private GmaeManage gameManager;

		void Start ()
		{
			gameManager = GameObject.Find("Follow Camera").GetComponent<GmaeManage>();

			if(gameManager.consoleControllerType == ConsoleControllerType.PS3){
				tutorial = GameObject.Find ("Tutorial_PS3").GetComponent<Tutuorial> ();
			}else if(gameManager.consoleControllerType == ConsoleControllerType.XBox){
				tutorial = GameObject.Find ("Tutorial_XBox").GetComponent<Tutuorial> ();
			}		
		}
	
		void OnTriggerStay (Collider col)
		{
			if (col.gameObject.tag == "NPC_talk") {
					tutorial.objectTag = col.gameObject.tag;

			} else if (col.gameObject.tag == "NPC") {
				tutorial.objectTag = "";
			}
		}

		void OnTriggerExit (Collider col)
		{ //failsafe incase they leave the trigger without finishing the tutorial.
			if (col.gameObject.tag == "Interaction"
			    || col.gameObject.tag == "NPC_talk" 
			    || col.gameObject.tag == "NPC" 
			    || col.gameObject.tag == "Pickup" 
			    || col.gameObject.tag == "") {

				tutorial.objectTag = "";
			}
		}
	}
}
