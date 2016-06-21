using UnityEngine;
using System.Collections;

public class AnimPlayScript : MonoBehaviour {
	
	private Animator anim;
	private bool played=false;
	private AnimatorStateInfo currentBaseState;	
	static int part1_2state = Animator.StringToHash("Base Layer.part1_2");
	
	public Camera animCamera;
	public Camera mainCamera;
	private GameObject mainCarl;
	private GameObject animCarl;
	
	private bool playdirectly;
	private GameObject radioToGrab;
	
	int stage;

	public AudioClip openingClip;
	public Texture2D backgroundTexture;
	private bool isGrabbing=false;
	private Transform rightHand;
	// Use this for initialization
	void Start () {
		animCamera.enabled=true;
		mainCamera.enabled=false;
		played=false;
		anim=GameObject.Find("carl2").GetComponent<Animator>();
		GameObject.Find("carl2").transform.localScale=GameObject.Find("carl").transform.localScale;
		iTween.MoveTo(gameObject,iTween.Hash("x",1.0f,"y",1.5f,"time",15.0f,"easetype","linear"));
		AudioController.Play("OpeningRadio");
		AudioController.Play ("HorrorDance");
		
		stage=0;
		//AudioSource.PlayClipAtPoint(openingClip,GameObject.Find("carl2").transform.position);
		radioToGrab=GameObject.Find("radioForGrab");
		playdirectly=false;
		if(playdirectly)stage=1;
		rightHand=GameObject.Find("carl2").transform.Find("Hips").Find("Spine").Find("Spine1").Find("Spine2").Find("RightShoulder").Find("RightArm").Find("RightForeArm").Find("RightHand");
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(!isGrabbing && gameObject.transform.position.y>0.9f && gameObject.transform.position.y<1.1f)
		{
			isGrabbing=true;
			Debug.Log("Hey im MVP");
		}
		if(isGrabbing)
		{
			
			radioToGrab.transform.position=rightHand.position+new Vector3(0.08f,0,0);
			if(gameObject.transform.position.y>1.39f)
			{
				radioToGrab.rigidbody.useGravity=true;
				isGrabbing=false;
			}
		}
		if(stage==0)
		{
			if(gameObject.transform.position.x>0.2f)
			{
				played=true;
				anim.SetBool("StartPlay",true);
				stage=1;
			}
			
		}

		if(stage==1)
		{

			mainCamera.transform.position=animCamera.transform.position;
			mainCamera.transform.forward=animCamera.transform.forward;
			
			currentBaseState=anim.GetCurrentAnimatorStateInfo(0);
			if((currentBaseState.nameHash==part1_2state)|| playdirectly)
			{
				stage=2;
				played=false;
				transform.position=new Vector3(0,0,0);
				animCamera.enabled=false;
				mainCamera.enabled=true;
				mainCarl=GameObject.Find("carl");
				animCarl=GameObject.Find("carl2");
				Vector3 temp=new Vector3(animCarl.transform.position.x,animCarl.transform.position.y,animCarl.transform.position.z);
				animCarl.transform.position=new Vector3(mainCarl.transform.position.x+500,mainCarl.transform.position.y,mainCarl.transform.position.z);
				mainCarl.transform.position=temp;
				mainCarl.transform.forward=animCarl.transform.forward;
				TutorialTargetScript tts=GameObject.Find("TutorialTarget1").GetComponent<TutorialTargetScript>();
				tts.eligible=true;
				
				GameObject theChair=GameObject.Find("Chair_Special");
				theChair.transform.position+=new Vector3(10.0f,0,0);
			}
		}
	}
}
