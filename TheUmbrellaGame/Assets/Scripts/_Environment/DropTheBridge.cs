using UnityEngine;
using System.Collections;

public class DropTheBridge : MonoBehaviour
{
	public bool drop{ private get; set; }
	private Animator animator;
	private AudioClip BridgeDrop;
	private AudioSource gameObjectAudio;

	void Start ()
	{
		animator = GetComponent<Animator> ();
		if (Application.loadedLevelName == "Boucing") {
			gameObjectAudio = GetComponent<AudioSource> ();
			BridgeDrop = gameObjectAudio.clip;
		}
	}
	
	void Update ()
	{
		if (drop) {
			animator.SetBool ("Fixed", false);
			if (gameObjectAudio != null) {
				gameObjectAudio.PlayOneShot (BridgeDrop);
			}
		}
	}
}