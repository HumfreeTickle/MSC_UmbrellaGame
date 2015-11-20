using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class HorseRescue : MonoBehaviour
{

	public Transform[] destinations = new Transform[4];
//	public Transform destination1;
//	public Transform destination2;
//	public Transform destination3;
	private NavMeshAgent horse;
	public float timer;
	private Animator anim;
	private Transform Brolly;
	public bool atdestination;
	private float speed;
	private float run;
	public bool isMoving;
	private AudioClip Neigh;
	private AudioSource audio2;

	private Transform horse1Destinations;
	private Transform horse2Destinations;

	
	//public Transform currentDestination;// where is the horse now
	public Transform nextDestination;//where is the horse going next if moving forward
	public Transform lastDestination;//where was the horse before

	void Start ()
	{
		horse1Destinations = GameObject.Find("Horse1Destinations").transform;
		horse2Destinations = GameObject.Find("Horse2Destinations").transform;
		if(destinations.Length != 4){
			destinations = new Transform[4];
		}
		//currentDestination = destination0;//horse will start at 0
		if(this.name == "Horse1"){
			for(int child = 0; child < horse1Destinations.childCount; child++){
				destinations[child] = horse1Destinations.GetChild(child); 
			}

		}else if(this.name == "Horse2"){
			for(int child = 0; child < horse2Destinations.childCount; child++){
				destinations[child] = horse2Destinations.GetChild(child); 
			}
		}


		horse = gameObject.GetComponent<NavMeshAgent> ();
		horse.SetDestination (destinations[0].position);// make destination 0 its current destination
		timer = 500;
		audio2 = GetComponent<AudioSource>();
		Neigh = audio2.clip;

		anim = gameObject.GetComponent<Animator> ();
		nextDestination = destinations[1];
		Brolly = GameObject.Find ("main_Sphere").transform;

	}

	void Update ()
	{

		speed = Mathf.Clamp (speed, 0, Mathf.Infinity);
		anim.SetFloat ("SPEED", speed);

		run = Mathf.Clamp(run, 0, Mathf.Infinity);
		anim.SetFloat("SpeedRun",run);

		anim.SetFloat ("SpeedRun", run);

		if (Vector3.Distance (horse.transform.position, destinations[0].position) <= 5f) {
			nextDestination = destinations[1];
			speed--;
			run--;
			horse.tag ="Interaction";
			audio2.Stop();


		}

		if (Vector3.Distance (horse.transform.position, destinations[1].position) <= 5f) {
			lastDestination = destinations[0];
			nextDestination = destinations[2];
			speed--;
			run--;
			timer--;
			horse.tag ="Interaction";
			audio2.Stop();

		}

		if (Vector3.Distance (horse.transform.position, destinations[2].position) <= 5f) {
			lastDestination = destinations[1];
			nextDestination = destinations[3];
			speed--;
			run--;
			timer--;
			horse.tag ="Interaction";
			audio2.Stop();

		}

		if (Vector3.Distance (horse.transform.position, destinations[3].position) <= 5f) {
			speed = 0;

		}


		if (timer == 0) {
			horse.SetDestination (lastDestination.position);
			timer = 500;
			speed = 0;
			run = 80;
			horse.speed = 35;
			audio2.PlayOneShot(Neigh);

			if(Vector3.Distance(horse.transform.position, lastDestination.position) <= 35f){

				run=0;
				speed = 10;
				horse.speed = 10;

			}
		}

		if(Vector3.Distance(horse.transform.position, nextDestination.position) <= 35f){
			run=0;
			speed = 10;
			horse.speed = 10;
		}
	}

	void OnTriggerStay (Collider col)    
	{
		if (col.gameObject.tag == "Player") {
			if ((Input.GetButton ("Interact")) & (Vector3.Distance (horse.transform.position, Brolly.position) >= 5f)) {
				timer = 500;
				horse.SetDestination (nextDestination.position);
				run = 80;
				horse.speed= 35;
				audio2.PlayOneShot(Neigh);
			}
		}
	}
}