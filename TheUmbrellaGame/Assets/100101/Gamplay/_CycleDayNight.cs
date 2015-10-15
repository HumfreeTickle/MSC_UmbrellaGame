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
		/// The scene fog color to use at dawn and dusk.  
		public Color dawnDuskSun = new Color (133.0f / 255.0f, 124.0f / 255.0f, 102.0f / 255.0f);  
	
		/// The scene fog color to use during the day.  
		public Color daySun = new Color (180.0f / 255.0f, 208.0f / 255.0f, 209.0f / 255.0f);  
	
		/// The scene fog color to use at night.  
		public Color nightSun = new Color (12.0f / 255.0f, 15.0f / 255.0f, 91.0f / 255.0f); 


		//
		public Color DawnSkyTint;

		//
		public Color DaySkyTint;

		//
		public Color EveningSkyTint;

		//
		public Color NightSkyTint;

		//
		public Color DawnGround;
	
		//
		public Color DayGround;

		//
		public Color EveningGround;

		//
		public Color NightGround;
	
		//
		private Color currentSkyTint;

		//
		private Color currentGround;

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
		public bool updateSkyboxes;
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

			currentSkyTint = DawnSkyTint;
			currentGround = DawnGround;

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
//				blendRatio = 0.7f;

				break;

			case MissionController.CatMission:
				SetDay (0.9f); //mid-day 
				SetSun (90);
//				blendRatio = 1;

				break;

			case MissionController.BoxesMission:
				SetDusk (0.5f); //mid afternoon 
				SetSun (160);
//				blendRatio = 0.7f;

				break;

			case MissionController.HorsesMission:
				SetDusk (0.3f);  
				SetSun (200);
//				blendRatio = 0;

				break;

			case MissionController.BridgeMission:
				SetNight (); 
				SetSun (270);
//				blendRatio = 1;

				break;

			case MissionController.Default:
				SetDawn ();  
				SetSun (0);
//				blendRatio = 0;

				break;

			default:
				SetDay (1);
				break;

			}

			// Perform standard updates:    
			UpdateFog (); 
			if (updateSkyboxes) {
				UpdateSkyboxBlendFactor (blendRatio);
			}
		
		}  
	
		/// Sets the currentPhase to Dawn, turning on the directional light, if any.  
		public void SetDawn ()
		{  
			if (sun != null) {
				sun.intensity = Mathf.Lerp (sun.intensity, 0.3f, Time.deltaTime);

			}  

			RenderSettings.ambientIntensity = 0.3f;

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
			}  
			currentPhase = DayPhase.Day;
			RenderSettings.ambientIntensity = lighting;
			if (RenderSettings.skybox != day && updateSkyboxes) {
				RenderSettings.skybox = day;
			}

		}  
	
		/// Sets the currentPhase to Dusk.  
		public void SetDusk (float lighting)
		{  
			if (sun != null) {
				sun.intensity = Mathf.Lerp (sun.intensity, lighting, Time.deltaTime);
			} 
			RenderSettings.ambientIntensity = lighting;

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
			} 
			RenderSettings.ambientIntensity = 0.3f;

			
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
			RenderSettings.skybox.SetFloat("_AtomosphereThickness", 1);
			RenderSettings.skybox.SetColor("_SkyTint", currentSkyTint);
			RenderSettings.skybox.SetColor("_groundColor", currentGround);
			RenderSettings.skybox.SetFloat("_Exposure", 0.6f);

			if (currentPhase == DayPhase.Dawn) {  
				sun.color = Color.Lerp (dawnDuskSun, daySun, Time.deltaTime);

				currentSkyTint = Color.Lerp(currentSkyTint, DawnSkyTint, Time.deltaTime);
				currentGround = Color.Lerp(currentGround, DawnGround, Time.deltaTime);

			} else if (currentPhase == DayPhase.Day) {  
				sun.color = Color.Lerp (daySun, dawnDuskSun, Time.deltaTime); 
				
				currentSkyTint = Color.Lerp(currentSkyTint, DaySkyTint, Time.deltaTime);
				currentGround = Color.Lerp(currentGround, DayGround, Time.deltaTime);

			} else if (currentPhase == DayPhase.Dusk) {  
				sun.color = Color.Lerp (dawnDuskSun, nightSun, Time.deltaTime); 
				
				currentSkyTint = Color.Lerp(currentSkyTint, EveningSkyTint, Time.deltaTime);
				currentGround = Color.Lerp(currentGround, EveningGround, Time.deltaTime);

			} else if (currentPhase == DayPhase.Night) {  
				sun.color = Color.Lerp (nightSun, dawnDuskSun, Time.deltaTime);
				
				currentSkyTint = Color.Lerp(currentSkyTint, NightSkyTint, Time.deltaTime);
				currentGround = Color.Lerp(currentGround, NightGround, Time.deltaTime);
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