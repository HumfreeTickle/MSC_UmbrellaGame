using UnityEngine;
using System.Collections;

public class StartScreenFollower : MonoBehaviour
{
	public float speed;
	public Transform umbrella;
	
	void Update ()
	{
		transform.LookAt(umbrella);
//			Quaternion rotation = Quaternion.LookRotation (umbrella.position - transform.position);
//			transform.rotation = Quaternion.Slerp (transform.rotation, rotation, Time.deltaTime / speed);
	}
}
