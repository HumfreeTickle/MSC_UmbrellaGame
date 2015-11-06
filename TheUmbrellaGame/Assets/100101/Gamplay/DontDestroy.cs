using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DontDestroy : MonoBehaviour
{
	private bool started = true;
	public bool useThis;
	public List<Material> allTheColoursOfTheUmbrella;
	public List<string> taggedNames;
	private Material umbrellaColour;
	public Transform canopyCOlours;
	public int x;
	public int y;

	void Awake ()
	{
		DontDestroyOnLoad (transform.gameObject);
	}

	void Update ()
	{
		if (useThis) {
			if (Application.loadedLevelName == "Start_Screen") {
				if (started) {
					transform.position = new Vector3 (0.42f, 8.46f, 13.864f);
					transform.localScale = new Vector3 (32, 32, 32);		
					started = false;
				}

				//bring up the music/fx
				//move the umbrella up when button is pressed
				ChangeColours (canopyCOlours);
			}
		}

		if (Application.loadedLevelName == "Boucing") {
			Destroy (this.gameObject);
		}
	}

	void ChangeColours (Transform obj)
	{
		for (int child = 0; child< obj.childCount; child++) { //goes through each child object one at a time
			if (obj.GetChild (child).transform.childCount > 0) {
				ChangeColours (obj.GetChild (child));
			} else {
				umbrellaColour = allTheColoursOfTheUmbrella [x];

				if (obj.GetChild (child).GetComponent<MeshRenderer> ()) {
					if (obj.GetChild (child).tag == taggedNames[y]) {// checks to see if there is a mesh renderer attached to child
						MeshRenderer umbrellaChild = obj.GetChild (child).GetComponent<MeshRenderer> ();
						umbrellaChild.material.Lerp (umbrellaChild.material, umbrellaColour, Time.deltaTime * 5);
						if (Vector4.Distance (umbrellaChild.material.color, umbrellaColour.color) <= 0.05f) {
							if (x < allTheColoursOfTheUmbrella.Count - 1) {
								x += 1;
								y += 1;
							}
						}
					}
				}
			}
		}
	}
}
