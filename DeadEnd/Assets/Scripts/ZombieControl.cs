using UnityEngine;
using System.Collections;

public class ZombieControl : MonoBehaviour {
	
	enum Behaviors { idle, chase, attack, eat};
	string animationStr;
	public bool usePursuitScript = false; 	
	
	//**************************************************************
	//Variables to play around w/ to get better results
	//TODO - modify these on your individual zombies to improve results!!!!
	public float timeToHungerChange = 40.0f; 
	public float timeToStopChasing = 8.0f; //Tells how long to chase for after player has left line of sight
	public int hungerLevel = 1; //Max is 5
	public float baseSpeed = 2.0f;  
	public float smellRadius = 5.0f; 
	public float visionDistance = 60.0f;
	//********************************************************
	public Vector3 lastPosition;
	//These may not need to be public
	public int behavior; 
 
	
	float hungerChangeTimer; 
	float justSpottedPlayerTimer; //Use to make initial zombie war cry
	public bool hasSeenPlayer; //Used to turn off chase after lost player
	public float hasSeenPlayerTimer; //Public for debugging purposes
	float distanceToPlayer; 
	NavMeshAgent nA;
	NavMeshSimpleFollow navMeshScript; 
	GameObject player; 
	
	//Controls for the animations
	protected Animator avatar;
	private float SpeedDampTime = .25f;	
	private float DirectionDampTime = .25f;	
	int randIdle; 
	
	
	//Handles turning on/off the zombie ai 
	bool forceAIoff = false;
	float turnBackOnTimer = 0.0f; 
	public bool allowDisableZombieRender = false;
	public float distanceToEnableRender = 100.0f; 
	bool zombieIsTrapped = false; 
	//Chasing turn on/off
	public float stopChasingTime = 30.0f;
	//TODO- make private
	float stopChasingTimer = 0.0f; 
	public float allowChaseTime = 15.0f; 
	float allowChaseTimer = 0.0f; 
	bool canChase = true; 
	
	//For Zombie Audio
	public bool chowing;
	
	//For stunning
	public bool stunned;
	public float stunDuration;
	public float stunStart;
	public bool beginTheStun;
	
	// Use this for initialization
	void Start () {
		lastPosition = transform.position; 
		nA = GetComponent<NavMeshAgent>();
		navMeshScript = GetComponent<NavMeshSimpleFollow>(); 
		player = GameObject.Find("carl");
		avatar = GetComponent<Animator>();
		
		//Initial values for the zombie
		behavior = (int) Behaviors.idle; 
		
		distanceToPlayer = Mathf.Infinity;
		hungerChangeTimer = 0.0f; 
		justSpottedPlayerTimer = 0.0f; 
		hasSeenPlayer = false; 
		//smellRadius = 7.0f; 
		
		//Set initial speed for two speed control
		
		nA.speed = baseSpeed;
		
		//Pick 1 of 3 idle animations to start from
		UnityEngine.Random.seed = (int) Time.time; 
		randIdle = UnityEngine.Random.Range(0, 2);
		avatar.SetFloat("RandIdle", randIdle);
		
		chowing = false;
		
		//Stunning behavior
		stunned = false;
		beginTheStun = false;
		stunDuration = 20.0f;
		stunStart = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
		
		//Tests if the zombie has been stunned
		if(beginTheStun){
			turnOffZombieLogic();
			stunStart = 0.0f;
			stunned = true;
			beginTheStun = false;
		}
		if(stunned){
			stunStart += Time.deltaTime;
			if(stunStart >= stunDuration){
				stunned = false;
				zombieIsTrapped = false;
			}	
		}
		
		
		//Update hunger level based on timer
		hungerChangeTimer += Time.deltaTime; 
		if (hungerChangeTimer > timeToHungerChange && hungerLevel < 5) {
			hungerLevel += 1; 
			nA.speed += 0.5f; 
			hungerChangeTimer = 0.0f; 
		}
		
		//Don't update AI if the zombie has been trapped
		if (zombieIsTrapped == true) return; 
		
		//Don't update AI if the zombie was chasing & is in "time out"
		if (!canChase) {
			if (checkAllowChase() == false)
				return; 
		}
		//Disable if too far & can't see & option to disable is selected
		disableFarZombies(); 
		
		//If we have turned the AI & renderer off, make sure the player isn't getting close
		if (forceAIoff == true) {
			turnBackOnTimer += Time.deltaTime; 
			if (turnBackOnTimer > 20.0f) {
				checkIfZombieOn(); 
			}
			return; 
		}
		
		//Check if can see player & act based on distance
		if (inLineOfSight()) {
			hasSeenPlayer = true; 
			if (distanceToPlayer < 3) {
				behavior = (int) Behaviors.attack;
				if(!chowing) {
					//AudioController.Play("ZombieChow");
					chowing = true;
				}
			}
			else {
				behavior = (int) Behaviors.chase;
				chowing = false;
			}
			hasSeenPlayerTimer = 0.0f; 
		} else if (hasSeenPlayer) {
			//Check to see if player just left zombie's line of sight
				hasSeenPlayerTimer += Time.deltaTime; 
				if (hasSeenPlayerTimer < timeToStopChasing) {
					behavior = (int) Behaviors.chase; 
				} else 	{
					hasSeenPlayer = false; 
					hasSeenPlayerTimer = 0.0f; 
					behavior = (int) Behaviors.idle; 
				}
		} else {
			//Check to see if the zombie can "smell" the player
			if (distanceToPlayer < smellRadius) {
				behavior = (int) Behaviors.chase; 
				hasSeenPlayer = true; 
			}
		}
		
		//Check if the zombie has been chasing the player for too long & force stop if so
		if (behavior == (int) Behaviors.chase)
			checkChaseTime(); 
	}
	
