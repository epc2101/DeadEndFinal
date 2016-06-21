using UnityEngine;
using System.Collections;

public class CamPosScript : MonoBehaviour {

	// Use this for initialization
	
	private float offset;
	private bool flag;
	private GameObject standard;
	void Start () {
		standard=GameObject.Find("CamPosStandard");
		transform.position=standard.transform.position;
		transform.forward=standard.transform.forward;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		flag=false;
		float mindist=100000000.0f;
		
		//Debug.Log(standard.transform.position.ToString()+";;"+standard.transform.forward.ToString());
		float raylength=5.0f;
		Vector3 startpoint=standard.transform.position+standard.transform.forward.normalized*raylength;
		RaycastHit[] rhs=Physics.RaycastAll(startpoint,-standard.transform.forward.normalized,raylength);
		
		
		if(rhs.Length==0) return;
		
		foreach(RaycastHit hitinfo in rhs)
		{
			if(hitinfo.collider.gameObject.layer!=10) continue;
			float tempdist=Vector3.Distance(startpoint,hitinfo.point);
			if(tempdist<mindist)
			{
				mindist=tempdist;
			}
		}
		if(mindist>100.0f) return;
		gameObject.transform.position=startpoint-standard.transform.forward.normalized*mindist;
	}
}
