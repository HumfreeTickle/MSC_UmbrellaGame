using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using CameraScripts;
using Player;

public class Tutuorial : MonoBehaviour
{	
	//------------------------------//
	private GmaeManage gameManager;
	private GameState gameState;
	//------------------------------//
	public float secondsToStart = 4;
	public float startDelay = 1.1f;
	private bool started;
	private bool gameStart;
	//------------------------------//
	/// <summary>
	/// Gets or sets the object tag.
	/// Used to display the correct button.
	/// </summary>
	/// <value>The object tag.</value>
	public string objectTag{ get; set; }
	//------------------------------//
	public Animator windAnim{ get; private set; }
	private Animator Talk_Animator;
	private Animator Interact_Animator;
	private Image Talk_Button;
	private Image Interact_Button;


	void Start ()
	{
		gameManager = GameObject.Find ("Follow Camera").GetComponent<GmaeManage> ();

		if (gameManager.controllerType == ControllerType.ConsoleContoller) {

			if(gameManager.consoleControllerType == ConsoleControllerType.PS3){
				windAnim = GameObject.Find("Tutorial_PS3").GetComponent<Animator> ();
				Talk_Button = GameObject.Find ("L1_tutorial").GetComponent<Image> ();
				Interact_Button = GameObject.Find ("R1_tutorial").GetComponent<Image> ();
				
				Talk_Animator = GameObject.Find ("L1_tutorial").GetComponent<Animator> ();
				Interact_Animator = GameObject.Find ("R1_tutorial").GetComponent<Animator> ();


			}else if(gameManager.consoleControllerType == ConsoleControllerType.XBox){
				windAnim = GameObject.Find("Tutorial_XBox").GetComponent<Animator> ();
				Talk_Button = GameObject.Find ("LB_tutorial").GetComponent<Image> ();
				Interact_Button = GameObject.Find ("RB_tutorial").GetComponent<Image> ();
				
				Talk_Animator = GameObject.Find ("LB_tutorial").GetComponent<Animator> ();
				Interact_Animator = GameObject.Find ("RB_tutorial").GetComponent<Animator> ();
				Debug.Log(windAnim);

			}


		} else if (gameManager.controllerType == ControllerType.Keyboard) {
			Talk_Button = GameObject.Find ("Q_tutorial").GetComponent<Image> ();
			Interact_Button = GameObject.Find ("E_tutorial").GetComponent<Image> ();
		
			windAnim = GameObject.Find ("UP_tutorial").GetComponent<Animator> ();
			Talk_Animator = GameObject.Find ("Q_tutorial").GetComponent<Animator> ();
			Interact_Animator = GameObject.Find ("E_tutorial").GetComponent<Animator> ();
		}
	}
	
	void Update ()
	{
		//------------- Failsafe ----------------//
		if (gameManager == null) {
			return;
		}

		//----------------- Changes the tutorial animation ----------------//
		if (gameManager.gameState == GameState.Intro) {
			if(Time.time < secondsToStart*startDelay){
				Input.ResetInputAxes(); // Stops the player from activating the umbrella too soon
			}

			if (!gameStart) {
				Invoke ("StartingPositions", secondsToStart);
				gameStart = true;
			}

		} else if (gameManager.gameState == GameState.Game) {
			windAnim.SetBool ("Wind", false);
			PressButtions ();
		}

		//------------- Removes tutorial if game is paused or character is dead ---------------------//
		if (windAnim != null && Talk_Button != null && Interact_Button != null) {
			if (gameManager.gameState == GameState.Pause 
			    || gameManager.gameState == GameState.GameOver
			    || gameManager.gameState == GameState.MissionEvent) {

				windAnim.enabled = false;
				Talk_Button.enabled = false;
				Interact_Button.enabled = false;
				if (gameManager.controllerType == ControllerType.ConsoleContoller) {
					for (int i = 0; i < transform.childCount; i++) {
						transform.GetChild (i).gameObject.SetActive (false);
					}
				}
			} else {
				windAnim.enabled = true;
				Talk_Button.enabled = true;
				Interact_Button.enabled = true;
				if (gameManager.controllerType == ControllerType.ConsoleContoller) {
					for (int i = 0; i < transform.childCount; i++) {
						transform.GetChild (i).gameObject.SetActive (true);
					}
				}
			}
		}
	}

	void StartingPositions ()
	{
		windAnim.SetBool ("Wind", true);
	}

//-------------------------------------- Switch statement for all the various Tutorial states --------------------------------------

	void PressButtions ()
	{
		switch (objectTag) {

		case "NPC_talk":
		//-------------------L1 || LB------------------//
			Talk_Animator.SetBool ("Talk", true);
			Interact_Animator.SetBool ("Interact", false);
			break;

		case "Interaction":
		//-------------------R1 || RB------------------//
			Talk_Animator.SetBool ("Talk", false);
			Interact_Animator.SetBool ("Interact", true);
			break;

		case "":
			Talk_Animator.SetBool ("Talk", false);
			Interact_Animator.SetBool ("Interact", false);
			break;

		default:
			Talk_Animator.SetBool ("Talk", false);
			Interact_Animator.SetBool ("Interact", false);
			break;
		}

	}
}//end
