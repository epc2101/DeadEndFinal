using UnityEngine;
using System.Collections;

public class AnimCameraScript : MonoBehaviour {

	// Use this for initialization
	public GameObject target;
	public Texture2D cameraTexture;
	public float smooth = 3f;		
	Transform standardPos;	
	
	void Start () {
		
		standardPos = target.transform;
		
	   iTween.CameraFadeAdd(cameraTexture,200);
	   iTween.CameraFadeFrom(iTween.Hash("amount",1.0f,"time",5.0f,"easetype","easeOutCubic"));
	    
	}
	
		
	
	
	void FixedUpdate ()
	{
		transform.position = Vector3.Lerp(transform.position, standardPos.position, Time.deltaTime * smooth);	
		transform.forward = Vector3.Lerp(transform.forward, standardPos.forward, Time.deltaTime * smooth);
		

	}
}
