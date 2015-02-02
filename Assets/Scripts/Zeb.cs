using UnityEngine;
using System.Collections;

public class Zeb : PE_Obj {

	public float hitDamage = 0f;
	public float xVel = 2f;
	private bool startMovement = false;
	
	// Update is called once per frame
	void Update () {
		if(vel.x > 0){
			transform.localScale = new Vector3 (-1, 1, 1);
		} else if (vel.x < 0) {
			transform.localScale = new Vector3 (1, 1, 1);
		}
		if(!startMovement){
			vel.x = 0;
		} else {
			vel.x = xVel;
		}
	}

	void OnBecameVisible(){
		startMovement = true;
	}
	
	void OnTriggerEnter(Collider other) {
		// Ignore bullet collisions, as they are handled by Enemy
		if (other.gameObject.layer == LayerMask.NameToLayer ("Player")) {
			other.GetComponent<SamusMovement>().CauseDamage(hitDamage);
		}
		if (other.gameObject.layer == LayerMask.NameToLayer ("Player Bullet")) {
			PhysEngine.objs.Remove(other.GetComponent<PE_Obj>());
			Destroy(other.gameObject);
		}
		if(other.gameObject.layer == LayerMask.NameToLayer("Ground")){
			PhysEngine.objs.Remove(this);
			Destroy(this.gameObject);
		}
	}
}
