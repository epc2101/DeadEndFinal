using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[RequireComponent(typeof (Collider))]
[RequireComponent(typeof (Rigidbody))]

public class ObjectScript : MonoBehaviour {
	private GameObject player;
	private bool isFollowing;
	private bool isThrown;
	private bool lastIsFollowing;
	
	private bool isKnocked;
	
	private int continuousFollowTimer;			//the time before the object is actually added force
	private int continuousFollowTime=45;
	
	private int height, direction;

	private GameObject target;
	private Vector3 targetPos;
	
	
	public bool fragile=false;
	public int Size = 0; 		//0 for small, 1 for med, 2 for big
	
	private Texture2D mouseOverTexture;
	
	//Zombie distractions
	public bool objectShouldDistract = true; 
	public float distractDistance = 18.0f; 
	public float distractChaseZDist = 12.0f; 
	private GameObject closestZombie = null; //TODO - change this to all zombies in a range
	private bool hasThrownObj = false;
	private bool shouldLookForPlayer = false; 
	private List<GameObject> zombiesInRange; 
	private List<bool> zombiesWereChasing; 
	private float lookForPlayerTimer = 0.0f; 
	public float minTimeBeforeHuntPlayer = 10.0f; 
	public float maxTimeBeforeHuntPlayer = 50.0f; 
	//private Dictionary<GameObject, bool> zombiesInRange; 
	
	public bool isFlashbang;
	
	// Use this for initialization
	void Start () {
		lastIsFollowing=false;
		isFollowing=false;
		isThrown=false;
		player=GameObject.Find("carl");
		if(player==null) player=GameObject.Find("CarlPerfab");
		target=null;
		try {mouseOverTexture=((m_Controller)player.GetComponent<m_Controller>()).objCursor;}catch{}
		zombiesInRange = new List<GameObject>(); 
		zombiesWereChasing = new List<bool>(); 
		
		Flashbang banger = gameObject.GetComponent<Flashbang>();
		if(banger!=null){
			isFlashbang = true;	
		}
		else {
		isFlashbang = false;
		}
		
	}
	
	// Update is called once per frame
	public void startFollow()
	{
		if(isFollowing)return;
		Debug.Log(gameObject.name+"is Following Player");
		isFollowing=true;
	}
	
	public void beThrown(out int direction)//0 for forward, 1 for backward
	{
		direction=0;
		if(isThrown) return;
		//Debug.Log(gameObject.name+" is Thrown Away");
		getMousePick(out targetPos, out target);
		direction=0;
		isThrown=true;
		hasThrownObj = true; 
	}
	
	public void beKnocked(int h, int d)//height : 0 for small(kick), 1 for med(push), 2 for high(shoulder)
													//direction: 0 for fwd, 2 for bwd, 1 for left, 3 for right
	{
		Debug.Log(gameObject.name+" is Knocked Down");
		isKnocked=true;
		height=h;
		direction=d;
		AudioController.Play("CrateImpact");
		
	}
	
	private GameObject findNearestZombie(out int direction)
	{
		GameObject[] zombies=GameObject.FindGameObjectsWithTag("Zombie");
		float mindist=50;
		GameObject result=null;
		direction=0;
		foreach(GameObject zombie in zombies)
		{
			float angle=Vector3.Angle(zombie.transform.position-player.transform.position,player.transform.forward);
			angle=Mathf.Abs(angle);
			//if(angle<60|| angle>300)
			{
				float temp=Vector3.Distance(zombie.transform.position,player.transform.position);
				if(temp<mindist)
				{
					mindist=temp;
					result=zombie;
					if(angle<90||angle>270) direction=0;
					else direction=1;
				}
				
			}
		}
		return result;
	}
	
	public bool getMousePick(out Vector3 position, out GameObject targetZombie)
	{
		position=Vector3.zero;
		float mindist=10000000;
		targetZombie=null;
		Vector3 targetPos=Vector3.zero;
		RaycastHit[] hits;
		Ray r=Camera.main.ScreenPointToRay(Input.mousePosition);
		hits=Physics.RaycastAll(r);
		if(hits.Length>0)
		{
			foreach(RaycastHit hit in hits)
			{
				if(hit.collider.gameObject.CompareTag("Player") || hit.collider.gameObject==gameObject) continue;
				float temp=Vector3.Distance(Camera.main.transform.position,hit.collider.gameObject.transform.position);
				if(temp<mindist)
				{
					mindist=temp;
					targetZombie=hit.collider.gameObject;
					targetPos=hit.point;
				}
			}
			position=targetPos;
			Debug.Log(targetZombie.name);
			return true;
			
		}
		return false;
		
	}
	void OnMouseEnter()
	{
		if(isFollowing) return;
		if(isThrown)return;
		m_Controller mc=player.GetComponent<m_Controller>();
		if(mc.holdingStuff())return;
		Cursor.SetCursor(mouseOverTexture,new Vector2(0.5f,5.5f),CursorMode.Auto);
		
		try {
		mc.activeObj=gameObject;
		} catch{}
	}
	
