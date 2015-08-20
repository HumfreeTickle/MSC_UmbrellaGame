using UnityEngine;
using System.Collections;
using Environment;

public class LightOnRotate : MonoBehaviour {

	public _CycleDayNight sun;
	public DayPhase currentPhase;

	void Update () {

		if(!sun){
			return;
		}

		currentPhase = sun.CurrentPhase;

		if(currentPhase == DayPhase.Night){
			transform.GetChild(0).gameObject.SetActive (true);// = true;
		}else{
			transform.GetChild(0).gameObject.SetActive (false);
		}
		transform.Rotate(0, 15* Time.deltaTime, 0);
	}
}
