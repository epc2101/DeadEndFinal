using UnityEngine;
using System.Collections;

public class ZombieShutOffTrigger : MonoBehaviour {
	public GameObject shutOffPlane; 
	public bool turnOff = true;
	
	// Use this for initialization
	void Start () {
	
	}
	
	void OnTriggerEnter(Collider collision) {
		if (collision.gameObject.CompareTag("Player"))	{
			if (shutOffPlane != null){
				Vector3 min = shutOffPlane.transform.collider.bounds.min;
				Vector3 max = shutOffPlane.transform.collider.bounds.max; 
				GameObject [] zombies = GameObject.FindGameObjectsWithTag("Zombie"); 
				foreach (GameObject z in zombies) {
					Vector3 pos = z.transform.position; 
					if (pos.x > min.x && pos.x < max.x && pos.z > min.z && pos.z < max.z) {
						ZombieControl zctrl = z.GetComponent<ZombieControl>(); 
						zctrl.turnZombieOffOn(turnOff); 
					}
				}
			}
			turnOff = !turnOff; 
		}
	}
}
