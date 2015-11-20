using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class SignBlow : MonoBehaviour {

	private Animator anim;
	public float timer;
	public bool swinging;
	public Rigidbody brolly;
	//private float theSpeed;
//	public Vector3 back;
//	public Vector3 forward;
	private AudioClip Creaky;
	private AudioSource audio2;



	// Use this for initialization
	void Start () {
		anim = gameObject.GetComponent<Animator> ();
		timer = 100;
		audio2 = GetComponent<AudioSource>();
		Creaky = audio2.clip;
		}
	
	// Update is called once per frame
	void Update () {

		if(timer <= 0){

			anim.SetBool("Swing", false);
			anim.SetBool("SwingBack", false);
			timer = 100;
			swinging = false;
			audio2.Stop();
		}
		if(swinging){

			timer--;
			audio2.PlayOneShot(Creaky);
		}

	}

	void OnTriggerEnter(Collider col){

		if((col.gameObject.tag == "Player")){

			if(brolly.velocity.x >= 0 ){


			//Debug.Log("Should Go Back");
			anim.SetBool("Swing", true);
			swinging = true;
			
			}
				
			if(brolly.velocity.x <= 1 ){
					
			//Debug.Log("Should Go forward");
			anim.SetBool("SwingBack", true);
			swinging = true;

			}
	
	}
}
}