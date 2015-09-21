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

public enum GameState // sets what game state is currently being viewed
{
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
	Event,
	/// <summary>
	/// Main state of the game. Allows for full controller over the umbrella
	/// </summary>
	Game,
	/// <summary>
	/// Game Over :(
	/// </summary>
	GameOver
}

//Might work better as a delegate
public enum ControllerType
{
	NullState,
	Keyboard,
	ConsoleContoller
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
	private NPCManage npcManager;
//--------------------------------------------------------------------------------------------------//


	private GameObject cameraController; // 

	//--- Audio Stuff ---
	private AudioClip harpIntroClip;
	private AudioSource harpIntroSource;
	public AudioMixerSnapshot start;
	private AudioClip mainThemeMusic;
	private GameObject backgroundMusic;
	public AudioMixerSnapshot PausedSnapShot;
	public AudioMixerSnapshot InGameSnapShot;


	//-- Canvas Stuff ---
	private Image PauseScreen; // pause screen image
	private Image WhiteScreen;
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
<<<<<<< HEAD
=======

	//-----------------------------------------------//
	public Vector3 lastKnownPosition;

	public Vector3 LAstKnownPosition {
		set {
			lastKnownPosition = value;
		}
	}

	public Vector3 startingPos;
	//-----------------------------------------------//
>>>>>>> origin/master
	private Camera cameraClipFar;
	public List<Material> allTheColoursOfTheUmbrella;
	public List<Transform> canopyColours;
	private Material umbrellaColour;
	public float thresholdVector;
	//Start script Stuff
	private bool nextLevel; // has the transtion to Level-1 been activated
	public bool timeStart;


	// Eventually these should be made into a dynamic list that is moved in and out depending on the type of controls needed
	private string controllerTypeVertical;
	private string controllerTypeHorizontal; 


	//-----------------------
	private Text npc_Talking;
	private Image npc_TalkingBox;
	private NPC_Interaction npc_Interact;

<<<<<<< HEAD
<<<<<<< HEAD
=======
	//Missions
	private NPC_TutorialMission tutorialMission;

>>>>>>> origin/master
=======
	//Missions
	private NPC_TutorialMission tutorialMission;

>>>>>>> origin/master

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
	
	public GameState gameState { get; set; }

	private GameState currentState = GameState.NullState;
	
	public ControllerType controllerType { get; set; }

	private ControllerType currentController = ControllerType.NullState;
	
	//--------------------------------------------------------------------------------------------------//

//-------------------------------------- The Setup -----------------------------------------------------------------

