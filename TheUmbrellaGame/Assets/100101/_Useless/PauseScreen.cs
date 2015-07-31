using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityStandardAssets.ImageEffects;

namespace Useless
{
	public class PauseScreen : MonoBehaviour
	{

//--------------------------------------------------- Needs to be moved into GmaeManager ------------------------------------------

		public float Timer;
		private float _timer;
		private GameObject Greyness;
		public bool isPaused;
		private Image GreyFade;
		public float speed;
		private Image Screen;
		private Color fader;
		public float SlowDown;
		public GameObject cameraGrey;
		private BlurOptimized cameraBlurring;

//--------------------------------------------------- Sets up all the relevent stuff ------------------------------------------
	
		void Start ()
		{
			_timer = Timer;
			isPaused = false;
			GreyFade = GetComponent<Image> ();
			fader = GetComponent<Image> ().color;
			fader.a = 0;
			Time.timeScale = 0f;
			cameraBlurring = cameraGrey.GetComponent<BlurOptimized> ();
		}

//--------------------------------------------------- Checks and calls ------------------------------------------
	
		void Update ()
		{
			if (Input.GetButtonDown ("Submit")) {
				isPaused = !isPaused;//!isPaused;//toggle between the 2 pause statements
			}

			if (!isPaused) {
				NotPaused ();
			} else {
				Paused ();
			}
		}

//---------------------------- So the timer stops counting when paused -----------------------------------------------------------------------


		void FixedUpdate ()
		{

			if (!Input.anyKey) {
			
				_timer -= Time.fixedDeltaTime;

			}
		
			if (_timer <= 0) {
			
				isPaused = true;//when the timer reaches 0 then the pause screen will activate
			}
		
			if (Input.anyKey) {
			
				_timer = Timer;//once a key is pressed the timer should revert back to 500
			}
    
		}

//---------------------------- Pausing Functions -----------------------------------------------------------------------


		void Paused ()
		{
			Time.timeScale = 0;

			cameraGrey.GetComponent<Grayscale> ().enabled = true;
			cameraBlurring.enabled = true;

			FadeIn ();
			GreyFade.color = fader;
		
		
			if (Input.GetKeyDown (KeyCode.L)) {
			
				Application.LoadLevel ("Start_Screen");
			}
			if (Input.GetKeyDown (KeyCode.R)) {
				Application.LoadLevel ("Boucing");
			}
		}

		void NotPaused ()
		{
			Time.timeScale = 1f;//runs at regular time
			cameraGrey.GetComponent<Grayscale> ().enabled = false;
			cameraGrey.GetComponent<BlurOptimized> ().enabled = false;

			FadeOut ();
			GreyFade.color = fader;
		}

//---------------------------- Image Fades -----------------------------------------------------------------------


		void FadeIn ()
		{

			fader.a = Mathf.Lerp (fader.a, 1, Time.fixedDeltaTime * SlowDown);

		}

		void FadeOut ()
		{

			fader.a = Mathf.Lerp (fader.a, 0, Time.fixedDeltaTime * SlowDown);//this should lerp between the opacity values of the pause screen the a.value

		}

	}
}