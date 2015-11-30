using UnityEngine;
using System.Collections;

public class DropTheBridge : MonoBehaviour
{
	public bool drop{private get;set;}
	private Animator animator;
	private AudioClip BridgeDrop;
	private AudioSource audio2;

	void Awake ()
	{
		animator = GetComponent<Animator> ();
		audio2 = GetComponent<AudioSource>();
		BridgeDrop = audio2.clip;
	}
	
	void Update ()
	{
		if (drop)
		animator.SetBool ("Fixed", false);
		audio2.PlayOneShot (BridgeDrop);
	}
}