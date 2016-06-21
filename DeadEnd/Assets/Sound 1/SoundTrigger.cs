using UnityEngine;
using System.Collections;

public class SoundTrigger : MonoBehaviour {
	public string clipName;
	public GUIText message;
	public string messageToSay;
	bool collided;
	
	// Use this for initialization
	void Start () {
		GameObject o = GameObject.Find("Messenger");
		message = o.GetComponent<GUIText>();
		collided = false;
	}
	
	
	void OnTriggerEnter(Collider other){
		if(!collided && other.CompareTag("Player")){
			AudioController.Play(clipName);
			message.text = messageToSay;
			collided = true;
		}
		
	}
}
