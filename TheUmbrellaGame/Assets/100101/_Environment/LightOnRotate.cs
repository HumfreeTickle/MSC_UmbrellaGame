using UnityEngine;
using System.Collections;
using Environment;

public class LightOnRotate : MonoBehaviour {

	public bool lightHerUp{get; set;}
	public Material lighting;

	void Update () {

		if(lightHerUp){
			transform.GetChild(0).gameObject.SetActive (true);// = true;
			transform.Rotate(0, 15* Time.deltaTime, 0);
			GetComponent<MeshRenderer>().material = lighting;

		}else{
			transform.GetChild(0).gameObject.SetActive (false);
		}
	}
}
