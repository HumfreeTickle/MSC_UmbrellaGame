using UnityEngine;
using System.Collections;

public class DropTheBridge : MonoBehaviour {

	public bool drop;
	public float minAngle;
	public float maxAngle;
	public float speed;
	public Transform from;
	public Transform to;
	private Animator animator;

	// Use this for initialization
	void Awake () {
	
		animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
	
		if (drop){
<<<<<<< HEAD
			//transform.rotation = Quaternion.Slerp(from.rotation, to.rotation, Time.time * speed);
// 			float angle = Mathf.LerpAngle (minAngle, maxAngle, Time.deltaTime * speed);
			transform.eulerAngles = new Vector3(-112,0,0);
=======
//			//transform.rotation = Quaternion.Slerp(from.rotation, to.rotation, Time.time * speed);
// 			float angle = Mathf.LerpAngle (minAngle, maxAngle, Time.deltaTime * speed);
//			transform.eulerAngles = new Vector3(-112,0,0);
			animator.SetBool("Fixed", false);
>>>>>>> Peter
		}
	}
}