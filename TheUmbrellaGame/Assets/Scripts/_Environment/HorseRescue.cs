using UnityEngine;
using System.Collections;
using UnityEngine.Audio;
using UnityEngine.UI;

public class HorseRescue : MonoBehaviour
{
	public Transform[] destinations = new Transform[4];
	private GmaeManage gameManager;
	private NavMeshAgent horseNav;
	public float timer;
	private Animator horseAnim;
	private Transform umbrellaTr;
	public bool atdestination;
	public float speed;
	public float run;
	public bool isMoving;

	[SerializeField]
	private AudioClip neighSFX;
	[SerializeField]
	private AudioClip gallopSFX;

	private AudioSource gameObjectAudio;
	private bool play = true;
	private Transform horse1Destinations;
	private Transform horse2Destinations;
	private float clipTimer;

	public AudioClip Idle;
	public AudioClip Running;

	public AudioMixerSnapshot Stand;
	public AudioMixerSnapshot Run;
	
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

		if (this.name == "Horse1") {
			for (int child = 0; child < horse1Destinations.childCount; child++) {
				destinations [child] = horse1Destinations.GetChild (child); 
			}

		} else if (this.name == "Horse2") {
			for (int child = 0; child < horse2Destinations.childCount; child++) {
				destinations [child] = horse2Destinations.GetChild (child); 
			}
		}

		horseNav = gameObject.GetComponent<NavMeshAgent> ();
		horseNav.SetDestination (destinations [0].position);

		timer = 500;
		gameObjectAudio = GetComponent<AudioSource> ();

		horseAnim = gameObject.GetComponent<Animator> ();
		nextDestination = destinations [1];
		umbrellaTr = GameObject.Find ("main_Sphere").transform;

		Stand.TransitionTo(0.5f);
	}

	void Update ()
	{
		if (gameManager.missionState == MissionController.HorsesMission) {
			horseNav.tag = "Interaction";
		} 
		else {
			horseNav.tag = "Untagged";
		}

		speed = Mathf.Clamp (speed, 0, Mathf.Infinity);
		horseAnim.SetFloat ("SPEED", speed);

		run = Mathf.Clamp (run, 0, Mathf.Infinity);
		horseAnim.SetFloat ("SpeedRun", run);

		if (Vector3.Distance (horseNav.transform.position, destinations [0].position) <= 5f) {
			nextDestination = destinations [1];
			speed--;
			run--;
			Stand.TransitionTo(0.1f);
		}

		if (Vector3.Distance (horseNav.transform.position, destinations [1].position) <= 5f) {
			lastDestination = destinations [0];
			nextDestination = destinations [2];
			speed--;
			run--;
			timer--;
			Stand.TransitionTo(0.1f);
		}

		if (Vector3.Distance (horseNav.transform.position, destinations [2].position) <= 5f) {
			lastDestination = destinations [1];
			nextDestination = destinations [3];
			speed--;
			run--;
			timer--;
			Stand.TransitionTo(0.1f);
		}

		if (Vector3.Distance (horseNav.transform.position, destinations [3].position) <= 5f) {
			speed = 0;
			run = 0;
			timer = 500;
			Stand.TransitionTo(0.1f);
		}


		if (timer == 0) {
			horseNav.SetDestination (lastDestination.position);
			timer = 500;
			speed = 0;
			run = 80;
			horseNav.speed = 35;
			Run.TransitionTo(0.1f);
			if (play) {
				play = false;
			}

			if (Vector3.Distance (horseNav.transform.position, lastDestination.position) <= 35f) {
				run = 0;
				speed = 10;
				horseNav.speed = 10;
			}
		}

		if (Vector3.Distance (horseNav.transform.position, nextDestination.position) <= 35f) {
			run = 0;
			speed = 10;
			horseNav.speed = 10;
		}
	}

	void OnTriggerStay (Collider col)
	{
		if (col.gameObject.tag == "Player") {
			if ((Input.GetButton (gameManager.controllerInteract)) & (Vector3.Distance (horseNav.transform.position, umbrellaTr.position) <= 20f)) {
				timer = 500;
				horseNav.SetDestination (nextDestination.position);
				run = 80;
				horseNav.speed = 35;
				Run.TransitionTo(0.1f);
			}
		}
	}
	
}