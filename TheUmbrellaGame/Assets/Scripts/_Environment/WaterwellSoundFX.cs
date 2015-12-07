using UnityEngine;
using System.Collections;

public class WaterwellSoundFX : MonoBehaviour {

	private AudioClip Pouring;
	private AudioSource gameObjectAudio;

	void Start () 
	{
		gameObjectAudio = GetComponent<AudioSource>();
		Pouring = gameObjectAudio.clip;
	}

	void OnTriggerEnter(Collider other){

		if (other.gameObject.tag == "NPC"){
			gameObjectAudio.PlayOneShot (Pouring);
		}
	}
}
