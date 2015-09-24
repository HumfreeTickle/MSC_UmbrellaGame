using UnityEngine;
using System.Collections;

namespace NPC
{
	public class NPC_Movements : MonoBehaviour
	{
		//There is some function that solves the whole "not being able to fully reach their destination"
		//I can probably add the wandering script to this as well and mishmash it up

//		[RequireComponent (typeof (NavMeshAgent))]
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
	}
}
