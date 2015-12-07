using UnityEngine;
using System.Collections;

public class LightHouseOPen : MonoBehaviour
{
	private AudioClip SFX;
	private AudioSource gameObjectAudio;
	public Animator Door;

	void Start ()
	{
		if (Application.loadedLevelName == "Boucing") {
			gameObjectAudio = GetComponent<AudioSource> ();
			SFX = gameObjectAudio.clip;
		}
	}

	void OnTriggerEnter (Collider other)
	{
		if (other.gameObject.tag == "NPC") {
			Door.SetBool ("DoorOpen", true);
			if (gameObjectAudio != null) {
				gameObjectAudio.PlayOneShot (SFX);
			}
		}
	}

	void OnTriggerExit (Collider other)
	{
		if (other.gameObject.tag == "NPC") {
			
			Door.SetBool ("DoorOpen", false);
			if (gameObjectAudio != null) {
				gameObjectAudio.PlayOneShot (SFX);
			}
		}
	}
}

