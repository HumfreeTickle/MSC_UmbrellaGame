using UnityEngine;
using System.Collections;

namespace Player
{
	public class Player_Interaction : MonoBehaviour
	{

		public GmaeManage GameManager;
		private Tutuorial tutorial;
		public bool helloTutorial = true; //stops the L1 tutorial from constantly activating
		public bool interactTutorial = true; //stops the R1 tutorial from constantly activating

		void Start ()
		{
			tutorial = GameObject.Find ("Tutorial").GetComponent<Tutuorial> ();
		}
	
		void OnTriggerStay (Collider col)
		{
			if (helloTutorial) {
				if (col.gameObject.tag == "NPC") {
					tutorial.X = 1;
					if (Input.GetButtonUp ("Talk")) {
						helloTutorial = false;
					}
				}
			}

			if (interactTutorial) {
				if (col.gameObject.tag == "Interaction") {
					tutorial.X = 2;
					if (Input.GetButtonUp ("Interact")) {
						interactTutorial = false;
					}
				}
			}
		}

		void OnTriggerExit(Collider col){
			tutorial.X = 5;
		}
	}
}
