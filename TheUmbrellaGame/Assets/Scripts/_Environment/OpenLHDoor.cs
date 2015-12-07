using UnityEngine;
using System.Collections;

public class OpenLHDoor : MonoBehaviour
{
	public Animator Door;

	void OnTriggerEnter (Collider other)
	{
		if (other.gameObject.tag == "NPC") {
			Door.SetBool ("DoorOpen", true);
		}
	}

	void OnTriggerExit (Collider other)
	{
		if (other.gameObject.tag == "NPC") {
			Door.SetBool ("DoorOpen", false);			
		}
	}
}