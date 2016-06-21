using UnityEngine;
using System.Collections;

public class OilFire : MonoBehaviour {
	public GameObject explosionPrefab;
	public GameObject shutOffPlane; 
	public bool blownUp;
	public bool ignite;
	public bool ignited;
	
	// Use this for initialization
	void Start () {
		blownUp = false;
		ignite = false;
		ignited = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(!blownUp && ignite){
			Instantiate(explosionPrefab, transform.position,Quaternion.identity);
			AudioController.Play("NearExplosionB");
			blownUp = true;
			ignited = true;
			ignite = true;	
		}
	}
	
	void OnTriggerEnter(Collider other){
		
		if (other.tag == "Flare" && !blownUp){
			Instantiate(explosionPrefab, transform.position,Quaternion.identity);
			//AudioController.Play("NearExplosionB");
			blownUp = true;
			ignited = true;
			ignite = true;
			turnOffZombiesOnPlane(); 
		}
	}
	
   void turnOffZombiesOnPlane() {
		if (shutOffPlane != null){
			Vector3 min = shutOffPlane.transform.collider.bounds.min;
			Vector3 max = shutOffPlane.transform.collider.bounds.max; 
			GameObject [] zombies = GameObject.FindGameObjectsWithTag("Zombie"); 
			foreach (GameObject z in zombies) {
				Vector3 pos = z.transform.position; 
				if (pos.x > min.x && pos.x < max.x && pos.z > min.z && pos.z < max.z) {
					ZombieControl zctrl = z.GetComponent<ZombieControl>(); 
					zctrl.turnOffZombieLogic(); 
				}
			}
		}
	}
			
}
