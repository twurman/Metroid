using UnityEngine;
using System.Collections;

public class ThrustPowerUp : MonoBehaviour {

	public Sprite standing, upwards, crouch1, crouch2, crouch3, crouch4;

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
			other.GetComponent<SamusMovement>().CrouchEnabled = false;
			other.GetComponent<SamusMovement>().standing = standing;
			other.GetComponent<SamusMovement>().upwards = upwards;
			other.GetComponent<SamusMovement>().crouch1 = crouch1;
			other.GetComponent<SamusMovement>().crouch2 = crouch2;
			other.GetComponent<SamusMovement>().crouch3 = crouch3;
			other.GetComponent<SamusMovement>().crouch4 = crouch4;
			Destroy(this.gameObject);
		}
	}
}
