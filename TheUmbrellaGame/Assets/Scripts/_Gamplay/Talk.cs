using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Talk : MonoBehaviour
{
	private GmaeManage gameManager;

	//--------------------------//
	private string npc_Message;
	private Text npc_Talking;
	//--------------------------//
	public float talkingSpeed = 0.02f;
	public float speed = 2f;
	public float talkboxSpeed = 0.1f;
	//--------------------------//
	private Image npc_TalkBox;
	public Animator Talk_Click;
	//--------------------------//
	private bool proceed;
	private int i = 0;
	private Vector4 colourFadeIN;
	private Vector4 colourFadeOUT;
	//--------------------------//

	private bool startCoroutine = false;
	/// <summary>
	/// Sets a value indicating whether this <see cref="Talk"/> start coroutine.
	/// **The getter is only for testing**
	/// </summary>
	/// <value><c>true</c> if start coroutine; otherwise, <c>false</c>.</value>
	public bool StartCoroutineTalk {
		get {
			return startCoroutine;
		}
		private set{ startCoroutine = value;}
	}

	private _MoveCamera cameraMove;

	void Start ()
	{
		gameManager = GameObject.Find ("Follow Camera").GetComponent<GmaeManage> ();
		npc_Talking = GameObject.Find ("NPC_Talking").GetComponent<Text> ();
		npc_TalkBox = GetComponent<Image> ();

		if (gameManager.ControllerType == ControllerType.ConsoleContoller) {
			Talk_Click = this.transform.GetChild (0).GetComponent<Animator> ();
		}else if(gameManager.ControllerType == ControllerType.Keyboard) {
			Talk_Click = this.transform.GetChild (1).GetComponent<Animator> ();
		}

		cameraMove = GameObject.Find ("Follow Camera").GetComponent<_MoveCamera> ();
		colourFadeIN = new Vector4 (npc_TalkBox.color.r, npc_TalkBox.color.g, npc_TalkBox.color.b, 0.5f);
		colourFadeOUT = new Vector4 (npc_TalkBox.color.r, npc_TalkBox.color.g, npc_TalkBox.color.b, 0f); 
	}

	//-------------------------------- Overload method --------------------------------//
	// means I don't have to always create an array to pass into "whatToSay"
	/// <summary>
	///	Creates a staggered chatbox.
	/// </summary>
	/// <param name="whatToSay">What to say.</param>
	/// <param name="finishedCallBack">Action to take at the end of the coroutine.</param>
	public IEnumerator Talking (string whatToSay, System.Action finishedCallBack = null)
	{
		return Talking (new string[]{whatToSay}, finishedCallBack);
	}

	/// <summary>
	///	Creates a staggered chatbox.
	/// </summary>
	/// <param name="whatToSay">What to say.</param>
	/// <param name="finishedCallBack">Action to take at the end of the coroutine.</param>
	public IEnumerator Talking (string[] whatToSay, System.Action finishedCallBack = null)
	{
		// Doesn't allow for multiple calls.
		if (startCoroutine) {
			Debug.LogError ("Already Talking");
			yield break;
		}

		i = 0;// resets i
		StartCoroutineTalk = true;

		if (gameManager.GameState != GameState.MissionEvent) {
			gameManager.GameState = GameState.MissionEvent;
		}

		// 
		while (Vector4.Distance(npc_TalkBox.color, colourFadeIN) > Mathf.Clamp(talkboxSpeed, 0.001f, Mathf.Infinity)) {
			npc_TalkBox.color = Vector4.Lerp (npc_TalkBox.color, colourFadeIN, Time.deltaTime * Mathf.Clamp (speed, 1, Mathf.Infinity));
			yield return null;
		}

		foreach (string text in whatToSay) {// cycles through the array and out puts a new piece of dialouge after each button press
			while (i <= text.Length) {
				Talk_Click.SetBool ("proceed", false);

				npc_Message = text;
				npc_Talking.text = (npc_Message.Substring (0, i)); //adds a new character each cycle
				i += 1;
				yield return new WaitForSeconds (talkingSpeed); // how fast each character appears on screen
			}

			while (!Input.GetButtonDown(gameManager.controllerTalk)) {// waits for a button press to contiue
				Talk_Click.SetBool ("proceed", true);
				i = 0; 
				yield return null;
			}
		}

		//--------- Clean up ------------------//
		npc_Message = "";
		npc_Talking.text = "";
		while (Vector4.Distance(npc_TalkBox.color, colourFadeOUT) > Mathf.Clamp(talkboxSpeed, 0.001f, Mathf.Infinity)) {
			npc_TalkBox.color = Vector4.Lerp (npc_TalkBox.color, colourFadeOUT, Time.deltaTime * Mathf.Clamp (speed, 1, Mathf.Infinity));
			yield return null;
		}
		npc_TalkBox.color = colourFadeOUT;
		Talk_Click.SetBool ("proceed", false);

		//------------------------------------//
		if (!cameraMove.StartCoroutineCamera) {
			if (gameManager.GameState == GameState.MissionEvent) {
				gameManager.GameState = GameState.Game; // default play state
			}
		}

		if (finishedCallBack != null) {
			finishedCallBack (); 
			// calls the system action if there is one
			// useful to allow the coroutine to be generic
		}

		startCoroutine = false; // stops coroutine
		yield break;
	}
}
