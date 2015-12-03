using UnityEngine;
using System.Collections;

public class DropTheBridge : MonoBehaviour
{
	public bool drop{ private get; set; }

	private Animator animator;
	private AudioClip BridgeDrop;
	private AudioSource audio2;

	void Start ()
	{
		animator = GetComponent<Animator> ();
		if (Application.loadedLevelName == "Boucing") {
			audio2 = GetComponent<AudioSource> ();
			BridgeDrop = audio2.clip;
		}
	}
	
	void Update ()
	{
		if (drop) {
			animator.SetBool ("Fixed", false);
			if (audio2 != null) {
				audio2.PlayOneShot (BridgeDrop);
			}
		}
	}
}