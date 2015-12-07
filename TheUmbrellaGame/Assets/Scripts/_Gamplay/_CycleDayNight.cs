using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Environment
{
	public enum DayPhase
	{  
		Night = 0,  
		Dawn = 1,  
		Day = 2,  
		Dusk = 3  
	}

	public class _CycleDayNight : MonoBehaviour
	{  
		/// current day phase  
		public DayPhase currentPhase{ get; private set; }

		/// The scene fog color to use at dawn and dusk.  
		public Color dawnDuskSun = new Color (133.0f / 255.0f, 124.0f / 255.0f, 102.0f / 255.0f);  
	
		/// The scene fog color to use during the day.  
		public Color daySun = new Color (180.0f / 255.0f, 208.0f / 255.0f, 209.0f / 255.0f);  
	
		/// The scene fog color to use at night.  
		public Color nightSun = new Color (12.0f / 255.0f, 15.0f / 255.0f, 91.0f / 255.0f); 

		/// One quarter the value of dayCycleLength.  
		private float quarterDay;
		private float halfquarterDay;  

		//Light Component
		private Light sun;
		public Flare coldSunFlare;
		private Transform mapCentre; 

		public Material dawn;
		public Material day;
		public Material evening;
		public Material night;

		//
		private GmaeManage gameManager;
		private MissionController missionState;
		public bool updateSkyboxes;
		public float ambientIntensity = 0.3f;
		public bool testing;

		/// Initializes working variables and performs starting calculations.  
		void Initialize ()
		{  
			gameManager = GameObject.Find ("Follow Camera").GetComponent<GmaeManage> ();
			missionState = gameManager.missionState;
			mapCentre = GameObject.Find ("Centre of Map").transform;  
			sun = GetComponent<Light> ();
		}  
	
		/// Sets the script control fields to reasonable default values for an acceptable day/night cycle effect.  
		void Reset ()
		{    
			dawnDuskSun = new Color (133.0f / 255.0f, 124.0f / 255.0f, 102.0f / 255.0f);  
			daySun = new Color (180.0f / 255.0f, 208.0f / 255.0f, 209.0f / 255.0f);  
			nightSun = new Color (12.0f / 255.0f, 15.0f / 255.0f, 91.0f / 255.0f);  
		}  
	
		// Use this for initialization  
		void Start ()
		{  
			Initialize ();  
		}
	
		void Update ()
		{  
			if (gameManager.gameState != GameState.NullState) {
				missionState = gameManager.missionState;
			} 

			switch (missionState) {

				
			case MissionController.Default:
				SetDawn ();  
				SetSun (350);
				break;

			case MissionController.TutorialMission:
				SetDay (0.6f); //early morning 
				SetSun (45);
				break;

			case MissionController.CatMission:
				SetDay (0.6f); //mid-day 
				SetSun (90);
				break;

			case MissionController.BoxesMission:
				SetDusk (0.6f); //mid afternoon 
				SetSun (160);
				break;

			case MissionController.HorsesMission:
				SetDusk (0.5f); //mid afternoon 
				SetSun (160);
				break;

			case MissionController.FinalMission:
				SetNight (); 
				SetSun (270);
				break;

			default:
				SetDay (1);
				break;

			}
			// Perform standard updates:    
			UpdateFog (); 
		}  
	
		/// Sets the currentPhase to Dawn, turning on the directional light, if any.  
		public void SetDawn ()
		{  
			if (sun != null) {
				sun.intensity = Mathf.Lerp (sun.intensity, 0.3f, Time.deltaTime);
				sun.flare = coldSunFlare;
			}  

			RenderSettings.ambientIntensity = ambientIntensity;

			currentPhase = DayPhase.Dawn;
			if (RenderSettings.skybox != dawn && updateSkyboxes) {
				RenderSettings.skybox = dawn;
			}
		}  
	
		/// Sets the currentPhase to Day, ensuring full day color ambient light, and full  
		/// directional light intensity, if any.  
		public void SetDay (float lighting)
		{  
			if (sun != null) {
				sun.intensity = Mathf.Lerp (sun.intensity, lighting, Time.deltaTime);
				sun.flare = coldSunFlare;

			}  
			currentPhase = DayPhase.Day;
			RenderSettings.ambientIntensity = ambientIntensity;

			if (RenderSettings.skybox != day && updateSkyboxes) {
				RenderSettings.skybox = day;
			}
		}  
	
		/// Sets the currentPhase to Dusk.  
		public void SetDusk (float lighting)
		{  
			if (sun != null) {
				sun.intensity = Mathf.Lerp (sun.intensity, lighting, Time.deltaTime);
				sun.flare = coldSunFlare;
			} 
			RenderSettings.ambientIntensity = ambientIntensity;

			currentPhase = DayPhase.Dusk;
			if (RenderSettings.skybox != evening && updateSkyboxes) {
				RenderSettings.skybox = evening;
			}
		}  
	
		/// Sets the currentPhase to Night, ensuring full night color ambient light, and  
		/// turning off the directional light, if any.  
		public void SetNight ()
		{  
			if (sun != null) {
				sun.intensity = Mathf.Lerp (sun.intensity, 0.3f, Time.deltaTime);
				sun.flare = null;
			} 
			RenderSettings.ambientIntensity = 0.9f;

			currentPhase = DayPhase.Night;  
			if (RenderSettings.skybox != night && updateSkyboxes) {
				RenderSettings.skybox = night;
			}
		}  

		/// <summary>
		/// Sets the sun.
		/// </summary>
		/// <param name="placement">Placement of Sun in the sky.</param>
		void SetSun (float placement)
		{
			mapCentre.rotation = Quaternion.Slerp (mapCentre.rotation, Quaternion.Euler (new Vector3 (-placement, 0, 0)), Time.deltaTime);
		}

		// Interpolates the fog color between the specified phase colors during each phase's transition.  
		// eg. From DawnDusk to Day, Day to DawnDusk, DawnDusk to Night, and Night to DawnDusk
	 
		private void UpdateFog ()
		{  
			if (currentPhase == DayPhase.Dawn) {  
				sun.color = Color.Lerp (dawnDuskSun, daySun, Time.deltaTime);

			} else if (currentPhase == DayPhase.Day) {  
				sun.color = Color.Lerp (daySun, dawnDuskSun, Time.deltaTime); 

			} else if (currentPhase == DayPhase.Dusk) {  
				sun.color = Color.Lerp (dawnDuskSun, nightSun, Time.deltaTime); 

			} else if (currentPhase == DayPhase.Night) {  
				sun.color = Color.Lerp (nightSun, dawnDuskSun, Time.deltaTime);
			}  
		}  
//	
//
//		/// <summary>
//		/// Can be left alone for the new day/night cycle stuff
//		/// </summary>
//		private void UpdateSkyboxBlendFactor (float blend = 0)
//		{  
//			switch (currentPhase) {
//
//			case DayPhase.Dawn: 
//				SkyboxBlendFactor = Mathf.Lerp (SkyboxBlendFactor, blend, Time.deltaTime); // 0
//				break;
//
//			case DayPhase.Day:
//				SkyboxBlendFactor = Mathf.Lerp (SkyboxBlendFactor, blend, Time.deltaTime); // 1
//				break;
//
//			case DayPhase.Dusk: 
//				SkyboxBlendFactor = Mathf.Lerp (SkyboxBlendFactor, blend, Time.deltaTime); // 0
//				break;
//
//			case DayPhase.Night: 
//				SkyboxBlendFactor = Mathf.Lerp (SkyboxBlendFactor, blend, Time.deltaTime); // 1
//				break;
//
//			default:
//				break;
//			}
//
//			RenderSettings.skybox.SetFloat ("_Blend", SkyboxBlendFactor);
//		}
	}
}