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
		private int i = 0;
		private List<MissionController> listOFMissionTesting = new List<MissionController> (); 

		/// current day phase  
		public DayPhase currentPhase;

		public DayPhase CurrentPhase {
			get {
				return currentPhase;
			}
		} 
	
		/// The scene ambient color used for full daylight.  
		private Color fullLight = new Color (253.0f / 255.0f, 248.0f / 255.0f, 223.0f / 255.0f);  
	
		/// The scene ambient color used for full night.  
		private Color fullDark = new Color (32.0f / 255.0f, 28.0f / 255.0f, 46.0f / 255.0f);  
	
		/// The scene fog color to use at dawn and dusk.  
		private Color dawnDuskFog = new Color (133.0f / 255.0f, 124.0f / 255.0f, 102.0f / 255.0f);  
	
		/// The scene fog color to use during the day.  
		private Color dayFog = new Color (180.0f / 255.0f, 208.0f / 255.0f, 209.0f / 255.0f);  
	
		/// The scene fog color to use at night.  
		private Color nightFog = new Color (12.0f / 255.0f, 15.0f / 255.0f, 91.0f / 255.0f);    
	
		// The amount of blending that occurs between each skybox
		private float blendRatio;

		/// One quarter the value of dayCycleLength.  
		private float quarterDay;
		private float halfquarterDay;  

		//Light Component
		private Light sun;
		private Transform mapCentre; 

		// blend value of skybox using SkyBoxBlend Shader in render settings range 0-1  
		private float SkyboxBlendFactor = 0.0f;
		public Material dawn;
		public Material day;
		public Material evening;
		public Material night;

		//
		private GmaeManage gameManager;
		private MissionController missionState;
		/// Initializes working variables and performs starting calculations.  
		void Initialize ()
		{  
			gameManager = GameObject.Find ("Follow Camera").GetComponent<GmaeManage> ();
			missionState = gameManager.missionState;
			mapCentre = GameObject.Find ("Centre of Map").transform;  
			sun = GetComponent<Light> ();

			listOFMissionTesting.Add (MissionController.TutorialMission);
			listOFMissionTesting.Add (MissionController.CatMission);
			listOFMissionTesting.Add (MissionController.BoxesMission);
			listOFMissionTesting.Add (MissionController.HorsesMission);
			listOFMissionTesting.Add (MissionController.BridgeMission);

		}  
	
		/// Sets the script control fields to reasonable default values for an acceptable day/night cycle effect.  
		void Reset ()
		{    
			fullDark = new Color (32.0f / 255.0f, 28.0f / 255.0f, 46.0f / 255.0f);  
			fullLight = new Color (253.0f / 255.0f, 248.0f / 255.0f, 223.0f / 255.0f);  
			dawnDuskFog = new Color (133.0f / 255.0f, 124.0f / 255.0f, 102.0f / 255.0f);  
			dayFog = new Color (180.0f / 255.0f, 208.0f / 255.0f, 209.0f / 255.0f);  
			nightFog = new Color (12.0f / 255.0f, 15.0f / 255.0f, 91.0f / 255.0f);  
		}  
	
		// Use this for initialization  
		void Start ()
		{  
			Initialize ();  
		}

	
		void Update ()
		{  
			//------------------------------------------------------//
			//For testing purposes only
			if (Input.GetButtonDown ("Undefined")) {
				if (i < listOFMissionTesting.Count - 1) {
					i += 1;
				}else{
					i = 0;
				}
			}

			if (Input.GetButtonUp ("Undefined")) {
				gameManager.missionState = listOFMissionTesting [i];

			}
			//------------------------------------------------------//

			missionState = gameManager.missionState;

			switch (missionState) {
			case MissionController.TutorialMission:
				SetDay (0.5f); //early morning 
				SetSun (45);
				blendRatio = 0.7f;

				break;

			case MissionController.CatMission:
				SetDay (0.9f); //mid-day 
				SetSun (90);
				blendRatio = 1;

				break;

			case MissionController.BoxesMission:
				SetDusk (0.5f); //mid afternoon 
				SetSun (160);
				blendRatio = 0.7f;

				break;

			case MissionController.HorsesMission:
				SetDusk (0.3f);  
				SetSun (200);
				blendRatio = 0;

				break;

			case MissionController.BridgeMission:
				SetNight (); 
				SetSun (270);
				blendRatio = 1;

				break;

			case MissionController.Default:
				SetDawn ();  
				SetSun (0);
				blendRatio = 0;

				break;

			default:
				SetDay (1);
				break;

			}

			// Perform standard updates:    
			UpdateFog (); 
			UpdateSkyboxBlendFactor (blendRatio);  
		
		}  
	
		/// Sets the currentPhase to Dawn, turning on the directional light, if any.  
		public void SetDawn ()
		{  
			if (sun != null) {
				sun.intensity = Mathf.Lerp(sun.intensity, 0.3f, Time.deltaTime);

			}  
			currentPhase = DayPhase.Dawn;
			if (RenderSettings.skybox != dawn) {
				RenderSettings.skybox = dawn;
			}
		}  
	
		/// Sets the currentPhase to Day, ensuring full day color ambient light, and full  
		/// directional light intensity, if any.  
		public void SetDay (float lighting)
		{  
			if (sun != null) {
				sun.intensity = Mathf.Lerp(sun.intensity, lighting, Time.deltaTime);
			}  
			currentPhase = DayPhase.Day;
			if (RenderSettings.skybox != day) {
				RenderSettings.skybox = day;
			}

		}  
	
		/// Sets the currentPhase to Dusk.  
		public void SetDusk (float lighting)
		{  
			if (sun != null) {
				sun.intensity = Mathf.Lerp(sun.intensity, lighting, Time.deltaTime);
			} 

			currentPhase = DayPhase.Dusk;
			if (RenderSettings.skybox != evening) {
				RenderSettings.skybox = evening;
			}

		}  
	
		/// Sets the currentPhase to Night, ensuring full night color ambient light, and  
		/// turning off the directional light, if any.  
		public void SetNight ()
		{  
//			RenderSettings.ambientLight = fullDark;  
			if (sun != null) {
				sun.intensity = Mathf.Lerp(sun.intensity, 0.3f, Time.deltaTime);
			} 
			currentPhase = DayPhase.Night;  
			if (RenderSettings.skybox != night) {
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

		/// Interpolates the fog color between the specified phase colors during each phase's transition.  
		/// eg. From DawnDusk to Day, Day to DawnDusk, DawnDusk to Night, and Night to DawnDusk  
		private void UpdateFog ()
		{  
			if (currentPhase == DayPhase.Dawn) {  
				sun.color = Color.Lerp (dawnDuskFog, dayFog, Time.deltaTime);  
			} else if (currentPhase == DayPhase.Day) {  
				sun.color = Color.Lerp (dayFog, dawnDuskFog, Time.deltaTime);  
			} else if (currentPhase == DayPhase.Dusk) {  
				sun.color = Color.Lerp (dawnDuskFog, nightFog, Time.deltaTime);  
			} else if (currentPhase == DayPhase.Night) {  
				sun.color = Color.Lerp (nightFog, dawnDuskFog, Time.deltaTime); 
			}  
		}  
	

		/// <summary>
		/// Can be left alone for the new day/night cycle stuff
		/// </summary>
		private void UpdateSkyboxBlendFactor (float blend = 0)
		{  
			switch (currentPhase) {

			case DayPhase.Dawn: 
				SkyboxBlendFactor = Mathf.Lerp (SkyboxBlendFactor, blend, Time.deltaTime); // 0
				break;

			case DayPhase.Day:
				SkyboxBlendFactor = Mathf.Lerp (SkyboxBlendFactor, blend, Time.deltaTime); // 1
				break;

			case DayPhase.Dusk: 
				SkyboxBlendFactor = Mathf.Lerp (SkyboxBlendFactor, blend, Time.deltaTime); // 0
				break;

			case DayPhase.Night: 
				SkyboxBlendFactor = Mathf.Lerp (SkyboxBlendFactor, blend, Time.deltaTime); // 1
				break;

			default:
				break;
			

			}

			RenderSettings.skybox.SetFloat ("_Blend", SkyboxBlendFactor);
		}
	}
}