using UnityEngine;
using System.Collections;

namespace Player
{
	public class Player_Interaction : MonoBehaviour
	{
		private Tutuorial tutorial;
		public bool helloTutorial = true; //stops the L1 tutorial from constantly activating

		void Start ()
		{
			tutorial = GameObject.Find ("Tutorial").GetComponent<Tutuorial> ();
		}
	
		void OnTriggerStay (Collider col)
		{
			if (col.gameObject.tag == "NPC_talk") {
				if (helloTutorial) {
					tutorial.objectTag = col.gameObject.tag;
					helloTutorial = false;
				}

				// I have a feeling this will be a problem with interactions
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
				helloTutorial = true;
			}
		}
	}
}
