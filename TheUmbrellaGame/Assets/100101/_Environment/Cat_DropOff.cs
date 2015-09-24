using UnityEngine;
using System.Collections;
using NPC;

public class Cat_DropOff : MonoBehaviour {

	private NPC_CatMission missions;

	void Start () 
	{
		missions = GameObject.Find ("Missions").GetComponent<NPC_CatMission>();
	}
	
	void OnTriggerStay(Collider col) 
	{
		if(col.gameObject.name == "kitten" && col.gameObject.tag == "Pickup"){
			missions.DropOff.tag = "NPC_talk";

			missions.CatDroppedOff = true;
			col.gameObject.tag = "Untagged";
		}
	}
}
