using UnityEngine;

public class StartUpSceneSelection : MonoBehaviour
{
 void Awake()
	{
		if (Input.GetJoystickNames ().Length > 0) {

			Application.LoadLevel("Controller Select");
			Debug.Log ("Controller");

		} else {

			Application.LoadLevel("Start_Screen");

			Debug.Log ("Keyboard");
		}	

		Debug.Log ("Running");
	}
}
