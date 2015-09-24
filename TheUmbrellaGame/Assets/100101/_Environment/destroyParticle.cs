using UnityEngine;
using System.Collections;
using Inheritence;

public class destroyParticle : MonoBehaviour {
	private DestroyObject destroy = new DestroyObject();
	public float time = 3;
	
//------------------------------ Destroys particles -----------------------------------

	void Update () {
		destroy.DestroyOnTimer(this.gameObject, time);
	}
}
