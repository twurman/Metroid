using UnityEngine;
using System.Collections;

public class SamusMovement : MonoBehaviour {

	public float SpeedMultiplier = 10.0f;

	void FixedUpdate() {
		float hSpeed = Input.GetAxis("Horizontal");

		if (hSpeed > 0) {
			transform.localScale = new Vector3( 1, 1, 1);
		} else if (hSpeed < 0) {
			transform.localScale = new Vector3(-1, 1, 1);
		}

		this.rigidbody2D.velocity = new Vector2 (hSpeed * SpeedMultiplier, this.rigidbody2D.velocity.y);
	}
}
