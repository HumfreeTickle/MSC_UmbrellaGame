using UnityEngine;
using System.Collections;

public class NavMeshMovement : MonoBehaviour
{

	public Transform destination;
	public Transform destination2;
	private Transform finalDestination;
	private NavMeshAgent agent;

	public bool isThere;
	public bool thereAgain;
	public Animator mover;


	public Transform BridgeDestination;
	
	private static bool finalMission;

	public bool FinalMission {
		set {
			finalMission = value;
		}
	}


	void Start ()
	{
		agent = gameObject.GetComponent<NavMeshAgent> ();
		agent.SetDestination (destination.position);
		finalDestination = GameObject.Find("StepsD (1)").transform;
//		mover.SetBool("IsMoving", true);
	}
	
	void Update ()
	{
		if (!finalMission) {
			if (Vector3.Distance (agent.transform.position, destination.position) <= 4f) {
				agent.SetDestination (destination2.position);
			} else if (Vector3.Distance (agent.transform.position, destination2.position) <= 4f) {
				agent.SetDestination (destination.position);
				isThere = true;
			}
		} else {
//			agent.SetDestination (finalDestination.position);
			if(Vector3.Distance (agent.transform.position, finalDestination.position) <= 4f){
				agent.Stop();
			}
		}
	}

}

