using UnityEngine;
using System.Collections;

public class NavMeshMovement : MonoBehaviour {

	public Transform destination;
	public Transform destination2;
	public Transform destination3;
	public Transform destination4;
	private NavMeshAgent agent;
	public bool isThere;
	public bool thereAgain;

	// Use this for initialization
	void Start () {
	
		agent = gameObject.GetComponent<NavMeshAgent>();
		agent.SetDestination(destination.position);

	}
	
	// Update is called once per frame
	void Update () {

		if (Vector3.Distance(agent.transform.position, destination.position) <= 2f){

			agent.SetDestination(destination2.position);

		}
		if (Vector3.Distance(agent.transform.position, destination2.position) <= 2f){

			agent.SetDestination(destination3.position);

		}
		if (Vector3.Distance(agent.transform.position, destination3.position) <= 2f){
			
			agent.SetDestination(destination4.position);
			
		}
		if (Vector3.Distance(agent.transform.position, destination4.position) <= 2f){
			
			agent.SetDestination(destination.position);
			
		}
//
//		if(thereAgain){
//
//			agent.SetDestination(destination.position);
//
//		}
	
	}
}