	void Awake ()
	{
		DontDestroyOnLoad (transform.gameObject);

		//-------------------- For the different controllers ---------------------------------
		if (Input.GetJoystickNames ().Length > 0) {// checks to see if a controller is connected
			controllerType = ControllerType.ConsoleContoller;
			controllerTypeVertical = "Vertical_L";
			controllerTypeHorizontal = "Horizontal_L";
			print ("Player 1: Connected");


		} else if (Input.GetJoystickNames ().Length == 0) {
			controllerType = ControllerType.Keyboard;
			controllerTypeVertical = "Vertical";
			controllerTypeHorizontal = "Horizontal";

		} else {
			controllerType = ControllerType.NullState;
			Debug.Log ("Disconnected");
		}

		//-------------------- For the different Scenes ---------------------------------

		if (Application.loadedLevel == 0) { //Start screen

			gameState = GameState.Intro; 
			startButton = GameObject.Find ("Start Button").GetComponent<Image> ();
			umbrellaGame = GameObject.Find ("Umbrella Logo").GetComponent<Image> ();
			WhiteScreen = GameObject.Find ("WhiteScreen").GetComponent<Image> ();
			harpIntroSource = GameObject.Find ("Harp intro").GetComponent<AudioSource> ();
			harpIntroClip = harpIntroSource.clip;
			harpIntroSource.pitch = -0.6f;


			umbrella = GameObject.Find ("Umbrella");
			umbrellaRb = umbrella.GetComponent<Rigidbody> ();

			if (!startButton || !umbrellaGame || !umbrella || !WhiteScreen) {
				return;
			}

		} else if (Application.loadedLevel == 1) { //Main screen

			gameState = GameState.Intro; 
			PauseScreen = GameObject.Find ("Pause Screen").GetComponent<Image> ();
			WhiteScreen = GameObject.Find ("WhiteScreen").GetComponent<Image> ();
			WhiteScreen.color = Color.white;

			cameraClipFar = GetComponent<Camera> ();
//			cameraClipFar.farClipPlane = 800;


			umbrella = GameObject.Find ("main_Sphere");
			umbrellaRb = umbrella.GetComponent<Rigidbody> ();
			if (PlayerPrefs.GetFloat ("PlayerX") != 0 || PlayerPrefs.GetFloat ("PlayerY") != 0 || PlayerPrefs.GetFloat ("PlayerZ") != 0) {
				startingPos = new Vector3 (PlayerPrefs.GetFloat ("PlayerX"), PlayerPrefs.GetFloat ("PlayerY"), PlayerPrefs.GetFloat ("PlayerZ"));
			}else{
				startingPos = umbrella.transform.localPosition;
			}
			umbrella.transform.localPosition = startingPos;

			backgroundMusic = GameObject.Find ("Music");
			npcManager = this.GetComponent<NPCManage> ();
			npc_Talking = GameObject.Find ("NPC_Talking").GetComponent<Text> ();
			npc_Talking.fontSize = 25;
<<<<<<< HEAD
<<<<<<< HEAD
=======


			//----------------- Missions Complete Stuff --------------------//
			tutorialMission = GameObject.Find("NPC_Tutorial").GetComponent<NPC_TutorialMission>();
>>>>>>> origin/master
=======


			//----------------- Missions Complete Stuff --------------------//
			tutorialMission = GameObject.Find ("NPC_Tutorial").GetComponent<NPC_TutorialMission> ();
>>>>>>> origin/master

			if (!PauseScreen || !WhiteScreen) {
				return;
			}
		}
	}

//-------------------------------------- All the calls -----------------------------------------------------------------

	void Update ()
	{
		if (Input.GetButtonDown ("Undefined")) {
			if (progression == currentProgress) {
				progression += 1;
			}
		}

		if (progression > currentProgress) {
			Progress ();
		}


		if (Input.GetButtonDown ("Undefined")) {
			if (progression == currentProgress) {
				progression += 1;
			}
		}

		if (progression > currentProgress) {
			Progress ();
		}

		if (gameState == GameState.Intro) {
			StartGame ();

		} else if (gameState != GameState.Intro) {//gameState == GameState.Game || gameState == GameState.Pause || gameState == GameState.GameOver ) {
<<<<<<< HEAD
<<<<<<< HEAD
//			progression = Mathf.Clamp (progression, 1, Mathf.Infinity);
=======
>>>>>>> origin/master
=======
>>>>>>> origin/master

			RestartGame ();

			if (gameState != GameState.GameOver) {//so the player can't pause when they die
				PauseGame ();
			}
			EndGame ();
		}
		CheckStates ();
	}

//-------------------------------------- Start Game is elsewhere for some reason -----------------------------------------------------------------
	
	void StartGame ()
	{
		if (Application.loadedLevel == 0) { //Opening screen

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

		} else if (Application.loadedLevel == 1) { //Main game screen
			Physics.gravity = new Vector3 (0, -18.36f, 0);
<<<<<<< HEAD
			//----------------------------------//
			if (Time.timeSinceLevelLoad > 4) {
				gameState = GameState.Game;
			}
			//----------------------------------//
=======
>>>>>>> origin/master

			WhiteScreenTransisitions ();

		}
	}

//-------------------------------------- Resets the game -----------------------------------------------------------------
	
	void RestartGame ()
	{
		if (Input.GetKeyDown (KeyCode.R)) {
			Application.LoadLevel ("Boucing");
		}
	}

//-------------------------------------- Pauses the game -----------------------------------------------------------------
	
	void PauseGame ()
	{
		if (Input.GetButtonDown ("Submit")) {

			if (gameState == GameState.Game) {
				gameState = GameState.Pause;
				PausedSnapShot.TransitionTo (0);

			} else if (gameState == GameState.Pause) {
				gameState = GameState.Game;
				autoPauseTimer = 0;
				NotPaused ();
				InGameSnapShot.TransitionTo (0);
			}
		}

		if (gameState == GameState.Pause) {
			Paused ();
		} else if (gameState == GameState.Game) {
			NotPaused ();
		}
		
		FixedPause ();	
	}
	
//-------------------------------------- Ending the game is here (sort of) -----------------------------------------------------------------
	
