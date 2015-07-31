using UnityEngine;
using System.Collections;
using Inheritence;

namespace NPC
{
// this can probably be made into an inheritence class called DestroyObject

	public class DestroyTheCat : MonoBehaviour
	{
		private DestroyObject destroy = new DestroyObject();
//----------------------------------------- Achievement Unlocked -------------------------------------------------------

		void OnCollisionEnter (Collision Col)
		{
			destroy.DestroyOnCollsion(Col);
		}
	}
}
