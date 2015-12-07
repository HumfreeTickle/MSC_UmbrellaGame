using UnityEngine;
using System.Collections;


/// <summary>
/// Can't remember if this is used or not
/// </summary>
public class StartScreenFollower : MonoBehaviour
{
	public float speed;
	public Transform umbrella;
	
	void Update ()
	{
		transform.LookAt(umbrella);
	}
}
