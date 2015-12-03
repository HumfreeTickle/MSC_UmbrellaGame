using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Audio;

public class Credits : MonoBehaviour
{
	private Transform cameraMarkers;
	private Transform[] cameraPoints;
	private int i = 0;
	private int j = 0;
	private bool startCoroutine;
	private GameObject cmaera;
	public float scrollSpeed;
	private GameObject creditScroll;
	private Text creditsText;
	private Vector3 startingPos;
	private List<string> creditList = new List<string> ();
	private AudioSource rain;
	private string creditsList1 = 
		"Animator \t Stephen Larkin  \n" +
		"Animator \t Peter Cantwell  \n" +
		"Animator \t Peter Cantwell \n" +
		"Animator \t Stephen Larkin \n" +
		"Artwork Director \t Peter Cantwell \n" +
		"Assistant Director \t Stephen Larkin \n" +
		"CG Artist \t Peter Cantwell \n" +
		"CG Artist \t Stephen Larkin \n" +
		"CG Artist \t Peter Cantwell \n" +
		"CG Artist \t Stephen Larkin \n" +
		"CG Artist \t Peter Cantwell \n" +
		"CG Artist \t Peter Cantwell \n" +
		"CG Artist \t Stephen Larkin \n";
	private string creditsList2 = 
		"Character/Mechanical Design \t Stephen Larkin \n" +
		"Development Tool Programmer \t Peter Cantwell \n" +
		"Director \t Hideo Kojima \n" +
		"Executive Producer \t Peter Cantwell \n" +
		"Executive Producer \t Stephen Larkin \n" +
		"Main Programmer \t Stephen Larkin \n" +
		"Motion Director \t Peter Cantwell \n" +
		"Music \t Peter Cantwell \n" +
		"Music \t Stephen Larkin \n";
	private string creditsList3 = 
		"Producer \t Peter Cantwell \n" +
		"Producer \t Stephen Larkin \n" +
		"Programmer \t Peter Cantwell \n" +
		"Programmer \t Stephen Larkin \n" +
		"Programmer \t Peter Cantwell \n" +
		"Programmer \t Peter Cantwell \n" +
		"Programmer \t Stephen Larkin \n" +
		"Script \t Peter Cantwell \n" +
		"Script \t Stephen Larkin \n" +
		"Script \t Peter Cantwell \n";
	private string creditsList4 = 
		"Sound Design \t Peter Cantwell \n" +
		"Sound Design \t Stephen Larkin \n" +
		"Sound Design \t Stephen Larkin \n" +
		"Sound Effects Director \t Stephen Larkin \n" +
		"Texture/Pixel Artist \t Peter Cantwell \n";
	private string creditsList5 = 
		"Voice Actor - NPC_Tutorial \t  Morgan Freeman \n" +
		"Voice Actor - NPC_Farmer \t Kiefer Sutherland \n" +
		"Voice Actor - NPC_Water \t Emma Stone \n" +
		"Voice Actor - NPC_House \t Charlize Theron \n" +
		"Voice Actor - NPC_Worker \t George Clooney \n" +
		"Voice Actor - NPC_Cliff \t Anthony Hopkins \n" +
		"Voice Actor - NPC_Bridge \t Brad Pitt \n" +
		"Voice Actor - NPC_Cat \t Jennifer Lawrence \n" +
		"Voice Actor - NPC_Boxes \t Cate Blanchett \n" +
		"Voice Actor - NPC_LightHouseKeeper \t Michael Caine \n" +
		"Voice Actor - Priest \t Tom Hanks \n" +
		"Voice Actor - Npc_HorseGuy \t Clint Eastwood \n" +
		"Voice Actor - Kitten \t Ryan Gosling \n" +
		"Voice Actor - Molly the Brolly \t Dame Judy Dench \n";
	private string creditsList6 = 
		"Created by \t Stephen Larkin & Peter Cantwell \n";
	private Image TitleCard;

	private string StartButton;
	private GmaeManage gameManager;
	void Start ()
	{
		cmaera = GameObject.Find ("Follow Camera");
		cameraMarkers = GameObject.Find ("CameraPoints").transform;
		cameraPoints = new Transform[cameraMarkers.childCount];
		gameManager = cmaera.GetComponent<GmaeManage>();

		for (int child = 0; child < cameraMarkers.childCount; child++) {
			cameraPoints [child] = cameraMarkers.GetChild (child).transform; 
		}

		rain = GameObject.Find ("Main_Music").GetComponent<AudioSource> ();

		creditScroll = GameObject.Find ("Credits");
		creditsText = creditScroll.GetComponent<Text> ();
		startingPos = creditScroll.transform.position;

		creditList.Add (creditsList1);
		creditList.Add (creditsList2);
		creditList.Add (creditsList3);
		creditList.Add (creditsList4);
		creditList.Add (creditsList5);
		creditList.Add (creditsList6);

		TitleCard = GameObject.Find ("Title Card").GetComponent<Image> ();

		if(gameManager.ControllerType == ControllerType.ConsoleContoller){
			if(gameManager.consoleControllerType == ConsoleControllerType.PS3){
				StartButton = "Submit_1";
			}else if(gameManager.consoleControllerType == ConsoleControllerType.XBox){
				StartButton = "Submit_2";
			}
		}else{
			StartButton = "Submit_1";
		}
	}
	
	void Update ()
	{	
		StartCoroutine (creditsScroll ());

		if(Input.GetButtonDown(StartButton)){
			Application.LoadLevel ("Start_Screen");
		}
		

		if (i < cameraMarkers.childCount) {
			cmaera.transform.position = cameraPoints [i].position;
			cmaera.transform.rotation = cameraPoints [i].rotation;
		} else {
			if (Vector4.Distance (TitleCard.color, new Vector4 (1, 1, 1, 1)) <= 0.1f) {
				if (rain.clip.length - rain.time < 5f) {
					rain.volume = Mathf.Lerp (rain.volume, 0, Time.deltaTime);
				}

				if (rain.volume < 0.1f) {
					Application.LoadLevel ("Start_Screen");
				}
			}
		}
	}

	IEnumerator creditsScroll ()
	{
		if (startCoroutine) {
			yield break;
		}


		startCoroutine = true;

		yield return null;

		while (i < cameraMarkers.childCount) {
			creditsText.transform.position = startingPos;

			if (j < creditList.Count) {
				creditsText.text = creditList [j];
			}

			while (creditsText.transform.position.y < -1.5f*startingPos.y) {
				creditsText.transform.position += Vector3.up * scrollSpeed;
				yield return new WaitForEndOfFrame ();

			}
				
			i += 1;
			j += 1;

			yield return null;
		}

		while (Vector4.Distance(TitleCard.color, new Vector4(1,1,1,1)) > 0.1f) {
			TitleCard.color = Color.Lerp (TitleCard.color, Color.white, Time.deltaTime / 5);
			yield return new WaitForEndOfFrame ();
		}

		yield break;

	}

}
