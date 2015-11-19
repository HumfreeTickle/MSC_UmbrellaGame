using UnityEngine;
using System.Collections;

namespace Player
{
	public class Player_Interaction : MonoBehaviour
	{
		private Tutuorial tutorial;

		void Start ()
		{
			tutorial = GameObject.Find ("Tutorial").GetComponent<Tutuorial> ();
		}
	
		void OnTriggerStay (Collider col)
		{
			if (col.gameObject.tag == "NPC_talk") {
					tutorial.objectTag = col.gameObject.tag;

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
			}
		}
	}
}
