using UnityEngine;
using System.Collections;

// Require these components when using this script
[RequireComponent(typeof (Animator))]
[RequireComponent(typeof (CapsuleCollider))]
[RequireComponent(typeof (Rigidbody))]
public class m_Controller : MonoBehaviour
{
	[System.NonSerialized]					
	public float lookWeight;					// the amount to transition when using head look
	
	[System.NonSerialized]
	//public Transform enemy;						// a transform to Lerp the camera to during head look
	
	public float animSpeed = 1.5f;				// a public setting for overall animator animation speed
	public float lookSmoother = 3f;				// a smoothing setting for camera motion
	public bool useCurves;						// a setting for teaching purposes to show use of curves

	
	private Animator anim;							// a reference to the animator on the character
	private AnimatorStateInfo currentBaseState;			// a reference to the current state of the animator, used for base layer
	private AnimatorStateInfo layer2CurrentState;	// a reference to the current state of the animator, used for layer 2
	private CapsuleCollider col;					// a reference to the capsule collider of the character
	

	static int idleState = Animator.StringToHash("Base Layer.Idle");	
	static int locoState = Animator.StringToHash("Base Layer.Locomotion");			// these integers are references to our animator's states
	static int jumpState = Animator.StringToHash("Base Layer.Jump");				// and are used to check state for various actions to occur
	static int standJumpState = Animator.StringToHash("Base Layer.jump_standing");		// within our FixedUpdate() function below
	static int fallState = Animator.StringToHash("Base Layer.Fall");
	static int rollState = Animator.StringToHash("Base Layer.Roll");
	static int catchBreathState = Animator.StringToHash("Base Layer.CatchBreath");
	static int kickBackState = Animator.StringToHash("Base Layer.PushKickBackwardLow");
	static int kickRightState = Animator.StringToHash("Base Layer.PushKickRightLow");
	static int kickForwardState = Animator.StringToHash("Base Layer.PushKickFront");
	static int kickLeftState = Animator.StringToHash("Base Layer.PushKickLeftLow");
	static int dogdgeState1 = Animator.StringToHash("Base Layer.DodgeLeft");
	static int dogdgeState2 = Animator.StringToHash("Base Layer.DodgeRight");
	static int idlelState_L2=Animator.StringToHash("Layer2.Nothing");
	
	private int holdStuff=0;
	private GameObject stuffInHand;
	private bool locker=false;
	private int locktime;
	private int timer;
	
	private int infectTimer=0;
	private int infectMaxTime=500;
	
	private int fallTimer=0;
	private int fallMaxTime=10;
	
	private int invulnerableTimer=0;
	private int invulnerableMaxTime=500;
	private bool firstInject = true;
	Light halo; 
	
	private int deadTimer;
	private int deadMaxTime;
	
	private bool isBleeding;
	
	private int injectRemain;
	private float heightNow;
	
	private float maxCurrentHeight;
	private float defaultPosy;
	private float defaultHeight;
	
	public Texture2D objCursor;
	public GameObject activeObj;
	
	private bool isInvulnerable;
	
	//AudioController data
	public bool isWalking;
	public bool isPanting;
	public bool isJumping;
	public bool playingWalking;
	public bool playingHeartBeat;
	public bool playingHeartBeatFast;
	public bool playingPanting;
	bool isRolling;
	
