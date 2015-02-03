using UnityEngine;
using System.Collections;

public class Energy : MonoBehaviour {

	public float EnergyAmount = 0f;

	// TODO : destroy after timeout

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.layer == LayerMask.NameToLayer ("Player")) {
			other.GetComponent<SamusMovement>().CauseDamage(-1 * EnergyAmount);
			Destroy(this.gameObject);
		}
	}
}
