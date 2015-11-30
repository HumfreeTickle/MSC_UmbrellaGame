using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityStandardAssets.ImageEffects;

public class Presentation : MonoBehaviour
{

	public List<Material> allTheColoursOfTheUmbrella;
	private Material umbrellaColour;
	private Transform canopyCOlours;
	public enum slide
	{
		nullstate,
		firstSlide,
		secondSlide,
		thirdSlide,
		endPresentation
	}
	private bool progress = true;
	public slide currentSlide;
	private GameObject theCanvas;

	//------- Umbrella ----------//
	private GameObject umbrella;
	private Animator umbrellaAnim;
	private Transform umbrellaTr;

	//------- Camera ----------//
	private GameObject cmaera;
	private Transform cmaeraTr;
	public List<Transform> scenePlacements;
	public float speed;
	public float offset;
	private VignetteAndChromaticAberration blurring;

	//--------NPC-------------//
	private Transform npcTR;
	private Animator npcAnim;
	private Animator npcHands;
	private bool Scene3;
	private bool Scene2;
	private bool Scene1;
	private bool endOfScene = true;

	//------Kitten-----------//
	private Transform kittentr;
	private Animator kittenAnim;
	private Color fullcolour = new Color (0.5f, 0.5f, 0.5f, 0);
	private bool final;

	void Start ()
	{
		currentSlide = slide.nullstate;
		theCanvas = GameObject.Find ("Canvas");
		canopyCOlours = GameObject.Find ("Canopy_Colours").transform;
		cmaera = GameObject.Find ("Main Camera");
		cmaeraTr = cmaera.transform;

		umbrella = GameObject.Find ("Umbrella_Presentation");
		umbrellaTr = umbrella.transform;
		umbrellaAnim = umbrella.GetComponent<Animator> ();
		npcTR = GameObject.Find ("NPC").transform;
		npcAnim = GameObject.Find ("NPC").GetComponent<Animator> ();
		npcHands = GameObject.Find ("Hands").GetComponent<Animator> ();
		kittenAnim = GameObject.Find ("kitten 1").GetComponent<Animator> ();
		kittentr = GameObject.Find ("kitten 1").transform;
		blurring = cmaera.GetComponent<VignetteAndChromaticAberration> ();
	}
	
	void Update ()
	{
		if (progress) {
			if (endOfScene) {
				switch (currentSlide) {

				case slide.nullstate:
					progress = false;
					theCanvas.transform.GetChild (0).GetComponent<Image> ().CrossFadeAlpha (0f, 1f, false);
					if (theCanvas.transform.GetChild (0).GetComponent<Image> ().color.a > 0.9f) {
						currentSlide = slide.firstSlide;
					}
					break;

				case slide.firstSlide:
					progress = false;

					currentSlide = slide.secondSlide;
					break;

				case slide.secondSlide:
					progress = false;

					currentSlide = slide.thirdSlide;
					break;
				
				case slide.thirdSlide:
					progress = false;
						
					currentSlide = slide.endPresentation;
					break;

				default:
				
					break;
				}
			}

		}
		Transition ();
	}

