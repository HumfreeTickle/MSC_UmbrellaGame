using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Tutuorial : MonoBehaviour
{

//------------------------------------------ Needs to be completely overhalled ------------------------------------------
//------------------------------------------- "Don't leave half of the tutorial out" - Owen Harris, 2015 ------------------------------------------

	public GmaeManage GameManager;
	private Animator animator;
	public int x;

	public int X {
		get {
			return x;
		}
		set {
			x = value;
		}
	}

	public float _time = 0;
	
	void Start ()
	{
		animator = GetComponent<Animator> ();
	}
	
	void Update ()
	{
		//------------- Failsafe ----------------//
		if (GameManager == null) {
			return;
		}

		//----------------- Changes the tutorial animation ----------------//
		if (GameManager.gameState == GameState.Idle) {
			animator.SetBool ("GameState", false);
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
			if (Mathf.Abs (Input.GetAxisRaw ("Vertical_L")) > 0 || Mathf.Abs (Input.GetAxisRaw ("Horizontal_L")) > 0) {
				_time += Time.deltaTime;
				if (_time >= 5) {
					x = 5;
					_time = 0;
				}
			}
			break;

		case 1: //L1
			if (Input.GetButtonDown ("Talk")) {
				x = 5;
			}
			break;

		case 2: //R1
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
