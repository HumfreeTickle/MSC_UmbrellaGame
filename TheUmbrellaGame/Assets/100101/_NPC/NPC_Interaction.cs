using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace NPC
{
	public class NPC_Interaction : MonoBehaviour
	{
		public AudioClip c_AudioClip;
		public AudioClip e_AudioClip;
		public AudioClip g_AudioClip;
		public AudioClip b_AudioClip;
		private float talktime;
		private AudioSource npcAudioSource;
		private bool saidHello;

		public delegate void MissionDelegation ();

		private MissionDelegation misssionDelegate;
	
		public MissionDelegation MissionDelegate{
			set{
				misssionDelegate = value;
			}
		}
		private NPC_Class npc_class = new NPC_Class ();

		void Start ()
		{
			npcAudioSource = GetComponent<AudioSource> ();
		}
	
		void OnTriggerEnter (Collider col)
		{
			talktime = Random.Range (3, 5);
			if (col.gameObject.tag == "Player") {
				if (!npc_class.coroutineRunning) {
					npc_class.coroutineRunning = true;
					StartCoroutine (npc_class.Talk (talktime, npcAudioSource, c_AudioClip, e_AudioClip, g_AudioClip, b_AudioClip));
				}
			}
			if (col.gameObject.tag == "NPC") {
				StartCoroutine (npc_class.Talk (talktime, npcAudioSource, c_AudioClip, e_AudioClip, g_AudioClip, b_AudioClip));
				// stop whatever they're doing
			}
		}

		void OnTriggerStay (Collider col)
		{
			if (col.gameObject.tag == "Player") {
				if (Input.GetButtonDown ("Talk")) {
					saidHello = true;
				}

				if(saidHello){
					if (misssionDelegate != null) {
						misssionDelegate ();
						saidHello = false;
					}
				}
			}
		}

		void OnTriggerExit ()
		{
			saidHello = false;
			npc_class.coroutineRunning = false;
		}
	}
}