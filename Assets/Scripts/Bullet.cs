using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	public float velocity = 1.0f;
	public float maxDistance = 3.0f;

	private float initialPosition;

	// Use this for initialization
	void Start () {
		this.rigidbody2D.velocity = new Vector2 (velocity*this.rigidbody2D.velocity.x, 0);
		initialPosition = this.rigidbody2D.position.x;
	}

	void FixedUpdate() {
		if (Mathf.Abs(this.rigidbody2D.position.x - initialPosition) > maxDistance) {
			Destroy(this.gameObject);
		}
	}

	// Update is called once per frame
	void Update () {
		
	}
}
