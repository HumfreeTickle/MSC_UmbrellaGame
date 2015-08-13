using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Tutuorial : MonoBehaviour
{

//------------------------------------------ Needs to be completely overhalled ------------------------------------------
//------------------------------------------- "Don't leave half of the tutorial out" - Owen Harris, 2015 ------------------------------------------

	public GmaeManage GameManager;
	private Animator animator;
	private int x;

	public int X {
		get {
			return x;
		}
		set {
			x = value;
		}
	}

	public float _time = 0;
	public Sprite frontPS3;
	public Sprite backPS3;
	private Image tutorialImage;

	void Start ()
	{
		tutorialImage = GetComponent<Image>();
		tutorialImage.sprite = frontPS3;
		animator = GetComponent<Animator> ();
	}
	
	void Update ()
	{
		//------------- Failsafe ----------------//
		if (GameManager == null) {
			return;
		}

		//----------------- Changes the tutorial animation ----------------//
		if (GameManager.gameState == GameState.Intro) {
			animator.SetBool ("GameState", false);
			tutorialImage.overrideSprite = frontPS3;

		} else if (GameManager.gameState == GameState.Game) {
			WalkThroughConditions ();
			animator.SetInteger("State", X);
			animator.SetBool ("GameState", true);
		} else {
			animator.SetBool ("GameState", false);
		}

		//------------- Removes tutorial if game is paused or character is dead ---------------------//
		if (GameManager.gameState == GameState.Pause || GameManager.gameState == GameState.GameOver) {
			GetComponent<Image> ().enabled = false;
		} else {
			GetComponent<Image> ().enabled = true;
		}
	}

//-------------------------------------- Switch statement for all the various Tutorial states --------------------------------------

	void WalkThroughConditions ()
	{

		switch (x) {

		case 0: // Movement
			tutorialImage.sprite = frontPS3;

			if (Mathf.Abs (Input.GetAxis ("Vertical_L")) > 0.1f || Mathf.Abs (Input.GetAxis ("Horizontal_L")) > 0.1f) {
				_time += Time.deltaTime;
				if (_time > 5) {
					x = 5;
					_time = 0;
				}
			}
			break;

		case 1: //L1
			tutorialImage.sprite = backPS3;
			if (Input.GetButtonDown ("Talk")) {
				x = 5;
			}
			break;

		case 2: //R1
			tutorialImage.sprite = backPS3;

			if (Input.GetButtonDown ("Interact")) {
				x = 5;
			}
			break;

		case 5: //Default blank state
			break;
		default: //Fail safe
			break;
		}
	}
}
