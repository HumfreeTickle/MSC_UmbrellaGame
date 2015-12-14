using UnityEngine;
using System.Collections;

public class NavMeshMovement : MonoBehaviour
{
	public Transform destination;
	public Transform destination2;
	private Transform finalDestination;
	private NavMeshAgent agent;
	private static bool finalMission;

	public bool FinalMission {
		set {
			finalMission = value;
		}
	}

	void Start ()
	{
		agent = gameObject.GetComponent<NavMeshAgent> ();
		finalDestination = GameObject.Find ("StepsD (1)").transform;


		if (destination != null) {
			agent.SetDestination (destination.position);
		}

		if (destination == null) {
			Debug.LogWarning (this.name + " : finalDestination");
		}
		if (destination2 == null) {
			Debug.LogWarning (this.name + " : destination2");
		}
		if (finalDestination == null) {
			Debug.LogWarning (this.name + " : destination");
		}

	}
	
	void Update ()
	{
		if (destination != null && destination2 != null) {
			if (!finalMission) {
				if (Vector3.Distance (agent.transform.position, destination.position) <= 4f) {
					agent.SetDestination (destination2.position);
				} else if (Vector3.Distance (agent.transform.position, destination2.position) <= 4f) {
					agent.SetDestination (destination.position);
				}
			} else {
				agent.SetDestination (finalDestination.position);
				if (Vector3.Distance (agent.transform.position, finalDestination.position) <= 10f) {
					agent.Stop ();
				}
			}
		} 
	}

}

