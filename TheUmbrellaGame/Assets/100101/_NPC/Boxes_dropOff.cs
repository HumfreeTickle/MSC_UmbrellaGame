using UnityEngine;
using System.Collections;
using NPC;

public class Boxes_dropOff : MonoBehaviour {

	public int numberOfBoxesCollected;
	private NPC_BoxesMission boxMission;

	void Start () 
	{
		boxMission = GameObject.Find("Missions").GetComponent<NPC_BoxesMission>();
	}

	void Update(){
		if(numberOfBoxesCollected >= 4 && !boxMission.BoxesDropped){
			boxMission.JumpAround_Boxes = true;			
			boxMission.BoxesDropped = true;
		}
	}
	
	void OnTriggerEnter(Collider col)
	{
		if(col.name == "Pickup_Crate" && col.tag == "Pickup"){
			col.tag = "Untagged";
			numberOfBoxesCollected += 1;
			print ("got: " + numberOfBoxesCollected);

		}
	}
}
