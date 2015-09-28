using UnityEngine;
using System.Collections;

namespace Player
{
	public class Player_Interaction : MonoBehaviour
	{
		private Tutuorial tutorial;
		private bool helloTutorial = true; //stops the L1 tutorial from constantly activating
		private bool interactTutorial = true; //stops the R1 tutorial from constantly activating
		private Transform handle;

		void Start ()
		{
			handle = GameObject.Find ("handle").transform;
			tutorial = GameObject.Find ("Tutorial").GetComponent<Tutuorial> ();
		}
	
		void OnTriggerStay (Collider col)
		{
			if (helloTutorial) {
				if (col.gameObject.tag == "NPC_talk") {
					tutorial.ObjectTag = col.gameObject.tag;
					helloTutorial = false;
				}else{
					tutorial.ObjectTag = "";
				}
			}

			if (interactTutorial) {
				if (col.transform.parent != handle) {
					if (col.gameObject.tag == "Interaction" || col.gameObject.tag == "Pickup") {
						tutorial.ObjectTag = "Interaction";
						interactTutorial = false;
					}
				}else{
					tutorial.ObjectTag = "";
				}
			}
		}

		void OnTriggerExit (Collider col)
		{ //failsafe incase they leave the trigger without finishing the tutorial.
			if (col.gameObject.tag == "Interaction" || col.gameObject.tag == "NPC_talk" || col.gameObject.tag == "NPC" || col.gameObject.tag == "Pickup") {
				tutorial.ObjectTag = "";
				helloTutorial = true;
				interactTutorial = true;
			}
		}
	}
}
