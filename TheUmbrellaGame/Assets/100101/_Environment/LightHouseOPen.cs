using UnityEngine;
using System.Collections;

public class LightHouseOPen : MonoBehaviour
{
	private AudioClip Open;
	private AudioSource audio2;
	public Animator Door;

	// Use this for initialization

	void Start(){

		audio2 = GetComponent<AudioSource>();
		Open = audio2.clip;
	}

	void OnTriggerEnter (Collider other)
	{

		if (other.gameObject.tag == "NPC") {

			Door.SetBool ("DoorOpen", true);
			audio2.PlayOneShot (Open);
		}

	}

	void OnTriggerExit (Collider other)
	{

		if (other.gameObject.tag == "NPC") {
			
			Door.SetBool ("DoorOpen", false);
			audio2.PlayOneShot (Open);

		}
	}
}

