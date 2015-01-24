using UnityEngine;
using System.Collections;

public class SamusMovement : MonoBehaviour
{
		public float BulletOffset_x = 0f;
		public float BulletOffset_y = 0f;
		public float BulletOffset_y_vertical = 0f;
		
		public float fireRate = 0.5F;
		private float nextFire = 0.0F; 
		public GameObject projectile;

		void Update() {
			if (Input.GetButton("Fire") && Time.time > nextFire) {
				nextFire = Time.time + fireRate;

				if (Input.GetButton("Vertical")) {
                    Vector3 bullet_pos = new Vector3(
											transform.position.x,
			                                transform.position.y + BulletOffset_y_vertical,
											transform.position.z);
					GameObject clone = Instantiate(projectile, bullet_pos, transform.rotation) as GameObject;
                    clone.layer = LayerMask.NameToLayer("Player Bullet");
                    PE_Obj pe = clone.GetComponent<PE_Obj>();
                    pe.vel = new Vector3(0, 1, 0);
				} else {
                    Vector3 bullet_pos = new Vector3(
											transform.position.x + (BulletOffset_x * transform.localScale.x),
			                                transform.position.y + BulletOffset_y,
											transform.position.z);
					GameObject clone = Instantiate(projectile, bullet_pos, transform.rotation) as GameObject;
                    clone.layer = LayerMask.NameToLayer("Player Bullet");
                    PE_Obj pe = clone.GetComponent<PE_Obj>();
                    pe.vel = new Vector3(transform.localScale.x, 0, 0);
				}
				
			}
		}

		void FixedUpdate() {
			float hSpeed = Input.GetAxis ("Horizontal");
			
			if (hSpeed > 0) {
				transform.localScale = new Vector3 (1, 1, 1);
			} else if (hSpeed < 0) {
				transform.localScale = new Vector3 (-1, 1, 1);
			}
		}
}
