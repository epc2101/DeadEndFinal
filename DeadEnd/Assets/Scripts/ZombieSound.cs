using UnityEngine;
using System.Collections;

public class ZombieSound : MonoBehaviour {
	AudioSource zombieSource;
	bool seenPlayer;
	ZombieControl control;
	bool moaning;
	public CameraSound sounder;
	
	// Use this for initialization
	void Start () {
		//zombieSource = gameObject.GetComponent<AudioSource>();
		control = gameObject.GetComponent<ZombieControl>();
		sounder = GameObject.Find("Main Camera").GetComponent<CameraSound>();
		seenPlayer = false;
		moaning = false;
		//zombieSource.loop = true;
		//zombieSource.Pause();
		
	}
	
	// Update is called once per frame
	void Update () {
		seenPlayer = control.hasSeenPlayer;
		if(seenPlayer && !moaning){
			//Debug.Log ("Saw the player");
			AudioController.Play("ZombiePursue");
	
			sounder.numZombiesLooking++;
			//Debug.Log(sounder.numZombiesLooking + " have seen player");
			moaning = true;
		}
		if(!seenPlayer && moaning){
			//zombieSource.Pause();
			AudioController.Stop("ZombiePursue");
			sounder.numZombiesLooking--;
			moaning = false;
		}
		
	}
}
