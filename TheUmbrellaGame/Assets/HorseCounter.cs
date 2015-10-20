using UnityEngine;
using System.Collections;

public class HorseCounter : MonoBehaviour
{

	public int numberOfHorseHome;
	public Animator GateClose;


	// Use this for initialization
	void Start (){
	
		numberOfHorseHome = 0;

	}
	
	// Update is called once per frame
	void Update (){

		if (numberOfHorseHome >= 2) {

			Debug.Log ("Completed");
			GateClose.SetBool("Close", true);
		}
	
	}

	void OnTriggerEnter (Collider other){

		if (other.gameObject.tag == "Interaction") {

			numberOfHorseHome ++;
			Debug.Log ("HorseHome");
		}
	}
}