using UnityEngine;
using System.Collections;

namespace Inheritence
{
	public class DestroyObject
	{
		private float _timer;
//-------------------------------------- Destroys GameObject On Collision -------------------------------------------------------
		public void DestroyOnCollsion (Collision Col)
		{
			if (Col.gameObject.tag == "Collectable") {
				Col.gameObject.SetActive (false);
			}
		}//end

//-------------------------------------- Destroys GameObject On Timer Countdown -------------------------------------------------------

		public void DestroyOnTimer (GameObject objectToDestroy, float DestroyDelay)
		{
			MonoBehaviour.Destroy (objectToDestroy, DestroyDelay);
		}//end

//-------------------------------------- Destroys GameObject On Button Press -------------------------------------------------------

		public void DestroyOnButton (GameObject objectToDestroy)
		{

		}//end

//-------------------------------------- Destroys GameObject On Trigger -------------------------------------------------------


		public void DestroyOnTrigger (Collider col, GameObject objectToDestroy)
		{

		}//end
	}//end
}//end
