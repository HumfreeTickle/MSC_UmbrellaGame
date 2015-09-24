using UnityEngine;
using System.Collections;
using NPC;

namespace Environment
{
	public class RotationBlades : MonoBehaviour
	{
		private bool rotation;//this is the blades turning
		private Light activeLight;//the halo pointing where to interact with the windmill
		private NPC_TutorialMission npc_TutorialMission;
		private Tutuorial tutorial;
		public float blowforce;//the force that will be applied to the blowback from the windmill
		private bool turning;
		private bool tutorialRunning;
		private NPCManage npcManager = new NPCManage();
	    
		void Start(){
			tutorial = GameObject.Find ("Tutorial").GetComponent<Tutuorial>();
			npc_TutorialMission = GameObject.Find("Missions").GetComponent<NPC_TutorialMission>();
			activeLight = transform.FindChild("Activate").GetComponent<Light>();
		}

		void Update ()
		{
			tutorialRunning = npc_TutorialMission.TutorialRunning;

			if (this.tag == "Interaction") {
				activeLight.enabled = true;
			}

			if (rotation) {
				onRotation ();
			}
		}

		void onRotation ()
		{
			transform.Rotate (0, 5 * Time.deltaTime, 0);//the direction and speed at which the windmill will move
		}

		void OnTriggerStay (Collider col)
		{
			if (col.gameObject.tag == "Player") {//if the umbrella interacts with the windmill
				if (Input.GetButtonDown ("Interact")) {
					if (tutorialRunning) {
						rotation = true;//turn on the windmill
						activeLight.enabled = false;
						tutorial.ObjectTag = "";
						this.tag = "Untagged";
						col.GetComponent<Rigidbody> ().AddForce (col.transform.forward * -1 * blowforce);//blow back the umbrella
						//----------------------------------//
						npcManager.WindmillMission = true;
						//------------
						npc_TutorialMission.Tut_X = 3;
						npc_TutorialMission.TutorialRunning = false;
						npc_TutorialMission.JumpAround_Tut = true;
					}
				}
			}
		}
	}
}