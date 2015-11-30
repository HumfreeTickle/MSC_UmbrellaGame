using UnityEngine;
using System.Collections;

public class WaterwellSoundFX : MonoBehaviour {

	private AudioClip Pouring;
	private AudioSource audio2;

	// Use this for initialization
	void Start () {
	
		audio2 = GetComponent<AudioSource>();
		Pouring = audio2.clip;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnTriggerEnter(Collider other){

		if (other.gameObject.tag == "NPC"){

			Debug.Log("Pour");
			audio2.PlayOneShot (Pouring);
		}
	}
}
