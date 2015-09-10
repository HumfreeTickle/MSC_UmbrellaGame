using UnityEngine;
using System.Collections;

public class Collision : MonoBehaviour
{
	private Tutuorial tutorialCanvas;

	void Awake(){

		tutorialCanvas = GameObject.Find("Tutorial").GetComponent<Tutuorial>();
	}

	void OnTriggerEnter (Collider col)
	{
		if (col.gameObject.tag == "Player") {
			if (!IsInvoking ("InvokeX")) {
				Invoke ("InvokeX", 0.1f);
			}
		}
	}

	void InvokeX ()
	{
		tutorialCanvas.goXgo = true;
	}
}
