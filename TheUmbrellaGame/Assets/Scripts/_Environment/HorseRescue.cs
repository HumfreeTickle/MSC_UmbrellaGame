using UnityEngine;
using System.Collections;
using UnityEngine.Audio;
using UnityEngine.UI;

public class HorseRescue : MonoBehaviour
{

	public Transform[] destinations = new Transform[4];
	private GmaeManage gameManager;
	private NavMeshAgent horse;
	public float timer;
	private Animator anim;
	private Transform Brolly;
	public bool atdestination;
	public float speed;
	public float run;
	public bool isMoving;

	[SerializeField]
	private AudioClip Neigh;
	[SerializeField]

	private AudioClip Gallop;
	private AudioSource audio2;
	private bool play = true;
	private Transform horse1Destinations;
	private Transform horse2Destinations;
	private float clipTimer;

	public AudioClip Idle;
	public AudioClip Running;

	public AudioMixerSnapshot Stand;
	public AudioMixerSnapshot Run;


	
	//public Transform currentDestination;// where is the horse now
	public Transform nextDestination;//where is the horse going next if moving forward
	public Transform lastDestination;//where was the horse before

	void Start ()
	{
		gameManager = GameObject.Find ("Follow Camera").GetComponent<GmaeManage> ();

		clipTimer = Time.time;
		horse1Destinations = GameObject.Find ("Horse1Destinations").transform;
		horse2Destinations = GameObject.Find ("Horse2Destinations").transform;
		if (destinations.Length != 4) {
			destinations = new Transform[4];
		}
		//currentDestination = destination0;//horse will start at 0
		if (this.name == "Horse1") {
			for (int child = 0; child < horse1Destinations.childCount; child++) {
				destinations [child] = horse1Destinations.GetChild (child); 
			}

		} else if (this.name == "Horse2") {
			for (int child = 0; child < horse2Destinations.childCount; child++) {
				destinations [child] = horse2Destinations.GetChild (child); 
			}
		}


		horse = gameObject.GetComponent<NavMeshAgent> ();
		horse.SetDestination (destinations [0].position);

		timer = 500;
		audio2 = GetComponent<AudioSource> ();
		//Idle = audio2.clip;

		anim = gameObject.GetComponent<Animator> ();
		nextDestination = destinations [1];
		Brolly = GameObject.Find ("main_Sphere").transform;

		Stand.TransitionTo(0.5f);
	}

	void Update ()
	{
		if (gameManager.MissionState == MissionController.HorsesMission) {
			horse.tag = "Interaction";
		} 
		else {
			horse.tag = "Untagged";
		}


		speed = Mathf.Clamp (speed, 0, Mathf.Infinity);
		anim.SetFloat ("SPEED", speed);

		run = Mathf.Clamp (run, 0, Mathf.Infinity);
		anim.SetFloat ("SpeedRun", run);

		if (Vector3.Distance (horse.transform.position, destinations [0].position) <= 5f) {
			nextDestination = destinations [1];
			speed--;
			run--;
			Stand.TransitionTo(0.1f);
//			audio2.PlayOneShot(Idle);
		}

		if (Vector3.Distance (horse.transform.position, destinations [1].position) <= 5f) {
			lastDestination = destinations [0];
			nextDestination = destinations [2];
			speed--;
			run--;
			timer--;
			//audio2.clip = Idle;
			Stand.TransitionTo(0.1f);
//			audio2.PlayOneShot(Idle);
		}

		if (Vector3.Distance (horse.transform.position, destinations [2].position) <= 5f) {
			lastDestination = destinations [1];
			nextDestination = destinations [3];
			speed--;
			run--;
			timer--;
			Stand.TransitionTo(0.1f);
			//audio2.clip = Idle;
//			audio2.PlayOneShot(Idle);
		}

		if (Vector3.Distance (horse.transform.position, destinations [3].position) <= 5f) {
			speed = 0;
			run = 0;
			timer = 500;
			Stand.TransitionTo(0.1f);
			//audio2.clip = Idle;
//			audio2.PlayOneShot(Idle);
		}


		if (timer == 0) {
			horse.SetDestination (lastDestination.position);
			timer = 500;
			speed = 0;
			run = 80;
			horse.speed = 35;
			Run.TransitionTo(0.1f);
			if (play) {
				Debug.Log ("Played");
//				audio2.PlayOneShot (Neigh);
				play = false;
			}

			if (Vector3.Distance (horse.transform.position, lastDestination.position) <= 35f) {
				run = 0;
				speed = 10;
				horse.speed = 10;
			}
		}

		if (Vector3.Distance (horse.transform.position, nextDestination.position) <= 35f) {
			run = 0;
			speed = 10;
			horse.speed = 10;
		}

//		if ((Time.time - clipTimer) > 8f && Vector3.Distance (Brolly.position, this.transform.position) <= 20) {
//			clipTimer = Time.time;
//
//			if (speed <= 0) {
//				audio2.PlayOneShot (Neigh);
//				Invoke ("", audio2.clip.length);
//
//			} else {
//				audio2.PlayOneShot (Gallop);
//			}
//		}
	}

	void OnTriggerStay (Collider col)
	{
		if (col.gameObject.tag == "Player") {
			if ((Input.GetButton (gameManager.controllerInteract)) & (Vector3.Distance (horse.transform.position, Brolly.position) <= 20f)) {
				timer = 500;
				horse.SetDestination (nextDestination.position);
				run = 80;
				horse.speed = 35;
				Run.TransitionTo(0.1f);

//				audio2.PlayOneShot(Running);

			}
		}
	}
	
}