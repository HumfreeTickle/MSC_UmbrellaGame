using UnityEngine;
using System.Collections;

namespace Environment
{
	public class WhenShouldItRain : MonoBehaviour
	{
// Just had a thought that all this should be moved to an environment Manager of somekind which would deal with the weather, time of day, etc.. ----------------//
// Everything marked with the "Environment" namespace should probably all be together in one script
// Or most of them at any rate;


		public Material sun;
		public Material storm;

		//----------- Colours the sky turns to -------------//
		public Color morningSun;
		public Color daytimeMadness;
		public Color rainySkies;
		public Color blissfullEvening;
		public Color relaxingTwilight;
		private Color currentTimeofDay;
		private float sunlightIntensity; // Maybe able to create like a name:key thing for this so depending on the Colour input you'd also have a light intesity value

		//----------- The sun :) -----------------//
		public Light sunLight;

		//----------- Sound stuff ----------------//
		public GameObject raining; //rain sound
		public GameObject sunshine; //daytime sound
		public GameObject morningCrickets; //morning sound
		public GameObject nightTimeSaunter; //nighttime sound
		public Transform centralPoint;

		// private functions shhhhhh ------------//
		private float whenToRain;
		private Color nullColour = new Vector4 (0, 0, 0, 0);

		public float WhenToRain {
			get {
				return whenToRain;
			}
		}

		public float duration = 60; // This will be your time in seconds.
		public float smoothness = 0.02f; // This will determine the smoothness of the lerp. Smaller values are smoother. Really it's the time between updates.


		void Start ()
		{
			if (morningSun == nullColour) {
				morningSun = Color.red;
			}
			if (daytimeMadness == nullColour) {
				daytimeMadness = Color.yellow;
			}
			if (rainySkies == nullColour) {
				rainySkies = Color.blue;
			}
			if (blissfullEvening == nullColour) {
				blissfullEvening = Color.white;
			}
			if (relaxingTwilight == nullColour) {
				relaxingTwilight = Color.grey;
			}
			if(centralPoint == null){
				return;
			}
			sunLight.color = morningSun;
			sunlightIntensity = 1;
			currentTimeofDay = daytimeMadness;
//			StartCoroutine ("TimeOfDay");

		}

		void Update ()
		{
			whenToRain += Time.deltaTime;

			if (whenToRain > 60) {
//				weatherSkybox = GetComponent<Renderer>().material.Lerp(weatherSkybox, storm, Time.deltaTime);
//				RenderSettings.skybox = storm;
//				sunLight.color = Color.Lerp (sunLight.color, rainySkies, Time.deltaTime);
//				sunLight.intensity = Mathf.Lerp (sunLight.intensity, 0.5f, Time.deltaTime);
				raining.SetActive (true);
				sunshine.SetActive (false);

			} else if (whenToRain > 120) {
//				weatherSkybox = GetComponent<Renderer>().material.Lerp(weatherSkybox, sun, Time.deltaTime);
//				RenderSettings.skybox = sun;
//				sunLight.color = Color.Lerp (sunLight.color, morningSun, Time.deltaTime);
//				sunLight.intensity = Mathf.Lerp (sunLight.intensity, 0.78f, Time.deltaTime);
				raining.SetActive (false);
				sunshine.SetActive (true);
			} 
//			else {
//				//add a case for all the different types of sun colour 
//
//				sunLight.color = Color.Lerp (sunLight.color, daytimeMadness, Time.deltaTime/60);
//				sunLight.intensity = Mathf.Lerp(sunLight.intensity, 1f, Time.deltaTime/60);
//			}
		}

		IEnumerator TimeOfDay ()
		{
			float progress = 0; //This float will serve as the 3rd parameter of the lerp function.
			float increment = smoothness / duration; //The amount of change to apply.
			while (progress < 1) {
				print (progress);
				sunLight.color = Color.Lerp (sunLight.color, currentTimeofDay, progress);
				sunLight.intensity = Mathf.Lerp(sunLight.intensity, sunlightIntensity, progress);
				sunLight.transform.RotateAround(centralPoint.position, Vector3.right, progress);
				progress += increment;
				yield return new WaitForSeconds (smoothness);
			}

			while (progress >= 1) {

			// needs to be a case in here that determines which time of day to go to
				currentTimeofDay = relaxingTwilight;
				sunlightIntensity = 0.4f;
				progress = 0;
				StartCoroutine("TimeOfDay");
			}
//			yield return true;
		}

	}
}



//void Start()
//{
//}
//
