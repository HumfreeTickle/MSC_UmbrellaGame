using UnityEngine;
using System.Collections;
using NPC;

public class Boxes_dropOff : MonoBehaviour
{

	private int numberOfBoxesCollected;
	private int boxCount;
	private NPC_BoxesMission boxMission;
	private GmaeManage gameManager;
	public GameObject dropParticle;

	
	void Start ()
	{
		boxCount = GameObject.Find ("CratePickupCollection").transform.childCount; // the number of boxes in the mission
		boxMission = GameObject.Find ("Missions").GetComponent<NPC_BoxesMission> ();
		gameManager = GameObject.Find ("Follow Camera").GetComponent<GmaeManage> ();

		if(!dropParticle){
			Debug.LogError("No Particles - Boxes");
		}
	}

	void Update ()
	{
		if (gameManager.missionState == MissionController.BoxesMission) {
			if (boxMission.boxesMission && boxMission.boxes_X == 0) {
				GetComponent<MeshRenderer> ().enabled = true;
			} 


			if (numberOfBoxesCollected >= boxCount && !boxMission.boxesDropped) {
				boxMission.boxesDropped = true;
				boxMission.jumpAround_Boxes = true;
				boxMission.boxesGuy.tag = "NPC_talk";
				GetComponent<MeshRenderer> ().enabled = false;

			}
		} else {
			GetComponent<MeshRenderer> ().enabled = false;

		}
	}
	
	void OnTriggerEnter (Collider col)
	{
		if (col.name == "Pickup_Crate" && col.tag == "Pickup") {
			col.tag = "Untagged";
			numberOfBoxesCollected += 1;
			Instantiate(dropParticle,col.gameObject.transform.position, Quaternion.identity);

			if (col.transform.childCount > 0) {
				for (int i = 0; i > col.transform.childCount; i++) {
					col.transform.GetChild (i).GetComponent<Light> ().enabled = false;
				}
			}
		}
	}
}
