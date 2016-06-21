using UnityEngine;
using System.Collections;

public class Injection : MonoBehaviour {
	public int numInjections;	
	public GUIText injectCount;
	
	// Use this for initialization
	void Start () {
		numInjections = 3;
		injectCount = gameObject.GetComponent<GUIText>();
	}
	
	// Update is called once per frame
	void Update () {
		if(numInjections <= 0){
			numInjections = 0;
			injectCount.text = "Injections: 0";
		}
		else {
			injectCount.text = "Injections: "+numInjections;	
		}
	}
}
