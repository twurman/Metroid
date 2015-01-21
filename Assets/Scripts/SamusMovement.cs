using UnityEngine;
using System.Collections;

public class SamusMovement : MonoBehaviour
{

		public float fireRate = 0.5F;
		private float nextFire = 0.0F; 
		public GameObject projectile;

		void Update() {
			Debug.Log ("in SamusMovement::Update()");

			if (Input.GetButton("Fire") && Time.time > nextFire) {
				nextFire = Time.time + fireRate;
				GameObject clone = Instantiate(projectile, transform.position, transform.rotation) as GameObject;
				clone.rigidbody2D.velocity = new Vector2(transform.localScale.x, 0);
			}
		}
}
