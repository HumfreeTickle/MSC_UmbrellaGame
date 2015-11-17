using UnityEngine;
using System.Collections;
using NPC;

public class HorseCounter : MonoBehaviour
{
	private HorseMission_BackEnd horseMission;
	private int numberOfHorseHome;
	private Animator GateClose;
	public GameObject dropParticle;
	private GmaeManage gameManager;
	
	void Start ()
	{
		horseMission = GameObject.Find ("Missions").GetComponent<HorseMission_BackEnd> ();
		if (!dropParticle) {
			Debug.LogError ("No Particles - Horse");
		}
		GateClose = GameObject.Find("GateCloser").GetComponent<Animator>();
		gameManager = GameObject.Find ("Follow Camera").GetComponent<GmaeManage> ();
	}

	void Update ()
	{
		if (gameManager.MissionState == MissionController.HorsesMission) {
			if (numberOfHorseHome >= 2) {
				GateClose.SetBool ("Close", true);
				horseMission.horseReturned = true;
				horseMission.jumpAround_Horses = true;
				horseMission.horseGuy.tag = "NPC_talk";
				GetComponent<MeshRenderer>().enabled = false;
			}
		}
	
	}

	void OnTriggerEnter (Collider col)
	{
		if (col.gameObject.tag == "Interaction") {
			print (col.name);
			numberOfHorseHome += 1;
			Instantiate (dropParticle, col.gameObject.transform.position, Quaternion.identity);

			if (col.transform.FindChild ("Activate").GetComponent<Light> ()) {
				col.transform.FindChild ("Activate").GetComponent<Light> ().enabled = false;
			}

		}
	}
}