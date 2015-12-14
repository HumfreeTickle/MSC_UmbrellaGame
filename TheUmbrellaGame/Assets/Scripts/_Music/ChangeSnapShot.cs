using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

// Not used anymore
// Was meant to change the instrumentation in and out
// Had lag issues, so the tracks didn't line up

namespace Music
{
	public class ChangeSnapShot : MonoBehaviour
	{

		//Sets up all the audio snapshots
		public AudioMixerSnapshot strings;
		public AudioMixerSnapshot orchestra;
		public AudioMixerSnapshot arcade;
		public AudioMixerSnapshot rock;
		public AudioMixerSnapshot techno;
		public AudioMixerSnapshot house;

		//Annoying work around for the fact switches can't take floats
		public float timerFloor = 0;
		public int timer = 0;

		// Update is called once per frame
		void Update ()
		{
			timerFloor += Time.deltaTime; // regular timer using the upate time
			timer = Mathf.FloorToInt (timerFloor); // makes that timer into an int
			snapShots (); //calls the snapshot switches
		}

		void snapShots ()
		{
			switch (timer) {
			case 0:
				strings.TransitionTo (1); // the transition between each
				break;
			case 5:
				orchestra.TransitionTo (1);
				break;
			case 10:
				arcade.TransitionTo (1);
				break;
			case 15:
				rock.TransitionTo (1);
				break;
			case 20:
				techno.TransitionTo (1);
				break;
			case 25:
				house.TransitionTo (1);
				break;
			case 30:
				timerFloor = 0;
				break;
			}
		}
	}
}
