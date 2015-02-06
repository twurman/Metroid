using UnityEngine;
using System.Collections;

public class Skree : PE_Obj {

	public GameObject player;

	public float AttackDistanceX = 5;

	public float AttackDistanceY = 10;

	public float BulletDamage = 0f;

	public GameObject projectile;

	public float fallDamage = 0f;

	private bool collided = false;

	private bool falling = false;

	public float Acceleration = 1f;

	public float bullet_release_time = 0f;

	public float bullet_delay = 0.2f;

	// Update is called once per frame
	void Update () {
		if (collided) return;
		if (Mathf.Abs(player.transform.position.x - this.gameObject.transform.position.x) < AttackDistanceX
		    && Mathf.Abs(player.transform.position.y - this.gameObject.transform.position.y) < AttackDistanceY) {
			this.grav = PE_GravType.constant;
			falling = true;
		}
	}

	void FixedUpdate() {
		if (collided) return;
		if (falling) {
			float dist = Mathf.Abs(player.transform.position.x - this.gameObject.transform.position.x);
			float accX =  dist > Acceleration ? Acceleration : dist;
			if (player.transform.position.x - this.gameObject.transform.position.x > 0) {
				acc.x =  1 * accX;
			} else if (player.transform.position.x - this.gameObject.transform.position.x < 0) {
				acc.x = -1 * accX;
			}
		}
	}

	private void CreateBullet(Vector3 bulletVel) {
		bulletVel.Normalize();
		Vector3 bullet_pos = new Vector3(
			transform.position.x,
			transform.position.y,
			transform.position.z);
		GameObject clone = Instantiate(projectile, bullet_pos, transform.rotation) as GameObject;
		PE_Obj pe = clone.GetComponent<PE_Obj>();
		pe.vel = bulletVel;
		clone.layer = LayerMask.NameToLayer("Enemy Bullet");
		clone.GetComponent<Bullet>().damage = BulletDamage;
	}
	
	void LateUpdate() {
		if (collided && Time.time >= bullet_release_time) {
			Destroy (this.gameObject);
			PhysEngine.objs.Remove(this);

			CreateBullet(new Vector3(.75f, 1.5f, 0f));
			CreateBullet(new Vector3(-.75f, 1.5f, 0f));
			CreateBullet(new Vector3(-1f, 0f, 0f));
			CreateBullet(new Vector3(1f, 0f, 0f));
		} 
	}


	void OnTriggerEnter(Collider other) {
		// Ignore bullet collisions, as they are handled by Enemy
		if (other.gameObject.layer == LayerMask.NameToLayer ("Player")) {
			other.GetComponent<SamusMovement>().CauseDamage(fallDamage);
		}


		if (other.gameObject.layer == LayerMask.NameToLayer ("Ground")) {
            collided = true;
			acc.x = 0;
			acc.y = 0;
			vel.x = 0;
			vel0.x = 0;
			still = true;
			bullet_release_time = Time.time + bullet_delay;
        }
	}
}
