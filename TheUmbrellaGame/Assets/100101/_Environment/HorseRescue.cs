using UnityEngine;
using System.Collections;

public class HorseRescue : MonoBehaviour {

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

	// Use this for initialization
	void Start () {
		//currentDestination = destination0;//horse will start at 0
		horse = gameObject.GetComponent<NavMeshAgent>();
		horse.SetDestination(destination0.position);// make destination 0 its current destination
		timer = 500;
		anim = gameObject.GetComponent<Animator>();
		nextDestination= destination1;
<<<<<<< HEAD
	
=======
		Brolly = GameObject.Find("main_Sphere").transform;
>>>>>>> origin/master
	}
	
	// Update is called once per frame
	void Update () {

		speed = Mathf.Clamp(speed, 0, Mathf.Infinity);
		anim.SetFloat("SPEED",speed);
		anim.SetFloat("SpeedRun",run);

<<<<<<< HEAD


=======
>>>>>>> origin/master
		if (Vector3.Distance(horse.transform.position, destination0.position) <= 5f){
			nextDestination= destination1;
			speed--;

		}

		if (Vector3.Distance(horse.transform.position, destination1.position) <= 5f){
			lastDestination = destination0;
			nextDestination = destination2;
			speed--;
			timer--;

		}

		if (Vector3.Distance(horse.transform.position, destination2.position) <= 5f){
			lastDestination = destination1;
			nextDestination = destination3;
			speed--;
			timer--;

		}

		if (Vector3.Distance(horse.transform.position, destination3.position) <= 5f){
			//lastDestination = destination2;
			speed = 0;
		}

		if((Input.GetButton("Interact")) & (Vector3.Distance(horse.transform.position, Brolly.position) <= 20f)){
			timer=500;
			horse.SetDestination(nextDestination.position);
			speed = 80;
			}

//		if(Vector3.Distance(horse.transform.position, nextDestination.position) >= 15f){
//			run = 200;
//		}

		if(timer == 0){
			horse.SetDestination(lastDestination.position);
			timer=500;
			speed=80;
		}


////------------------------The horse will begin at destination point zero which will be its current destination-----------------------
//		if (Vector3.Distance(horse.transform.position, destination0.position) <= 9f){
//			isMoving= false;
//			nextDestination = destination1;
//			//timer = 500;
//
//
//		
//	}
//
//
////---------This is the function to move to next spot. There is not a timer for the first spot---------------------------------
//		
//		if((Input.GetButton("Interact")) & (Vector3.Distance(horse.transform.position, Brolly.position) <= 20f)){
//			isMoving = true;
//			//finaldestination = destination0
//			//in switch this is the case for destination0	
//			//nextDestination = destination1;
//			//Debug.Log("MOVE!!!!!!!!!!!!!!");
//
//			horse.SetDestination(nextDestination.position);
//
//			}
//
//			if(isMoving){
//			timer =500;
//			speed = 60;
//			//horse.SetDestination(destination1.position);
//			//Debug.Log ("Should move to 1");
//        
//      		}
//	
////
////----------This is what happens when the horse arrives at the first desitination------------------------------------
////----------The timer will start and if it reaches 0 the horse will return to previous spot---------------------
//	
//		if (Vector3.Distance(horse.transform.position, nextDestination.position) <= 3f){
//			isMoving = false;
//			lastDestination = destination0;
////			currentDestination = destination1;
////			nextDestination = destination2;
//			Debug.Log("Problem here??");
//
//			speed-- ;
//			timer --;
//			anim.SetBool("IsWalking", false);
//			Debug.Log ("ArrivedAtOne");
//			//print(Vector3.Distance(horse.transform.position, destination1.position));
//
//			if((Input.GetButtonDown("Interact")) & (Vector3.Distance(horse.transform.position, Brolly.position) <= 20f)){
//
//			//	currentDestination = destination1;
//				lastDestination = destination1;
//				nextDestination = destination2;
//				isMoving = true;
//				// nextDestination = destination2;
//				speed = 60;
//				timer =500;
//				anim.SetBool("IsWalking", true);
//				horse.SetDestination(nextDestination.position);
//				Debug.Log("last should be one");
//
//				}
//
//
//		if(timer <= 0){
//
//			speed = 80;
//			horse.SetDestination(lastDestination.position);
//				if (Vector3.Distance(horse.transform.position, nextDestination.position) <= 3f){
//					timer = 500;
//				}
//
//			Debug.Log("Go back to zero!!!!!");
//        
     		}
		}
	//}
//}
////----------This is what happens when the horse arrives at the second desitination------------------------------------
////----------The timer will start and if it reaches 0 the horse will return to previous spot---------------------
//		if (Vector3.Distance(horse.transform.position, destination2.position) <= 6f){
//      		
//			//speed = 4;
//			speed--;
//			timer--;
//			Debug.Log("Arrived at 2");
//			print(Vector3.Distance(horse.transform.position, destination2.position));
//
//
//			if((Input.GetButtonDown("Interact")) & (Vector3.Distance(horse.transform.position, Brolly.position) <= 20f)){
//			
//			speed =100;
//			timer = 500;
//			//anim.SetBool("IsWalking", true);
//			horse.SetDestination(destination3.position);
//        
//      		}
//
//		if(timer == 0){
//			
//			speed =100;
//			horse.SetDestination(destination1.position);
//			timer = 500;
//     		}

		//}
//-----------------arrived at destination 3..... last destination------------------------

//		if (Vector3.Distance(horse.transform.position, destination3.position) <= 10f){
//
//			speed = 4;
//			speed --;
//
//		}
	


//}
//}
	//}
//}