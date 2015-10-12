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
	public Transform Brolly;

	public bool atdestination;
	
	public Transform lastDestination;
	public Transform nextDestination;

	private float speed;
	public bool isMoving;

	public float fast;

	public bool demover;

	// Use this for initialization
	void Start () {

		horse = gameObject.GetComponent<NavMeshAgent>();
		horse.SetDestination(destination0.position);
		timer = 500;
		anim = gameObject.GetComponent<Animator>();

		//speed = 5; 





	
	}
	
	// Update is called once per frame
	void Update () {
		fast = gameObject.GetComponent<Rigidbody>().velocity.magnitude;
		anim.SetFloat("SPEED",speed);


//------------------------The horse will begin at destination point zero-----------------------
		if (Vector3.Distance(horse.transform.position, destination0.position) <= 9f){
			isMoving= false;

			if(!isMoving){
      		speed --;
			//speed = 4;
			timer = 500;
			Debug.Log("ATzero");
			print (Vector3.Distance(horse.transform.position, destination0.position));
			}

			print(fast);


//---------This is the function to move to next spot. There is not a timer for the first spot---------------------------------
		
		if((Input.GetButton("Interact")) & (Vector3.Distance(horse.transform.position, Brolly.position) <= 20f)){
				isMoving = true;
				horse.SetDestination(destination1.position);

			}

			if(isMoving){
			speed = 60;
			//horse.SetDestination(destination1.position);
			Debug.Log ("Should move to 1");
        
      		}
     	
		}
//----------This is what happens when the horse arrives at the first desitination------------------------------------
//----------The timer will start and if it reaches 0 the horse will return to previous spot---------------------
	
		if (Vector3.Distance(horse.transform.position, destination1.position) <= 3f){
			isMoving = false;

			speed-- ;
			timer--;
			anim.SetBool("IsWalking", false);
			Debug.Log ("ArrivedAtOne");
			print(Vector3.Distance(horse.transform.position, destination1.position));

			if((Input.GetButtonDown("Interact")) & (Vector3.Distance(horse.transform.position, Brolly.position) <= 20f)){
				isMoving = true;
				speed = 60;
				timer = 500;
				anim.SetBool("IsWalking", true);
				horse.SetDestination(destination2.position);

				}

			}
		if(timer == 0){

			speed = 60;
			horse.SetDestination(destination0.position);
			timer = 500;
        
     		}

//----------This is what happens when the horse arrives at the second desitination------------------------------------
//----------The timer will start and if it reaches 0 the horse will return to previous spot---------------------
		if (Vector3.Distance(horse.transform.position, destination2.position) <= 6f){
      		
			//speed = 4;
			speed--;
			timer--;
			Debug.Log("Arrived at 2");
			print(Vector3.Distance(horse.transform.position, destination2.position));


			if((Input.GetButtonDown("Interact")) & (Vector3.Distance(horse.transform.position, Brolly.position) <= 20f)){
			
			speed =100;
			timer = 500;
			//anim.SetBool("IsWalking", true);
			horse.SetDestination(destination3.position);
        
      		}

		if(timer == 0){
			
			speed =100;
			horse.SetDestination(destination1.position);
			timer = 500;
     		}

		}
//-----------------arrived at destination 3..... last destination------------------------

		if (Vector3.Distance(horse.transform.position, destination3.position) <= 10f){

			speed = 4;
			speed --;

		}
	


}
}