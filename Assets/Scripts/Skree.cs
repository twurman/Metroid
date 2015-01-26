using UnityEngine;
using System.Collections;

public class Skree : PE_Obj {

	public GameObject player;

	public float AttackDistance = 5;

	private bool collided = false;

	// Update is called once per frame
	void Update () {

		if (Mathf.Abs(player.transform.position.x - this.gameObject.transform.position.x) < AttackDistance) {
			this.grav = PE_GravType.constant;
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
        if (other.gameObject.name != "Bullet(Clone)") {
            collided = true;
        }
	}
}
