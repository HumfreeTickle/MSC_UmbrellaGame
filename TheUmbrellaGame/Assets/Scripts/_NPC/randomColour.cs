using UnityEngine;
using System.Collections;

public class randomColour : MonoBehaviour {

	private Material npcMaterial;

	void Awake () 
	{
		npcMaterial = this.GetComponent<MeshRenderer>().material;
		npcMaterial.color = new Color(Random.Range(0.0f,1.0f),Random.Range(0.0f,1.0f),Random.Range(0.0f,1.0f));
		this.GetComponent<MeshRenderer>().material = npcMaterial;
	}

}
