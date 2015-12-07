using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityStandardAssets.ImageEffects;
using Inheritence;
using CameraScripts;
using NPC;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// The various states of the game
/// </summary>
public enum GameState // sets what game state is currently being viewed
{
	/// <summary>
	/// Default state
	/// </summary>
	NullState,

	/// <summary>
	/// The beginning state of each scene. Player cannont move. 
	/// </summary>
	Intro,

	/// <summary>
	/// Time = 0. Screen is overlayed with pause image. Camera can be freely moved about
	/// </summary>
	Pause,

	/// <summary>
	/// The player cannot move and the camera can change look at target
	/// </summary>
	MissionEvent,

	/// <summary>
	/// Main state of the game. Allows for full controller over the umbrella
	/// </summary>
	Game,

	/// <summary>
	/// Game Over :(
	/// </summary>
	GameOver
}

/// <summary>
/// Controller type.
/// </summary>
public enum ControllerType
{
	NullState,
	Keyboard,
	ConsoleContoller
}

/// <summary>
/// Console controller type.
/// </summary>
public enum ConsoleControllerType
{
	PS3,
	XBox
}

/// <summary>
/// Which mission is currently happening
/// </summary>
public enum MissionController
{
	TutorialMission,
	CatMission,
	BoxesMission,
	HorsesMission,
	FinalMission,
	Default
}

/// <summary>
/// Umbrella type.
/// Based on whether the player skipped the opening scene or not
/// </summary>
public enum UmbrellaType
{
	Umbrella,
	Umbrella_Presentation
}

public class GmaeManage : MonoBehaviour
{
	//------------- Inherited Classes ---------------------//
	public FadeScript fading = new FadeScript ();
	//-----------------------------------------------------//

	private GameObject cameraController;

	//--- Audio Stuff ---//
	private AudioClip harpIntroClip;
	private AudioSource harpIntroSource;
	private AudioClip mainThemeMusic;
	public AudioMixerSnapshot PausedSnapShot;
	public AudioMixerSnapshot InGameSnapShot;
	public AudioMixerSnapshot EnvironmentPausedSnapshot;
	public AudioMixerSnapshot EnvironmentInGameSnapshot;

	//-- Canvas Stuff --//
	private Image PauseScreen; // pause screen image
	private Image WhiteScreen;
	private Image umbrellaGame;
	private Image startButton;

	//-- Umbrella Stuff--//
	private GameObject umbrella;
	private Rigidbody umbrellaRb;

	//-- Timers --//
	public float autoPauseTimer; // idle timer till game auto pauses
	public float transitionSpeed; // speed of transitions
	public float _gameOverTimer{ get; private set; }

	private Image StartPlay;

	//---------------- Progression ------------------//

	private int progression = 1;

	public int Progression {
		get {
			return progression;
		}
		set {
			progression = value;
		}
	}

	private int currentProgress = 1;
	//-----------------------------------------------//

	//-- Colour changing --//
	public List<Material> allTheColoursOfTheUmbrella;
	public Transform canopyColour;
	private Material umbrellaColour;
	//---------------------//
	public float _progresssionThreshold;

	//--Start script Stuff--//
	private bool _startScreen;

	public string controllerTypeVertical_L{ get; private set; }

	public string controllerTypeHorizontal_L{ get; private set; }

	public string controllerTypeVertical_R{ get; private set; }

	public string controllerTypeHorizontal_R{ get; private set; }

	public string controllerInteract{ get; private set; }

	public string controllerTalk{ get; private set; }

	public string controllerStart{ get; private set; }
	//-----------------------//

	private NPC_Interaction npc_Interact;

	//-- Text stuff--//
	private Text npc_Talking;
	private float _oldHeight;
	private float _oldWidth;
	public float fontRatio = 30;

	public float textSpeed{ get; private set; }
	
	//------------------------------------------ State Checks ------------------------------------------//
	public GameState gameState{ get; set; }

	private GameState currentState = GameState.Intro;

	public ControllerType controllerType{ get; set; }

	private ControllerType currentController = ControllerType.NullState;

	public MissionController missionState{ get; set; }

	private MissionController currentMission = MissionController.Default;

