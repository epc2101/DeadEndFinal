using UnityEngine;
using System.Collections;

public class LoadingBarScript : MonoBehaviour {
	
	AsyncOperation async;
	// Use this for initialization
	void Start () {

		async=Application.LoadLevelAsync(GeneralControl.CurrentLevel);

	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(!async.isDone)
		Debug.Log((async.progress*100.0f).ToString("0.00"));
			
	}
}
