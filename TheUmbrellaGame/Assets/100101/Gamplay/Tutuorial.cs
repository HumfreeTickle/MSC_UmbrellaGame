using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using CameraScripts;
using Player;

public class Tutuorial : MonoBehaviour
{	
	private GmaeManage GameManager;
	public float secondsToStart = 4;
	private GameState gameState;
	private bool started;
	private bool inTheBeginning;
	public Animator windAnim{ get; private set; }

	private Animator Talk_Animator;
	private Animator Interact_Animator;
	private Image Talk_Button;
	private Image Interact_Button;

	public string objectTag{ get; set; }
	
	void Awake ()
	{
		GameManager = GameObject.Find ("Follow Camera").GetComponent<GmaeManage> ();

		if (GameManager.ControllerType == ControllerType.ConsoleContoller) {
			windAnim = GetComponent<Animator> ();

			Talk_Button = GameObject.Find ("L1_tutorial").GetComponent<Image> ();
			Interact_Button = GameObject.Find ("R1_tutorial").GetComponent<Image> ();

			Talk_Animator = GameObject.Find ("L1_tutorial").GetComponent<Animator> ();
			Interact_Animator = GameObject.Find ("R1_tutorial").GetComponent<Animator> ();
		} else if (GameManager.ControllerType == ControllerType.Keyboard) {
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
		if (GameManager == null) {
			return;
		}

		//----------------- Changes the tutorial animation ----------------//
		if (GameManager.GameState == GameState.Intro) {
			if (!inTheBeginning) {
				Invoke ("StartingPositions", secondsToStart);
				inTheBeginning = true;
			}

		} else if (GameManager.GameState == GameState.Game) {
			windAnim.SetBool ("Wind", false);
			PressButtions ();
		}

		//------------- Removes tutorial if game is paused or character is dead ---------------------//
		if (GameManager.GameState == GameState.Pause || GameManager.GameState == GameState.GameOver || GameManager.GameState == GameState.MissionEvent) {

			windAnim.enabled = false;
			Talk_Button.enabled = false;
			Interact_Button.enabled = false;
			if (GameManager.ControllerType == ControllerType.ConsoleContoller) {
				for (int i = 0; i < transform.childCount; i++) {
					transform.GetChild (i).gameObject.SetActive (false);
				}
			}
		} else {
			windAnim.enabled = true;
			Talk_Button.enabled = true;
			Interact_Button.enabled = true;
			if (GameManager.ControllerType == ControllerType.ConsoleContoller) {
				for (int i = 0; i < transform.childCount; i++) {
					transform.GetChild (i).gameObject.SetActive (true);
				}
			}
		}
	}

	public void StartingPositions ()
	{
		windAnim.SetBool ("Wind", true);
	}

//-------------------------------------- Switch statement for all the various Tutorial states --------------------------------------

	void PressButtions ()
	{
		switch (objectTag) {

		case "NPC_talk":
							//L1
			Talk_Animator.SetBool ("Talk", true);
			Interact_Animator.SetBool ("Interact", false);
			break;

		case "Interaction":
							//R1
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
