using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	public float velocity = 1.0f;
	public float maxDistance = 3.0f;

	private Vector3 initialPosition;

	// Use this for initialization
	void Start () {
		this.rigidbody.velocity = new Vector2 (velocity*this.rigidbody.velocity.x, velocity*this.rigidbody.velocity.y);
		initialPosition = this.rigidbody.position;
	}

	void FixedUpdate() {
		if ((this.rigidbody.position - initialPosition).magnitude > maxDistance) {
			Destroy(this.gameObject);
		}
	}

	// Update is called once per frame
	void Update () {
		
	}
}
