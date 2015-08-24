using UnityEngine;
using System.Collections;

namespace Environment
{
	public class TurnOnLight : MonoBehaviour
	{
		public bool LitUp = false;
		private DayPhase day_Night;
		private _CycleDayNight sun;

		void Awake ()
		{
			sun = GameObject.Find ("Sun").GetComponent<_CycleDayNight> ();
		}

		void Update ()
		{
			day_Night = sun.CurrentPhase;

			if (day_Night == DayPhase.Night || day_Night == DayPhase.Dusk) {
				this.tag = "Interaction";

				if (LitUp) {
					GetComponent<Light> ().enabled = true;
				}

			} else {
				this.tag = "Untagged";
				GetComponent<Light> ().enabled = false;
			}
		}

		void OnTriggerEnter (Collider col)
		{
			if (col.gameObject.tag == "Player") {
				if (day_Night == DayPhase.Night || day_Night == DayPhase.Dusk) {
					LitUp = true;
				}
			}
		}
	}
}