using UnityEngine;
using System.Collections;
using CameraScripts;

public class _MoveCamera : MonoBehaviour
{
	private GmaeManage gameManager;
	private Controller cmaera;
	private GameObject cmaeraSet;
	private GameObject umbrella;

	private Talk talkCoroutine;
	private bool startCoroutineCamera = false;

	/// <summary>
	/// Sets a value indicating whether this <see cref="Talk"/> start coroutine.
	/// **The getter is only for testing**
	/// </summary>
	/// <value><c>true</c> if start coroutine; otherwise, <c>false</c>.</value>
	public bool StartCoroutineCamera {
		get {
			return startCoroutineCamera;
		}
		private set{ startCoroutineCamera = value;}
	}

	void Start ()
	{
		gameManager = GetComponent<GmaeManage> ();
		cmaera = GetComponent<Controller> ();
		umbrella = GameObject.Find ("main_Sphere");
		talkCoroutine = GameObject.Find("NPC_TalkBox").GetComponent<Talk>();
	}

	/// <summary>
	/// Moves the camera's focus and location as needed
	/// </summary>
	/// <returns>The move.</returns>
	/// <param name="lookAT">What to focus the camera on</param>
	/// <param name="moveTo">Location to move camera to</param>
	public IEnumerator cameraMove (GameObject lookAT, System.Action finishedCallBack = null, Transform moveTo = null, float waitTime = 3f)
	{
		if (startCoroutineCamera) { // stops coroutine from constatly triggering
			Debug.LogError ("Camera Already Moving");
			yield break;
		}

		startCoroutineCamera = true;

		if (gameManager.GameState != GameState.MissionEvent) {
			gameManager.GameState = GameState.MissionEvent;
		}

		yield return null;

		cmaeraSet = lookAT; // assigns the cameraSet ot lookAT
		cmaera.lookAt = cmaeraSet; // changes the camera's focus
		cmaera.MoveYerself = false; // stops part of the camera controller script from happening so the camera doesn't move it's own position


		if (moveTo != null) {// is there a location to move to 

			while (Vector3.Distance(cmaera.transform.position, moveTo.position) > 10) {
				cmaera.transform.position = Vector3.Lerp (cmaera.transform.position, moveTo.position, Time.deltaTime / 2);
						
				yield return null;
			}
					
		}

		yield return new WaitForSeconds (waitTime);

		cmaeraSet = umbrella; // assigns the cameraSet ot lookAT
		cmaera.lookAt = cmaeraSet; // changes the camera's focus

		if (moveTo != null) {
			while (Vector3.Distance(cmaera.transform.position, umbrella.transform.position) > 15) {
				cmaera.transform.position = Vector3.Lerp (cmaera.transform.position, umbrella.transform.position, Time.deltaTime);
				yield return null;
			}
		}

		cmaera.MoveYerself = true; // stops part of the camera controller script from happening so the camera doesn't move it's own position

		if (!talkCoroutine.StartCoroutineTalk) {
			if (gameManager.GameState == GameState.MissionEvent) {
				gameManager.GameState = GameState.Game; // default play state
			}
		}

		yield return null;

		if (finishedCallBack != null) {
			finishedCallBack (); 
			// calls the system action if there is one
			// useful to allow the coroutine to be generic
		}
		
		startCoroutineCamera = false;
		
		yield break;
	}


	// needs an overload so I can just use a list to access different camera positions and focuses
}
