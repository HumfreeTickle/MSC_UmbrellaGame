using UnityEngine;
using System.Collections;

public class Tutuorial : MonoBehaviour {

//------------------------------------------ Needs to be completely overhalled ------------------------------------------
//------------------------------------------- "Don't leave half of the tutorial out" - Owen Harris, 2015 ------------------------------------------

	private Animator animator;
	public int x = 0; 
	public bool upClicked = false;
	public bool backClicked = false;
	public float _time = 0;
	
	
	void Start () {
		animator = GetComponent<Animator>();
	}
	
	void Update () {
		WalkThroughConditions();
		animator.SetInteger("Section",x);
	}
	
	void WalkThroughConditions(){
		if(Input.GetKeyUp(KeyCode.Return)){
			x = 5;
		}
		switch(x){
		case 0:
			if(Input.GetKeyUp(KeyCode.UpArrow)){
				x = 1;
			}
			break;
		case 1:
			if(Input.GetKeyUp(KeyCode.DownArrow)){
				x = 2;
			}
			break;
		case 2:
			if(Input.GetKeyUp(KeyCode.UpArrow)){
				upClicked = true;
			}
			if(Input.GetKeyUp(KeyCode.DownArrow)){
				backClicked = true;
			}
			if(upClicked & backClicked){
				x = 3;
			}
			break;
		case 3:
			if(Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow)) {
				x = 4;
			}
			break;
		case 4:
			
			if(Input.GetKey(KeyCode.Space)){
				_time += Time.deltaTime;
			}
			if(Input.GetKeyUp(KeyCode.Space) & _time >= 1){
				x = 5;
			}
			break;
		case 5:
			this.gameObject.SetActive(false);
			break;
		default:
			break;
		}
	}
}
