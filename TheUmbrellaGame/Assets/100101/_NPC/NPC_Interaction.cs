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
//		public GameObject musicalNote;

		private AudioClip talkyTalk;
		private AudioSource npcAudioSource;
		private float timeToTalk; // time in between each audio call
		List<AudioClip> musicalNotes = new List<AudioClip>();


		// Use this for initialization
		void Start ()
		{
			musicalNotes.Add(c_AudioClip);
			musicalNotes.Add(e_AudioClip);
			musicalNotes.Add(g_AudioClip);
			musicalNotes.Add(b_AudioClip);

			talkyTalk = c_AudioClip;
			timeToTalk = talkyTalk.length;
			npcAudioSource = GetComponent<AudioSource> ();
		}
	
		void OnTriggerStay (Collider col)
		{
			if (col.gameObject.tag == "Player") {
				if (!IsInvoking("TalkBack")) {
					Invoke ("TalkBack", talkyTalk.length/timeToTalk);
				}
			}
		}

		void OnTriggerExit(){
			if(IsInvoking("TalkBack")){
				CancelInvoke("TalkBack");
			}
			timeToTalk = talkyTalk.length;
		}

		void TalkBack ()
		{
//			int i = Mathf.RoundToInt(Random.Range(0, 3));
//			talkyTalk = musicalNotes[i];
//			Instantiate(musicalNote, transform.position, Quaternion.identity);

			float n = Mathf.Floor(Random.Range(-1, 1));
			float j = Mathf.Pow(1.05946f, (12*n));
//-------------- time in between each audio call, need to actually make this in time ----------------------------------------------//
			timeToTalk = Random.Range(1 ,5);

			print ("Note: " + talkyTalk);
			npcAudioSource.pitch = j;
			npcAudioSource.PlayOneShot (talkyTalk, 1f);
		}
	}
}
