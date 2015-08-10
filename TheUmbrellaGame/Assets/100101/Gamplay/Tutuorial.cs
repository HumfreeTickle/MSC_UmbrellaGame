using UnityEngine;
using System.Collections;

public class Tutuorial : MonoBehaviour
{

//------------------------------------------ Needs to be completely overhalled ------------------------------------------
//------------------------------------------- "Don't leave half of the tutorial out" - Owen Harris, 2015 ------------------------------------------

	public GmaeManage GameManager;
	private Animator animator;
	public int x;

	public int X{
		get{
			return x;
		}
		set{
			x = value;
		}
	}

	public float _time = 0;
	
	void Start ()
	{
		animator = GetComponent<Animator> ();
	}
	
	void Update ()
	{
		if(GameManager == null){
			return;
		}

		if(GameManager.gameState == GameState.Idle){
			X = 5;
		}else if (GameManager.gameState == GameState.Game) {
			WalkThroughConditions ();
			animator.SetInteger ("State", X);
		}
	}
	
	void WalkThroughConditions ()
	{

		switch (x) {

		case 0:
			if (Mathf.Abs (Input.GetAxisRaw ("Vertical_L")) > 0 || Mathf.Abs (Input.GetAxisRaw ("Horizontal_L")) > 0) {
				_time += Time.deltaTime;
				if (_time > 5) {
					x = 5;
					_time = 0;
				}
			}
			break;

		case 1:
			if (Input.GetButtonDown ("Talk")) {
				x = 5;
			}
			break;

		case 2:
			if (Input.GetButtonDown ("Interact")) {
				x = 5;
			}
			break;
//		case 3:
//			if (Input.GetKeyUp (KeyCode.LeftArrow) || Input.GetKeyUp (KeyCode.RightArrow)) {
//				x = 4;
//			}
//			break;
//		case 4:
//			
//			if (Input.GetKey (KeyCode.Space)) {
//				_time += Time.deltaTime;
//			}
//			if (Input.GetKeyUp (KeyCode.Space) & _time >= 1) {
//				x = 5;
//			}
//			break;
		case 5:
			break;
		default:
			break;
		}
	}
}
