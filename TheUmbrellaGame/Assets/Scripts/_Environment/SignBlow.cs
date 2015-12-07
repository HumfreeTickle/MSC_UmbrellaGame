using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class SignBlow : MonoBehaviour
{
	private Animator anim;
	private float timer;
	private bool swinging;
	private Rigidbody umbrellaRb;
	private AudioClip swingingSFX;
	private AudioSource gameObjectAudio;

	void Start ()
	{
		anim = gameObject.GetComponent<Animator> ();

		timer = 100;
		gameObjectAudio = GetComponent<AudioSource> ();

		timer = 10;
		if (GameObject.Find ("main_Sphere")) {
			umbrellaRb = GameObject.Find ("main_Sphere").GetComponent<Rigidbody> ();
		}
		gameObjectAudio = GetComponent<AudioSource> ();
		swingingSFX = gameObjectAudio.clip;
	}
	
	void Update ()
	{
		if (timer <= 0) {
			anim.SetBool ("Swing", false);
			anim.SetBool ("SwingBack", false);
			swinging = false;
			timer = 10;

		}
		if (swinging) {
			timer--;
		}
	}

	void OnTriggerEnter (Collider col)
	{
		if ((col.gameObject.tag == "Player")) {
		
			if (umbrellaRb.velocity.x > 1) {
				anim.SetBool ("Swing", true);
				swinging = true;
				gameObjectAudio.PlayOneShot (swingingSFX);
			}
				
			if (umbrellaRb.velocity.x < -1) {
				anim.SetBool ("SwingBack", true);
				swinging = true;
				gameObjectAudio.PlayOneShot (swingingSFX);
			}
	
		}
	}
}