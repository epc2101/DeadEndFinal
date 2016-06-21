using UnityEngine;
using System.Collections;

public class OilFirestorm : MonoBehaviour {
	public GameObject[] fireArray;
	public bool igniteAll;
	public bool ignitedAll;
	public int numFires;
	
	// Use this for initialization
	void Start () {
		//fireArray = new Gam[numFires];
		igniteAll = false;
		ignitedAll = false;
	}
	
	// Update is called once per frame
	void Update () {
		//If the fire isn't going then ignite!
		if(!ignitedAll){
			for(int i = 0; i<fireArray.Length; i++){
				OilFire fire = fireArray[i].GetComponent<OilFire>() as OilFire;
				if(fire.ignite){
					igniteAll = true;	
				}
			}
			
			if(igniteAll){
				for(int i = 0; i<fireArray.Length; i++){
				OilFire fire = fireArray[i].GetComponent<OilFire>()as OilFire;
				if(!fire.ignite){
					fire.ignite = true;
					}
				}
				ignitedAll = true;
			}
			
		}
	}
}
