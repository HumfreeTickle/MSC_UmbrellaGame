using UnityEngine;
using System.Collections;

public class NPCManage: MonoBehaviour
{
	// Holds whether the player has completed the windmil mission
	private bool windmillMission;

	public bool WindmillMission {
		get{
			return windmillMission;
		}

		set {
			windmillMission = value;
		}
	}

	//Holds whether the player has finished the tutorial
	//If false the player can't talk to anyone else or leave towards the HUB
	private bool tutorialMission;

	public bool TutorialMission {
		get{
			return tutorialMission;
		}

		set {
			tutorialMission = value;
		}
	}

	//Holds whether the player has finished the horse gathering mission
	private bool horseMission;
	
	public bool HorseMission {
		get{
			return horseMission;
		}

		set {
			horseMission = value;
		}
	}

	//Holds whether the player has rescued and delivered the cat
	private bool catRescueMission;
	
	public bool CatRescueMission {
		get{
			return catRescueMission;
		}

		set {
			catRescueMission = value;
		}
	}

	//Holds whether the player has collected each box
	private bool boxPickupMission;
	
	public bool BoxPickupMission {
		get{
			return boxPickupMission;
		}

		set {
			boxPickupMission = value;
		}
	}

	//how many lights have been turned on
	// will need to be clamped
	private int lightActivation;
	
	public int LightActivation {
		get{
			return lightActivation;
		}

		set {
			lightActivation += value;
		}
	}
}
