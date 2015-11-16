﻿using UnityEngine;
using System.Collections;
using NPC;

public class KeeperDrop : MonoBehaviour {

	private NPC_FinalMission xChange;

	void Start () 
	{
		xChange = GameObject.Find("Missions").GetComponent<NPC_FinalMission>();
	}
	
	void OnTriggerEnter(Collider col){

		if(col.gameObject.name == "NPC_LightHouseKeeper"){
			GetComponent<MeshRenderer>().enabled = false;
			transform.GetChild(0).gameObject.SetActive(false);

			col.tag = "NPC";
			col.transform.rotation = Quaternion.identity;
			if (col.GetComponent<Rigidbody> ()) {
				col.GetComponent<Rigidbody> ().freezeRotation = true;
			}

			xChange.final_X = 12;
		}
	}
}
