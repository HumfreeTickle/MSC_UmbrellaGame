using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Audio;

public class Achievements : MonoBehaviour
{

	//-------------------------------//

	private Image achievementNotification;
	private Color startColourText;
	private Color fullColourText;
	private Color startColourBG;
	private Color fullColourBG;
	private Text achievementText;
	private AudioSource jingle;
	
	//-------------------------------//

	public List<string> achievements;// = new List<string> ();

	private bool coroutineInMotion;

	public bool CoroutineInMotion {
		get {
			return coroutineInMotion;
		}

		set {
			coroutineInMotion = value;
		}
	}

	//-------------------------------//

//
	void OnValidate ()
	{
		if (achievements.Count < 9) {
			achievements.Add ("Splish. Splash.");
			achievements.Add ("Assassin's Jump");
			achievements.Add ("Relaxing Bench");
			achievements.Add ("Bach to basics");
			achievements.Add ("Oh so corny");
			achievements.Add ("Somewhere over the river");
			achievements.Add ("Let there be light");
			achievements.Add ("Like a prayer");
			achievements.Add ("Ring the bell");
		}

	}

	void Start ()
	{	
		achievementNotification = GameObject.Find ("Achievements_Box").GetComponent<Image> ();
		achievementText = GameObject.Find ("Achievemts_text").GetComponent<Text> ();
		startColourBG = achievementNotification.color;
		fullColourBG = new Vector4 (startColourBG.r, startColourBG.g, startColourBG.b, 1);

		startColourText = achievementText.color;
		fullColourText = new Vector4 (startColourText.r, startColourText.g, startColourText.b, 1);
	}

	public IEnumerator Notification (string notificationText)
	{
		coroutineInMotion = true;

		while (Vector4.Distance (achievementNotification.color, fullColourBG) > 0.01f) {
			achievementNotification.color = Color.Lerp (achievementNotification.color, fullColourBG, Time.deltaTime * 5); 
			achievementText.text = notificationText;
			achievementText.color = Color.Lerp (achievementText.color, fullColourText, Time.deltaTime * 5);
			yield return null;
		} 

		yield return new WaitForSeconds (1);

		while (Vector4.Distance (achievementNotification.color, startColourBG) > 0.01f) {
			achievementNotification.color = Color.Lerp (achievementNotification.color, startColourBG, Time.deltaTime * 5); 
			achievementText.color = Color.Lerp (achievementText.color, startColourText, Time.deltaTime * 5);
			yield return null;
		} 
		achievementNotification.color = startColourBG;
		achievementText.color = startColourText;
		achievementText.text = "";

		achievements.Remove (notificationText);
		coroutineInMotion = false;
		yield break;
	}
}
