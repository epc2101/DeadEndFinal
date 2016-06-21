using UnityEngine;
using System.Collections;

public class Flashbang : MonoBehaviour {
	public float blastRadius;
	public bool exploded;
	
	
	// Use this for initialization
	void Start () {
		blastRadius = 15.0f;
		exploded = false;
	}
	
	public void Detonate(){
		if(!exploded){
			ParticleSystem sys = gameObject.GetComponent<ParticleSystem>();
			sys.Play();
			Debug.Log ("Exploded!!");
			AudioController.Play("Flashbang");
			exploded = true;
		}
	}
	
	
}
