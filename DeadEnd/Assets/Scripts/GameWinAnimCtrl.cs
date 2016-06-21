using UnityEngine;
using System.Collections;

public class GameWinAnimCtrl : MonoBehaviour {
	
	public Camera mainCam;
	public GameObject[] camBuoy;
	public GUIText youtext;
	public GUIText fortext;
	// Use this for initialization
	void Start () {
		float nowTime=0.0f;
		float timegap=3.0f;
		float []timeslot=new float[]{timegap,timegap,timegap,0.5f,5.0f,timegap,timegap,timegap};
		int i=0;
		AudioController.Play("MissionAtlantis");
		youtext.material.color=new Color(0,1,0,0);
		fortext.material.color=new Color(1,0,0,0);
		
		foreach(GameObject camB in camBuoy)
		{
			if(i==4)
				iTween.ShakePosition(mainCam.gameObject,iTween.Hash("time",5.0f,"delay",nowTime,"x",3.0f,"y",3.0f,"z",3.0f));		
			else
				iTween.MoveTo(mainCam.gameObject,iTween.Hash("position",camB.transform.position,"time",timeslot[i],"delay",nowTime,"easetype","linear"));
			iTween.RotateTo(mainCam.gameObject,iTween.Hash("rotation",camB.transform.rotation.eulerAngles,"time",timeslot[i],"delay",nowTime,"easetype","linear"));
			iTween.ScaleTo(mainCam.gameObject,iTween.Hash("x",camB.transform.localScale.x,"time",timeslot[i],"delay",nowTime,"easetype","linear"));
			nowTime+=timeslot[i];
			
			i++;
		}
		iTween.ColorTo(youtext.gameObject,iTween.Hash("Time",2.0f,"Delay",24.0f,"a",1.0f));
		iTween.ColorTo(fortext.gameObject,iTween.Hash("Time",3.0f,"Delay",27.0f,"a",1.0f));
	}
	
	
	// Update is called once per frame
	void FixedUpdate () {
		GlowEffect ge=mainCam.gameObject.GetComponent<GlowEffect>();
		ge.glowIntensity=mainCam.gameObject.transform.localScale.x-0.2f;
	}
}
