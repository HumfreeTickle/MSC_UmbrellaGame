using UnityEngine;
using System.Collections;

public class SignBlow : MonoBehaviour {

	private Animator anim;
	public float timer;
	public bool swinging;
	public Rigidbody brolly;
	//private float theSpeed;
	public Vector3 back;
	public Vector3 forward;



	// Use this for initialization
	void Start () {
		anim = gameObject.GetComponent<Animator> ();
		timer = 10;
		}
	
	// Update is called once per frame
	void Update () {

		if(timer <= 0){

			anim.SetBool("Swing", false);
			anim.SetBool("SwingBack", false);
			timer = 100;
			swinging = false;
		}
		if(swinging){

			timer--;
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