	void Transition ()
	{
		switch (currentSlide) {
		
		case slide.firstSlide:
			StartCoroutine (firstScene ());
			
			umbrellaAnim.SetBool ("Scene 1", Scene1);

//			umbrellaColour = allTheColoursOfTheUmbrella [0];
//			ChangeColours (canopyCOlours);

			break;
		case slide.secondSlide:
			StartCoroutine (secondScene ());
		
			umbrellaAnim.SetBool ("Scene 2", Scene2);

			npcHands.SetBool ("Scene2", Scene2);
			npcAnim.SetBool ("Scene2", Scene2);
//			umbrellaColour = allTheColoursOfTheUmbrella [1];
//			ChangeColours (canopyCOlours);

			
			break;

		case slide.thirdSlide:
			StartCoroutine (thirdScene ());

			umbrellaAnim.SetBool ("Scene 3", Scene3);


//			umbrellaColour = allTheColoursOfTheUmbrella [2];
//			ChangeColours (canopyCOlours);

			
			break;

		case slide.endPresentation:
			StartCoroutine (transitionToNextScene ());
//			umbrellaColour = allTheColoursOfTheUmbrella [3];

//			ChangeColours (canopyCOlours);
			progress = true;

			break;
		default:

			break;

		}
	}

//	void ChangeColours (Transform obj)
//	{
//		for (int child = 0; child< obj.childCount; child++) { //goes through each child object one at a time
//			if (obj.GetChild (child).transform.childCount > 0) {
//				ChangeColours (obj.GetChild (child));
//			} else {
//				if (obj.GetChild (child).GetComponent<MeshRenderer> ()) {
//					if (obj.GetChild (child).tag == umbrellaColour.name) {// checks to see if there is a mesh renderer attached to child
//						MeshRenderer umbrellaChild = obj.GetChild (child).GetComponent<MeshRenderer> ();
//						umbrellaChild.material.Lerp (umbrellaChild.material, umbrellaColour, Time.deltaTime / 2);
//
//					}
//				}
//			}
//		}
//	}
	
	IEnumerator firstScene ()
	{
		while (currentSlide == slide.firstSlide) {
			while (Vector3.Distance (umbrellaTr.position, scenePlacements [0].position) <= 10) {
				Scene1 = false;
				fullcolour = Color.Lerp (fullcolour, Color.black, Time.deltaTime);
				theCanvas.transform.GetChild (1).GetComponent<Text> ().color = fullcolour;
				endOfScene = true;
				yield return null;

			} 
			yield return new WaitForSeconds (speed);

			endOfScene = false;
			Scene1 = true;
			blurring.blur = Mathf.Lerp (blurring.blur, 0.7f, Time.deltaTime);
			umbrellaTr.parent.position = Vector3.Lerp (umbrellaTr.parent.position, scenePlacements [0].position, Time.deltaTime / speed);
			float x = Mathf.Lerp (cmaeraTr.position.x, scenePlacements [0].position.x + offset, Time.deltaTime / speed);
			cmaeraTr.position = new Vector3 (x, cmaeraTr.position.y, cmaeraTr.position.z);


			yield return new WaitForSeconds (4);
			
			currentSlide = slide.secondSlide;
			progress = true;
		}

		Scene2 = true;
		theCanvas.transform.GetChild (1).GetComponent<Text> ().enabled = false;



		yield break;

	}
	
	IEnumerator secondScene ()
	{
		while (currentSlide == slide.secondSlide) {
			if (Vector3.Distance (cmaeraTr.position, scenePlacements [1].position) < 37.126f) {
				Scene2 = false;

				fullcolour = Color.Lerp (fullcolour, Color.black, Time.deltaTime);
				theCanvas.transform.GetChild (2).GetComponent<Text> ().color = fullcolour;
				yield return new WaitForSeconds (speed);

				endOfScene = true;
				yield return new WaitForSeconds (speed);

			} else {
				endOfScene = false;
				fullcolour = new Color (0.5f, 0.5f, 0.5f, 0); 
				float x = Mathf.Lerp (cmaeraTr.position.x, scenePlacements [1].position.x, Time.deltaTime / speed);
				float y = Mathf.Lerp (cmaeraTr.position.y, scenePlacements [1].position.y, Time.deltaTime / speed);
				cmaeraTr.position = new Vector3 (x, y, cmaeraTr.position.z);
				yield return new WaitForSeconds (speed * 2);
			}
			
		}
		theCanvas.transform.GetChild (2).GetComponent<Text> ().enabled = false;

		yield return new WaitForSeconds (4);

		currentSlide = slide.thirdSlide;
		progress = true;

		yield break;

	}