	void OnMouseExit()
	{
		Cursor.SetCursor(null,Vector2.zero,CursorMode.Auto);
	}
	void FixedUpdate()
	{
		//Check if object was thrown & zombie has reached the obj
		if (shouldLookForPlayer) lookForPlayer(); 
				
		if(continuousFollowTimer>0)
		{
			continuousFollowTimer++;
			if(isFollowing)
			{
				gameObject.transform.localPosition=Vector3.zero;
			}
			if(continuousFollowTimer>=continuousFollowTime)
			{
				if(isThrown)
				{
					//TODO - make sure that we really want this
					followObjectIfAttacking(); 
					
					float orientationAngle=player.rigidbody.rotation.eulerAngles.y;
					orientationAngle*=Mathf.PI/180.0f;
					Vector3 force=new Vector3(Mathf.Sin(orientationAngle),0,Mathf.Cos(orientationAngle));
					
					
			//		if(targetzombie!=null)
					if(target!=null)
					{
			//			force=(targetzombie.transform.position-GameObject.Find("c_LeftHand").transform.position);
					//	if(target.CompareTag("Zombie"))	targetPos=target.transform.position;
						Debug.Log(target.name);
						force=(targetPos-GameObject.Find("c_LeftHand").transform.position);
						
						force=force.normalized;
						force.y+=0.05f;
					}
					force*=1500.0f*gameObject.rigidbody.mass;

					rigidbody.AddForce(force);
					gameObject.transform.parent=null;
					continuousFollowTimer=0;
					gameObject.rigidbody.useGravity=true;
					isFollowing=false;
					lastIsFollowing=false;
					isThrown=false;		
				}
				else if (isKnocked)
				{	
					
					Vector3 force=transform.position-player.transform.position;
					force=force.normalized;
					force*=10000.0f;
					
					GameObject targetzombie=findNearestZombie(out direction);
					if(targetzombie!=null)
					{
						force=targetzombie.transform.position-transform.position;
						force.y=0;
						force=force.normalized*10000.0f;
					}
					
					
					if(height==2) rigidbody.AddForceAtPosition(force,player.transform.position+new Vector3(0,0.5f,0));
					else if (height==1) rigidbody.AddForce(force);
					else
					{
						force/=15.0f;
						rigidbody.AddForce(force);
						
					}
					isKnocked=false;
					continuousFollowTimer=0;
				}
			}
		}
		if(isThrown)
		{
			if(continuousFollowTimer==0)
			{
				continuousFollowTimer=1;	
				continuousFollowTime=45;
			}
		}
		else if(isKnocked)
		{
			if(continuousFollowTimer==0)
			{

				continuousFollowTimer=1;
				continuousFollowTime=15;
			}
		}
		else if(isFollowing)
		{
			if(!lastIsFollowing)
			{
				collider.isTrigger=true;
				collider.rigidbody.useGravity=false;
				
				GameObject followedTarget =GameObject.Find("c_LeftHand");
				gameObject.transform.parent=followedTarget.transform;
			}
			gameObject.transform.localPosition=Vector3.zero;
			lastIsFollowing=true;
			
		}
		
		if(fragile)return;
		if(collider.isTrigger && !isFollowing)
		{
			if(Vector3.Distance(transform.position,player.transform.position)>3.0f)
			{
				gameObject.collider.isTrigger=false;
				
			}
			
		}
	}
	

	//*********************************************************************************************
	//Code to distract zombie or to prevent zombie from getting you
	private void OnCollisionEnter(Collision col) {
		
		if (!objectShouldDistract) return; 
		if (hasThrownObj) {
			AudioController.Play("Impact1");
			int dir = 10; 
			if(isFlashbang){
				Flashbang banger = gameObject.GetComponent<Flashbang>();
				banger.Detonate();
				findZombiesToFrag(banger.blastRadius);
				
				if (zombiesInRange.Count != 0) {
				
					foreach (GameObject z in zombiesInRange) {
					ZombieControl x = z.GetComponent<ZombieControl>();
					x.beginTheStun = true;
					}
				}
			} else {
				
				findZombiesInRange(false); 
				//TODO - change!
				if (zombiesInRange.Count != 0) {
					
					foreach (GameObject z in zombiesInRange) {
						NavMeshSimpleFollow nFollowScript = z.GetComponent<NavMeshSimpleFollow>(); 
						if (nFollowScript != null) {
							nFollowScript.player = this.gameObject; 
							nFollowScript.hasHeardObject = true; 
							//hasDistracted = true; 
						}
					}
				}
				hasThrownObj = false; 
				shouldLookForPlayer = true; 
			}
		}	
	}
	
