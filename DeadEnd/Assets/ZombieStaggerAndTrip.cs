using UnityEngine;
using System.Collections;

public class ZombieStaggerAndTrip : MonoBehaviour {

	// Use this for initialization
	private Animator anim;
	static int beingStaggered=Animator.StringToHash("Base Layer.ZombieStagger");
	private GameObject player;
	public Texture2D mouseOverTexture;
	
	void Start () {
		anim=gameObject.GetComponent<Animator>();
		player=GameObject.Find("carl");
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		AnimatorStateInfo current=anim.GetCurrentAnimatorStateInfo(0);
		if(current.nameHash==beingStaggered)
			anim.SetBool("IsStaggered",false);
	}
	
	void OnTriggerEnter(Collider collision)
	{
		
		//Debug.Log("Hi!");
		//
		ObjectScript os=collision.collider.GetComponent<ObjectScript>();
		if(os==null) return;
		
		if(collision.collider.rigidbody.velocity.magnitude<0.01f)return;
		Vector3 v1=transform.position-player.transform.position;
		Vector3 v2=gameObject.transform.forward;
		v1.y=0;v2.y=0;
		float angle=Vector3.Angle(v1,v2);
		if(Mathf.Abs(angle-180)<90)
		{
			Debug.Log("Hit from front!");
			anim.SetBool("IsStaggered",true);
		}
		else
		{
			Debug.Log("Hit from back!");
			anim.SetBool("IsStaggered",true);
		}
	}
	
	void OnMouseEnter()
	{
		Cursor.SetCursor(mouseOverTexture,Vector2.zero,CursorMode.Auto);
	}
	
	void OnMouseExit()
	{
		Cursor.SetCursor(null,Vector2.zero,CursorMode.Auto);
	}
}
