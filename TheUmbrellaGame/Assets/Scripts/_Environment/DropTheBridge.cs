using UnityEngine;
using System.Collections;

public class DropTheBridge : MonoBehaviour
{
	//---------- used at the end of the game to bring the bridge down -----------//

	public bool drop{ private get; set; }
	private Animator animator;
	private AudioClip BridgeDrop;
	private AudioSource gameObjectAudio;
	public bool playSFX{get;set;}

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
				if(playSFX){
				gameObjectAudio.PlayOneShot (BridgeDrop);
					playSFX = false;
				}
			}
		}
	}
}