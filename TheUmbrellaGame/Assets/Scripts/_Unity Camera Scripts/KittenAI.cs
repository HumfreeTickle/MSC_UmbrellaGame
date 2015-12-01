using UnityEngine;
using System.Collections;

namespace NPC
{
	public class KittenAI : MonoBehaviour
	{

		public GameObject rotatingMoon;
		Vector3 fleeDirection;
		Vector3 fleePosition;
		float distanceFromFleePoint;
		bool flee;
		bool escaped;
		NavMeshAgent agent;
//		Animator anim;  

//------------------------------------------ Script Zoey sent me -------------------------------------------------------
//------------------------------------------ Should go through it at some point -------------------------------------------------------
//------------------------------------------ I'll give it a new folder so I can regret not looking at it later -------------------------------------------------------


		void Start ()
		{
			agent = GetComponent <NavMeshAgent> ();
//			anim = GetComponent<Animator> ();
			//kittenObject = GetComponentInChildren<GameObject> ();
			flee = false;
			escaped = true;
			InvokeRepeating ("SeekMoon", 1.0f, 2.0f);
		}
	
		void Update ()
		{
			//print (escaped);
			if (!escaped) {
				distanceFromFleePoint = Vector3.Distance (transform.position, fleePosition);
				//print (string.Format("Curr pos: {0}", transform.position) );
				//print (string.Format("Desired pos: {0}", fleePosition) );
				agent.SetDestination (fleePosition);
				if (distanceFromFleePoint < 2) {
					escaped = true;
					agent.acceleration = 2;
					agent.speed = 2;
					agent.SetDestination (rotatingMoon.transform.position);
					//print ("Escaped!");
				}
			}
		}
	
		//Want to seek rotation of moon, not position
		//look at target
		private void SeekMoon ()
		{ 
			if (!flee & escaped) {		// && escaped) {
				agent.SetDestination (rotatingMoon.transform.position);
			} 
		}
	
		void OnTriggerEnter (Collider other)
		{
			//print ("Colliding...");
			// If the entering collider is the player...
			if (other.gameObject.tag == "Player") {
				//print ("Fleeing!");
				flee = true;
				escaped = false;
				//Calculate a suitable direction, then location to flee to
				fleeDirection = Vector3.Normalize (transform.position - other.transform.position);
				fleePosition = transform.position + fleeDirection * 8;
				fleePosition.y = transform.position.y;
				agent.acceleration = 20;
				agent.speed = 10;
				agent.SetDestination (fleePosition);
			}
		}
	
		void OnTriggerExit (Collider other)
		{
			// If the exiting collider is the player...
			if (other.gameObject.tag == "Player") {
				//print ("Exiting collider...");
				flee = false;
			}
		}
	}
}
