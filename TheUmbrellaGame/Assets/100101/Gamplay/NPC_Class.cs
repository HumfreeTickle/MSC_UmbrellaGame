using UnityEngine;
using System.Collections;

public class NPC_Class
{
	//What would be kept between each NPC
	//-- Well they move about the areas
	//-- Stop for chats with one another
	//-- Gives out tasks
	//-- Checks to see if it is raining and moves home if it is

//	//	public LayerMask rainSystemLayer;
//	public Vector3 up;
//	public Transform waypoint1;
//	public bool wet;
//	public RaycastHit rainCheck;
//	
//	void Start ()
//	{
//		up = transform.TransformDirection (Vector3.up);
//	}
//	
//	void Update ()
//	{
//		isItRaining ();
//		getOutofTheRain ();
//		
//	}
//	
//	void isItRaining ()
//	{
//		Debug.DrawRay (transform.position, up);
//		if (Physics.Raycast (transform.position, up, out rainCheck, 200)) {
//			//			print (rainCheck.collider);
//			
//			if (rainCheck.collider.tag == "Rain") {
//				wet = true;
//			}
//		}
//	}
//	
//	void getOutofTheRain ()
//	{
//		if (wet) {
//			GetComponent<Animation> ().Play ("Run");
//			transform.position = Vector3.Lerp (transform.position, waypoint1.position, Time.deltaTime / 2);
//		}
//		
//		if (Physics.Raycast (transform.position, up, out rainCheck, Mathf.Infinity)) {
//			if (rainCheck.collider.tag == "Player") {
//				//				print (rainCheck.collider.tag);
//				GetComponent<Animation> ().Play ("Ithcing");
//				
//			}
//			if (rainCheck.collider.tag == "Shelter") {
//				wet = false;
//				GetComponent<Animation> ().Play ("IdleSit");
//			}
//		}
//	}

}
