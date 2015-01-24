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
		Debug.Log ("Enemy::OnCollisionEnter: " + other.gameObject.name);
	}

}
