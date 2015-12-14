using UnityEngine;
using System.Collections;

public class ChurchEnter : MonoBehaviour {

	void OnTriggerEnter(Collider col){
		if(col.gameObject.tag == "Player"){
			GetComponent<MeshRenderer>().enabled = false;
		}
	}
}
