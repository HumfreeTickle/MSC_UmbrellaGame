﻿using UnityEngine;
using System.Collections;
using Inheritence;


namespace Player
{
	public class wind : MonoBehaviour
	{
		public DestroyObject destroyObject = new Inheritence.DestroyObject();

		public float windForce;
		public Transform umbrellaObject;

		void Awake ()
		{
			umbrellaObject = GameObject.Find ("Umbrella").transform;
		}

		void Update ()
		{
			transform.LookAt (GameObject.Find ("main_Sphere").transform);
			destroyObject.DestroyOnTimer(this.gameObject, 2f);
		}

		//----------------------------- OTHER FUNCTIONS ------------------------------------------------------------------------


		void OnParticleCollision (GameObject umbrella)
		{
			if (umbrella.name == "main_Sphere") {
				umbrella.GetComponent<Rigidbody> ().AddForce (Vector3.up * windForce);
			}
		}
	}
}