using UnityEngine;
using System.Collections;

public class WaterFallRainSFX : MonoBehaviour {

	private AudioSource gameObjectAudio;

	void Start () {
		gameObjectAudio = GetComponent<AudioSource>();
	}
	

	void OnTriggerEnter(Collider col){
		if (col.gameObject.tag == "Player") {
			gameObjectAudio.Play();
		}	
	}
	void OnTriggerExit(Collider col){
		if (col.gameObject.tag == "Player") {
			gameObjectAudio.Stop();
		}

	}

}
