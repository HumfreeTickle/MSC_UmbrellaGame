using UnityEngine;

/// <summary>
/// Easiest way I found to change to a particular scene
/// Depending on whether a control is attached or not
/// </summary>
public class StartUpSceneSelection : MonoBehaviour
{
 void Awake()
	{
		if (Input.GetJoystickNames ().Length > 0) {
			Application.LoadLevel(1);
		} else {
			Application.LoadLevel(2);
		}	
	}
}
