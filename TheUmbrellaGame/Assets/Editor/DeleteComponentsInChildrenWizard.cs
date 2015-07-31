using UnityEngine;
using UnityEditor;
using System.Collections;


public class DeleteComponentsInChildrenWizard : ScriptableWizard
{
	public string typeString;
	
	[MenuItem ( "GameObject/Delete Components in Children Wizard" )]
	static void DoSet()
	{
		ScriptableWizard.DisplayWizard("Set the string of type", typeof(DeleteComponentsInChildrenWizard), "Set");
	}
	
	void OnWizardUpdate()
	{
		helpString = "Set the string of the Component type you want to delete:";
	}
	
	void OnWizardCreate()
	{
		DeleteChildren();
	}
	
	void DeleteChildren()
	{
		Transform[] allTransforms = Selection.GetTransforms( SelectionMode.Deep ); //Get all Children
		
		foreach( Transform eachTransform in allTransforms )
		{
			if ( eachTransform.GetComponent ( typeString ) != null )
			{
				Object.DestroyImmediate ( eachTransform.GetComponent (  typeString ) );
			}
		}
	}
}