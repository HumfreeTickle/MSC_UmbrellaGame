using UnityEngine;
using System.Collections;

public class DropTheBridge : MonoBehaviour
{
	public bool drop{private get;set;}
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