using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

    private PE_Obj peo;

	public float damage = 0f;

	public float velocity = 1.0f;
	public float maxDistance = 3.0f;

    private bool initial_set = false;
	private Vector3 initialPosition;

	// Use this for initialization
	void Start () {
        peo = GetComponent<PE_Obj>();
		peo.vel = new Vector2 (velocity*peo.vel.x, velocity*peo.vel.y);
	}

	void FixedUpdate() {
        if (!initial_set) {
            initial_set = true;
            initialPosition = peo.pos0;
        }

		if ((peo.pos0 - initialPosition).magnitude > maxDistance) {
            PhysEngine.objs.Remove(peo);
			Destroy(this.gameObject);
		}
	}

}
