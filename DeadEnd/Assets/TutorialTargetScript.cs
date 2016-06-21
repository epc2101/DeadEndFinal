using UnityEngine;
using System.Collections;

public class TutorialTargetScript : MonoBehaviour {

	// Use this for initialization
	
	public bool eligible=false;
	public bool activate=false;
	public bool dontshowagain=false;
	public string textToShow;
	public GameObject nextTutorialTarget;
	public Texture2D backGroundTex;
	
	public bool gamePaused=false;
	public string targetName;
	
	private int timer=0;
	public int activateTime=100;
	public int task=0; //0 is sth arrival, 1 is interact, 2 is immediately after eligible, 3 is inject
						//4 is cost exhaustion
	
	void Start() {

	}
	
	// Update is called once per frame
	void FixedUpdate () {
		ParticleSystem ps=gameObject.transform.FindChild("Shining").gameObject.GetComponent<ParticleSystem>();
		
		if(task==2)
		{
			if(eligible)timer++;
			if(timer>activateTime)
			{
				timer=0;
				if(nextTutorialTarget!=null && nextTutorialTarget.name=="TutorialTarget7")
				{
					m_Controller mc=GameObject.Find("carl").GetComponent<m_Controller>();
					mc.getBitten();
				}
				actiateNext();
			}
		}
		if(task==3 && eligible)
		{
			m_Controller mc=GameObject.Find("carl").GetComponent<m_Controller>();
			if(mc.getInjectNum()<5)
			{
				actiateNext();
			}
		}
		if(task==4 && eligible)
		{
			ExhaustedScript es=GameObject.Find("carl").GetComponent<ExhaustedScript>();
			if(es.currentExhaustedVal/es.totalExhaustedVal<0.9f)
			{
				actiateNext();
			}
		}
		if(!eligible)
		{
			if(ps.isPlaying)
			{
			//	gameObject.renderer.enabled=false;
				ps.Pause();
				ps.Clear();
			}
		}
		else
		{
			if(ps.isPaused)
			{
			//	gameObject.renderer.enabled=true;
				ps.Play();
			}
		}
		
	}
	void Update()
	{
	//	if(gamePaused)Time.timeScale=0.0f;
	//	else Time.timeScale=1.0f;
	}
	void OnGUI()
	{
		if(nextTutorialTarget!=null)
		{
			TutorialTargetScript tts=nextTutorialTarget.GetComponent<TutorialTargetScript>();
			if(tts.activate)dontshowagain=true;
		}
		if(activate&&!dontshowagain)
		{
			GUI.skin.button.fontSize=20;
			GUI.skin.label.fontSize=20;
			GUI.DrawTexture(new Rect(-Screen.width/2.0f,70,Screen.width,Screen.height),backGroundTex);
			float buttonWidth=Screen.width/3.5f/1.5f;
			float buttonHeight=Screen.height/18.0f;
			float leftpadding=Screen.width/2.0f-buttonWidth/2.0f;
			GUILayout.BeginArea(new Rect(leftpadding-leftpadding+5, Screen.height *0.13f+85,
			Screen.width -2*leftpadding, Screen.height));	
			GUILayout.Label(textToShow,GUILayout.Width(buttonWidth));
			if(GUILayout.Button("Dismiss",GUILayout.Width(buttonWidth),GUILayout.Height(buttonHeight)))
			{
				dontshowagain=true;
			}
			GUILayout.EndArea();
		}
	}
	void OnTriggerEnter(Collider collider)
	{
		if(!eligible)return;
		if(activate)return;
		if(task!=0)return;
		if(collider.name!=targetName)return;
		actiateNext();
	}
	
	void OnTriggerStay(Collider collider)
	{
		if(!eligible)return;
		if(activate)return;
		if(task!=1)return;
		if(collider.name!="carl") return;
		if(transform.parent.parent==null)return;
		if(transform.parent.parent.gameObject.name!="c_LeftHand")return;
		
		actiateNext();
		
	}
	
	private void actiateNext()
	{		
		activate=true;
		if(nextTutorialTarget!=null)
		{
			TutorialTargetScript tts=nextTutorialTarget.GetComponent<TutorialTargetScript>();
			if(tts!=null)tts.eligible=true;	
		}
		eligible=false;
		
	}
}
