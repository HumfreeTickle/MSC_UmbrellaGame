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
//	NullState,
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

public enum UmbrellaType
{
	Umbrella,
	Umbrella_Presentation
}

public class GmaeManage : MonoBehaviour
{

	// Doesn't allow another instance of GmaeManage to be used within the scene --- http://rusticode.com/2013/12/11/creating-game-manager-using-state-machine-and-singleton-pattern-in-unity3d/

//	protected GmaeManage ()
//	{
//		//not sure why this is empty
//	}
//
//	private static GmaeManage _instance = null;
//	
//	// Singleton pattern implementation
//	public static GmaeManage Instance { 
//		get {
//			if (GmaeManage._instance == null) {
//				GmaeManage._instance = new GmaeManage (); 
//			}  
//			return GmaeManage._instance;
//		} 
//	}



//------------------------------------------- Inherited Classes ------------------------------------//

	public FadeScript fading = new FadeScript ();
//	private NPCManage npcManager;
//--------------------------------------------------------------------------------------------------//


	private GameObject cameraController; // 

	//--- Audio Stuff ---
	private AudioClip harpIntroClip;
	private AudioSource harpIntroSource;
	public AudioMixerSnapshot start;
	private AudioClip mainThemeMusic;
//	private GameObject backgroundMusic;
	public AudioMixerSnapshot PausedSnapShot;
	public AudioMixerSnapshot InGameSnapShot;


	//-- Canvas Stuff ---
	public Image PauseScreen; // pause screen image
	public Image WhiteScreen;
	private Image umbrellaGame;
	private Image startButton;

	//-- The rest :) --
	private GameObject umbrella;
	public Rigidbody umbrellaRb;
	public float autoPauseTimer; // idle timer till game auto pauses
	public float transitionSpeed; // speed of transitions
	public float _gameOverTimer; // 

	//---------------- Progression ------------------//

	public int progression = 1;

	public int Progression {
		get {
			return progression;
		}
		set {
			progression = value;
		}
	}

	public int currentProgress = 1;
	//-----------------------------------------------//
	public Vector3 lastKnownPosition;

	public Vector3 LAstKnownPosition {
		set {
			lastKnownPosition = value;
		}
	}

	public Vector3 startingPos;
	//-----------------------------------------------//
	public List<Material> allTheColoursOfTheUmbrella;
//	public List<Transform> canopyColours;
	public Transform canopyColour;
	private Material umbrellaColour;
	public float thresholdVector;
	//Start script Stuff
	private bool nextLevel; // has the transtion to Level-1 been activated
	public bool timeStart;


	// Eventually these should be made into a dynamic list that is moved in and out depending on the type of controls needed
	private string controllerTypeVertical;
	private string controllerTypeHorizontal; 


	//-----------------------

	private NPC_Interaction npc_Interact;
	private float _oldHeight;
	private float _oldWidth;
	public float Ratio = 30;
	private Text npc_Talking;
	//Missions
	public float textSpeed;

	public float TextSpeed {
		get {
			return textSpeed;
		}
	}


//------------------------------------ Getters and Setters ------------------------------------------------------------

	//Needs to be renamed to gameOverTimer
	public float gameOver_Timer { //timer used elsewhere to end the game
		get {
			return _gameOverTimer;
		}
	}

	//
	public string ControllerTypesHorizontal {
		get {
			return controllerTypeHorizontal;
		}
	}

	public string ControllerTypeVertical {
		get {
			return controllerTypeVertical;
		}
	}

	//------------------------------------------ State Checks ------------------------------------------//
	public GameState gameState;

	public GameState GameState {
		get {
			return gameState;
		}
		set {
			gameState = value;
		}
	}

	private GameState currentState = GameState.Intro;
	public ControllerType controllerType;

	public ControllerType ControllerType {
		get {
			return controllerType;
		}
		set {
			controllerType = value;
		}
	}

	private ControllerType currentController = ControllerType.NullState;
	public MissionController missionState;

	public MissionController MissionState {
		get {
			return missionState;
		}
		set {
			missionState = value;
		}
	}

	private MissionController currentMission = MissionController.Default;
	public UmbrellaType umbrellaType = UmbrellaType.Umbrella;