	//Chase the object immediately if the zombies are attacking 
	private void followObjectIfAttacking() {
		//if (isThrown && continuousFollowTimer > continuousFollowTime) {
		//True gets only the zombies who have seen player in range
			findZombiesInRange(true); 
			foreach (GameObject z in zombiesInRange) {
				ZombieControl zctrl = z.GetComponent<ZombieControl>(); 
				if (!zctrl.inLineOfSight() ) {
					NavMeshSimpleFollow nFollowScript = z.GetComponent<NavMeshSimpleFollow>(); 
					if (nFollowScript != null) {
						nFollowScript.player = this.gameObject; 
						nFollowScript.hasHeardObject = true; 
						//hasDistracted = true; 
					}
					
				}
			}
		
		//hasThrownObj = false; 
		//shouldLookForPlayer = true; 
	}
	
	private void lookForPlayer() {
		lookForPlayerTimer += Time.deltaTime; 
		if (lookForPlayerTimer < minTimeBeforeHuntPlayer) return; 
		
		Vector3 correctedPos = transform.position; 
		if (transform.position.y < -1.0f) {
			correctedPos.y = -1.0f; 
			transform.position = correctedPos; 
		}
		int count = zombiesInRange.Count;
		for (int i = 0; i < zombiesInRange.Count; i++) {
			GameObject z = zombiesInRange[i]; 
			float distanceToObject = Vector3.Distance(z.transform.position, transform.position);
			float distanceToPlayer = Vector3.Distance(player.transform.position, z.transform.position); 
			if (distanceToObject < 2.0f || (distanceToPlayer < 3.0f && zombiesWereChasing[i] == false)) {
				removeZombieFromList(i, z);
				//TODO - ADD IN LOOK BOTH WAYS WHEN ZOMBIE GETS HERE
			}			
		}
		
		//If the zombies are taking forever to find the object, just look for the player again 
		if (maxTimeBeforeHuntPlayer > lookForPlayerTimer)  {
			for (int i = 0; i < zombiesInRange.Count; i++) {
				GameObject z = zombiesInRange[i]; 
				removeZombieFromList(i, z);	
			}
		}
		
		if (zombiesInRange.Count == 0) shouldLookForPlayer = false;
	}
	
	//Clears zombies from list
	private void removeZombieFromList(int i, GameObject z) {
		NavMeshSimpleFollow nFollowScript = z.GetComponent<NavMeshSimpleFollow>(); 
		nFollowScript.player = player; 
		nFollowScript.hasHeardObject = false;  
		zombiesInRange.RemoveAt(i); 
		zombiesWereChasing.RemoveAt(i); 		
	}
	
	//Finds all zombies that are in the player chasing range or w/in the object range
	private void findZombiesInRange(bool getOnlyChasers) {
		GameObject [] zombies = GameObject.FindGameObjectsWithTag("Zombie"); 
		//List<GameObject> zombiesInRange = new List<GameObject>(); 
		foreach (GameObject z in zombies) {
			ZombieControl zctrl = z.GetComponent<ZombieControl>(); 
			float distanceFromZombie = Vector3.Distance(z.transform.position, transform.position); 
			if (getOnlyChasers == false) {
				//Check if close enough to object
				if (distanceFromZombie < distractDistance && 
					zctrl.hasSeenPlayer == false) {
					zombiesInRange.Add(z); 
					zombiesWereChasing.Add(false); 
				}
			}
			//Check if a zombie is chasing the player & distract at any distance
			float distToPlayer = Vector3.Distance(z.transform.position, player.transform.position); 
			if (distToPlayer < distractChaseZDist &&
				zctrl.hasSeenPlayer) {
				zombiesInRange.Add(z);
				zombiesWereChasing.Add(true); 
			}
		}
	}
	
	private void findZombiesToFrag(float fragRange) {
		GameObject [] zombies = GameObject.FindGameObjectsWithTag("Zombie"); 
		//List<GameObject> zombiesInRange = new List<GameObject>(); 
		foreach (GameObject z in zombies) {
			ZombieControl zctrl = z.GetComponent<ZombieControl>(); 
			float distanceFromZombie = Vector3.Distance(z.transform.position, transform.position); 
			
			//Check if close enough to object
			if (distanceFromZombie < fragRange) {
				zombiesInRange.Add(z); 
				 
			}
		}
	}
	
}
