using UnityEngine;
using System.Collections;

public class DropTheBridge : MonoBehaviour {

	public bool drop;
	public float minAngle;
	public float maxAngle;
	public float speed;
	public Transform from;
	public Transform to;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		if (drop){
			//transform.rotation = Quaternion.Slerp(from.rotation, to.rotation, Time.time * speed);
 			float angle = Mathf.LerpAngle (minAngle, maxAngle, Time.deltaTime * speed);
			transform.eulerAngles = new Vector3(-112,0,0);
		}
	}
}