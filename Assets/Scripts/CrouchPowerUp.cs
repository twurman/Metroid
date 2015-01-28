using UnityEngine;
using System.Collections;

public class CrouchPowerUp : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other) {
		// Ignore bullet collisions, as they are handled by Enemy
		if (other.gameObject.layer == LayerMask.NameToLayer ("Player")) {
			other.GetComponent<SamusMovement>().CrouchEnabled = true;
			Destroy(this.gameObject);
		}
	}
}
