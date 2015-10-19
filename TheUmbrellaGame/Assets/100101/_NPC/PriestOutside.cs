using UnityEngine;
using System.Collections;
using NPC;

public class PriestOutside : MonoBehaviour {

	private Animator anim;
	private NPC_FinalMission finalMission;
	private Transform spawnPoint;

	// Use this for initialization
	void Start () {
		anim= GetComponent<Animator>();
		spawnPoint = GameObject.Find("Priest_Spawn").transform;
		finalMission = GameObject.Find("Missions").GetComponent<NPC_FinalMission>();
	}
	void OnTriggerEnter(Collider other){

		if(other.gameObject.tag == "ChurchExit"){

			anim.SetBool("GoOutside", false);
			anim.enabled = false;
			transform.position = spawnPoint.position;
			finalMission.Outside = true;
			finalMission.X = 5;

		}
	}

}
