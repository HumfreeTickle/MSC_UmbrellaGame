using UnityEngine;
using System.Collections;
using NPC;

public class PriestOutside : MonoBehaviour {

	private Animator anim;
	private AudioClip OPEN;
	private AudioSource audio2;

	private NPC_FinalMission finalMission;
	private Transform spawnPoint;

	// Use this for initialization
	void Start () {
		anim= GetComponent<Animator>();
		spawnPoint = GameObject.Find("Priest_Spawn").transform;
		finalMission = GameObject.Find("Missions").GetComponent<NPC_FinalMission>();
		audio2 = GetComponent<AudioSource>();
		OPEN = audio2.clip;
	}
	void OnTriggerEnter(Collider other){

		if(other.gameObject.tag == "ChurchExit"){
			this.gameObject.tag = "NPC_talk";
			audio2.PlayOneShot (OPEN);
			finalMission.jumpAround_Final = true;
			anim.SetBool("GoOutside", false);
			anim.enabled = false;
			transform.position = spawnPoint.position;
			finalMission.outside = true;
			finalMission.final_X = 4;

		}
	}
}
