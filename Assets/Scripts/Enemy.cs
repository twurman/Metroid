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

	void CauseDamage(float amount) {
		health -= amount;
		if (health <= 0) {
			Debug.Log ("died");
			Destroy (this.gameObject);
			PhysEngine.objs.Remove(GetComponent<PE_Obj>());
		}
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.layer == LayerMask.NameToLayer ("Player Bullet")) {
			CauseDamage(other.GetComponent<Bullet>().damage);
		}
	}

}
