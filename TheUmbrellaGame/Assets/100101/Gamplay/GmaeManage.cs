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
	public float _charge; // the umbrella's energy charge

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

	private NPC_TutorialMission tutorialMission;
	private NPC_CatMission catMission;
	private NPC_BoxesMission boxesMission;
	private HorseMission_BackEnd horseMission;
	
//------------------------------------ Getters and Setters ------------------------------------------------------------

	//Needs to be renamed to gameOverTimer
	public float Timer { //timer used elsewhere to end the game
		get {
			return _gameOverTimer;
		}
	}

	// The total charge left in the umbrella
	public float UmbrellaCharge {
		get {
			return _charge;
		}
		set {
			_charge = value;
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
	
	//--------------------------------------------------------------------------------------------------//


	//------Presentation Stuff-------//
	public string umbrellaObject;

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
			umbrella.transform.localPosition = startingPos;

//			backgroundMusic = GameObject.Find ("Music");
//			npcManager = GetComponent<NPCManage> ();

			//----------------- Missions Complete Stuff --------------------//
			tutorialMission = GameObject.Find ("Missions").GetComponent<NPC_TutorialMission> ();
			catMission = GameObject.Find ("Missions").GetComponent<NPC_CatMission> ();
			boxesMission = GameObject.Find ("Missions").GetComponent<NPC_BoxesMission> ();
			horseMission = GameObject.Find ("Missions").GetComponent<HorseMission_BackEnd> ();

			if (canopyColour == null) {
				canopyColour = GameObject.Find ("Canopy_Colours").transform;
			}

			if (!PauseScreen || !WhiteScreen) {
				return;
			}
		}
	}

//-------------------------------------- All the calls -----------------------------------------------------------------

	void Update ()
	{
		if (Application.loadedLevelName == "Boucing") {



			if (_oldWidth != Screen.width || _oldHeight != Screen.height) {
				_oldWidth = Screen.width;
				_oldHeight = Screen.height;
				npc_Talking.fontSize = Mathf.RoundToInt (Mathf.Min (Screen.width, Screen.height) / Ratio);
			}
		}

		if (progression > currentProgress) {
			Progress ();
		}

		if (GameState == GameState.Intro) {
			StartGame ();
			Terrain.activeTerrain.detailObjectDensity = Mathf.Lerp (Terrain.activeTerrain.detailObjectDensity, 1, Time.deltaTime);
			MissionState = MissionController.Default;

		} else if (GameState != GameState.Intro) {//gameState == GameState.Game || gameState == GameState.Pause || gameState == GameState.GameOver ) {

			RestartGame ();

			if (GameState != GameState.GameOver) {//so the player can't pause when they die
				PauseGame ();
			}
			EndGame ();
		}
		CheckStates ();
	}

//-------------------------------------- Start Game is elsewhere for some reason -----------------------------------------------------------------
	
	void StartGame ()
	{
		if (Application.loadedLevelName == "Start_Screen") { //Opening screen

			if (Input.GetButtonDown ("Submit")) {
				if (!startButton) {
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
			}

		} else if (Application.loadedLevelName == "Boucing") { //Main game screen
			Physics.gravity = new Vector3 (0, -18.36f, 0);
			WhiteScreenTransisitions ();

		}
	}

//-------------------------------------- Resets the game -----------------------------------------------------------------
	
	void RestartGame ()
	{
		if (Input.GetKeyDown (KeyCode.R)) {
			GameState = GameState.GameOver;
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
				GameState = GameState.Game;
				autoPauseTimer = 0;
				NotPaused ();
				InGameSnapShot.TransitionTo (0);
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
	
//-------------------------------------- Ending the game is here (sort of) -----------------------------------------------------------------
	
	void EndGame ()
	{
		if (_charge < 1) {
			GameState = GameState.GameOver;
		}

		if (GameState == GameState.GameOver) {


			_gameOverTimer += Time.deltaTime;
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


	//------------------------------------- Pause State Calls ------------------------------------------------------------

	// using fixed Delta Time is not a good solution - Fabrizio


	void Paused ()
	{
		GetComponent<BlurOptimized> ().enabled = true;
		
		Time.timeScale = 0; //game paused

		fading.FadeIN (PauseScreen, transitionSpeed);

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
		
		Time.timeScale = 1f; //runs at regular time
		fading.FadeOUT (PauseScreen, transitionSpeed);
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
				GameState = GameState.Pause;//when the timer reaches 0 then the pause screen will activate
			}
			
		} else if (umbrellaRb.velocity.magnitude > 2) {
			autoPauseTimer = 0;//once a key is pressed the timer should revert back to 0
		}
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
//						PlayerPrefs.SetFloat ("PlayerX", lastKnownPosition.x);
//						PlayerPrefs.SetFloat ("PlayerY", lastKnownPosition.y);
//						PlayerPrefs.SetFloat ("PlayerZ", lastKnownPosition.z);
						Application.LoadLevel ("Boucing");

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
			ChangeColours (canopyColour);
		}else{
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

						if (Vector4.Distance (umbrellaChild.material.color, umbrellaColour.color) <= thresholdVector) { // || umbrellaChild.material.color.g >= umbrellaColour.color.g - 0.001f || umbrellaChild.material.color.b >= umbrellaColour.color.b - 0.001f) {
							currentProgress = progression;

							switch (progression) {

							case 2:
								tutorialMission.TutorialMisssionFinished = true;
								break;
							case 3:
								catMission.CatMissionFinished = true;
								break;
							case 4:
								boxesMission.BoxesMisssionFinished = true;
								break;
							case 5:
								horseMission.HorseMisssionFinished = true;
								break;
							default:
								break;
							}
						}
					}
				}
			}
		}
	}

	void OnApplicationQuit ()
	{
		PlayerPrefs.DeleteAll ();
	}
	
}//End of Class

