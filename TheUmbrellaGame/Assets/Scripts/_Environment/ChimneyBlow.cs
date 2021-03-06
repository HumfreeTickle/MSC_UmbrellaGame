﻿using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

namespace Enivironment
{
	public class ChimneyBlow : MonoBehaviour
	{
		public float push;
		private float savedPush;
		private Animator umbrellaAnim;
		private AudioClip chimneySFX;
		private AudioSource gameObjectAudio;

		void Start ()
		{
			savedPush = push;
			if (GameObject.Find ("Umbrella")) {
				umbrellaAnim = GameObject.Find ("Umbrella").GetComponent<Animator> ();
			}

			gameObjectAudio = GetComponent<AudioSource> ();
			chimneySFX = gameObjectAudio.clip;
		}

		void OnTriggerStay (Collider other)
		{
			if (other.gameObject.tag == "Player") {
				if (other.GetComponent<Rigidbody> ()) {
					other.GetComponent<Rigidbody> ().AddForce (transform.forward * push);

				}
				umbrellaAnim.SetBool ("Hit", true);

				push -= 1;
				push = Mathf.Clamp (push, 0, savedPush);

			}
		}

		void OnTriggerEnter (Collider other)
		{
			if (other.gameObject.tag == "Player") {
				gameObjectAudio.PlayOneShot (chimneySFX);
			}
		}

		void OnTriggerExit (Collider other)
		{
			if (other.gameObject.tag == "Player") {
				umbrellaAnim.SetBool ("Hit", false);
				push = savedPush;
				
			}
		}
	}
}