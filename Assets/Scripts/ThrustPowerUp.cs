using UnityEngine;
using System.Collections;

public class ThrustPowerUp : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnTriggerEnter(Collider other) {
		// Ignore bullet collisions, as they are handled by Enemy
		if (other.gameObject.layer == LayerMask.NameToLayer ("Player")) {
			other.GetComponent<PE_Controller>().thrustEnabled = true;
			Destroy(this.gameObject);
		}
	}
}
