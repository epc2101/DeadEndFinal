using UnityEngine;
using System.Collections;

public class lightIntesityChange : MonoBehaviour {

	// Use this for initialization
	private GameObject ThunderPos;
	void Start () {
		ThunderPos = GameObject.Find("ThunderPosition");
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Light li=gameObject.GetComponent<Light>();
		li.intensity=ThunderPos.transform.position.x;
	}
}
