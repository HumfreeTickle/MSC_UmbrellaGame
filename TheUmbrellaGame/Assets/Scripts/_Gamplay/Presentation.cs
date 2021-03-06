﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityStandardAssets.ImageEffects;

// Not used in final game
// Used during the presentations throughout the year.

public class Presentation : MonoBehaviour
{
//	public List<Material> allTheColoursOfTheUmbrella;
//	private Material umbrellaColour;
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

	//----- Coroutines -----//
	private bool startCoroutineOne;
	private bool startCoroutineTwo;
	private bool startCoroutineThree;
	private bool startCoroutineEnd;

	void Start ()
	{
		currentSlide = slide.nullstate;
		theCanvas = GameObject.Find ("Canvas");
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
			if (!startCoroutineOne) {
				StartCoroutine (firstScene ());
			}
			umbrellaAnim.SetBool ("Scene 1", Scene1);
			break;

		case slide.secondSlide:
			if (!startCoroutineTwo) {
				StartCoroutine (secondScene ());
			}
			umbrellaAnim.SetBool ("Scene 2", Scene2);
			npcHands.SetBool ("Scene2", Scene2);
			npcAnim.SetBool ("Scene2", Scene2);

			break;

		case slide.thirdSlide:
			if (!startCoroutineThree) {
				StartCoroutine (thirdScene ());
			}
			umbrellaAnim.SetBool ("Scene 3", Scene3);

			break;

		case slide.endPresentation:
			if (!startCoroutineEnd) {
				StartCoroutine (transitionToNextScene ());
				progress = true;
			}
			break;
		default:

			break;

		}
	}

	IEnumerator firstScene ()
	{
		if (startCoroutineOne) {
			Debug.LogError ("Already Running - 1");
			yield break;
		}

		startCoroutineOne = true;

		while (currentSlide == slide.firstSlide) {




			while ((scenePlacements [0].position.x + offset - cmaeraTr.position.x) > 5f) {
				endOfScene = false;
				Scene1 = true;
				blurring.blur = Mathf.Lerp (blurring.blur, 0.7f, Time.deltaTime);
				umbrellaTr.parent.position = Vector3.Lerp (umbrellaTr.parent.position, scenePlacements [0].position, Time.deltaTime / speed);

				float x = Mathf.Lerp (cmaeraTr.position.x, scenePlacements [0].position.x + offset, Time.deltaTime / speed);
				cmaeraTr.position = new Vector3 (x, cmaeraTr.position.y, cmaeraTr.position.z);
				yield return null;
			}

			yield return null;

			while (Vector3.Distance (umbrellaTr.position, scenePlacements [0].position) <= 10) {
				Scene1 = false;
				fullcolour = Color.Lerp (fullcolour, Color.black, Time.deltaTime);
				theCanvas.transform.GetChild (1).GetComponent<Text> ().color = fullcolour;
				endOfScene = true;
				yield return null;
			} 

			progress = true;

			yield return new WaitForSeconds (1);

		}

		Scene2 = true;
		theCanvas.transform.GetChild (1).GetComponent<Text> ().enabled = false;

		yield break;

	}
	
	IEnumerator secondScene ()
	{
		if (startCoroutineTwo) {
			Debug.LogError ("Already Running - 2");
			yield break;
		}
		
		startCoroutineTwo = true;

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

			progress = true;
			Debug.Log (2);

			yield return new WaitForSeconds (1);
		}

		theCanvas.transform.GetChild (2).GetComponent<Text> ().enabled = false;

		yield break;

	}

	IEnumerator thirdScene ()
	{
		if (startCoroutineThree) {
			Debug.LogError ("Already Running - 3");
			yield break;
		}
		
		startCoroutineThree = true;

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
			yield return new WaitForSeconds (3);
			progress = true;
			Debug.Log (3);

			yield return new WaitForSeconds (1);

		}

		theCanvas.transform.GetChild (3).GetComponent<Text> ().enabled = false;

		yield break;
		
	}

	IEnumerator transitionToNextScene ()
	{
		if (startCoroutineEnd) {
			Debug.LogError ("Already Running - End");
			yield break;
		}
		
		startCoroutineEnd = true;

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

