using UnityEngine;
using System.Collections;

public class MouseDetectScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	
	public bool getMousePick(out Vector3 position)
	{
		position=Vector3.zero;
		float mindist=10000000;
		GameObject target=null;
		Vector3 targetPos=Vector3.zero;
		RaycastHit[] hits;
		Ray r=Camera.main.ScreenPointToRay(Input.mousePosition);
		hits=Physics.RaycastAll(r);
		if(hits.Length>0)
		{
			foreach(RaycastHit hit in hits)
			{
				if(hit.collider.gameObject.CompareTag("Player")) continue;
				float temp=Vector3.Distance(Camera.main.transform.position,hit.collider.gameObject.transform.position);
				if(temp<mindist)
				{
					mindist=temp;
					target=hit.collider.gameObject;
					targetPos=hit.point;
				}
			}
			position=targetPos;
			return true;
			Debug.Log(targetPos.ToString());
		}
		return false;
		
	}
	void FixedUpdate () {
		
		
	}
}
