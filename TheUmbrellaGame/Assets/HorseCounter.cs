using UnityEngine;
using System.Collections;
using NPC;

public class HorseCounter : MonoBehaviour
{
	private HorseMission_BackEnd horseMission;
	public int numberOfHorseHome;
	public Animator GateClose;

	void Start(){
		horseMission = GameObject.Find("Missions").GetComponent<HorseMission_BackEnd>();
	}

	void Update (){

		if (numberOfHorseHome >= 2) {

			Debug.Log ("Completed");
			GateClose.SetBool("Close", true);
//			horseMission.Horses_X = 3;
			horseMission.HorseReturned = true;

		}
	
	}

	void OnTriggerEnter (Collider other){

		if (other.gameObject.tag == "Horsey") {
			numberOfHorseHome +=1;
			Debug.Log ("HorseHome");
		}
	}
}