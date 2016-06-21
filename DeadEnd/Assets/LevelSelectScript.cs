using UnityEngine;
using System.Collections;

public class LevelSelectScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	
	void OnGUI()
	{
		int leftpadding=200;
		GUI.skin.button.fontSize=13;
		GUILayout.BeginArea(new Rect(leftpadding, Screen.height / 2 - 100,
		Screen.width -2*leftpadding, 200));
		// Load the main scene
		// The scene needs to be added into build setting to be loaded!
		if (GUILayout.Button("Open Scene"))
		{	
			Application.LoadLevel("Opening");
			Destroy(gameObject);
		}
		if (GUILayout.Button("Tutorial Level"))
		{
			GeneralControl.CurrentLevel=1;
			Application.LoadLevel("LoadingBar");
			Destroy(gameObject);
		}
		if(GUILayout.Button("Level 1"))
		{
			GeneralControl.CurrentLevel=2;
			Application.LoadLevel("LoadingBar");
		}
		if(GUILayout.Button("Level 2"))
		{
			GeneralControl.CurrentLevel=3;
			Application.LoadLevel("LoadingBar");
		}
		if(GUILayout.Button("Level 3"))
		{
			GeneralControl.CurrentLevel=4;
			Application.LoadLevel("LoadingBar");
		}
		if(GUILayout.Button("Level 4"))
		{
			GeneralControl.CurrentLevel=5;
			Application.LoadLevel("LoadingBar");
		}
		if(GUILayout.Button("Winning Movie"))
		{
			Application.LoadLevel("GameWinScene");
		}
		if (GUILayout.Button("Exit"))
		{
			Application.Quit();
		}
		GUILayout.EndArea();
	}
	void Update () {
	
	}
}
