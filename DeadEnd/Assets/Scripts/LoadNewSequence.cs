using UnityEngine;
using System.Collections;

public class LoadNewSequence : MonoBehaviour {
	public int nextScene;
	public Texture2D fadeTexture; 
	
	float loadNextSceneTimer; 
	bool loadNextScene; 
	
	// Use this for initialization
	void Start () {
		loadNextSceneTimer = 0.0f; 
		loadNextScene = false; 
		iTween.CameraFadeAdd(fadeTexture, 200); 
		//iTween.CameraFadeFrom(iTween.Hash("amount", 1.0f, "time", 6.0f, "easetype", "easeOutCubic")); 
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		
		
		if (loadNextSceneTimer > 5.0f) {
			if (nextScene == null) nextScene = 0; 
			{
				GeneralControl.CurrentLevel=nextScene;
				Application.LoadLevel("LoadingBar");
			}
		} else if (loadNextSceneTimer > 0.1f) {
			iTween.CameraFadeTo(iTween.Hash("amount", 1.0f, "time", 4.0f, "easetype", "easeInCubic")); 	
		}
		if (loadNextScene == true) { 
			loadNextSceneTimer += Time.deltaTime; 
		}
	}
	
	void OnTriggerEnter(Collider other) {
		if (other.CompareTag("Player")) {
			loadNextScene = true; 
			
			//Find the directional light & turn it off
//			GameObject dirLight = GameObject.FindGameObjectWithTag("MainLight"); 
//			if (dirLight != null) {
//				Light l = dirLight.GetComponent<Light>(); 
//				l.intensity = 0.0f; 
//			}
		}
	}
}
