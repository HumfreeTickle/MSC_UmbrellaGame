using UnityEngine;
using System.Collections;
using NPC;

namespace Environment
{
	public class RotationBlades : MonoBehaviour
	{
		private bool rotation;//this is the blades turning
		public Light activeLight;//the halo pointing where to interact with the windmill
		public NPC_TutorialMission npc_TutorialMission;
		public float blowforce;//the force that will be applied to the blowback from the windmill
		private bool turning;
		public bool tutorialMission;
	    
		void Update ()
		{
			tutorialMission = npc_TutorialMission.TutorialMission;

			if (tutorialMission) {
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
				print ("Player1");
				if (Input.GetButtonDown ("Interact")) {
					print ("Clicked");
					if (tutorialMission) {
						rotation = true;//turn on the windmill
						activeLight.enabled = false;
						npc_TutorialMission.TutorialMission = false;
						col.GetComponent<Rigidbody> ().AddForce (col.transform.forward * -1 * blowforce);//blow back the umbrella
					}
				}
			}
		}
	}
}