using UnityEngine;
using System.Collections;

public class Skree : PE_Obj {

	public GameObject player;

	public float AttackDistance = 5;

	public float fallDamage = 0f;

	private bool collided = false;

	private bool falling = false;

	public float Acceleration = 1f;

	// Update is called once per frame
	void Update () {
		if (Mathf.Abs(player.transform.position.x - this.gameObject.transform.position.x) < AttackDistance) {
			this.grav = PE_GravType.constant;
			falling = true;
		}
	}

	void FixedUpdate() {
		if (falling) {
			if (player.transform.position.x - this.gameObject.transform.position.x > 0) {
				acc.x =  1 * Acceleration;
			} else if (player.transform.position.x - this.gameObject.transform.position.x < 0) {
				acc.x = -1 * Acceleration;
			}
		}
	}

	void LateUpdate() {
		if (collided) {
			Destroy (this.gameObject);
			PhysEngine.objs.Remove(this);
		}

	}


	void OnTriggerEnter(Collider other) {
		// Ignore bullet collisions, as they are handled by Enemy
		if (other.gameObject.layer == LayerMask.NameToLayer ("Player")) {
			other.GetComponent<SamusMovement>().CauseDamage(fallDamage);
		}
		if (other.gameObject.layer != LayerMask.NameToLayer ("Player Bullet")) {
            collided = true;
        }
	}
}
