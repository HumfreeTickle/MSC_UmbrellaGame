using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Tutuorial : MonoBehaviour
{	
	public GmaeManage GameManager;
	public GameObject activate;
	private GameObject activateNode;
	//---------------- Positions of waypoints ------------//
	public Transform movementTutorialPos;
	public Transform upTutorialPos;
	public Transform downTutorialPos;
	public Transform NPCTutorial;
	//----------------------------------------------------//
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

	public Sprite frontPS3;
	public Sprite backPS3;
	private Image tutorialImage;
	public bool movementDone;

	void Awake ()
	{
		tutorialImage = GetComponent<Image> ();
		tutorialImage.sprite = frontPS3;
		animator = GetComponent<Animator> ();

		//---------FailSafes----------//
		if (!activate) {
			return;
		}
		if (!movementTutorialPos) {
			Debug.Log ("movementTutorialPos");
			return;
		} else if (!upTutorialPos) {
			Debug.Log ("upTutorialPos");
			return;
		} else if (!downTutorialPos) {
			Debug.Log ("downTutorialPos");
			return;
		} else if (!NPCTutorial) {
			Debug.Log ("NPCTutorial");
			return;
		}
		//----------------------------//

		activateNode = Instantiate (activate, movementTutorialPos.position, Quaternion.identity) as GameObject;
		activateNode.transform.parent = this.transform;
		activateNode.GetComponent<Light> ().intensity = 0;
		activateNode.GetComponent<Light> ().enabled = false;
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
				animator.SetBool ("GameState", false);
				tutorialImage.sprite = frontPS3;

			} else if (GameManager.gameState == GameState.Game) {
				WalkThroughConditions ();
				animator.SetInteger ("State", X);
				animator.SetBool ("GameState", true);
			} else {
				animator.SetBool ("GameState", false);
			}

			//------------- Removes tutorial if game is paused or character is dead ---------------------//
			if (GameManager.gameState == GameState.Pause || GameManager.gameState == GameState.GameOver || GameManager.gameState == GameState.Talking) {
				GetComponent<Image> ().enabled = false;
			} else {
				GetComponent<Image> ().enabled = true;
			}

			if (movementDone) {
				activateNode.GetComponent<Light> ().intensity = Mathf.Lerp (activateNode.GetComponent<Light> ().intensity, 0, Time.deltaTime*10);
			}
		}
	}

//-------------------------------------- Switch statement for all the various Tutorial states --------------------------------------

	void WalkThroughConditions ()
	{
		switch (x) {

		case 0: // Movement
			tutorialImage.sprite = frontPS3;
			activateNode.transform.position = movementTutorialPos.position;
			activateNode.GetComponent<Light> ().enabled = true;
			activateNode.GetComponent<Light> ().intensity = Mathf.Lerp (activateNode.GetComponent<Light> ().intensity, 4, Time.deltaTime);

			break;

		case 1: //R2
			activateNode.transform.position = upTutorialPos.position;
			tutorialImage.sprite = backPS3;
			animator.SetBool ("Controller", true);

			break;
			
		case 2: //L2
			tutorialImage.sprite = backPS3;
			animator.SetBool ("Controller", true);
			activateNode.transform.position = downTutorialPos.position;

			break;

		case 3: //R1
			tutorialImage.sprite = backPS3;
			animator.SetBool ("Controller", true);
			if (Input.GetButtonDown ("Interact")) {
				movementDone = true;
				x = 5;
			}
			break;
				
		case 4: //L1
			tutorialImage.sprite = backPS3;
			animator.SetBool ("Controller", true);
			if (Input.GetButtonDown ("Talk")) {
				movementDone = true;
				x = 5;
			}
			break;

		case 5: //Default blank state
			if (!movementDone) {
				activateNode.transform.position = NPCTutorial.position;
			}
			break;
		default: //Fail safe
			break;
		}
	}
}
