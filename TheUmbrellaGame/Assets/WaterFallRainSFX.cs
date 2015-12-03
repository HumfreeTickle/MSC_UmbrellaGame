using UnityEngine;
using System.Collections;

public class WaterFallRainSFX : MonoBehaviour {

	private AudioClip Rain;
	private AudioSource audio2;

	// Use this for initialization
	void Start () {
	
		audio2 = GetComponent<AudioSource>();
		Rain = audio2.clip;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnTriggerEnter(Collider col){

		if (col.gameObject.tag == "Player") {

		
			audio2.Play();
		}
	

	}
	void OnTriggerExit(Collider col){

		if (col.gameObject.tag == "Player") {
			

			audio2.Stop();
		}

	}

}
