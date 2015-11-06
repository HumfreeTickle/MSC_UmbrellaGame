using UnityEngine;
using System.Collections;

namespace Debuggin
{
	public class CollisionDebug : MonoBehaviour
	{
		private Tutuorial tutorialCanvas;
		public LayerMask playerLayer;

		void OnCollisionStay (Collision col)
		{
			if (col.gameObject.layer != playerLayer) {
				print (col.gameObject.name + " : " + col.gameObject.layer);
			}
		}
	}
}
