using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Inheritence
{
	public class FadeScript
	{
//		private Color transparent = new Vector4 (1, 1, 1, 0);
		private bool fadeIn = true; 
//---------------------------- Image Fades -----------------------------------------------------------------------

		/// <summary>
		/// Brings the alpha up to full
		/// </summary>
		/// <param name="myImage">Image to transition.</param>
		/// <param name="speed">Speed of transition.</param>
		/// 
		public void FadeIN (Image myImage, float speed)
		{
			Color alphaColour = myImage.color;
			alphaColour.a = Mathf.Lerp (alphaColour.a, 1, Time.fixedDeltaTime * speed);
			myImage.color = alphaColour;
		}

		/// <summary>
		/// Brings the alpha down to nothing
		/// </summary>
		/// <param name="myImage">Image to transition.</param>
		/// <param name="speed">Speed of transition.</param>
		/// 
		public void FadeOUT (Image myImage, float speed)
		{
			Color alphaColour = myImage.color;
			alphaColour.a = Mathf.Lerp (alphaColour.a, 0, Time.fixedDeltaTime * speed);
			myImage.color = alphaColour;
		}

		/// <summary>
		/// Fades the image in and out
		/// </summary>
		/// <param name="myImage">Image to transition..</param>
		/// <param name="speed">Speed of transition.</param>
		public void FadeINandOUT (Image myImage, float speed)
		{
			if (fadeIn) {
				FadeIN (myImage, speed);
				if (myImage.color.a > 0.9f) {
					fadeIn = false;
				}
			}
			if (!fadeIn) {
				FadeOUT (myImage, speed);
			}
		}
	}
}

