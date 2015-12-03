using UnityEngine;
using System.Collections;

namespace Environment
{
	public class rainShowers : MonoBehaviour
	{
//			
//		private Material cloudColor;
//		private Vector3 cloudSize;
//		private Vector3 originalCloudSize;
//		private Color soNowItRains = new Vector4 (0.3f, 0.3f, 0.3f, 1);
//		private MissionController currentMission;
//		private ParticleSystem rainSystem;
//
//		void Start ()
//		{
//			cloudColor = GetComponent<MeshRenderer> ().materials [0];
//			originalCloudSize = transform.localScale;
//			currentMission = GameObject.Find ("Follow Camera").GetComponent<GmaeManage> ().MissionState;
//			rainSystem = GameObject.Find("Rain System").GetComponent<ParticleSystem>();
//			rainSystem.maxParticles = 0;
//			cloudSize= transform.localScale * 2.5f;
//
//
//		}
//	
//		void Update ()
//		{  
////			currentMission = GameObject.Find ("Follow Camera").GetComponent<GmaeManage> ().MissionState;
//
//
//			if (currentMission == MissionController.BoxesMission) {
//				ChangeColour ();
//			} 
//			else {
//				StopTheRain ();
//			}
//		}
//
////--------------------------------------------------- Darkens the clouds ------------------------------------------
//	
//		void ChangeColour ()
//		{
//			Color darker = new Vector4 (0.2f, 0.2f, 0.2f, 1);
//			cloudColor.color = Color.Lerp (cloudColor.color, darker, Time.deltaTime / 10);
//
//
//			if (Vector4.Distance (cloudColor.color, darker) < 1) {
//				MakeItRain ();
//			}
//		}
//
////--------------------------------------------------- Turns on the Rain System ------------------------------------------
//
//		void MakeItRain ()
//		{
//			if (cloudColor.color.r >= soNowItRains.r) {
//				transform.localScale = Vector3.Lerp (transform.localScale, cloudSize, Time.deltaTime / 10);
//			}
//			if (cloudColor.color.r <= soNowItRains.r) {
//				int particles = Mathf.RoundToInt(Mathf.Lerp(rainSystem.maxParticles, 1000000, Time.deltaTime));
//				rainSystem.maxParticles = particles;
//			}
//		}
//
////--------------------------------------------------- Resets Colour and Turns off rain system ------------------------------------------
//	
//		void StopTheRain ()
//		{
//			transform.localScale = Vector3.Lerp (transform.localScale, originalCloudSize, Time.deltaTime / 10);
//
//			cloudColor.color = Color.Lerp (cloudColor.color, Color.white, Time.deltaTime / 10);
//
//			int particles = Mathf.RoundToInt(Mathf.Lerp(rainSystem.maxParticles, 0, Time.deltaTime));
//			rainSystem.maxParticles = particles;
//
//
//		}
	}
}