	IEnumerator thirdScene ()
	{
		while (currentSlide == slide.thirdSlide) {
			if (Vector3.Distance (cmaeraTr.position, scenePlacements [2].position) < 38.2f) {
				Scene3 = true;
				kittenAnim.SetBool ("Scene 3", Scene3);

				fullcolour = Color.Lerp (fullcolour, Color.black, Time.deltaTime);
				theCanvas.transform.GetChild (3).GetComponent<Text> ().color = fullcolour;
				yield return new WaitForSeconds (speed);
				endOfScene = true;

			} else {

				endOfScene = false;
				fullcolour = new Color (0.5f, 0.5f, 0.5f, 0);
				npcHands.SetBool ("Scene2", true);
				float x = Mathf.Lerp (cmaeraTr.position.x, scenePlacements [2].position.x, Time.deltaTime / (speed * 2));
				
				cmaeraTr.position = new Vector3 (x, cmaeraTr.position.y, cmaeraTr.position.z);
				yield return new WaitForSeconds (speed);

			}
			
		}
		theCanvas.transform.GetChild (3).GetComponent<Text> ().enabled = false;

		yield return new WaitForSeconds (4);

		currentSlide = slide.endPresentation;
		progress = true;

		yield break;
		
	}

	IEnumerator transitionToNextScene ()
	{
		while (currentSlide == slide.endPresentation) {

			if (!final) {
				kittenAnim.SetBool ("End", true);


				if (npcAnim.isActiveAndEnabled) {
					npcAnim.SetBool ("End", true);
					npcAnim.enabled = false;
				}

				if (umbrellaAnim.isActiveAndEnabled) {
					umbrellaAnim.SetBool ("End", true);
					umbrellaAnim.enabled = false;
				}

				float umbrellaX = Mathf.Lerp (umbrellaTr.position.x, scenePlacements [4].position.x, Time.deltaTime / (speed));
				float umbrellaY = Mathf.Lerp (umbrellaTr.position.y, scenePlacements [4].position.y, Time.deltaTime / (speed));
				umbrellaTr.position = new Vector3 (umbrellaX, umbrellaY, umbrellaTr.position.z);
				umbrellaTr.rotation = Quaternion.Euler (new Vector3 (0, 90, 0));

				
				float kittenX = Mathf.Lerp (kittentr.position.x, scenePlacements [3].position.x, Time.deltaTime / (speed * 2));
				kittentr.position = new Vector3 (kittenX, kittentr.position.y, kittentr.position.z);
				if (Vector3.Distance (umbrellaTr.position, scenePlacements [4].position) <= 3) {
					npcTR.rotation = Quaternion.Euler (new Vector3 (0, -90, 0));
					float npcX = Mathf.Lerp (npcTR.position.x, scenePlacements [5].position.x, Time.deltaTime / (speed * 3));
					float npcY = Mathf.Lerp (npcTR.position.y, scenePlacements [5].position.y, Time.deltaTime / (speed * 2));
					npcTR.position = new Vector3 (npcX, npcY, npcTR.position.z);
				}
				if (Vector3.Distance (npcTR.position, scenePlacements [5].position) <= 3) {
					npcHands.SetBool ("Scene2", false);
					final = true;

				}

				yield return new WaitForSeconds (speed);


			} else {
				npcHands.SetBool ("Final", final);

				
				if (npcTR.localScale.x <= 8) {
					Application.LoadLevel ("Start_Screen");

				}
				yield return new WaitForSeconds (speed);
				umbrellaTr.parent = GameObject.Find ("Hand_L").transform;
				umbrellaTr.localPosition = new Vector3 (0, -3.8f, 0);

				yield return new WaitForSeconds (speed / 2);
				npcHands.SetBool ("EndWalk", true);
				npcTR.rotation = Quaternion.Slerp (npcTR.rotation, Quaternion.Euler (new Vector3 (0, 180, 0)), Time.deltaTime);


				yield return new WaitForSeconds (speed);
				npcTR.localScale = Vector3.Lerp (npcTR.lossyScale, new Vector3 (7, 7, 7), Time.deltaTime / (speed * 2));
				
			}
			yield return null;

		}

		yield break;

	}
}

