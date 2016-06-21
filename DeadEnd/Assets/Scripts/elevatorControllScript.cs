using UnityEngine;
using System.Collections;

public class elevatorControllScript : MonoBehaviour {

	// Use this for initialization
	private GameObject door;
	
	public float height=10.0f;
	private bool closed;
	public bool lowFloor=true;
	
	void Start () {
		door=transform.Find("elevator_door_1").gameObject;
		
		//closed=false;
		//if(!closed)
		//DoorOpen(0);
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void FixedUpdate()
	{
	}
	
	void OnTriggerEnter(Collider cld)
	{
		if(!cld.CompareTag("Player")) return;
		ElevatorMove(1.0f);
	}
	public void ElevatorMove(float delay)
	{
		if(lowFloor)ElevatorAscend(delay);
		else ElevatorDescend(delay);
	}
	public void ElevatorAscend(float delay)
	{
		if(!lowFloor) return;
		lowFloor=false;
		float nowtime=delay;
		if(!closed)
		{
			DoorClose(nowtime);
			nowtime+=1.0f;
		}
		iTween.MoveBy(gameObject,iTween.Hash("y",height,"delay",nowtime,"time",height/2.0f,"easeType","linear"));
		nowtime+=height/2.0f;
		DoorOpen(nowtime);
	}
	public void ElevatorDescend(float delay)
	{
		if(lowFloor)return;
		lowFloor=true;
		float nowtime=delay;
		if(!closed)
		{
			DoorClose(nowtime);
			nowtime+=1.0f;
		}
		iTween.MoveBy(gameObject,iTween.Hash("y",-height,"delay",nowtime,"time",height/2.0f,"easeType","linear"));
		nowtime+=height/2.0f;
		DoorOpen(nowtime);
	}
	public void DoorToggle(float delay)
	{
		if(closed) DoorOpen(delay);
		else DoorClose(delay);
	}
	public void DoorOpen(float delay)
	{
		if(!closed)return;
		closed=false;
		iTween.MoveBy(door,iTween.Hash("z",1.6f,"delay",delay));
		
	}
	public void DoorClose(float delay)
	{
		if(closed)return;
		closed=true;
		iTween.MoveBy(door,iTween.Hash("z",-1.6f,"delay",delay));
	}
}
