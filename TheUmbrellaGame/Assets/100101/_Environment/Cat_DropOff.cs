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
	}
	
	void OnTriggerEnter(Collider col) 
	{
		if(col.gameObject.name == "kitten"){

			if(!droppedOff){
				Instantiate(dropParticle,col.gameObject.transform.position, Quaternion.identity);
				GetComponent<MeshRenderer>().enabled = false;
				droppedOff = true;
			}
			catGuy.enabled = true;
			missions.DropOff.tag = "NPC_talk";
			missions.CatDroppedOff = true;
			col.gameObject.tag = "Untagged";
		}
	}
}
