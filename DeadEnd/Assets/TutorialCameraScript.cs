using UnityEngine;
using System.Collections;

public class TutorialCameraScript : MonoBehaviour {

	// Use this for initialization
	public Texture2D cameraTexture;
	void Start () {
		

	  iTween.CameraFadeAdd(cameraTexture,200);

	    iTween.CameraFadeFrom(iTween.Hash("amount",1.0f,"time",5.0f,"easetype","easeOutCubic"));
	
	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
