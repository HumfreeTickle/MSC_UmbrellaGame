﻿using UnityEngine;
using System.Collections;

public class NavMeshMovement : MonoBehaviour {

	public Transform destination;
	public Transform destination2;
	private NavMeshAgent agent;
	public bool isThere;
	public bool thereAgain;
	public Animator mover;

	public Transform BridgeDestination;


	private static bool finalMission;

	public bool FinalMission{
		set{
			finalMission = value;
		}
	}



	// Use this for initialization
	void Start () {
	
		agent = gameObject.GetComponent<NavMeshAgent>();
		agent.SetDestination(destination.position);
//		mover.SetBool("IsMoving", true);

	}
	
	// Update is called once per frame
	void Update () {


		if (Vector3.Distance(agent.transform.position, destination.position) <= 4f){

			agent.SetDestination(destination2.position);




		}
		if (Vector3.Distance(agent.transform.position, destination2.position) <= 4f){

			agent.SetDestination(destination.position);

			isThere = true;

		}
	}

//		
	
	}

