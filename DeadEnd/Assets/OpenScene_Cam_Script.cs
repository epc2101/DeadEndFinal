using UnityEngine;
using System.Collections;

public class OpenScene_Cam_Script : MonoBehaviour {
	private GameObject DeadText;
	private GameObject EndText;
	private GameObject BackGround;
	private GameObject presentText;
	private GameObject continueText;
	private GameObject pointLight;
	private GameObject thunderIntensity;
	
	private GameObject thunder;
	float nowtime;
	public Texture2D cameraTexture;
	bool played, playedSecond;
	// Use this for initialization
	void Start () {
		played = false;
		playedSecond = false;
		nowtime=0.0f;
		DeadText=GameObject.Find("Dead_Text");
		EndText=GameObject.Find("End_Text");
		BackGround=GameObject.Find("Plane");
		presentText=GameObject.Find("PresentText");
		pointLight=GameObject.Find("PointLight1");
		thunderIntensity=GameObject.Find("ThunderPosition");
		continueText=GameObject.Find("ContinueText");
		
		continueText.renderer.material.color=new Color(0,0,0,0);
		//iTween.ColorTo(continueText,iTween.Hash("Color",new Color(0,0,0,0),"delay",0.0f, "time",0.0f,"easetype", "easeInOutCubic"));
		iTween.ColorFrom(presentText,iTween.Hash("Color",new Color(0,0,0,1),"delay",nowtime, "time",3.0f,"easetype", "easeInOutCubic"));
		iTween.MoveBy(presentText,iTween.Hash("z",-2.0f,"delay",nowtime, "time",3.0f,"easetype", "easeOutCubic"));
		nowtime+=3.0f;
		iTween.ColorTo(presentText,iTween.Hash("Color",new Color(0,0,0,1),"delay",nowtime, "time",2.0f,"easetype", "easeInCubic"));
		nowtime+=2.0f;
		iTween.MoveBy(presentText,iTween.Hash("y",300,"time",0.05f,"delay",nowtime));
		
		iTween.ColorFrom(BackGround,iTween.Hash("Color",new Color(0,0,0,1),"delay",nowtime, "time",3.0f,"easetype", "easeInCubic"));
		nowtime+=3.0f;
		iTween.MoveFrom(DeadText, iTween.Hash("x", DeadText.transform.position.x-3,"y", DeadText.transform.position.y+3, "z" , DeadText.transform.position.z-3, "easetype", "easeInCubic", "loopType", "none", "delay", nowtime));
		nowtime+=1.0f;
		
		iTween.ShakePosition(gameObject,iTween.Hash("x",0.1,"delay",nowtime));
		nowtime+=0.1f;
		iTween.MoveFrom(EndText, iTween.Hash("x", EndText.transform.position.x+3,"y", EndText.transform.position.y+3, "z" , EndText.transform.position.z-3, "easetype", "easeInCubic", "loopType", "none", "delay", nowtime));
		nowtime+=1.0f;
		
		iTween.ShakePosition(gameObject,iTween.Hash("x",0.1,"delay",nowtime));
		nowtime+=1.5f;
		iTween.MoveBy(pointLight,iTween.Hash("x",10.0f,"delay",nowtime,"time",0.8f,"easetype","easeInOutCubic"));
		iTween.MoveBy(thunderIntensity,iTween.Hash("y",1.0f,"delay",nowtime,"time",0.1f));
		float nowtime2=nowtime+1.0f;
		nowtime+=4.0f;
		for(int i=0;i<200;i++)
		{
			onContinueText(continueText,nowtime2);
			nowtime2+=2.0f;
		}
		
		for(int i=0;i<200;i++)
		{
			onThunder(thunderIntensity, nowtime);
			nowtime+=6.0f;
		}
		
	}
	
	void onThunder(GameObject intense, float basicDelay)
	{
		iTween.ShakePosition(intense,iTween.Hash("x",5.0f,"delay",basicDelay,"time",2.0f));
		
	}
	void onContinueText(GameObject conText, float basicDelay)
	{
		iTween.ColorTo(conText,iTween.Hash("Color",new Color(1,1,1,1),"delay",basicDelay, "time",1.0f,"easetype", "easeInOutCubic"));
		iTween.ColorTo(conText,iTween.Hash("Color",new Color(0,0,0,0),"delay",basicDelay+1.0f, "time",1.0f,"easetype", "easeInOutCubic"));
	}
	// Update is called once per frame
	public void OnGUI() {
		if(DeadText.transform.position.x > -3.06 && !played) {
			AudioController.Play("Boom");
			played = true;
		}
		if(EndText.transform.position.x < -.22 && !playedSecond){
			AudioController.Play("Boom");
			playedSecond = true;
		}
		
    if (Event.current.type == EventType.KeyDown) {
        KeyPressedEventHandler();
  	  }
	}
	
	
	private void KeyPressedEventHandler() {
		doContinue();

	}
	
	private void doContinue()
	{
		if(thunderIntensity.transform.position.y>0.5f)
		{
			GeneralControl.CurrentLevel=1;
    		Application.LoadLevel("LoadingBar");
		}
	}

}
