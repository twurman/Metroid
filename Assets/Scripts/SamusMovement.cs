using UnityEngine;
using System.Collections;

public class SamusMovement : MonoBehaviour
{

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

		void FixedUpdate() {
			float hSpeed = Input.GetAxis ("Horizontal");
			
			if (hSpeed > 0) {
				transform.localScale = new Vector3 (1, 1, 1);
			} else if (hSpeed < 0) {
				transform.localScale = new Vector3 (-1, 1, 1);
			}
		}
}
