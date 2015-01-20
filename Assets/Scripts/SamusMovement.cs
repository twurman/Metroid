using UnityEngine;
using System.Collections;

public class SamusMovement : MonoBehaviour {

	public float SpeedMultiplier = 10.0f;
	public float vSpeed = 0f;
	const float negVelocity = -1f;

	void FixedUpdate() {
		float hSpeed = Input.GetAxis ("Horizontal");
		float vDir = Input.GetAxis ("Vertical");
		Vector3 position = this.transform.position;

		if (hSpeed > 0) {
			transform.localScale = new Vector3 (1, 1, 1);
		} else if (hSpeed < 0) {
			transform.localScale = new Vector3 (-1, 1, 1);
		}


		if (position.y == 0 && vDir > 0) {
			vSpeed += 2f;
		} else if (position.y > 0) {
			vSpeed -= 0.1f;
		} else if (position.y < 0) {
			vSpeed = 0f;
			position.y = 0;
			this.transform.position = position;
		}






		this.rigidbody2D.velocity = new Vector2 (hSpeed * SpeedMultiplier, vSpeed * SpeedMultiplier);
	}
}
