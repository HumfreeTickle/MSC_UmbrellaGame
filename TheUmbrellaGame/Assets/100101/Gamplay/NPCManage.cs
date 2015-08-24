using UnityEngine;
using System.Collections;

public class NPCManage: MonoBehaviour
{

	public int missionsComplete;

	public int MissionsComplete {
		get {
			return missionsComplete;
		}
	}

	public bool windmillMisson;

	public bool WindmillMission {
		set {
			windmillMisson = value;
			missionsComplete += 1;
		}
	}
}
