using UnityEngine;
using System.Collections;

namespace Environment
{
	public class WhenShouldItRain : MonoBehaviour
	{

		public Material sun;
		public Material storm;
		public Light sunLight;
		public float whenToRain;
		public GameObject raining;
		public GameObject sunshine;

		// Update is called once per frame
		void Update ()
		{
			whenToRain += Time.deltaTime;
			if (whenToRain > 60) {
				RenderSettings.skybox = storm;
				sunLight.color = Color.Lerp(sunLight.color, Color.blue, Time.deltaTime);
				sunLight.intensity = Mathf.Lerp (sunLight.intensity, 0.5f, Time.deltaTime);
				raining.SetActive (true);
				sunshine.SetActive (false);

			}
			if (whenToRain > 120) {
				RenderSettings.skybox = sun;
				sunLight.color = Color.Lerp(sunLight.color, Color.yellow, Time.deltaTime);
				sunLight.intensity = Mathf.Lerp (sunLight.intensity, 0.78f, Time.deltaTime);
				raining.SetActive (false);
				sunshine.SetActive (true);
			}
		}
	}
}
