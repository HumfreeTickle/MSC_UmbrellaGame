using UnityEngine;
using System.Collections;

public class BoatRocking : MonoBehaviour {
	private Animator anim;
	public float timer;
	public bool Rock;
	private AudioClip Splash;
	private AudioSource audio2;


	// Use this for initialization
	void Start () {
		timer = 500;
		anim = gameObject.GetComponent<Animator> ();
		Rock = false;
		audio2 = GetComponent<AudioSource>();
		Splash = audio2.clip;
		//brolly = GameObject.Find ("main_Sphere").GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {
	
		if(timer <= 0){

			timer = 500;
			anim.SetBool("Rocking", false);
			Rock = false;
		}

		if(Rock == true){
			timer --;

		}
	}
	void OnTriggerEnter (Collider col)
	{
		if ((col.gameObject.tag == "Player")) {
			anim.SetBool ("Rocking", true);
			Rock = true;
			audio2.PlayOneShot (Splash);

		}
	}
}
