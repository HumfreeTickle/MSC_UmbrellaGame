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
	private Animator animator;

	public Animator AnimatorYeah {
		get {
			return animator;
		}

	}
	public string objectTag;

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
	
	void Awake ()
	{
		GameManager = GameObject.Find ("Follow Camera").GetComponent<GmaeManage>();
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
			if (GameManager.gameState == GameState.Pause || GameManager.gameState == GameState.GameOver || GameManager.gameState == GameState.MissionEvent) {
				GetComponent<Image> ().enabled = false;
				for(int i = 0; i < transform.childCount; i++){
					transform.GetChild(i).gameObject.SetActive(false);
				}
			} else {
				GetComponent<Image> ().enabled = true;
				for(int i = 0; i < transform.childCount; i++){
					transform.GetChild(i).gameObject.SetActive(true);
				}
			}
		}else{
			GetComponent<Image>().enabled = false;
			for(int i = 0; i < transform.childCount; i++){
				transform.GetChild(i).gameObject.SetActive(false);
			}
		}
	}

	public void StartingPositions ()
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
