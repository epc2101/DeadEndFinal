using UnityEngine;
using System.Collections;

public class CameraSound : MonoBehaviour {
	public AudioClip zombieMoan;
	AudioSource sourcer;
	public GameObject carl;
	bool playingMoan;
	public int numZombiesLooking;
	
	int currentSong;
	
	
	// Use this for initialization
	void Start () {
		playingMoan = false;
		//sourcer = GetComponent<AudioSource>();
		//sourcer.loop = true;
		//sourcer.clip = zombieMoan;
		//sourcer.Play();
		//sourcer.volume = 0.5f;
		AudioController.PlayMusicPlaylist();
		numZombiesLooking = 0;
		currentSong = 0;
		
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log("Current num looking is: " + numZombiesLooking);
		if(numZombiesLooking > 0 && currentSong == 0){
			AudioController.PlayNextMusicOnPlaylist();
			Debug.Log("Switching to zombie");
			currentSong = 1;
		}
		if(numZombiesLooking <= 0 && currentSong == 1){
			AudioController.PlayPreviousMusicOnPlaylist();
			Debug.Log("Switching to wind");
			currentSong = 0;
		}
	}
}