	//--------------------------------------------------------------------------------------------------//


	//------Presentation Stuff-------//
	private string umbrellaObject;
	private bool playParticles = true;
	private GameObject particales;
	public List<GameObject> particles;
	private AudioSource rain;
	private AudioSource progressionSFX;
	private AudioClip progressionClip;
	private bool playSFX;


//-------------------------------------- The Setup -----------------------------------------------------------------

	void Awake ()
	{
//		DontDestroyOnLoad (transform.gameObject);

		//-------------------- For the different controllers ---------------------------------
		if (Input.GetJoystickNames ().Length > 0) {// checks to see if a controller is connected
			ControllerType = ControllerType.ConsoleContoller;
			controllerTypeVertical = "Vertical_L";
			controllerTypeHorizontal = "Horizontal_L";
			print ("Player 1: Connected");


		} else if (Input.GetJoystickNames ().Length == 0) {
			ControllerType = ControllerType.Keyboard;
			controllerTypeVertical = "Vertical";
			controllerTypeHorizontal = "Horizontal";

		} else {
			ControllerType = ControllerType.NullState;
			Debug.Log ("Disconnected");
		}

		//-------------------- For the different Scenes ---------------------------------

		if (Application.loadedLevelName == "Start_Screen") { //Start screen

			GameState = GameState.Intro; 
			startButton = GameObject.Find ("Start Button").GetComponent<Image> ();
			umbrellaGame = GameObject.Find ("Umbrella Logo").GetComponent<Image> ();
			WhiteScreen = GameObject.Find ("WhiteScreen").GetComponent<Image> ();
			harpIntroSource = GameObject.Find ("Harp intro").GetComponent<AudioSource> ();
			harpIntroClip = harpIntroSource.clip;
			harpIntroSource.pitch = -0.6f;

			rain = GetComponent<AudioSource> ();
			rain.volume = 0f;

			umbrellaObject = umbrellaType.ToString ();
			umbrella = GameObject.Find (umbrellaObject);
			umbrellaRb = umbrella.GetComponent<Rigidbody> ();

			if (!startButton || !umbrellaGame || !umbrella || !WhiteScreen) {
				return;
			}

		} else if (Application.loadedLevelName == "Boucing") { //Main screen

			Terrain.activeTerrain.detailObjectDensity = 0;
			GameState = GameState.Intro; 
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
			if (PlayerPrefs.GetFloat ("PlayerX") != 0 || PlayerPrefs.GetFloat ("PlayerY") != 0 || PlayerPrefs.GetFloat ("PlayerZ") != 0) {
				startingPos = new Vector3 (PlayerPrefs.GetFloat ("PlayerX"), PlayerPrefs.GetFloat ("PlayerY"), PlayerPrefs.GetFloat ("PlayerZ"));
			} else {
				startingPos = umbrella.transform.localPosition;
			}
//			umbrella.transform.localPosition = startingPos;

//			backgroundMusic = GameObject.Find ("Music");
//			npcManager = GetComponent<NPCManage> ();

			progressionSFX = GameObject.Find ("main_Sphere").GetComponent<AudioSource> ();
			progressionClip = progressionSFX.clip;

			if (canopyColour == null) {
				canopyColour = GameObject.Find ("Canopy_Colours").transform;
			}

			if (!PauseScreen || !WhiteScreen) {
				Debug.LogError ("Check Screen Changes");
				return;
			}
		}
	}

//-------------------------------------- All the calls -----------------------------------------------------------------

	void Update ()
	{
		if (Application.loadedLevelName == "Start_Screen") {
			StartGame ();
		}

		if (Application.loadedLevelName == "Boucing") {

//			if (Input.GetButtonDown ("Undefined")) {
//				progression += 1;
//			}

			if (_oldWidth != Screen.width || _oldHeight != Screen.height) {
				_oldWidth = Screen.width;
				_oldHeight = Screen.height;
				npc_Talking.fontSize = Mathf.RoundToInt (Mathf.Min (Screen.width, Screen.height) / Ratio);
			}

			if (progression > currentProgress) {
				Progress ();
				playParticles = true;
				if (!playSFX) {
					progressionSFX.PlayOneShot (progressionClip);
					playSFX = true;
				}

			}

			if (GameState == GameState.Intro) {
				StartGame ();
				Terrain.activeTerrain.detailObjectDensity = Mathf.Lerp (Terrain.activeTerrain.detailObjectDensity, 1, Time.deltaTime);// what is this ???
				MissionState = MissionController.Default;

			} else if (GameState != GameState.Intro) {
				RestartGame ();

				if (GameState != GameState.GameOver) {//so the player can't pause when they die
					PauseGame ();
				}
				EndGame ();
			}
			CheckStates ();
		}
	}

//-------------------------------------- Start Game is elsewhere for some reason -----------------------------------------------------------------
	
