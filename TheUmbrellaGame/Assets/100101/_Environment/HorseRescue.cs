using UnityEngine;
using System.Collections;

public class HorseRescue : MonoBehaviour
{

	public Transform destination0;
	public Transform destination1;
	public Transform destination2;
	public Transform destination3;
	private NavMeshAgent horse;
	public float timer;
	private Animator anim;
	private Transform Brolly;
	public bool atdestination;
	private float speed;
	private float run;
	public bool isMoving;
	
	//public Transform currentDestination;// where is the horse now
	public Transform nextDestination;//where is the horse going next if moving forward
	public Transform lastDestination;//where was the horse before

	void Start ()
	{
		//currentDestination = destination0;//horse will start at 0
		horse = gameObject.GetComponent<NavMeshAgent> ();
		horse.SetDestination (destination0.position);// make destination 0 its current destination
		timer = 500;

		anim = gameObject.GetComponent<Animator> ();
		nextDestination = destination1;
		Brolly = GameObject.Find ("main_Sphere").transform;

		anim = gameObject.GetComponent<Animator>();
		nextDestination= destination1;
	}
	
	// Update is called once per frame
	void Update ()
	{
		speed = Mathf.Clamp (speed, 0, Mathf.Infinity);
		anim.SetFloat ("SPEED", speed);

		//anim.SetFloat ("SpeedRun", run);

		run = Mathf.Clamp(run, 0, Mathf.Infinity);
		anim.SetFloat("SpeedRun",run);

		if (Vector3.Distance (horse.transform.position, destination0.position) <= 5f) {
			nextDestination = destination1;
			speed--;
			run--;
			horse.tag ="Interaction";

		}

		if (Vector3.Distance (horse.transform.position, destination1.position) <= 5f) {
			lastDestination = destination0;
			nextDestination = destination2;
			speed--;
			run--;
			timer--;
			horse.tag ="Interaction";

		}

		if (Vector3.Distance (horse.transform.position, destination2.position) <= 5f) {
			lastDestination = destination1;
			nextDestination = destination3;
			speed--;
			run--;
			timer--;
			horse.tag ="Interaction";

		}

		if (Vector3.Distance (horse.transform.position, destination3.position) <= 5f) {
			speed = 0;
		}


		if (timer == 0) {
			horse.SetDestination (lastDestination.position);
			timer = 500;
			speed = 0;
			run = 80;
			horse.speed = 25;

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



			}
		}
	}
}