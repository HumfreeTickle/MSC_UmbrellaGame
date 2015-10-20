using UnityEngine;
using System.Collections;

public class DropTheBridge : MonoBehaviour
{
	private bool drop;
	public bool Drop{
		set{
			drop = value;
		}
	}

	private Animator animator;

	void Awake ()
	{
		animator = GetComponent<Animator> ();
	}
	
	void Update ()
	{
		if (drop)
		animator.SetBool ("Fixed", false);
	}
}