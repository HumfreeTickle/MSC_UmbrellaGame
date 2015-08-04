using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Inheritence;

//------------------------------------------ Pretty sure this needs to be moved to the Gmaemange script -------------------------

namespace Useless
{
	public class EnterGame : MonoBehaviour
	{
		private FadeScript fading = new FadeScript ();
		public Image whiteIN;
		public float speed;
		public bool Starting;
	
		void Update ()
		{
			if (Starting) {
				fading.FadeIN (whiteIN, speed);

				if (whiteIN.color.a > 0.5) {
					Invoke ("whichLevel", 3); //delays the loading of Level-1
				}
			}
		}
	
		void OnTriggerEnter (Collider other)
		{
			if (other.gameObject.tag == "Player") {
				Starting = true;
			}
		}
		

	}
}