	void Start ()
	{
		GameObject ch = transform.Find("CarlHalo").gameObject;
	    halo = ch.GetComponent<Light>(); 
		
		isInvulnerable=true;
		setBleed(false);
		setInvulnerable(false);
		holdStuff=0;
		stuffInHand=null;
		// initialising reference variables
		anim = GetComponent<Animator>();					  
		col = GetComponent<CapsuleCollider>();				

		if(anim.layerCount ==2)
			anim.SetLayerWeight(1, 1);
		
		isBleeding=false;
		injectRemain=5;
		
		defaultPosy=col.center.y;
		defaultHeight=col.height;
		
		//AudioController data
		isWalking = true;
		isPanting = false;
		playingPanting = false;
		playingWalking = false;
		playingHeartBeat = false;
		playingHeartBeatFast = false;
		isJumping = false;
		isRolling = false;
	}
	
	
	void FixedUpdate ()
	{
		if(invulnerableTimer>0)
		{
			invulnerableTimer++;
			if(firstInject) {
				halo.intensity = 2.0f; 
				firstInject = false;
			} else
				halo.intensity -=0.0025f; 
			
			if(invulnerableTimer>=invulnerableMaxTime)
			{
				setInvulnerable(false);
			}
		}
		if(locker)
		{
			//Debug.Log("timer : "+timer.ToString());
			timer++;
			if(timer>=locktime)
			{
				locker=false;timer=0;
				Debug.Log("unlocked!");
			}
			
		}
		if(infectTimer>0)
		{
			infectTimer++;
			if(infectTimer>infectMaxTime)
			{
				GoDie(30);
			}
		}
		
		if(deadTimer>0)
		{
			deadTimer++;
			if(deadTimer>deadMaxTime)
			{
				Application.LoadLevel("GameOverScene");
			}
		}
		bool falling;
		float h = Input.GetAxis("Horizontal");				// setup h variable as our horizontal input axis
		float v = Input.GetAxis("Vertical");				// setup v variables as our vertical input axis
		if(!Input.GetButton("Fire2")){
			v*=0.3f;
			isWalking = true;
		} else {
			isWalking = false;	
		}
		anim.SetFloat("Speed", v);							// set our animator's float parameter 'Speed' equal to the vertical input axis				
		anim.SetFloat("Direction", h); 						// set our animator's float parameter 'Direction' equal to the horizontal input axis		
		anim.speed = animSpeed;								// set the speed of our animator to the public variable 'animSpeed'
		
		currentBaseState = anim.GetCurrentAnimatorStateInfo(0);	// set our currentState variable to the current state of the Base Layer (0) of animation
		
		if(anim.layerCount ==2)		
			layer2CurrentState = anim.GetCurrentAnimatorStateInfo(1);	// set our layer2CurrentState variable to the current state of the second Layer (1) of animation
	
		Ray downRay = new Ray(transform.position + Vector3.up, -Vector3.up);
		RaycastHit downHitInfo = new RaycastHit();

		//bool downRayHit = Physics.Raycast(downRay, out downHitInfo);
		bool downRayHit = Physics.Raycast(downRay, out downHitInfo);
		heightNow = 0f;
		if (downRayHit)
		{
			heightNow = downHitInfo.distance;
			if(heightNow<2.0f)
			{
				if(rigidbody.velocity.y<-12.0f*(Mathf.Sqrt(Physics.gravity.y/-9.81f)))GoDie(10);
			}
			if (heightNow > 2.2f)
			{
				fallTimer++;
			}
			else
			{
				fallTimer=0;	
			}
			if(fallTimer>=fallMaxTime) falling=true;
			else falling=false;
		}
		else
		{
			falling = false;
		}
		
		anim.SetBool("Falling", falling);

		// STANDARD JUMPING
		
		// if we are currently in a state called Locomotion (see line 25), then allow Jump input (Space) to set the Jump bool parameter in the Animator to true
		if (currentBaseState.nameHash == locoState || currentBaseState.nameHash==idleState ||
			currentBaseState.nameHash==catchBreathState)
			{
				resetCollider();
				if(Input.GetButtonDown("Jump"))
				{
					anim.SetBool("Jump", true);
					if(!isJumping){
					AudioController.Play("Jumper");
					isJumping = true;
				}
					
				
			}
		}
		
		// if we are in the jumping state... 
		else if(currentBaseState.nameHash == jumpState)
		{
			
			//  ..and not still in transition..
			if(!anim.IsInTransition(0))
			{

				col.center=new Vector3(col.center.x,defaultPosy+anim.GetFloat("ColliderY")*8.0f,col.center.z);
				anim.SetBool("Jump", false);
				isJumping = false;
			}
		}
		
		
		else if (currentBaseState.nameHash == standJumpState)
		{ 
			col.center=new Vector3(col.center.x,defaultPosy+anim.GetFloat("ColliderY")*10.0f,col.center.z);
			anim.SetBool("Jump",false);
			isJumping = false;
		}
		
		else if (currentBaseState.nameHash == fallState)
		{
			rigidbody.velocity+=Vector3.down*0.1f;
			col.height=1.0f;
			col.center=new Vector3(0,0.5f,0);
			isRolling = false;
		}
		
		else if (currentBaseState.nameHash == rollState)
		{
			if(!isRolling){
				AudioController.Play("BodyRoll");
				isRolling = true;
			}
			if(!anim.IsInTransition(0))
			{
				
				if(useCurves)
				{
					col.height = anim.GetFloat("ColliderHeight");
					col.center = new Vector3(0, anim.GetFloat("ColliderY"), 0);
				}
			}
		}
		else
		{
			resetCollider();
			
			if (currentBaseState.nameHash==kickBackState || currentBaseState.nameHash==kickRightState || 
			currentBaseState.nameHash==kickForwardState|| currentBaseState.nameHash==kickLeftState)
			{
				anim.SetBool("Pushing",false);
			}
		}
		
		if(layer2CurrentState.nameHash==Animator.StringToHash("Layer2.Injection"))
		{
			anim.SetBool("Injecting",false);
		}

		if(layer2CurrentState.nameHash!=idlelState_L2)
		{
			anim.SetInteger("holdStuff",-1);
			anim.SetBool("Pushing",false);
		}
		if(currentBaseState.nameHash== dogdgeState1 || currentBaseState.nameHash==dogdgeState2)
			anim.SetBool("Dodging",false);
		
		if(Input.GetButtonDown("Fire1")&& Input.GetButtonDown("Fire2"))
		{
			knockNearestZombie();
		}
		if(Input.GetButtonDown("Fire1"))
		{
			DetectObjScript dos=gameObject.GetComponent<DetectObjScript>();
			int x=dos.tryDodge();
			if(isBleeding)
			{
				tryInject();
			}
			else if(stuffInHand!=null)
			{
				throwToward();
			}
			else if (v < 0.7f) 
			{

				dos.TryInteract((v<0.7f));
			}
			else if(x>0)
			{
				dodge(x);
			}


		}
	}
	
