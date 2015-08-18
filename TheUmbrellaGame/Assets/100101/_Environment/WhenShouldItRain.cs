using UnityEngine;
using System.Collections;

namespace Environment
{
	public class WhenShouldItRain : MonoBehaviour
	{
// Just had a thought that all this should be moved to an environment Manager of somekind which would deal with the weather, time of day, etc.. ----------------//
// Everything marked with the "Environment" namespace should probably all be together in one script
// Or most of them at any rate;

		//----------- Sound stuff ----------------//
		public GameObject raining; //rain sound
		public GameObject sunshine; //daytime sound
		public GameObject morningCrickets; //morning sound
		public GameObject nightTimeSaunter; //nighttime sound

		// private functions shhhhhh ------------//
		private float whenToRain;

		public float WhenToRain {
			get {
				return whenToRain;
			}
		}

		void Update ()
		{
//			whenToRain += Time.deltaTime;

			if (whenToRain > 60) {
				raining.SetActive (true);
				sunshine.SetActive (false);

			} else if (whenToRain > 120) {
				raining.SetActive (false);
				sunshine.SetActive (true);
			} 

		}

	}
}



//void Start()
//{
//}
//
