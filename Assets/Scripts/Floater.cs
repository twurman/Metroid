﻿using UnityEngine;
using System.Collections;

public class Floater : PE_Obj {

	public float hitDamage = 0f;
	private bool collided = false;
	
	// Update is called once per frame
	void Update () {
		if(vel.x > 0){
			transform.localScale = new Vector3 (-1, 1, 1);
		} else if (vel.x < 0) {
			transform.localScale = new Vector3 (1, 1, 1);
		}
	}
	
	void OnTriggerEnter(Collider other) {
		// Ignore bullet collisions, as they are handled by Enemy
		if (other.gameObject.layer == LayerMask.NameToLayer ("Player")) {
			other.GetComponent<SamusMovement>().CauseDamage(hitDamage);
		}
		if (other.gameObject.layer != LayerMask.NameToLayer ("Player Bullet")) {
			collided = true;
		}
		if(other.gameObject.layer == LayerMask.NameToLayer("Ground")){
			float xVel = vel.x;
			vel.x = -xVel;
		}
	}
}