using UnityEngine;
using System.Collections;

public class ExhaustedScript : MonoBehaviour {
	
	public float currentExhaustedVal=0;
	public float totalExhaustedVal=100.0f;
	
	private float threshold1=1.0f;
	private float threshold2=4.0f;
	
	private Animator anim;	
	private AnimatorStateInfo currentBaseState;
	public bool isPanting;
	
	static int idleState = Animator.StringToHash("Base Layer.Idle");	
	static int locoState = Animator.StringToHash("Base Layer.Locomotion");			// these integers are references to our animator's states
	static int jumpState = Animator.StringToHash("Base Layer.Jump");				// and are used to check state for various actions to occur
	static int jumpDownState = Animator.StringToHash("Base Layer.JumpDown");		// within our FixedUpdate() function below
	static int fallState = Animator.StringToHash("Base Layer.Fall");
	static int rollState = Animator.StringToHash("Base Layer.Roll");
	static int kickBackState = Animator.StringToHash("Base Layer.PushKickBackwardLow");
	static int kickRightState = Animator.StringToHash("Base Layer.PushKickRightLow");
	static int kickForwardState = Animator.StringToHash("Base Layer.PushKickFront");
	static int kickLeftState = Animator.StringToHash("Base Layer.PushKickLeftLow");
	
	
	// Use this for initialization
	void Start () {
		currentExhaustedVal=totalExhaustedVal;
		anim = GetComponent<Animator>();
		isPanting = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(	currentExhaustedVal < totalExhaustedVal/5.0f){
				if(!isPanting){
						//AudioController.Play("BreatheHard");
						isPanting = true;
					}
		} 
		if(currentExhaustedVal > totalExhaustedVal/2.0f){
			//AudioController.Stop("BreatheHard");
			isPanting = false;
		}
				
	}
	
	void FixedUpdate()
	{
		float vel=Vector3.Distance(gameObject.rigidbody.velocity,new Vector3(0,gameObject.rigidbody.velocity.y,0));
		
		if(vel>threshold2) currentExhaustedVal-=(vel-threshold2)/10.0f;

		if(vel<threshold2) currentExhaustedVal+=(threshold2-vel)*0.1f;
//		Debug.Log(vel);
		if(currentExhaustedVal>totalExhaustedVal) currentExhaustedVal=totalExhaustedVal;
		if(currentExhaustedVal<0)currentExhaustedVal=0;
		
		currentBaseState = anim.GetCurrentAnimatorStateInfo(0);
		
		anim.SetFloat("ExhaustedVal",currentExhaustedVal/totalExhaustedVal);
		//Debug.Log();
	}
}
