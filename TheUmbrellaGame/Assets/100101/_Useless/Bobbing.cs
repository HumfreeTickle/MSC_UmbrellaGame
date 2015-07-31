//using UnityEngine;
//using System.Collections;
//
//public class Bobbing : MonoBehaviour {
//
//	public float speed = 1; //The higher the increase the smaller the sine wave curve
//	public float sine;
//	private bool gravity;
//
//	void Start(){
//		gravity = GetComponent<Rigidbody> ().useGravity;
//	}
//
//	// Update is called once per frame
//	void Update () {
//		if (gravity) { //change back to not gravity
//			//if(Mathf.Abs(GetComponent<Rigidbody>().velocity.x) < 0.1f || Mathf.Abs(GetComponent<Rigidbody>().velocity.z) < 0.1f ){
//			SineWave ();
//		}
//	}
//	
//
//	void SineWave(){
//		sine = 0;
//		float sw;
//	
//		//if (sine < 2*(Mathf.PI)) {
//			sine += Time.time;
//		//}
//
//		//transform.position -= Vector3.up*Time.deltaTime;
//		sw = Mathf.Sin(sine);
////		if (sw <= 0 & (Mathf.Abs(GetComponent<Rigidbody>().velocity.x) < 0.1f || Mathf.Abs(GetComponent<Rigidbody>().velocity.z) < 0.1f )) {
////			transform.position -= (Vector3.up * Time.deltaTime)/2;
////		}
//		transform.position += new Vector3 (0, (sw/speed), 0);
//	}
//		                                
//}
