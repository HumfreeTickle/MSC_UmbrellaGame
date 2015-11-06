using UnityEngine;
using System.Collections;
using CameraScripts;

public class _MoveCamera : MonoBehaviour
{
	private GmaeManage gameManager;
	private Controller cmaera;
	private GameObject cmaeraSet;
	private GameObject defaultState; // the umbrella's main sphere that the camera is always looking at
	private bool zeldafy; // used to reset the camera's look at target. Named so because the camera moves like in Zelda games.

	void Start ()
	{
		gameManager = GameObject.Find("Follow Camera").GetComponent<GmaeManage>();
		cmaera = GameObject.Find("Follow Camera").GetComponent<Controller>();
		defaultState = GameObject.Find("main_Sphere");
	}

	IEnumerator cameraMove (Transform moveTo)
	{
		while (true) {
			while (zeldafy) {
				gameManager.GameState = GameState.MissionEvent;
				cmaeraSet = this.gameObject;
				cmaera.lookAt = cmaeraSet;
				cmaera.MoveYerself = false;
				
				while (Vector3.Distance(cmaera.transform.position, moveTo.position) > 10 && cmaeraSet ==  this.gameObject) {
					if (!cmaera.MoveYerself) {
						cmaera.transform.position = Vector3.Lerp (cmaera.transform.position, moveTo.position, Time.deltaTime / 2);
						
						yield return null;
					}
					zeldafy = false;
					
					yield return null;
					
				}
				yield return null;
				
			}
			yield return new WaitForSeconds (5);
			cmaeraSet = defaultState;
			cmaera.lookAt = cmaeraSet;
			cmaera.MoveYerself = true;
			gameManager.GameState = GameState.Game;
		}
		
		yield break;
		//			look back at the windmill when you've made it start :)
	}
}
