using UnityEngine;
using System.Collections;

namespace NPC
{
	public class NPC_Movements : MonoBehaviour
	{

		private NavMeshAgent npcNavMeshAgent;
		public Transform waypoint1;
		public Transform waypoint2;
		public Transform destination;
		public GameObject lasthit;

//--------------------------------------------------- Sets up all the relevent stuff ------------------------------------------

		void Start ()
		{
			npcNavMeshAgent = GetComponent<NavMeshAgent> (); //gets the navmesh agent
			destination = waypoint1; //sets a starting destination
		}

//--------------------------------------------------- All the function calls ------------------------------------------
	
		void Update ()
		{
			Movement (destination); //
		}

//--------------------------------------------------- Moves the NPC towards the destination point ------------------------------------------


		void Movement (Transform waypoint)
		{
			npcNavMeshAgent.SetDestination (waypoint.position); // sets the point the nav agent is to move to
			transform.LookAt (waypoint.position); // makes the NPC look at the destination
			npcNavMeshAgent.stoppingDistance = 5f; //stopping distance, doesn't really work
		}

//--------------------------------------------------- Controls where to go ---------------------------------------------
//--------------------------------------------------- Not being called for some reason ------------------------------------------

		void Destination ()
		{
			if (destination == waypoint1) {
				destination = waypoint2;
			} else {
				destination = waypoint1;
			}
		}

//--------------------------------------------------- Was supposed to stop the NPC from constantly walking into walls ------------------------------------------
	
		void OnCollisionEnter (Collision col)
		{
//		print (this.gameObject + " hit " + col.gameObject);
			lasthit = col.gameObject;
			if (col.gameObject != lasthit || col.gameObject == null) {
				npcNavMeshAgent.Stop ();
				if (destination == waypoint1) {
					destination = waypoint2;
				} else {
					destination = waypoint1;
				}
				npcNavMeshAgent.Resume ();
			}

		}
	}
}
