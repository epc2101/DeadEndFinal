//NOTICE:
//This File actually contains all the GUI elements
//as well as the in-game menu
//NOT ONLY the Exhaustion Bar!
//THIS IS IMPORTANT

using UnityEngine;
using System.Collections;
 
public class EXHbarScript : MonoBehaviour
{

//current progress
    private float progress;
 	private float timeOut; 
	
    private Vector2 pos = new Vector2(10,50);
    private Vector2 size = new Vector2(300,30);
	
	private Vector2 pos2=new Vector2(Screen.width*0.6f,50);
	private Vector2 size2=new Vector2(40,200);
 
    public Texture2D emptyTex;
    public Texture2D fullTex;
	
	public Texture2D fullTimerTex;
	public Texture2D timerBarTex;
	
	public Texture2D niddleTex;
	
	public Texture2D backGroundTex;
	
	public bool gamePaused=false;
	
	private GameObject player;
	private string idiotText="You are so SILLY! How Can You Get Extra Injection From A RuinedCity!";
	private bool showIdiotText=false;
 
	void Start()
	{
		player=GameObject.Find("carl");
		gamePaused=false;
		
	}	
    void OnGUI()
    {
		
		GUI.skin.button.fontSize=30;
		GUI.skin.label.fontSize=20;
		GUI.DrawTexture(new Rect(pos.x+5.0f , pos.y+1.0f, size.x*0.97f*Mathf.Clamp01(progress), size.y*0.6f), fullTex);
		GUI.DrawTexture(new Rect(pos.x, pos.y-33, size.x, 60), emptyTex);
   		
		m_Controller mc=player.GetComponent<m_Controller>();
		if(mc.showDeathBar())
		{
			GUI.DrawTexture(new Rect(pos2.x, pos2.y, size2.x, size2.y), fullTimerTex);
			GUI.DrawTexture(new Rect(pos2.x+7.0f , pos2.y+58.0f+size2.y*Mathf.Clamp01(timeOut)*0.71f, size2.x*0.6f, size2.y*Mathf.Clamp01(1-timeOut)*0.71f), timerBarTex);
			
		}
		
		int num=mc.getInjectNum();
		float sizeniddlex=40;float sizeniddley=sizeniddlex*5.0f/3.0f;
		float niddlex=Screen.width-2*sizeniddlex;float niddley=20.0f;
		float buttonHeight=Screen.height/10.0f;
		for(int i=0;i<num;i++)
		{
			GUI.DrawTexture(new Rect(niddlex,niddley,sizeniddlex,sizeniddley),niddleTex);
			niddlex-=sizeniddlex;
		}
		if(gamePaused)
		{
			GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height),backGroundTex);
			float buttonWidth=Screen.width/3.5f;
			float leftpadding=Screen.width/2.0f-buttonWidth/2.0f;
			GUILayout.BeginArea(new Rect(leftpadding, Screen.height *0.13f,
			Screen.width -2*leftpadding, Screen.height));
			
			
			
			if (GUILayout.Button("Resume Game",GUILayout.Width(buttonWidth),GUILayout.Height(buttonHeight)))
			{	
				gamePaused=false;
			}
			GUILayout.Label(showIdiotText?idiotText:"",GUILayout.Width(buttonWidth),GUILayout.Height(buttonHeight));
			if(GUILayout.Button("Restart Game",GUILayout.Width(buttonWidth),GUILayout.Height(buttonHeight)))
			{
				Application.LoadLevel("LoadingBar");
			}
			if (GUILayout.Button("Buy Injections",GUILayout.Width(buttonWidth),GUILayout.Height(buttonHeight)))
			{
				showIdiotText=true;
			}
			if (GUILayout.Button("Exit Game",GUILayout.Width(buttonWidth),GUILayout.Height(buttonHeight)))
			{
				Application.Quit();
			}
			GUILayout.EndArea();
		}
    }
 
	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			gamePaused=!gamePaused;

		}
		if(gamePaused)Time.timeScale=0.0f;
		else
		{
			Time.timeScale=1.0f;
			showIdiotText=false;
		}
	}
    void FixedUpdate()
    {
		GameObject myDearCarl=GameObject.Find("carl");
		ExhaustedScript exs=myDearCarl.GetComponent<ExhaustedScript>();
		progress=exs.currentExhaustedVal/exs.totalExhaustedVal;
		m_Controller mc=myDearCarl.GetComponent<m_Controller>();

		timeOut=mc.getDyingProgress();
		

    }
 
}