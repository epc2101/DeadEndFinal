using UnityEngine;
using System.Collections;

public class TutorialEndScript : MonoBehaviour {

	// Use this for initialization
	
	private int timer;
	private int maxtime;
	
	public Texture2D cameraTexture;
	
	void Start () {
		timer=0;
		maxtime=170;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(timer>0)timer++;
		if(timer>=maxtime)
		{
			GeneralControl.CurrentLevel=2;
			Application.LoadLevel("LoadingBar");
		}
	}
	
	void OnTriggerEnter(Collider collider)
	{
		if(collider.gameObject.tag!="Player") return;
		
				
		iTween.CameraFadeAdd(cameraTexture,200);
		iTween.CameraFadeTo(iTween.Hash("amount",1.0f,"time",5.0f,"easetype","easeOutCubic"));
	    timer=1;
	}
}