	void EndGame ()
	{
		if (_charge < 1) {
			gameState = GameState.GameOver;
		}

		if (gameState == GameState.GameOver) {


			_gameOverTimer += Time.deltaTime;
			WhiteScreenTransisitions ();
			cameraClipFar.farClipPlane = Mathf.Lerp (cameraClipFar.farClipPlane, 2000, Time.deltaTime);
		}
	}

//-------------------------------------- State Checking ---------------------------------------------------------------------------
	void CheckStates ()
	{
		if (controllerType != currentController) {
			Debug.Log (controllerType);
		}
		
		if (gameState != currentState) {
			Debug.Log (gameState);
		}
		
		currentState = gameState;
		currentController = controllerType;
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
				gameState = GameState.Pause;//when the timer reaches 0 then the pause screen will activate
			}
			
		} else if (umbrellaRb.velocity.magnitude > 2) {
			autoPauseTimer = 0;//once a key is pressed the timer should revert back to 0
		}
	}
	
	
	
	//-------------------------------------- White Screen Stuff -----------------------------------------------------------------
	void WhiteScreenTransisitions ()
	{
		if (gameState == GameState.Intro) {
			fading.FadeOUT (WhiteScreen, 3);
			
		} else if (gameState == GameState.GameOver) {
			if (_gameOverTimer > 2) {
				fading.FadeIN (WhiteScreen, 1);

				if (WhiteScreen.color.a >= 0.95) {
					if (Application.loadedLevel == 0) {
						Application.LoadLevel ("Boucing");
					} else if (Application.loadedLevel == 1) {
						PlayerPrefs.SetFloat ("PlayerX", lastKnownPosition.x);
						PlayerPrefs.SetFloat ("PlayerY", lastKnownPosition.y);
						PlayerPrefs.SetFloat ("PlayerZ", lastKnownPosition.z);
						Application.LoadLevel ("Boucing");
					}
				}
			}
		}
	}

	void whichLevel ()
	{
		Application.LoadLevel (1); //Changes to the next scene
	}

	void Progress ()
	{
		if (progression < 6) {
			umbrellaColour = allTheColoursOfTheUmbrella [progression - 2];
			ChangeColours (canopyColours [progression - 2]);
		}
	}

	void ChangeColours (Transform obj)
	{
		for (int child = 0; child< obj.childCount; child++) { //goes through each child object one at a time
			if (obj.GetChild (child).transform.childCount > 0) {
				ChangeColours (obj.GetChild (child));
			} else {
				if (obj.GetChild (child).GetComponent<MeshRenderer> ()) { // checks to see if there is a mesh renderer attached to child
					MeshRenderer umbrellaChild = obj.GetChild (child).GetComponent<MeshRenderer> ();
					umbrellaChild.material.Lerp (umbrellaChild.material, umbrellaColour, Time.deltaTime);

<<<<<<< HEAD
					if (Vector4.Distance(umbrellaChild.material.color, umbrellaColour.color) <= thresholdVector){ // || umbrellaChild.material.color.g >= umbrellaColour.color.g - 0.001f || umbrellaChild.material.color.b >= umbrellaColour.color.b - 0.001f) {
						currentProgress = progression;
<<<<<<< HEAD
=======
						tutorialMission.MisssionFinished = true;
>>>>>>> origin/master
=======
					if (Vector4.Distance (umbrellaChild.material.color, umbrellaColour.color) <= thresholdVector) { // || umbrellaChild.material.color.g >= umbrellaColour.color.g - 0.001f || umbrellaChild.material.color.b >= umbrellaColour.color.b - 0.001f) {
						currentProgress = progression;
						tutorialMission.MisssionFinished = true;
>>>>>>> origin/master
					}
				}
			}
		}
	}
<<<<<<< HEAD
=======

	void OnApplicationQuit ()
	{
		PlayerPrefs.DeleteAll ();
	}
>>>>>>> origin/master
	
}//End of Class