	public ConsoleControllerType consoleControllerType{ get; private set; }

	private UmbrellaType umbrellaType = UmbrellaType.Umbrella;

	//--------------------------------------------------------------------------------------------------//


	//------Presentation Stuff-------//
	private string umbrellaObject;

	//--- Progression Feedback ---//
	private bool playParticles = true;
	private GameObject particales;
	public List<GameObject> particles;
	private AudioSource progressionSFX;
	private AudioClip progressionClip;
	private bool playSFX;

	//---  ---//
	private AudioSource mainAudioSource;


	//--- Controller selection stuff ---//
	private Image[] controllers = new Image[2];
	private int isquared = 0;
	private bool choose;
	private bool controllerselected;
	public AudioClip controllerSelectSFX;
	public AudioClip controllerSelectHarp;
	private bool playControllerSelect;
	private float playedTime;

//-------------------------------------- The Setup ----------------------------------------------------------------//

	void Awake ()
	{
		Cursor.visible = false;

		if (PlayerPrefs.HasKey ("controller")) {
			consoleControllerType = (ConsoleControllerType)System.Enum.Parse (typeof(ConsoleControllerType), PlayerPrefs.GetString ("controller"));
		} else {
			consoleControllerType = ConsoleControllerType.XBox;
		}

		//-------------------- For the different controllers ---------------------------------

		if (Input.GetJoystickNames ().Length > 0) {// checks to see if a controller is connected
			controllerType = ControllerType.ConsoleContoller;
			controllerTypeVertical_L = "Vertical_L";
			controllerTypeHorizontal_L = "Horizontal_L";
			controllerTypeVertical_R = "Vertical_R";

			if (consoleControllerType == ConsoleControllerType.PS3) {
				controllerTypeHorizontal_R = "Horizontal_R_PS3";
			} else if (consoleControllerType == ConsoleControllerType.XBox) {
				controllerTypeHorizontal_R = "Horizontal_R_XBox";
			}

			if (Application.loadedLevelName != "Controller Select") {
				
				if (consoleControllerType == ConsoleControllerType.PS3) {
					controllerInteract = "Interact_1";
					controllerTalk = "Talk_1";
					controllerStart = "Submit_1";
					
				} else if (consoleControllerType == ConsoleControllerType.XBox) {
					controllerInteract = "Interact_2";
					controllerTalk = "Talk_2";
					controllerStart = "Submit_2";
				}

			}

		} else if (Input.GetJoystickNames ().Length <= 0) {
			controllerType = ControllerType.Keyboard;
			controllerTypeVertical_L = "Vertical";
			controllerTypeHorizontal_L = "Horizontal";
			controllerTypeVertical_R = "Vertical_R_Keyboard";
			controllerStart = "Submit_1";
			controllerTalk = "Talk_1";
			controllerInteract = "Interact_1";

		} else {
			controllerType = ControllerType.NullState;
			Debug.LogError ("Disconnected");
		}

		//-------------------- For the different Scenes ---------------------------------
		if (Application.loadedLevelName == "Controller Select") {
			controllers [0] = GameObject.Find ("PS3").GetComponent<Image> ();
			controllers [1] = GameObject.Find ("XBOX").GetComponent<Image> ();

			controllers [0].color = new Color (1, 1, 1, 0.3f);
			controllers [1].color = new Color (1, 1, 1, 0.3f);
			WhiteScreen = GameObject.Find ("WhiteScreen").GetComponent<Image> ();

		} else if (Application.loadedLevelName == "Start_Screen") { //Start screen

			gameState = GameState.Intro; 
			startButton = GameObject.Find ("Start Button").GetComponent<Image> ();
			umbrellaGame = GameObject.Find ("Umbrella Logo").GetComponent<Image> ();
			WhiteScreen = GameObject.Find ("WhiteScreen").GetComponent<Image> ();
			harpIntroSource = GameObject.Find ("Harp intro").GetComponent<AudioSource> ();
			harpIntroClip = harpIntroSource.clip;
			harpIntroSource.pitch = -0.6f;

			mainAudioSource = GetComponent<AudioSource> ();
			mainAudioSource.volume = 0f;

			umbrellaObject = umbrellaType.ToString ();
			umbrella = GameObject.Find (umbrellaObject);
			umbrellaRb = umbrella.GetComponent<Rigidbody> ();

			if (!startButton || !umbrellaGame || !umbrella || !WhiteScreen) {
				return;
			}

		} else if (Application.loadedLevelName == "Boucing") { //Main screen
			if (consoleControllerType == ConsoleControllerType.PS3) {
				GameObject.Find ("Tutorial_XBox").SetActive (false);
				GameObject.Find ("Tutorial_PS3").SetActive (true);
			} else if (consoleControllerType == ConsoleControllerType.XBox) {
				GameObject.Find ("Tutorial_XBox").SetActive (true);
				GameObject.Find ("Tutorial_PS3").SetActive (false);
			}

			Terrain.activeTerrain.detailObjectDensity = 0;
			gameState = GameState.Intro; 
			if (PauseScreen == null) {
				PauseScreen = GameObject.Find ("Pause Screen").GetComponent<Image> ();
			}
			if (WhiteScreen == null) {
				WhiteScreen = GameObject.Find ("WhiteScreen").GetComponent<Image> ();
			}
			WhiteScreen.color = Color.white;


			npc_Talking = GameObject.Find ("NPC_Talking").GetComponent<Text> ();

			if (umbrella == null) {
				umbrella = GameObject.Find ("main_Sphere");
			}
			if (umbrella != null) {
				umbrellaRb = umbrella.GetComponent<Rigidbody> ();
			}
			if (PlayerPrefs.GetFloat ("PlayerProgression") != 0) {
				progression = PlayerPrefs.GetInt ("PlayerProgression");
			} else {
				progression = 1;
			}

			if (PlayerPrefs.HasKey ("Mission")) {
				missionState = (MissionController)System.Enum.Parse (typeof(MissionController), PlayerPrefs.GetString ("Mission"));
			}

			StartPlay = GameObject.Find ("PressStart").GetComponent<Image> ();
			progressionSFX = GameObject.Find ("main_Sphere").GetComponent<AudioSource> ();
			progressionClip = progressionSFX.clip;

			if (canopyColour == null) {
				canopyColour = GameObject.Find ("Canopy_Colours").transform;
			}

			if (!PauseScreen || !WhiteScreen) {
				Debug.LogError ("Check Screen Changes");
				return;
			}
		} else {
			if (PlayerPrefs.HasKey ("controller")) {
				consoleControllerType = (ConsoleControllerType)System.Enum.Parse (typeof(ConsoleControllerType), PlayerPrefs.GetString ("controller"));
			} else {
				consoleControllerType = ConsoleControllerType.PS3;
			}
			gameState = GameState.NullState;
			missionState = MissionController.Default;
			currentController = ControllerType.NullState;
		}
	}

//------------------------------------------------------------------------------------------------------------------------------------//

