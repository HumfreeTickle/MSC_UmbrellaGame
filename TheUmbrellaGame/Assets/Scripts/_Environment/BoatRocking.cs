using UnityEngine;
using System.Collections;

public class BoatRocking : MonoBehaviour {
	private Animator boatAnim;
	private float _timer;
	private bool moveBoat;
	private AudioClip splashSFX;
	private AudioSource gameObjectAudio;
	
	void Start () {
		_timer = 500;
		boatAnim = gameObject.GetComponent<Animator> ();
		moveBoat = false;
		gameObjectAudio = GetComponent<AudioSource>();
		splashSFX = gameObjectAudio.clip;
	}
	
	void Update () {
	
		if(_timer <= 0){
			_timer = 500;
			boatAnim.SetBool("Rocking", false);
			moveBoat = false;
		}

		if(moveBoat == true){
			_timer --;
		}
	}
	void OnTriggerEnter (Collider col)
	{
		if ((col.gameObject.tag == "Player")) {
			boatAnim.SetBool ("Rocking", true);
			moveBoat = true;
			gameObjectAudio.PlayOneShot (splashSFX);
		}
	}
}
