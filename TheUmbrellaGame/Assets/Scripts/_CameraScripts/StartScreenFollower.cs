using UnityEngine;
using System.Collections;
 // not used
public class StartScreenFollower : MonoBehaviour
{
	public float speed;
	public Transform umbrella;
	
	void Update ()
	{
		transform.LookAt(umbrella);
	}
}