	void Update ()
	{
		//----------- Quits Game from any Screen --------//
		if (Input.GetKey ("escape")) {
			Application.Quit ();
		}
		//-----------------------------------------------//

		if (Application.loadedLevelName == "Controller Select") {
			ControllerUpdate ();

		} else if (Application.loadedLevelName == "Start_Screen") {
			StartGame ();

		} else if (Application.loadedLevelName == "Boucing") {

			if (_oldWidth != Screen.width || _oldHeight != Screen.height) {
				_oldWidth = Screen.width;
				_oldHeight = Screen.height;
				npc_Talking.fontSize = Mathf.Clamp (Mathf.RoundToInt (Mathf.Min (Screen.width, Screen.height) / fontRatio), 15, 20);
			}

			if (progression > currentProgress) {
				Progress ();
				playParticles = true;

				if (!playSFX) {
					progressionSFX.PlayOneShot (progressionClip);
					playSFX = true;
				}

			}

			if (gameState == GameState.Intro) {
				StartGame ();
				Terrain.activeTerrain.detailObjectDensity = Mathf.Lerp (Terrain.activeTerrain.detailObjectDensity, 1, Time.deltaTime);// what is this ???
				missionState = MissionController.Default;

			} else if (gameState != GameState.Intro) {
				RestartGame ();

				if (gameState != GameState.GameOver) {//so the player can't pause when they die
					PauseGame ();
				}
				EndGame ();
			}
			CheckStates ();
		}
	}

//------------------------------------------------------------------------------------------------------------------------------//
	
