using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using CameraScripts;
using Player;

public class Tutuorial : MonoBehaviour
{	
	public GmaeManage GameManager;
	public GameObject activate;
	private GameObject activateNode;
	private Light activateNodeLight;
	private GameObject cmaera;
	public GameObject umbrella;
	public float secondsToStart = 5;
	private GameState gameState;
	private bool started;
	public bool goXgo;
	//---------------- Positions of waypoints ------------//
	public Transform movementTutorialPos;
	public Transform upTutorialPos;
	public Transform downTutorialPos;
	public Transform NPCTutorial;
	private Transform tutorialInfo;
	//----------------------------------------------------//
	private Animator animator;
	public Animator AnimatorYeah{
		get{
			return animator;
		}

	}
	private int t;
	private int x;

	public int X {
		get {
			return x;
		}
		set {
			x = value;
		}
	}

	private Image tutorialImage;
	public bool movementDone;

	void Awake ()
	{
		tutorialImage = GetComponent<Image> ();
		animator = GetComponent<Animator> ();
		cmaera = GameObject.Find ("Follow Camera");
		umbrella = GameObject.Find ("main_Sphere");
		tutorialInfo = GameObject.Find ("Tutorial_Info").transform;
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
		activateNode.transform.parent = tutorialInfo;
		activateNodeLight = activateNode.GetComponent<Light> ();
		activateNodeLight.enabled = false;
		animator.SetBool ("GameState", false);
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
				if(!IsInvoking("StartingPositions")){
					Invoke("StartingPositions", secondsToStart);
				}
//			} else if (GameManager.gameState == GameState.Game) {
////				if (!started) {
////					StartCoroutine (WalkThroughConditions ());
////				}
//				animator.SetInteger ("State", X);
//				animator.SetBool ("GameState", true);
			} else {
				animator.SetBool ("GameState", false);
			}

			//------------- Removes tutorial if game is paused or character is dead ---------------------//
			if (GameManager.gameState == GameState.Pause || GameManager.gameState == GameState.GameOver || GameManager.gameState == GameState.Event) {
				GetComponent<Image> ().enabled = false;
			} else {
				GetComponent<Image> ().enabled = true;
			}
		}
	}

	void StartingPositions(){
		animator.SetBool ("GameState", true);
		animator.SetBool ("Wind", true);

	}

//-------------------------------------- Switch statement for all the various Tutorial states --------------------------------------

	IEnumerator WalkThroughConditions ()
	{
		while (true) {
			if (goXgo) {
				if (X <= 2) {
					X += 1;
				} else {
					X = 5;
				}
				while (activateNodeLight.intensity >= 0.1f) {
					activateNodeLight.enabled = true;
					activateNodeLight.intensity = Mathf.Lerp (activateNodeLight.intensity, 0, Time.deltaTime);
					yield return null;
				}
				goXgo = false;
			}
		
			started = true;
			/// Glitch - if you activate the talk/interact cases then the activationnode glitches out (light disappears but the flare still stays. You can't activate it)


			//needs to raise and lower the light.intensity between each switch case
			//a camera lock for a second at the beinging of each 
			switch (x) {

			case 0: // Movement
				while (t < 1) { 
					activateNode.transform.position = movementTutorialPos.position;


					cmaera.GetComponent<Controller> ().height = 20;
					GameManager.gameState = GameState.Event;
					cmaera.GetComponent<Controller> ().lookAt = activateNode;

					yield return new WaitForSeconds (1);
					while (activateNodeLight.intensity <= 3.9f) {
						activateNodeLight.enabled = true;
						activateNodeLight.intensity = Mathf.Lerp (activateNodeLight.intensity, 4, Time.deltaTime);
						yield return null;
					}
					t = 1; 
				}


				cmaera.GetComponent<Controller> ().height = 5;
				yield return null;
				cmaera.GetComponent<Controller> ().lookAt = umbrella;
				yield return new WaitForSeconds (3);

				GameManager.gameState = GameState.Game;

				break;

			case 1: //R2
				while (t < 2) { 
					GameManager.gameState = GameState.Event;
					yield return new WaitForSeconds (2);

					activateNode.transform.position = upTutorialPos.position;
					activateNodeLight.intensity = Mathf.Lerp (activateNodeLight.intensity, 4, Time.deltaTime * 10);

					cmaera.GetComponent<Controller> ().height = 20;
					cmaera.GetComponent<Controller> ().lookAt = activateNode;

					yield return new WaitForSeconds (1);
					while (activateNodeLight.intensity <= 3.9f) {
						activateNodeLight.enabled = true;
						activateNodeLight.intensity = Mathf.Lerp (activateNodeLight.intensity, 4, Time.deltaTime);
						yield return null;
					}
					t = 2; 
				}
				cmaera.GetComponent<Controller> ().height = 5;
				yield return null;
				cmaera.GetComponent<Controller> ().lookAt = umbrella;
				yield return new WaitForSeconds (3);

				GameManager.gameState = GameState.Game;
				animator.SetBool ("Controller", true);

				yield return null;
				break;
			
			case 2: //L2
				while (t < 3) { 
					GameManager.gameState = GameState.Event;

					activateNode.transform.position = downTutorialPos.position;

					cmaera.GetComponent<Controller> ().height = 20;
					cmaera.GetComponent<Controller> ().lookAt = activateNode;
				
					yield return new WaitForSeconds (1);
					while (activateNodeLight.intensity <= 3.9f) {
						activateNodeLight.enabled = true;
						activateNodeLight.intensity = Mathf.Lerp (activateNodeLight.intensity, 4, Time.deltaTime);
						yield return null;
					}
					t = 3;
				}
				cmaera.GetComponent<Controller> ().height = 5;
				yield return null;
				cmaera.GetComponent<Controller> ().lookAt = umbrella;
				yield return new WaitForSeconds (3);

				GameManager.gameState = GameState.Game;

				animator.SetBool ("Controller", true);

				yield return null;

				break;

			case 3: //R1
				animator.SetBool ("Controller", true);
				if (Input.GetButtonDown ("Interact")) {
					movementDone = true;
					x = 5;
				}
				break;
				
			case 4: //L1
				animator.SetBool ("Controller", true);
				if (Input.GetButtonDown ("Talk")) {
					movementDone = true;
					x = 5;
				}
				break;

			case 5: //Default blank state
				if (movementDone) {
					activateNode.transform.position = NPCTutorial.position;
					while (activateNodeLight.intensity <= 3.9f) {
						activateNodeLight.enabled = true;
						activateNodeLight.intensity = Mathf.Lerp (activateNodeLight.intensity, 4, Time.deltaTime);
						yield return null;
					}
				}
				break;
			default: //Fail safe
				activateNode.transform.position = NPCTutorial.position;
				while (activateNodeLight.intensity <= 3.9f) {
					activateNodeLight.enabled = true;
					activateNodeLight.intensity = Mathf.Lerp (activateNodeLight.intensity, 4, Time.deltaTime);
					yield return null;
				}
				break;
			}
			yield return null;
		}
	}
}