	private void setBleed(bool bleed)
	{
		GameObject injuredsystem=GameObject.Find("InjuredSystem");
		ParticleSystem ps=injuredsystem.GetComponent<ParticleSystem>();
		if(!bleed)
		{
			
			ps.Pause();
			ps.Clear();
		}else ps.Play();
	}
	
	private void setInvulnerable(bool invulnerable)
	{
		if(invulnerable==isInvulnerable)return;
		isInvulnerable=invulnerable;

			
		//ParticleSystem ps=transform.Find("CarlAura").gameObject.GetComponent<ParticleSystem>();

		
		if(isInvulnerable)
		{
			invulnerableTimer=1;

		}
		else
		{
			invulnerableTimer=0;
			halo.intensity = 0.0f; 
			firstInject = true; 
		}
	}
	
	private void lockPlayerAction(int timeToLock)
	{
		if(locker)return;
		locker=true;
		locktime=timeToLock;
		timer=0;
		
	}
	public void pickUp(GameObject target)
	{
		if(locker)return;
		ObjectScript os=target.GetComponent<ObjectScript>();
		if(os==null) return;
		stuffInHand=target;
		holdStuff=1;
		os.startFollow();
		anim.SetInteger("holdStuff",0);
		lockPlayerAction(100);
	}
	public void throwToward()//0 for forward, 1 for backward
	{
		if(locker)return;
		AudioController.Play("Throw");
		ObjectScript os=stuffInHand.GetComponent<ObjectScript>();
		if(os==null) return;
		int direction=0;
		os.beThrown(out direction);
		stuffInHand=null;
		holdStuff=0;
		anim.SetInteger("holdStuff",direction+1);
		lockPlayerAction(100);
	}
	