	public bool inLineOfSight() {
		distanceToPlayer = Vector3.Distance(player.transform.position, transform.position); 
		if (distanceToPlayer > visionDistance) return false; 
		if (behavior == (int) Behaviors.attack) return true;//Force true if attacking
		float fov = 90.0f;
		RaycastHit hit; 
		Vector3 playerPos = player.transform.position;
		Vector3 rayDir = playerPos - transform.position; 
		
	    /*if (Vector3.Angle(rayDir, transform.forward) <= fov) {
			if (Physics.Linecast(transform.position, playerPos, out hit)) {
			//if (Physics.Linecast(transform.position, transform.position + transform.forward *80.0f, out hit)) {
				Debug.Log("Hit: " + hit.collider.gameObject.name); 
				if (hit.collider.gameObject.name == player.name ||
					hit.collider.gameObject.name.Equals("Char_CD")) {
					return true;
				}
			}
		
	    }*/
		if (Vector3.Angle(rayDir, transform.forward) <= fov) {
			RaycastHit[] hits;
			hits = Physics.RaycastAll(transform.position, rayDir, visionDistance); 
			foreach (RaycastHit h in hits) {
				if (h.collider.gameObject.layer == 10 || h.collider.gameObject.layer == 9) {
					Vector3 wallPos = h.collider.gameObject.transform.position; 
					if (distanceToPlayer > Vector3.Distance(wallPos, transform.position)) 
						return false; 
				}
			}
			return true; 
			
		}
		
		
		return false; 		
	}
	
	//Make sure that char controller collisions don't cause weird behavior
	//public float pushPower = 2.0F;
    void OnControllerColliderHit(ControllerColliderHit hit) {
		if(hit.collider.GetType() == typeof(CharacterController) || hit.gameObject.CompareTag("Player"))  {
			transform.position = lastPosition;
		}		
		//Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
		//transform.Translate (-pushDir * pushPower);
    }
	
