using UnityEngine;
using System.Collections;

public class SamusMovement : MonoBehaviour
{

		public float SpeedMultiplier = 10.0f;
		public float vSpeed = 0f;
		public bool falling = false;
		private float jumpStart = 0;

		public float fireRate = 0.5F;
		private float nextFire = 0.0F; 
		public GameObject projectile;

		void Update() {
			if (Input.GetButton("Fire") && Time.time > nextFire) {
				nextFire = Time.time + fireRate;
				GameObject clone = Instantiate(projectile, transform.position, transform.rotation) as GameObject;
				clone.rigidbody2D.velocity = new Vector2(transform.localScale.x, 0);
			}
		}

		void FixedUpdate ()
		{
				float hSpeed = Input.GetAxis ("Horizontal");
				float vDir = Input.GetAxis ("Vertical");
				Vector3 position = this.transform.position;

				if (hSpeed > 0) {
					transform.localScale = new Vector3 (1, 1, 1);
				} else if (hSpeed < 0) {
					transform.localScale = new Vector3 (-1, 1, 1);
				}


				if (falling || vDir <= 0) {
					vSpeed -= 0.1f;
				} else if (vDir > 0) {
					if(jumpStart == null){
						jumpStart = position.y;
					}
					vSpeed = 1.2f;
				}
	
				

				this.rigidbody2D.velocity = new Vector2 (hSpeed * SpeedMultiplier, vSpeed * SpeedMultiplier);

				if (position.y > jumpStart + 5) {
					falling = true;
				} 
//				else if (position.y < jumpStart) {
//					falling = false;
//					position.y = jumpStart;
//					vSpeed = 0;
//					this.transform.position = position;
//				}
		}

		void OnTriggerEnter(Collider other) {
			// Ignore collisions of still objects
			if(vSpeed >= 0) {
				vSpeed = 0;
				falling = true;
			} else {
				vSpeed = 0;
				falling = false;
			}
		}
		
		void OnTriggerStay(Collider other) {
			OnTriggerEnter(other);
		}
		
		void OnTriggerExit(Collider other) {
			//do nothing for ground
		}
}
