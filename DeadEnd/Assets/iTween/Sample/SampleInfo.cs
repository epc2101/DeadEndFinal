using UnityEngine;
using System.Collections;

public class SampleInfo : MonoBehaviour
{
	public bool shaked;
void OnGUI(){
		GUILayout.Label("iTween can spin, shake, punch, move, handle audio, fade color and transparency \nand much more with each task needing only one line of code.");
		GUILayout.BeginHorizontal();
		GUILayout.Label("iTween works with C#, JavaScript and Boo. For full documentation and examples visit:");
		if(GUILayout.Button("http://itween.pixelplacement.com")){
			Application.OpenURL("http://itween.pixelplacement.com");
		}
		GUILayout.EndHorizontal();
	}
	void Start()
	{
		shaked=false;
	}
	void FixedUpdate()
	{
		if(!shaked)tryShake();
	}
	public void tryShake()
	{
		iTween.ShakePosition(gameObject, new Vector3(0,1,0) , 1);
		shaked=true;
	}
}

