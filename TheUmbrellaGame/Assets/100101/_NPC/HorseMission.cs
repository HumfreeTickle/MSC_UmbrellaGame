using UnityEngine;
using System.Collections;


public class HorseMission : MonoBehaviour {

	public Transform destination1;
	public Transform destination2;
	public Transform destination3;
	private NavMeshAgent agent;
	public float timer;
	public float timer2;
	private Animator anim;
	public Transform destination0;
	public Transform Brolly;
	//private Rigidbody RB;
	//private float speed;
	public GameObject DOOR;
	// Use this for initialization


	void Start () {

		agent = gameObject.GetComponent<NavMeshAgent>();
		agent.SetDestination(destination1.position);
		timer = 100;
		timer2 = 500;
		anim = gameObject.GetComponent<Animator>();
		//RB = gameObject.GetComponent<Rigidbody>();
//		Move1();
//		Move2();
//		Move3();
//		atDest1();
//		atDest2();
//		atDest3();
	
	}
	void Update(){

		if (Vector3.Distance(agent.transform.position, destination0.position) <= 4f){

			Debug.Log("ATzero");
			anim.SetBool("IsWalking", false);

		}

//_____________________________________________first section will move to destination1_________________________________________

		if(agent.SetDestination(destination1.position)){

			anim.SetBool("IsWalking", true);

		} 

	    if (Vector3.Distance(agent.transform.position, destination1.position) <= 2f){

			anim.SetBool("IsWalking", false);
			timer--;

			if((Input.GetButton("Interact")) & (timer > 0) & (Vector3.Distance(agent.transform.position, Brolly.position) <= 5f)){

				agent.SetDestination(destination2.position);
			
			}
		}
		if(timer <=0){//if the timer reaches zero then the horse will move back to the original detination

			agent.SetDestination(destination0.position);
			anim.SetBool("IsWalking", true);
		
		}

//__________________________________________________move to destination 2_______________________________________________________

			if(agent.SetDestination(destination2.position)){
				
				anim.SetBool("IsWalking", true);
				
		} 
			
			if (Vector3.Distance(agent.transform.position, destination2.position) <= 2f){
				
				anim.SetBool("IsWalking", false);
				timer2--;
				
			if((Input.GetButton("Interact")) & (timer2 > 0) & (Vector3.Distance(agent.transform.position, Brolly.position) <= 5f)){
					
					agent.SetDestination(destination3.position);

					
				}
			}
			if(timer <=0){
				
				agent.SetDestination(destination1.position);
				anim.SetBool("IsWalking", true);
		}
////_______________________________________________At Destination 3________________________________________________________________________

			if(agent.SetDestination(destination3.position)){
				
				anim.SetBool("IsWalking", true);
				
			} 
			if (Vector3.Distance(agent.transform.position, destination3.position) <= 10f){
				

				anim.SetBool("IsWalking", false);
			
			}



		print (Vector3.Distance(agent.transform.position, destination3.position));
//		if(agent.SetDestination(destination3.position)){
//			
//			anim.SetBool("IsWalking", true);
//			
//		} 



	}


////	void FixedUpdate(){
////
//////		speed = RB.velocity.magnitude;
////	}
//
//
//
//
//	// Update is called once per frame
//	void FixedUpdate () {
//
////		Move1();
////		Move2();
////		Move3();
////		atDest1();
////		atDest2();
////		atDest3();
//
//	}
//
////		if(speed>=.0001){
////			anim.SetBool("IsWalking", true);
////		}
//	void Move1(){
//
//		agent.SetDestination(destination1.position);
//		anim.SetBool("IsWalking", true);
//		}
//		
//	void Move2(){
//
//	//	if (Vector3.Distance(agent.transform.position, destination1.position) <= 2f){
//		agent.SetDestination(destination2.position);
//		anim.SetBool("IsWalking", true);
//			//timer --;
//		}
//		
//	void Move3(){
//
//		agent.SetDestination(destination3.position);
//		anim.SetBool("IsWalking", true);
//
//	}
//
//	void atDest1(){
//
//		anim.SetBool("IsWalking", false);
//
//	}
//	void atDest2(){
//		
//		anim.SetBool("IsWalking", false);
//		timer--;
//
//		if(timer <= 0){
//
//			agent.SetDestination(destination1.position);
//			anim.SetBool("IsWalking", true);
//
//		}
//		
//	}
//	void atDest3(){
//		
//		anim.SetBool("IsWalking", false);
//		timer2--;
//
//		if(timer2 <= 0){
//			
//			agent.SetDestination(destination2.position);
//			anim.SetBool("IsWalking", true);
//			
//		}
//	}
//	void Update(){
//
//	if (Vector3.Distance(agent.transform.position, destination1.position) <= 2f){
//
//			atDest1();
//		}
//	if (Vector3.Distance(agent.transform.position, destination2.position) <= 2f){
//			
//			atDest2();
//		}
//	if (Vector3.Distance(agent.transform.position, destination3.position) <= 2f){
//			
//			atDest3();
//		}
//
//
//
//
//	}
//
//	void OnTriggerEnter(){
//
//		if(Input.GetButton("Talk")& timer > 0){}
//
//		Move1();
//
//	}
//
////	void Move
////
////		if (timer <= 0){
////
////			agent.SetDestination(destination2.position);
////			anim.SetBool("IsWalking", true);
////
////		}
////		if (Vector3.Distance(agent.transform.position, destination2.position) <= 2f){
////			
////			timer2 --;
////			anim.SetBool("IsWalking", false);
////
////		}
////
////		if (timer2<=0){
////
////				agent.SetDestination(destination3.position);
////
////			}
////		if (Vector3.Distance(agent.transform.position, destination3.position) <= 4f){
////				
////				anim.SetBool("IsWalking", false);
////
////			}
//
////	}
}
//}