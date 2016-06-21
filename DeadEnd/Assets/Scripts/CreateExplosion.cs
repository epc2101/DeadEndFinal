using UnityEngine;
using System.Collections;

/* This script is to create explosions around the player either before or after the player passes a location.  
 * Attach this script to a plane and make the plane a trigger.  
 *  Assign the game objects that you want to create the explosion around to objectsToExplode.
 * Use the Detonator objects to create explosions.  
 *  If you would like to specify your own prefab, match the explosions with the objsToExplode, 
 * otherwise a default will be used
 *  The explosion will be sourced from those locations & a fire will be created after. 
 */
public class CreateExplosion : MonoBehaviour {
	public GameObject [] objectsToExplode; 
	public GameObject [] explosions; 
	public GameObject fire; 
	public GameObject smoke; 
	public bool explodeOnlyOnce = true; 
	public bool useFlareAsTrigger = false; 
	
	bool startFire = false; 
	float startFireTimer = 0.0f; 
	bool hasExploded = false; 
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (startFire) loadFireAndSmoke(); 
	}
	
	//Creates an explosion when the player passes through the obj 
	void OnTriggerEnter(Collider collision) {
		bool goBoom = false; 
		if (useFlareAsTrigger == false && collision.gameObject.CompareTag("Player")) goBoom = true; 
		else if ( useFlareAsTrigger == true && collision.gameObject.CompareTag("Flare")) goBoom = true;
		
		if (goBoom) {
			//Check if we want to explode only the first time the trigger is hit or each time
			if (explodeOnlyOnce == true && hasExploded == true) return; 
			else if (explodeOnlyOnce == true && hasExploded == false) hasExploded = true; 
			
			AudioController.Play("NearExplosionB"); 
			//if (explosionNoise != null) 
			//	AudioSource.PlayClipAtPoint(explosionNoise, collision.gameObject.transform.position); //explosionNoise
			
			//Load the default explosion 
			if (explosions.Length < 1) {
				foreach (GameObject obj in objectsToExplode) {
					GameObject exp = Resources.Load("Detonator-Base") as GameObject;
					Instantiate(exp, obj.transform.position, Quaternion.identity);
					callExplosionScripts(obj);
				}
			}
			//Load the custom explosions
			else {
				for (int i = 0; i < objectsToExplode.Length; i++) {
					//Make sure that there is 1 explosion per obj
					if (i < (explosions.Length-1) && explosions[i] != null) {	
						Instantiate(explosions[i], objectsToExplode[i].transform.position, Quaternion.identity); 
					} else {
						GameObject exp = Resources.Load("Detonator-Base") as GameObject;
						Instantiate(exp, objectsToExplode[i].transform.position, Quaternion.identity); 
					}
					callExplosionScripts(objectsToExplode[i]);
				}
				
			}
			startFire = true; 
		}
	}
	
	//Loads fire & smoke after the explosion
	void loadFireAndSmoke() {
		startFireTimer += Time.deltaTime; 
		float timeToAddFire = 3.5f;
		if (startFireTimer > timeToAddFire) {
		foreach (GameObject obj in objectsToExplode) {
				if (obj != null) {
					if (startFireTimer > timeToAddFire) {
						if (fire != null) 
							Instantiate(fire, obj.transform.position, Quaternion.identity);	
						if (smoke != null) 
							Instantiate(smoke, obj.transform.position, Quaternion.identity);
						startFire = false;
				}
			}
		}
		}
	}
	
	//Calls the scripts that makes cooler explosions if they are added
	//Detonator calls all other linked scripts
	void callExplosionScripts(GameObject obj){
		Detonator d = obj.GetComponent<Detonator>(); 
		if ( d != null) d.Explode(); 
	}
}
