using UnityEngine;
using System.Collections;

public class GameWinCharacterCtrl : MonoBehaviour {

	// Use this for initialization
	
	private Animator anim;
	private int maxRunningTime;
	public int timer;
	
	void Start () {
		anim = GetComponent<Animator>();
		anim.SetFloat("ExhaustedVal",1.0f);
		maxRunningTime=700;
		timer=1;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(timer>0)
		{
			timer++;
			if(timer<=maxRunningTime)
			{
				anim.SetFloat("Speed",0.3f);
			}
			else
			{
				anim.SetFloat("Speed",0.0f);
			}
		}
	}
}
