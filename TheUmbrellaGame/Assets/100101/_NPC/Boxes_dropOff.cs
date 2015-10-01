using UnityEngine;
using System.Collections;
using NPC;

public class Boxes_dropOff : MonoBehaviour
{

	private int numberOfBoxesCollected;
	private Transform boxCount;
	private NPC_BoxesMission boxMission;

	void Start ()
	{
		boxCount = GameObject.Find("CratePickupCollection").transform;
		boxMission = GameObject.Find ("Missions").GetComponent<NPC_BoxesMission> ();
	}

	void Update ()
	{
		if (boxMission.BoxesMissionStart) {
			if (!boxMission.BoxesMisssionFinished) {
				GetComponent<MeshRenderer> ().enabled = true;
			} else {
				GetComponent<MeshRenderer> ().enabled = false;

			}
		}

		if (numberOfBoxesCollected >= boxCount.childCount  && !boxMission.BoxesDropped) {
			boxMission.JumpAround_Boxes = true;			
			boxMission.BoxesDropped = true;
		}
	}
	
	void OnTriggerEnter (Collider col)
	{
		if (col.name == "Pickup_Crate" && col.tag == "Pickup") {
			col.tag = "Untagged";
			numberOfBoxesCollected += 1;

			if(col.transform.childCount >0){
				for(int i = 0; i > col.transform.childCount; i++){
					col.transform.GetChild(i).GetComponent<Light>().enabled = false;
				}
			}
		}
	}
}
