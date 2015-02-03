using UnityEngine;
using System.Collections;

public class Start_Script : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Z)){
			Application.LoadLevel(1);
		}
		if(Input.GetKeyDown(KeyCode.X)){
			Application.LoadLevel(2);
		}
	}
}