	public void dodge(int direction)
	{
		if(locker)return;
		anim.SetBool("Dodging",true);
		anim.SetInteger("targetDir",direction);
		Debug.Log(direction.ToString());
		lockPlayerAction(100);
	}
	
	private void GoDie(int max)
	{
		if(deadTimer>0)return;
		deadTimer=1;
		deadMaxTime=max;
		
	}
	
	public void push(GameObject target, int height, int direction)//height:0 low, 1 med, 2 high; dir:0 fwd, 2 bkwd, 1:left, 3 right
	{
		if(locker)return;
		ObjectScript os=target.GetComponent<ObjectScript>();
		if(os==null) return;
		os.beKnocked(height,direction);

		anim.SetInteger("targetHeight",height);
		anim.SetInteger("targetDir",direction);
		anim.SetBool("Pushing",true);
		
		lockPlayerAction(70);
	}
	
	public void getBitten()
	{
		if(isBleeding) return;
		if(isInvulnerable)return;
		AudioController.Play("ZombieChow");
		AudioController.Play("Pain");
		AudioController.Play("Dying");
		AnimatorStateInfo current=anim.GetCurrentAnimatorStateInfo(0);

		if((current.nameHash==dogdgeState1) || (current.nameHash==dogdgeState2))
			return;
		isBleeding=true;
		setBleed(true);
		
		infectTimer=1;

	}
	
	public bool showDeathBar()
	{
		return isBleeding;
	}
	public void tryInject()
	{
		if(locker)return;
		if(!isBleeding)return;
		if(injectRemain==0) return;
		AudioController.Play("InjectNeedle");
		AudioController.Stop ("Dying");
		anim.SetBool("Injecting",true);
		setInvulnerable(true);
		infectTimer=0;
		setBleed(false);
		isBleeding=false;
		injectRemain--;
		lockPlayerAction(150);
		
		ExhaustedScript es=GetComponent<ExhaustedScript>();
		es.currentExhaustedVal+=0.3f*es.totalExhaustedVal;
				
	}
	public void playerDie()
	{
		//Application.LoadLevel(4);
	}
	public float getDyingProgress()
	{
		return (infectTimer+0.0f)/(infectMaxTime+0.0f);
	}
	
	public int getInjectNum()
	{
		return injectRemain;
	}
	
	private void resetCollider()
	{
		col.height=defaultHeight;
		col.center=new Vector3(0,defaultPosy,0);
	}
	
	private void knockNearestZombie()
	{
		ExhaustedScript es=GetComponent<ExhaustedScript>();
		if(es.currentExhaustedVal/es.totalExhaustedVal<0.2f) return;
		es.currentExhaustedVal-=0.2f*es.totalExhaustedVal;
		GameObject[] zombies=GameObject.FindGameObjectsWithTag("Zombie");
		float mindist=5;


		foreach(GameObject zombie in zombies)
		{
			float temp=Vector3.Distance(zombie.transform.position,transform.position);
			if(temp>mindist) continue;
			ZombieControl zc=zombie.GetComponent<ZombieControl>();
			zc.knockBack();
			AudioController.Play("KnockBack");
		}

	}
	
	//This is for calls to the Audiocontroller
	public void Update(){
		if(isWalking && !playingHeartBeat){
			AudioController.Play("HeartbeatSlow");
			
			playingHeartBeat = true;	
			if(playingHeartBeatFast){
				playingHeartBeatFast = false;
				AudioController.Stop ("HeartbeatFast");
				AudioController.Stop ("Run");
			}
		}
		if(!isWalking && !playingHeartBeatFast){
			
			playingHeartBeatFast = true;
			playingHeartBeat = false;
			AudioController.Stop ("HeartbeatSlow");
			AudioController.Play ("HeartbeatFast");
			AudioController.Play ("Run");
		}
		
	}
	
	public bool holdingStuff()
	{
		return holdStuff>0;
	}
}