	void StartGame ()
	{
		if (Application.loadedLevelName == "Start_Screen") { //Opening screen


			if (Input.GetButtonDown ("Submit")) {
				if (!startButton) {
					Debug.LogError ("No start button");
					return;
				}
				
				timeStart = true;
				startButton.GetComponent<Animator> ().enabled = false;

			}
			
			if (timeStart) {
				harpIntroSource.pitch = 0.6f;

				fading.FadeOUT (startButton, 3);
				fading.FadeINandOUT (umbrellaGame, 1);
				Invoke ("FlyUmbrellaFly", 0.5f);

				start.TransitionTo (40);

				harpIntroSource.volume = Mathf.Lerp (harpIntroSource.volume, 0, Time.deltaTime / 5);
				if (harpIntroSource.volume < 0.2f) {
					fading.FadeIN (WhiteScreen, 1);
					Invoke ("whichLevel", harpIntroClip.length / 2);
				}
			} else {
				harpIntroSource.volume = Mathf.Lerp (harpIntroSource.volume, 0.5f, Time.deltaTime / 5);
				rain.volume = Mathf.Lerp (rain.volume, 0.4f, Time.deltaTime / 5);
			}

		} else if (Application.loadedLevelName == "Boucing") { //Main game screen
			Physics.gravity = new Vector3 (0, -18.36f, 0);
			WhiteScreenTransisitions ();

		}
	}

//-------------------------------------- Resets the game -----------------------------------------------------------------
	
	void RestartGame ()
	{
		if (Input.GetKeyDown (KeyCode.R) || umbrella.transform.position.y <= -200f) {
			GameState = GameState.GameOver;
			print ("Reset");
		}
	}

//-------------------------------------- Pauses the game -----------------------------------------------------------------
	
	void PauseGame ()
	{
		if (Input.GetButtonDown ("Submit")) {

			if (GameState == GameState.Game) {
				GameState = GameState.Pause;
				PausedSnapShot.TransitionTo (0);

			} else if (GameState == GameState.Pause) {
//				if(Button == 0){
				GameState = GameState.Game;
				autoPauseTimer = 0;
				NotPaused ();
				InGameSnapShot.TransitionTo (0);
//			}
			}
		}

		if (GameState == GameState.Pause) {
			Paused ();
		} else if (GameState != GameState.Pause) {
			NotPaused ();
		}

		if (GameState == GameState.Game) {
			FixedPause ();	
		}
	}
	//------------------------------------- Pause State Calls ------------------------------------------------------------
	
	void Paused ()
	{
		GetComponent<BlurOptimized> ().enabled = true;
		
		Time.timeScale = 0; //game paused
		
		fading.FadeIN (PauseScreen, transitionSpeed);

		GameObject.Find("Achievements_Box").GetComponent<Image>().enabled = false;
		GameObject.Find("Achievemts_text").GetComponent<Text>().enabled = false;

		//		if (backgroundMusic.transform.childCount > 0) {
		//			for (int i =0; i < backgroundMusic.transform.childCount; i++) {
		//				backgroundMusic.transform.GetChild(i).GetComponent<AudioSource>().pitch = -1;
		//			}
		//		}else{
		//			backgroundMusic.GetComponent<AudioSource>().pitch = -1;
		//		}

		// Movement with
		if(Input.GetAxisRaw("Vertical_L") > 0){
			//move up one in the button array
			//if greater then 2 move to 0
		}else if(Input.GetAxisRaw("Vertical_L") > 0){
			//move down one in the button array
			//if less then 0 move to 2
		}
	}
	
