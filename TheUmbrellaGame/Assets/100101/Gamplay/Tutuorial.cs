using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using CameraScripts;
using Player;

public class Tutuorial : MonoBehaviour
{	
	public GmaeManage GameManager;

	public float secondsToStart = 5;
	private GameState gameState;
	private bool started;
	private bool inTheBeginning;
	private Animator animator;

	public Animator AnimatorYeah {
		get {
			return animator;
		}

	}
	private string objectTag;

	/// <summary>
	/// Used for the tutorial for button commands
	/// Based on what tag is attached to the object
	/// </summary>
	/// <value>The object tag.</value>

	public string ObjectTag {
		set {
			objectTag = value;
		}

	}

	public bool movementDone;

	void Awake ()
	{
		animator = GetComponent<Animator> ();
	}
	
	void Update ()
	{

		//------------- Failsafe ----------------//
		if (GameManager == null) {
			return;
		}
		if (GameManager.controllerType == ControllerType.ConsoleContoller) {


			//----------------- Changes the tutorial animation ----------------//
			if (GameManager.gameState == GameState.Intro) {
				if (!inTheBeginning) {
					Invoke ("StartingPositions", secondsToStart);
					inTheBeginning = true;
				}
			} else if (GameManager.gameState == GameState.Game) {
				animator.SetBool ("Wind", false);
				if (!started) {
					StartCoroutine (PressButtons ());
				}
			}

			//------------- Removes tutorial if game is paused or character is dead ---------------------//
			if (GameManager.gameState == GameState.Pause || GameManager.gameState == GameState.GameOver || GameManager.gameState == GameState.Event) {
				GetComponent<Image> ().enabled = false;
			} else {
				GetComponent<Image> ().enabled = true;
			}
		}
	}

	void StartingPositions ()
	{

		animator.SetBool ("Wind", true);

	}

//-------------------------------------- Switch statement for all the various Tutorial states --------------------------------------

	IEnumerator PressButtons ()
	{
		while (true) {
			started = true;

			switch (objectTag) {
			case "NPC_talk":
				//L1
				animator.SetBool ("Talk", true);
				animator.SetBool ("Interact", false);
				break;
			case "Interaction":
				//R1
				animator.SetBool ("Talk", false);
				animator.SetBool ("Interact", true);
				break;

			default:
				animator.SetBool ("Talk", false);
				animator.SetBool ("Interact", false);
				break;
			}

			yield return null;

		}

	}

}//end
