using UnityEngine;
using System.Collections;
using Inheritence;

public class destroyParticle : MonoBehaviour {
	private float _timer;	
	private DestroyObject destroy = new DestroyObject();
	
//------------------------------ Destroys particles -----------------------------------

	void Update () {
		destroy.DestroyOnTimer(this.gameObject, 3);
	}
}