	void NotPaused ()
	{
		GetComponent<BlurOptimized> ().enabled = false;
		
		Time.timeScale = 1f; //runs at regular time
		fading.FadeOUT (PauseScreen, transitionSpeed);

		GameObject.Find("Achievements_Box").GetComponent<Image>().enabled = true;
		GameObject.Find("Achievemts_text").GetComponent<Text>().enabled = true;
		//		if (backgroundMusic.transform.childCount > 0) {
		//			for (int i =0; i < backgroundMusic.transform.childCount; i++) {
		//				backgroundMusic.transform.GetChild(i).GetComponent<AudioSource>().pitch = 1;
		//			}
		//		}else{
		//			backgroundMusic.GetComponent<AudioSource>().pitch = 1;
		//		}
		//		backgroundMusic.pitch = 1;
	}
	
	void FixedPause ()
	{
		if (umbrellaRb.velocity.magnitude <= 2) {
			{
				autoPauseTimer += Time.deltaTime;
			}
			
			if (autoPauseTimer >= 45) {
				GameState = GameState.Pause;
			}
			
		} else if (umbrellaRb.velocity.magnitude > 2) {
			autoPauseTimer = 0;
		}
	}

	
//-------------------------------------- Ending the game is here (sort of) -----------------------------------------------------------------
	
	void EndGame ()
	{
		if (GameState == GameState.GameOver) {

			_gameOverTimer += Time.deltaTime;
			PauseScreen.enabled = false;
			WhiteScreenTransisitions ();
		}
	}

//-------------------------------------- State Checking ---------------------------------------------------------------------------
	void CheckStates ()
	{
		if (ControllerType != currentController) {
			Debug.Log (ControllerType);
		}
		
		if (GameState != currentState) {
			Debug.Log (GameState);
		}

		if (MissionState != currentMission) {
			Debug.Log (MissionState);
		}
		
		currentState = GameState;
		currentController = ControllerType;
		currentMission = MissionState;
	}
	
//----------------------------------------------- Other Funcitons ------------------------------------------

	void FlyUmbrellaFly ()
	{
		umbrellaRb.AddForce (1, 2.5f, 0);
	}
	

	//-------------------------------------- White Screen Stuff -----------------------------------------------------------------
	void WhiteScreenTransisitions ()
	{
		if (GameState == GameState.Intro) {
			fading.FadeOUT (WhiteScreen, 3);
			
		} else if (GameState == GameState.GameOver) {
			if (_gameOverTimer > 2) {
				fading.FadeIN (WhiteScreen, 1);

				if (WhiteScreen.color.a >= 0.95) {
					if (Application.loadedLevelName == "Start_Screen") {
						Application.LoadLevel ("Boucing");
					} else if (Application.loadedLevelName == "Boucing") {
						if (progression < 6) {
							Application.LoadLevel ("Boucing");
						}else{
							Application.LoadLevel ("Start_Screen");
						}
						//------------- No longer used stuff --------------//
//						PlayerPrefs.SetFloat ("PlayerX", lastKnownPosition.x);
//						PlayerPrefs.SetFloat ("PlayerY", lastKnownPosition.y);
//						PlayerPrefs.SetFloat ("PlayerZ", lastKnownPosition.z);
//						Application.LoadLevel ("Start_Screen");
					}
				}
			}
		}
	}

	void whichLevel ()
	{
		Application.LoadLevel ("Boucing"); //Changes to the next scene
	}

	void Progress ()
	{
		if (progression < 6) {
			umbrellaColour = allTheColoursOfTheUmbrella [progression - 2];
			particales = particles [progression - 2];
			ChangeColours (canopyColour);
		} else {
			GameState = GameState.GameOver;
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
						if (Vector4.Distance (umbrellaChild.material.color, umbrellaColour.color) <= thresholdVector) { // || umbrellaChild.material.color.g >= umbrellaColour.color.g - 0.001f || umbrellaChild.material.color.b >= umbrellaColour.color.b - 0.001f) {
							currentProgress = progression;
							playSFX = false;
						}
					}
				}
			}
		}
	}

//	void OnApplicationQuit ()
//	{
//		PlayerPrefs.DeleteAll ();
//	}


}//End of Class