	void StartGame ()
	{
		if (Application.loadedLevelName == "Start_Screen") { //Opening screen
			if (Time.time > 2f) {
				if (Input.GetButtonDown (controllerStart)) {
					if (!startButton) {
						Debug.LogError ("No start button");
						return;
					}
				
					_startScreen = true;
					startButton.GetComponent<Animator> ().enabled = false;

				}
			}
			
			if (_startScreen) {
				harpIntroSource.pitch = 0.6f;

				fading.FadeOUT (startButton, 3);
				fading.FadeINandOUT (umbrellaGame, 1.5f);
				Invoke ("FlyUmbrellaFly", 0.5f);

				harpIntroSource.volume = Mathf.Lerp (harpIntroSource.volume, 0, Time.deltaTime / 5);
				mainAudioSource.volume = Mathf.Lerp (mainAudioSource.volume, 0, Time.deltaTime / 5);

				if (harpIntroSource.volume < 0.2f) {
					fading.FadeIN (WhiteScreen, 1.5f);
					Invoke ("delayedLevelChange", harpIntroClip.length / 2);
				}
			} else {
				harpIntroSource.volume = Mathf.Lerp (harpIntroSource.volume, 0.5f, Time.deltaTime / 5);
				mainAudioSource.volume = Mathf.Lerp (mainAudioSource.volume, 0.4f, Time.deltaTime / 5);
			}

		} else if (Application.loadedLevelName == "Boucing") {
			Physics.gravity = new Vector3 (0, -18.36f, 0); //resets the gravity
			WhiteScreenTransisitions ();
		}
	}

//-------------------------------------- Resets the game -----------------------------------------------------------------//
	
	void RestartGame ()
	{
		if (Input.GetKeyDown (KeyCode.R)) {
			gameState = GameState.GameOver;
			PlayerPrefs.DeleteAll ();
		} else if (umbrella.transform.position.y <= -200f) {
			PlayerPrefs.SetInt ("PlayerProgression", progression);
			PlayerPrefs.SetString ("Mission", currentMission.ToString ());
			gameState = GameState.GameOver;
		}
	}

//-------------------------------------- Pauses the game -----------------------------------------------------------------//
	
	void PauseGame ()
	{
		if (Input.GetButtonDown (controllerStart)) {

			//------------ Game/Pause switch ------------//
			if (gameState == GameState.Game) {
				gameState = GameState.Pause;
				PausedSnapShot.TransitionTo (0.1f);
				EnvironmentPausedSnapshot.TransitionTo (0.1f);

			} else if (gameState == GameState.Pause) {
				gameState = GameState.Game;
				autoPauseTimer = 0;
				NotPaused ();
				InGameSnapShot.TransitionTo (0.1f);
				EnvironmentInGameSnapshot.TransitionTo (0f);
			}
		}

		if (gameState == GameState.Pause) {
			Paused ();
		} else if (gameState != GameState.Pause) {
			NotPaused ();
		}

		if (gameState == GameState.Game) {
			AutoPause ();	
		}
	}
	//------------------------------------- Pause State Calls ------------------------------------------------------------//
	
	void Paused ()
	{
		GetComponent<BlurOptimized> ().enabled = true;
		StartPlay.enabled = true;
		Time.timeScale = 0; //game paused
		fading.FadeIN (PauseScreen, transitionSpeed);

		GameObject.Find ("Achievements_Box").GetComponent<Image> ().enabled = false;
		GameObject.Find ("Achievemts_text").GetComponent<Text> ().enabled = false;

		//----------- Defunct background music changes --------------//
		//		if (backgroundMusic.transform.childCount > 0) {
		//			for (int i =0; i < backgroundMusic.transform.childCount; i++) {
		//				backgroundMusic.transform.GetChild(i).GetComponent<AudioSource>().pitch = -1;
		//			}
		//		}else{
		//			backgroundMusic.GetComponent<AudioSource>().pitch = -1;
		//		}

	}
	
