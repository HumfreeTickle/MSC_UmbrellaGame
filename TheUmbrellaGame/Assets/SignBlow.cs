using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class SignBlow : MonoBehaviour
{

	private Animator anim;
	private float timer;
	private bool swinging;
	private Rigidbody brolly;
	private AudioClip Creaky;
	private AudioSource audio2;



	// Use this for initialization
	void Start ()
	{
		anim = gameObject.GetComponent<Animator> ();

		timer = 100;
		audio2 = GetComponent<AudioSource> ();

		timer = 10;
		if (GameObject.Find ("main_Sphere")) {
			brolly = GameObject.Find ("main_Sphere").GetComponent<Rigidbody> ();
		}
		audio2 = GetComponent<AudioSource> ();

		Creaky = audio2.clip;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (timer <= 0) {
			anim.SetBool ("Swing", false);
			anim.SetBool ("SwingBack", false);
			swinging = false;
<<<<<<< HEAD
<<<<<<< HEAD
			//audio2.Stop();
=======
			audio2.Stop ();
>>>>>>> origin/master
=======
			audio2.Stop ();
>>>>>>> origin/master

			timer = 10;

		}
		if (swinging) {
			timer--;
		}

	}

	void OnTriggerEnter (Collider col)
	{
		if ((col.gameObject.tag == "Player")) {
		
			if (brolly.velocity.x > 1) {
				anim.SetBool ("Swing", true);
				swinging = true;
				audio2.PlayOneShot (Creaky);
			}
				
			if (brolly.velocity.x < -1) {
				anim.SetBool ("SwingBack", true);
				swinging = true;
				audio2.PlayOneShot (Creaky);

			}
	
		}
	}
}