using UnityEngine;
using System.Collections;

public class StickyBomb : MonoBehaviour {
	public float blastRadius;
	public float bombDuration;
	public bool exploded;
	
	
	// Use this for initialization
	void Start () {
		blastRadius = 5.0f;
		bombDuration = 10.0f;
		exploded = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnCollisionEnter(Collision collision){
		
		
		
	}
}
