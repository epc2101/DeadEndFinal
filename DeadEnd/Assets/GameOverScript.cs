using UnityEngine;
using System.Collections;

public class GameOverScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		AudioController.Play("GameOver");
	}
	
	void OnGUI()
	{
		float leftpadding=Screen.width/2.5f;
		GUI.skin.button.fontSize=13;
		GUILayout.BeginArea(new Rect(leftpadding, Screen.height*0.8f,
		Screen.width -2*leftpadding, 200));
		// Load the main scene
		// The scene needs to be added into build setting to be loaded!
		if (GUILayout.Button("Restart Game"))
		{	
			Application.LoadLevel("LoadingBar");
		}
		if (GUILayout.Button("Exit"))
		{
			Application.Quit();
		}
		GUILayout.EndArea();
	}
	// Update is called once per frame
	void Update () {
		
	}
}
