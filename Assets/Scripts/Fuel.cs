using UnityEngine;
using System.Collections;

public class Fuel : MonoBehaviour {

	public int FuelAmount = 100;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.layer == LayerMask.NameToLayer ("Player")) {
			other.GetComponent<PE_Controller>().fuel += FuelAmount;
			Destroy(this.gameObject);
		}
	}
}
