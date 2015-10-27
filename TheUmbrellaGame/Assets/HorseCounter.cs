using UnityEngine;
using System.Collections;
using NPC;

public class HorseCounter : MonoBehaviour
{
	private HorseMission_BackEnd horseMission;
	public int numberOfHorseHome;
	public Animator GateClose;

<<<<<<< HEAD

	// Use this for initialization
	void Start (){
	
		numberOfHorseHome = 0;

=======
	void Start(){
		horseMission = GameObject.Find("Missions").GetComponent<HorseMission_BackEnd>();
>>>>>>> origin/master
	}

	void Update (){

		if (numberOfHorseHome >= 2) {

			GateClose.SetBool("Close", true);
			horseMission.HorseReturned = true;
		}
	
	}

	void OnTriggerEnter (Collider other){

<<<<<<< HEAD
		if (other.gameObject.tag == "Interaction") {
			numberOfHorseHome +=1;
=======
<<<<<<< HEAD
		if (other.gameObject.tag == "Interaction") {

			numberOfHorseHome ++;
=======
		if (other.gameObject.tag == "Horsey") {
			numberOfHorseHome +=1;
>>>>>>> origin/master
			Debug.Log ("HorseHome");
>>>>>>> origin/master
		}
	}
}