	void NotPaused ()
	{
		GetComponent<BlurOptimized> ().enabled = false;
		StartPlay.enabled = false;

		Time.timeScale = 1f; //runs at regular time
		fading.FadeOUT (PauseScreen, transitionSpeed);

		GameObject.Find ("Achievements_Box").GetComponent<Image> ().enabled = true;
		GameObject.Find ("Achievemts_text").GetComponent<Text> ().enabled = true;

		//----------- Defunct background music changes --------------//
		//		if (backgroundMusic.transform.childCount > 0) {
		//			for (int i =0; i < backgroundMusic.transform.childCount; i++) {
		//				backgroundMusic.transform.GetChild(i).GetComponent<AudioSource>().pitch = 1;
		//			}
		//		}else{
		//			backgroundMusic.GetComponent<AudioSource>().pitch = 1;
		//		}
		//		backgroundMusic.pitch = 1;
	}
	
	void AutoPause ()
	{
		if (umbrellaRb.velocity.magnitude <= 2) {
			{
				autoPauseTimer += Time.deltaTime;
			}
			
			if (autoPauseTimer >= 45) {
				gameState = GameState.Pause;
			}
			
		} else if (umbrellaRb.velocity.magnitude > 2) {
			autoPauseTimer = 0;
		}
	}

	
//-----------------------------------------------------------------------------------------//
	
	void EndGame ()
	{
		if (gameState == GameState.GameOver) {

			_gameOverTimer += Time.deltaTime;
			PauseScreen.enabled = false;
			WhiteScreenTransisitions ();
		}
	}

//-------------------------------------- State Checking -----------------------------------------------------------------//
	void CheckStates ()
	{
		if (controllerType != currentController) {
			if (Input.GetJoystickNames ().Length > 0) {
				Debug.Log (controllerType + ": " + consoleControllerType);
			} else {
				Debug.Log (controllerType);

			}
		}
		
		if (gameState != currentState) {
			Debug.Log (gameState);
		}

		if (missionState != currentMission) {
			Debug.Log (missionState);
		}
		
		currentState = gameState;
		currentController = controllerType;
		currentMission = missionState;
	}
	
	//----------------------------------------------- Other Funcitons ------------------------------------------//

	void FlyUmbrellaFly ()
	{
		umbrellaRb.AddForce (1, 2.5f, 0);
	}
	

	//-------------------------------------- White Screen Transition -----------------------------------------------//
	void WhiteScreenTransisitions ()
	{
		if (Application.loadedLevelName == "Controller Select") {
			fading.FadeIN (WhiteScreen, 1);
			
			if (WhiteScreen.color.a >= 0.97) {
				Application.LoadLevel ("Start_Screen");
			}
		}


		if (gameState == GameState.Intro) {
			fading.FadeOUT (WhiteScreen, 3);
			
		} else if (gameState == GameState.GameOver) {
			if (_gameOverTimer > 2) {
				fading.FadeIN (WhiteScreen, 1);

				if (WhiteScreen.color.a >= 0.95) {
					if (Application.loadedLevelName == "Start_Screen") {
						Application.LoadLevel ("Boucing");
					} else if (Application.loadedLevelName == "Boucing") {
						if (progression < 6) {
							Application.LoadLevel ("Boucing");
						} else {
							Application.LoadLevel ("Credits");
						}
					}
				}
			}
		}
	}

	//-------------------------------------- Delayed Main Level Change -----------------------------------------------//
	void delayedLevelChange ()
	{
		Application.LoadLevel ("Boucing"); //Changes to the next scene
	}


	//-------------------------------------- Game Progression Funcitons -----------------------------------------------//

	void Progress ()
	{
		if (progression < 6) {
			umbrellaColour = allTheColoursOfTheUmbrella [progression - 2];
			particales = particles [progression - 2];
			ChangeColours (canopyColour);
		} else {
			gameState = GameState.GameOver;
		}
	}

