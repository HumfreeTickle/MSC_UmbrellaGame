﻿using UnityEngine;
using System.Collections;

public class Light_Distance : MonoBehaviour
{

	private Vector3 baseSize = new Vector3 (3, 3, 3);
	private Vector3 maxSize;
	private Transform umbrella;
	private Vector3 sizeChagne;
	private Color mainColour;
	public float intensity = 0.8f;
	public float threshold = 0;

	void Start ()
	{
		umbrella = GameObject.Find ("main_Sphere").transform;
		mainColour = GetComponent<MeshRenderer> ().material.color;
		baseSize = new Vector3(transform.localScale.x, transform.localScale.x, transform.localScale.z);
		maxSize = new Vector3(baseSize.x, baseSize.y * 100, baseSize.z);
	}
	
	void Update ()
	{

		Renderer renderer = GetComponent<MeshRenderer> ();
		Material mat = renderer.material;

		float distanceFrom = Mathf.Round ((Vector3.Distance (transform.position, umbrella.position) / 100) * 100) / 100 - threshold;

		
		float emission = Mathf.Lerp (0, intensity, distanceFrom);

		Color finalColor = mainColour * Mathf.LinearToGammaSpace (emission);
		
		mat.SetColor ("_EmissionColor", finalColor);



		GetComponent<MeshRenderer> ().enabled = true;
		GetComponent<Light> ().enabled = false;

		sizeChagne = Vector3.Lerp (baseSize, maxSize, distanceFrom);

		if (Vector3.Distance (transform.parent.position, umbrella.position) / 1000 > 0.02f) {
			GetComponent<Light> ().enabled = false;

		} else {
			GetComponent<MeshRenderer>().enabled = false;
			GetComponent<Light> ().enabled = true;
		}

		transform.localScale = sizeChagne;
	}
}
