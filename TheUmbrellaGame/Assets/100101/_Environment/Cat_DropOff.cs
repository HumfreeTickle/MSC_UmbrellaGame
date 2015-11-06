using UnityEngine;
using System.Collections;
using NPC;

public class Cat_DropOff : MonoBehaviour {

	private NPC_CatMission missions;
	private SphereCollider catGuy;
	public GameObject dropParticle;
	private bool droppedOff;

	void Start () 
	{
		catGuy = GameObject.Find("NPC_Cat").GetComponent<SphereCollider>();
		missions = GameObject.Find ("Missions").GetComponent<NPC_CatMission>();
		if(!dropParticle){
			Debug.LogError("No Particles - Cat");
		}
	}
	
	void OnTriggerEnter(Collider col) 
	{
		if(col.gameObject.name == "kitten"){

			if(!droppedOff){
				Instantiate(dropParticle,col.gameObject.transform.position, Quaternion.identity);
				GetComponent<MeshRenderer>().enabled = false;
				droppedOff = true;
				missions.jumpAround_Cat = true;
			}

			catGuy.enabled = true;
			missions.NPC_DropOff.tag = "NPC_talk";
			missions.catDroppedOff = true;
		}
	}
}
