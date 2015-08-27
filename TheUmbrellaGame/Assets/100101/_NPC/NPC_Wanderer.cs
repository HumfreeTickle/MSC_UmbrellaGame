using UnityEngine;
using System.Collections;

/// <summary>
/// Creates wandering behaviour for a CharacterController 
/// Doesn't contain a way to keep the NPC on the terrain
/// Also doesn't pay attention to objects (Figured that out)
/// Still needs work so it's not that jumpy
/// </summary>


namespace NPC
{
	[RequireComponent(typeof(CharacterController))]

	public class NPC_Wanderer : MonoBehaviour
	{
		public float speed = 5;
		public float directionChangeInterval = 1;
		public float maxHeadingChange = 30;
		CharacterController controller;
		float heading;
		Vector3 targetRotation;
		private IEnumerator movementCoroutine;
		private IEnumerator hitCoroutine;

		void Awake ()
		{
			controller = GetComponent<CharacterController> ();
			// Set random initial rotation
			heading = Random.Range (0, 360);
			transform.eulerAngles = new Vector3 (0, heading, 0);
			movementCoroutine = NewHeading();
			hitCoroutine = HitAThing();
			StartCoroutine (movementCoroutine);
		}

		/// <summary>
		/// Might be best to hold eveything in here so I can stop the NPC/coroutine at anytime
		/// </summary>
		void WanderAbout ()
		{
			transform.eulerAngles = Vector3.Slerp (transform.eulerAngles, targetRotation, Time.deltaTime * directionChangeInterval);
			var forward = transform.TransformDirection (Vector3.forward);
			controller.SimpleMove (forward * speed);
		}
		
		/// <summary>
		/// Repeatedly calculates a new direction to move towards.
		/// Use this instead of MonoBehaviour.InvokeRepeating so that the interval can be changed at runtime.
		/// </summary>

		IEnumerator NewHeading ()
		{
			float i = 0;
			while (true) {
				WanderAbout ();
				i += directionChangeInterval / Time.deltaTime;

				if (i >= directionChangeInterval) {
					NewHeadingRoutine ();
					i = 0;
					yield return null;
				}

				yield return null;
			}
		}
		/// <summary>
		/// When the npc hits something.
		/// </summary>
		IEnumerator HitAThing ()
		{
			yield return new WaitForSeconds (directionChangeInterval);
			NewHeading ();
			StartCoroutine(movementCoroutine);
			StopCoroutine(hitCoroutine);
		}
		
		/// <summary>
		/// Calculates a new direction to move towards.
		/// Should use the Navmesh agent
		/// </summary>
		void NewHeadingRoutine ()
		{
			var floor = Mathf.Clamp (heading - maxHeadingChange, 0, 360);
			var ceil = Mathf.Clamp (heading + maxHeadingChange, 0, 360);
			heading = Random.Range (floor, ceil);
			targetRotation = new Vector3 (0, heading, 0);
		}

		void OnControllerColliderHit (ControllerColliderHit hit)
		{
			if (hit.gameObject.name != "River") {
				StopCoroutine (movementCoroutine);
				StartCoroutine (HitAThing ());
			}
		}
	}
}