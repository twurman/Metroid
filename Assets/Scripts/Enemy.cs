using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	public float health = 9999f;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.layer == LayerMask.NameToLayer ("Player Bullet")) {
			health -= other.GetComponent<Bullet>().damage;
			Debug.Log(health);
		}

		if (health <= 0) {
			Debug.Log ("died");
			Destroy (this.gameObject);
			PhysEngine.objs.Remove(GetComponent<PE_Obj>());
		}
	}

}
