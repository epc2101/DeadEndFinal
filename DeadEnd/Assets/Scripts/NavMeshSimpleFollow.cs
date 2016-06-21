using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]  
public class NavMeshSimpleFollow : MonoBehaviour {
	NavMeshAgent agent;
	public GameObject player;
	ZombieControl zCtrl;
	enum Behaviors { idle, chase, attack, eat};
		
	public bool hasHeardObject = false; 
	protected Animator avatar;
	protected CharacterController controller;
	
	private float SpeedDampTime = .25f;	
	private float DirectionDampTime = .25f;	
		
	// Use this for initialization
	void Start () {
		agent = GetComponent<NavMeshAgent>(); 
		player = GameObject.FindGameObjectWithTag("Player"); 
		zCtrl = GetComponent<ZombieControl>(); 
		avatar = GetComponent<Animator>();
		controller = GetComponent<CharacterController>();	
		avatar.speed = 1 + UnityEngine.Random.Range(-0.1f, 0.1f);				
	}
	
	// Update is called once per frame
	void Update () {
		zCtrl.UpdateAnimations();	
	}
	
	void OnAnimatorMove()
	{
		if (zCtrl.hasSeenPlayer) {
			if(avatar.GetCurrentAnimatorStateInfo(0).nameHash==Animator.StringToHash("Base Layer.ZombieStagger")) {
				agent.Stop(true);
				
			} else if (avatar.GetBool("IsAttacking")) {
				agent.Stop (true);	
				transform.rotation = Quaternion.LookRotation(player.transform.position - transform.position); 	
			} else {
				if (zCtrl.ZombieIsTrapped())
					agent.Stop(true); 
				else
					updatePosition(); 
			}
		}
		else if (hasHeardObject) {
			updatePosition(); 	
			
		} 
		else {
			agent.Stop (true);	
		}
	}
	
	
	void updatePosition() 
	{
		Vector3 goToPos = player.transform.position;
		if (goToPos.y < 0) 
				goToPos.y = 0; 
			
		agent.destination = goToPos;
		agent.Stop (false);	
		agent.Resume(); 
		transform.rotation = avatar.rootRotation;
	}

}