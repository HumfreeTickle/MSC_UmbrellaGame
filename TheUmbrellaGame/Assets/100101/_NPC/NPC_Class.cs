using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace NPC
{
	public class NPC_Class
	{
		private AudioClip talkyTalk;
		private AudioSource npcAudioSource;
		private List<AudioClip> musicalNotes = new List<AudioClip> ();

		/// <summary>
		/// Coroutine for talking
		/// </summary>
		public bool coroutineRunning = false;
		/// <summary>
		/// NPC inheritence for Talking
		/// </summary>
		/// <param name="timeToTalk">Time to talk.</param>
		/// <param name="audioSource">Audio source.</param>
		public IEnumerator Talk (float timeToTalk, AudioSource audioSource, AudioClip C, AudioClip E, AudioClip G, AudioClip B)
		{
			npcAudioSource = audioSource;
			musicalNotes.Add (C);
			musicalNotes.Add (E);
			musicalNotes.Add (G);
			musicalNotes.Add (B);
			while (coroutineRunning) {
				// --------- First Note ---------//
				int note = Mathf.RoundToInt (Random.Range (0, 3));
				talkyTalk = musicalNotes [note];

				float n = Mathf.Floor (Random.Range (-1, 1)); //whether the pitch will be higher or lower
				float j = Mathf.Pow (1.05946f, (12 * n)); //Raises the note up or down and octave
				npcAudioSource.pitch = j;
				npcAudioSource.PlayOneShot (talkyTalk, 1f);

				// --------- Second Note ---------//
				note = Mathf.RoundToInt (Random.Range (0, 3));
				talkyTalk = musicalNotes [note];
				
				n = Mathf.Floor (Random.Range (-1, 1)); //whether the pitch will be higher or lower
				j = Mathf.Pow (1.05946f, (12 * n)); //Raises the note up or down and octave
				npcAudioSource.pitch = j;
				npcAudioSource.PlayOneShot (talkyTalk, 1f);

				// --------- Third Note ---------//
				note = Mathf.RoundToInt (Random.Range (0, 3));
				talkyTalk = musicalNotes [note];
				
				n = Mathf.Floor (Random.Range (-1, 1)); //whether the pitch will be higher or lower
				j = Mathf.Pow (1.05946f, (12 * n)); //Raises the note up or down and octave
				npcAudioSource.pitch = j;
				npcAudioSource.PlayOneShot (talkyTalk, 1f);
				yield return new WaitForSeconds (talkyTalk.length); //waits for the note clip to end
			}
		}
	}
}