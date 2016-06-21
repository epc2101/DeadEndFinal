using UnityEngine;
using System.Collections;

public class DetectObjScript : MonoBehaviour {
	
	private GameObject player;
	
	private float detectRadius=2.0f;
	
	
	// Use this for initialization
	void Start () {
		player=gameObject;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void TryInteract(bool slow)
	{
		int dir=0;
		float mindist=10000000000;
		Collider c=null;
		
		
		
		m_Controller mc=player.GetComponent<m_Controller>();
		if(mc.activeObj!=null && Vector3.Distance(mc.activeObj.transform.position,player.transform.position)<detectRadius*1.5f)
		{
			c=mc.activeObj.collider;
		}
		else
		{
			Collider[] colls=Physics.OverlapSphere(gameObject.transform.position,detectRadius*2.0f);
			if(colls.Length==0) return;
			foreach(Collider now in colls)
			{
				if (now==this.collider || now.name=="carl" || now.name=="Terrain") continue;
				if(now.gameObject.GetComponent<ObjectScript>()==null)continue;
				//if(!slow && (now.gameObject.GetComponent<ObjectScript>()).Size==0)continue;
				float tempdist=Vector3.Distance(gameObject.transform.position,now.transform.position);
				if(tempdist<mindist){mindist=tempdist;c=now;}
			}
		}
		if(c!=null)
		{

			
			float orientationAngle=player.rigidbody.rotation.eulerAngles.y;
			orientationAngle*=Mathf.PI/180.0f;
			orientationAngle-=Mathf.PI/4.0f;
			Vector3 s1=new Vector3(Mathf.Sin(orientationAngle),0,Mathf.Cos(orientationAngle));
			orientationAngle-=Mathf.PI/2.0f;
			Vector3 s2=new Vector3(Mathf.Sin(orientationAngle),0,Mathf.Cos(orientationAngle));
			Vector3 dist=c.gameObject.transform.position-player.transform.position;
			dist.y=0;
			
			
			bool dot1=Vector3.Dot(dist,s1)>0;
			bool dot2=Vector3.Dot(dist,s2)>0;
			if(dot1 && dot2) dir=1;
			else if (!dot1 && dot2) dir=2;
			else if (dot1 && !dot2) dir=0;
			else dir=3;
			

			
			ObjectScript os=c.gameObject.GetComponent<ObjectScript>();
			if(slow && Input.GetAxis("Vertical")>-0.1f && os.Size==0) mc.pickUp(c.gameObject);
			else mc.push(c.gameObject,os.Size,dir);
		//	Debug.Log(c.name);
		}
	}
	public int tryDodge()
	{
		int dir=0;
		float mindist=10000000000;
		Collider c=null;
		Collider[] colls=Physics.OverlapSphere(gameObject.transform.position,detectRadius);
		//if(colls.Length==0) return 0;
		
		m_Controller bcs=player.GetComponent<m_Controller>();
		float orientationAngle=player.rigidbody.rotation.eulerAngles.y;
		orientationAngle*=Mathf.PI/180.0f;
		orientationAngle-=Mathf.PI/2.0f;
		Vector3 s2=new Vector3(Mathf.Cos(orientationAngle),0,Mathf.Sin(orientationAngle));
		
		foreach(Collider now in colls)
		{
			if (!now.gameObject.CompareTag("Zombie")) continue;
			if(transform.position.y-now.gameObject.transform.position.y>2.0f) continue;
			
			float tempdist=Vector3.Distance(gameObject.transform.position,now.transform.position);
			if(tempdist<mindist)
			{
				mindist=tempdist;
				c=now;
				

				Vector3 dist=c.gameObject.transform.position-player.transform.position;
				dist.y=0;
				dir=(Vector3.Dot(s2,dist)>0)?1:2;
			}
		}
		
		return dir;
	}
	void FixedUpdate()
	{
		
	}
}
