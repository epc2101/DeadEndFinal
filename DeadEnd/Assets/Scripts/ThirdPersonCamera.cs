using UnityEngine;
using System.Collections;

public class ThirdPersonCamera : MonoBehaviour
{
	public float smooth = 3f;		// a public variable to adjust smoothing of camera motion
	Transform standardPos;			// the usual position for the camera, specified by a transform in the game
	//public GameObject targetPos;
	
	void Start()
	{
		// initialising references
		standardPos = GameObject.Find ("CamPos").transform;
		//standardPos=targetPos.transform;
	}
	
	void FixedUpdate ()
	{
		transform.position = Vector3.Lerp(transform.position, standardPos.position, Time.deltaTime * smooth);	
		transform.forward = Vector3.Lerp(transform.forward, standardPos.forward, Time.deltaTime * smooth);
	
	}
}