	void ChangeColours (Transform obj)
	{
		for (int child = 0; child< obj.childCount; child++) { //goes through each child object one at a time
			if (obj.GetChild (child).transform.childCount > 0) {
				ChangeColours (obj.GetChild (child));
			} else {
				if (obj.GetChild (child).GetComponent<MeshRenderer> ()) {
					if (obj.GetChild (child).tag == umbrellaColour.name) {// checks to see if there is a mesh renderer attached to child
						MeshRenderer umbrellaChild = obj.GetChild (child).GetComponent<MeshRenderer> ();
						umbrellaChild.material.Lerp (umbrellaChild.material, umbrellaColour, Time.deltaTime / 2);

						if (playParticles) {
							Instantiate (particales, umbrella.transform.position + new Vector3 (0, 1f, 0), Quaternion.Euler (new Vector3 (270, 0, 0)));
							playParticles = false;
						}

						if (Vector4.Distance (umbrellaChild.material.color, umbrellaColour.color) <= _progresssionThreshold) { // || umbrellaChild.material.color.g >= umbrellaColour.color.g - 0.001f || umbrellaChild.material.color.b >= umbrellaColour.color.b - 0.001f) {
							currentProgress = progression;
							playSFX = false;
						}
					}
				}
			}
		}
	}

	//-------------------------------------- Controller -----------------------------------------------//
	void ControllerUpdate ()
	{
		if (Mathf.Abs (Input.GetAxisRaw ("Horizontal_L")) > 0.1f) {
			choose = true;
		}

		Color defaultColor = new Color (1, 1, 1, 0.3f);
		Vector3 seletedScale = new Vector3 (1.5f, 1.5f, 1.5f);
		Vector3 smallScale = new Vector3 (0.5f, 0.5f, 0.5f);


		if (choose) {
			if (!IsInvoking ("WhiteScreenTransisitions")) {

				controllers [isquared].color = Color.Lerp (controllers [isquared].color, Color.white, Time.deltaTime);
				controllers [isquared].transform.localScale = Vector3.Lerp (controllers [isquared].transform.localScale, seletedScale, Time.deltaTime);

				if (consoleControllerType == ConsoleControllerType.XBox) {
					controllers [0].color = Color.Lerp (controllers [0].color, defaultColor, Time.deltaTime);
					controllers [0].transform.localScale = Vector3.Lerp (controllers [0].transform.localScale, smallScale, Time.deltaTime);

				} else if (consoleControllerType == ConsoleControllerType.PS3) {
					controllers [1].color = Color.Lerp (controllers [1].color, defaultColor, Time.deltaTime);
					controllers [1].transform.localScale = Vector3.Lerp (controllers [1].transform.localScale, smallScale, Time.deltaTime);

				}
			}
			if (Vector3.Distance (controllers [isquared].transform.localScale, seletedScale) <= 0.3f) { 
				if (Input.anyKeyDown) {
					GetComponent<AudioSource> ().pitch = 1f;
					GetComponent<AudioSource> ().volume = 0.4f;
					GetComponent<AudioSource> ().PlayOneShot (controllerSelectHarp);

					controllerselected = true;
				}
			}
			if (controllerselected) {
				PlayerPrefs.SetString ("controller", consoleControllerType.ToString ());
				Invoke ("WhiteScreenTransisitions", 0.1f);
			}
		}

		if (Input.GetAxisRaw ("Horizontal_L") > 0.1f) {
			if (consoleControllerType != ConsoleControllerType.PS3) {
				consoleControllerType = ConsoleControllerType.PS3;
				isquared = 0;
				playControllerSelect = true;
				playedTime = Time.time;

			}
		} else if (Input.GetAxisRaw ("Horizontal_L") < -0.1f) {
			if (consoleControllerType != ConsoleControllerType.XBox) {
				consoleControllerType = ConsoleControllerType.XBox;
				isquared = 1;
				playControllerSelect = true;
				playedTime = Time.time;
			}
		}


		if (playControllerSelect) {
			if (Time.time - playedTime < controllerSelectSFX.length) {
				GetComponent<AudioSource> ().pitch = 0.5f;
				GetComponent<AudioSource> ().PlayOneShot (controllerSelectSFX);
				playControllerSelect = false;
			}
		}
	}
	
	void OnApplicationQuit ()
	{
		PlayerPrefs.DeleteKey ("controller");
	}

	//---------------------------------------------------------------------------------------------------------//

}//End of Class