		public void UpdateAnimations() {
		/*if (zCtrl.behavior == (int) Behaviors.idle)
			avatar.SetBool("IsIdle", true);
		else 
			avatar.SetBool("IsIdle", false);*/

			if (avatar && player)
			{		
			if (hasSeenPlayer == true && avatar.GetCurrentAnimatorStateInfo(0).nameHash!=Animator.StringToHash("Base Layer.ZombieStagger")) {
				if(Vector3.Distance(player.transform.position,avatar.rootPosition) > 1.8f)
				{
					avatar.SetFloat("Speed",2,SpeedDampTime, Time.deltaTime);
					avatar.SetBool("IsAttacking", false);
					/*if (transform.rigidbody.velocity.magnitude > 2) 
						avatar.SetFloat("Speed",1.5f,SpeedDampTime, Time.deltaTime);
					else
						avatar.SetFloat("Speed",0.5f,SpeedDampTime, Time.deltaTime);
					*/
					Vector3 curentDir = avatar.rootRotation * Vector3.forward;
					Vector3 wantedDir = (player.transform.position - avatar.rootPosition).normalized;
		
					if(Vector3.Dot(curentDir,wantedDir) > 0)
					{
						avatar.SetFloat("Direction",Vector3.Cross(curentDir,wantedDir).y,DirectionDampTime, Time.deltaTime);
					}
					else
					{
	            		avatar.SetFloat("Direction", Vector3.Cross(curentDir,wantedDir).y > 0 ? 1 : -1, DirectionDampTime, Time.deltaTime);
					}
				} else if (Vector3.Distance(player.transform.position,avatar.rootPosition) > 1){
					//Attack start

	            	avatar.SetFloat("Speed",3,SpeedDampTime, Time.deltaTime);
				} else
				{
					//Attack
					m_Controller mc=player.GetComponent<m_Controller>();
					if (mc != null) mc.getBitten();
	            	avatar.SetFloat("Speed",0,SpeedDampTime, Time.deltaTime);
					avatar.SetBool("IsAttacking", true); 
					
					
				}
			} else if (navMeshScript.hasHeardObject) {
				avatar.SetFloat("Speed",2,SpeedDampTime, Time.deltaTime);
				avatar.SetBool("IsAttacking", false);
			} else {	
				//Idle animation
				avatar.SetFloat("Speed", 0);
				avatar.SetFloat("RandIdle", randIdle);
				
			}
		}
		}
	
	public void setTarget(GameObject newTarget) {
		player = newTarget; 
	}
	
	public void knockBack() {
		avatar.SetBool("IsStaggered",true);	
		AudioController.Play ("ZombieHit");

		Vector3 force = transform.position - player.transform.position;
		force.y=0;
		force=force.normalized*1100.0f;
		transform.rigidbody.AddForce(force); 
	}
	
	public void turnZombieOffOn(bool turnOff) {
		forceAIoff = turnOff;
		GameObject geo = transform.FindChild("Zombie_Geo").gameObject; 
		if (turnOff) {
			hasSeenPlayer = false; 
			hasSeenPlayerTimer = 0.0f; 
			behavior = (int) Behaviors.idle; 
			geo.renderer.enabled = false; 
			//r.enabled = false; 
		} else {
			geo.renderer.enabled = true; 
		}
	}
	
	public void checkIfZombieOn() {
		distanceToPlayer =  Vector3.Distance(player.transform.position, transform.position);
		if (distanceToPlayer < distanceToEnableRender ) {
			turnZombieOffOn(false); 
			turnBackOnTimer = 0.0f; 
		}
	}
	
	void disableFarZombies() {
		if (allowDisableZombieRender) {
			//Check if the player can't see & is far	
			RaycastHit hit; 
			Physics.Linecast(transform.position, player.transform.position, out hit);
			distanceToPlayer =  Vector3.Distance(player.transform.position, transform.position);
			if (hit.collider != null) {
				if (hit.collider.CompareTag("Player") || distanceToPlayer < distanceToEnableRender) return; 
				turnZombieOffOn(true); 
			}			
		}
	}
	
	public void turnOffZombieLogic() {
			hasSeenPlayer = false; 
			hasSeenPlayerTimer = 0.0f; 
			behavior = (int) Behaviors.idle; 
			zombieIsTrapped = true; 
	}
	
	public bool ZombieIsTrapped()  {
		return zombieIsTrapped; 	
	}
	
	//Checks to see if the zombie has been chasing for too long & forces to stop chasing
	void checkChaseTime() {
		stopChasingTimer += Time.deltaTime; 
		if (stopChasingTimer > stopChasingTime) {
			hasSeenPlayer = false; 
			hasSeenPlayerTimer = 0.0f; 
			behavior = (int) Behaviors.idle; 
			stopChasingTimer = 0.0f; 
			canChase = false; 
		}
	}
	
	//Checks if the zombie has stopped chasing for long enough and turns back on the AI if it has
	bool checkAllowChase() {
		allowChaseTimer += Time.deltaTime; 
		if (allowChaseTimer > allowChaseTime || Vector3.Distance(transform.position, player.transform.position) < 3.0f) {
			canChase = true; 
			allowChaseTimer = 0.0f; 
			return true; 
		}
		return false; 
	}
}
