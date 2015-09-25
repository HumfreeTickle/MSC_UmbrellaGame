using UnityEngine;
using System.Collections;

public class Temp_FinalMission : MonoBehaviour
{

	private Animator bridgeAnimation;
	private GmaeManage gameManager;
	private bool playParticles;
	private Transform umbrella;
	public GameObject particales;

	void Start ()
	{
		bridgeAnimation = GameObject.Find ("Walkway-Bridge_C-Basic").GetComponent<Animator> ();
		gameManager = GameObject.Find ("Follow Camera").GetComponent<GmaeManage> ();
		umbrella = GameObject.Find("main_Sphere").transform;
	}
	
	void OnTriggerStay (Collider col)
	{
		if (col.gameObject.tag == "Player") {
			if (Input.GetButtonDown ("Interact")) {
				bridgeAnimation.SetBool ("Fixed", false);
				if (playParticles) {
					Instantiate (particales, umbrella.position + new Vector3 (0, 1f, 0), Quaternion.identity);
					playParticles = false;
					
				}
				gameManager.Progression = 5;
			}
		}
	}
}
