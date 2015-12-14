using UnityEngine;
using System.Collections;
using UnityEngine.Audio;
using UnityEngine.UI;

public class HorseRescue : MonoBehaviour
{
	public Transform[] destinations = new Transform[4];
	private GmaeManage gameManager;
	private NavMeshAgent horseNav;
	public float timer;
	private float _timer;
	private Animator horseAnim;
	private Transform umbrellaTr;
	private float speedRun;
	private AudioClip neighSFX;
	private AudioClip gallopSFX;
	private AudioSource gameObjectAudio;
	private Transform horse1Destinations;
	private Transform horse2Destinations;
	private string horseName;
	private Transform nextDestination;//where is the horse going next if moving forward
	private Transform lastDestination;//where was the horse before
	private int i = 0;
	private string destination;
	
	void Start ()
	{
		
		gameManager = GameObject.Find ("Follow Camera").GetComponent<GmaeManage> ();
		_timer = timer;
		horse1Destinations = GameObject.Find ("Horse1Destinations").transform;
		horse2Destinations = GameObject.Find ("Horse2Destinations").transform;
		
		if (destinations.Length != 4) {
			destinations = new Transform[4];
		}
		
		if (this.name == "Horse1") {
			for (int child = 0; child < horse1Destinations.childCount; child++) {
				destinations [child] = horse1Destinations.GetChild (child); 
			}
			
		} else if (this.name == "Horse2") {
			for (int child = 0; child < horse2Destinations.childCount; child++) {
				destinations [child] = horse2Destinations.GetChild (child); 
			}
		}
		
		horseNav = gameObject.GetComponent<NavMeshAgent> ();
		horseNav.SetDestination (destinations [0].position);
		nextDestination = destinations [0];
		gameObjectAudio = GetComponent<AudioSource> ();
		
		horseAnim = gameObject.GetComponent<Animator> ();
		
		umbrellaTr = GameObject.Find ("main_Sphere").transform;
		
		speedRun = 0;
		horseName = this.name;
		
		destination = "nextDestination";
	}
	
	void Update ()
	{
		////////////////Switch statement to move the galloping sound volume up and down////////////////
		switch (horseName) {
			
		case ("Horse1"):
			if (speedRun > 5) {
				gameObjectAudio.volume = 1;
			} else if (speedRun < 5) {
				gameObjectAudio.volume = 0;
			}
			
			break;
			
		case ("Horse2"):
			if (speedRun > 5) {
				gameObjectAudio.volume = 1;
			} else if (speedRun < 5) {
				gameObjectAudio.volume = 0;
				
			}
			break;
		default:
			Debug.LogError ("Couldn't find horse - " + this.gameObject);
			break;
		}

		if (gameManager.missionState == MissionController.HorsesMission) {
			horseNav.tag = "Interaction";
		} else {
			horseNav.tag = "Untagged";
		}

		speedRun = Mathf.Clamp (speedRun, 0, Mathf.Infinity);
		horseAnim.SetFloat ("SpeedRun", speedRun);
		
		//////////////////// The following is what happens when the horse is at each of the 4 destinations ///////////
		if (Vector3.Distance (horseNav.transform.position, destinations [3].position) <= 15f) {
			speedRun = 0;
			timer = _timer;
			horseAnim.enabled = false;
		} else if (Vector3.Distance (horseNav.transform.position, nextDestination.position) <= 5f) {
			if (timer <= 0) {
				timer = _timer;
			}
			timer -= Time.deltaTime;
			speedRun -= Time.deltaTime;
		} 
		if (destination == "nextDestination") {
			speedRun = Vector3.Distance (horseNav.transform.position, nextDestination.position);
		}
		if (destination == "lastDestination") {
			speedRun = Vector3.Distance (horseNav.transform.position, lastDestination.position);
		}

		
		//////////////////////This sends horse back to the previous Dest when the timer reaches 0/////////////////////////////////
		
		if (timer <= 0) {
			if (lastDestination != null) {
				horseNav.SetDestination (lastDestination.position);
				destination = "lastDestination";
				speedRun = Vector3.Distance (horseNav.transform.position, lastDestination.position);
				i = i - 1;
			}
			
		}
	
		if (speedRun >= 30) {
			horseNav.speed = 35;
		} else {
			horseNav.speed = 15;
		}
	}		

	//////////////////Interaction between player and horse///////////////////////////////
	
	
	void OnTriggerStay (Collider col)
	{
		if (col.gameObject.tag == "Player") {
			if (Vector3.Distance (horseNav.transform.position, umbrellaTr.position) <= 30f) {
				if (Input.GetButtonDown (gameManager.controllerInteract)) {
					destination = "nextDestination";
					horseNav.SetDestination (nextDestination.position);
					speedRun = Vector3.Distance (horseNav.transform.position, nextDestination.position);
					if (speedRun <= 5) {
						i = i + 1;
					}
					timer = _timer;
					nextDestination = destinations [i];
					if (i > 0) {
						lastDestination = destinations [i - 1];
					}
				}
			}
		}
	}
}

