using UnityEngine;
using System.Collections;
//using UnityEditor;



public class PE_Obj : MonoBehaviour {
	public bool			still = false;
	public PE_Collider	coll = PE_Collider.sphere;
	public PE_GravType	grav = PE_GravType.constant;
	
	public Vector3		acc = Vector3.zero;

	public Vector3		vel = Vector3.zero;
	public Vector3		vel0 = Vector3.zero;
	public Vector3		velRel = Vector3.zero; // Velocity based on relative position from last frame to this

	public Vector3		_pos0 = Vector3.zero;
	public Vector3		_pos1 = Vector3.zero;

	public PE_Dir		dir = PE_Dir.still;

	public PE_Obj		ground = null; // Stores whether this is on the ground

	public Vector3		pos0 {
		get { return( _pos0); }
		set {
			float d = (value - _pos0).magnitude;
			if (d > 1 && gameObject.name == "PC") {
				Debug.Log ("Big change in pos0!");
			}
			_pos0 = value;
		}
	}
	
	public Vector3		pos1 {
		get { return( _pos1); }
		set {
			float d = (value - _pos1).magnitude;
			if (d > 1 && gameObject.name == "PC") {
				Debug.Log ("Big change in pos1!");
			}
			_pos1 = value;
		}
	}

	void Start() {
		if (PhysEngine.objs.IndexOf(this) == -1) {
			_pos1 = _pos0 = transform.position;
			PhysEngine.objs.Add(this);
		}
	}

	// Update is called once per frame
	void Update () {
	
	}


	bool IgnoreCollision(Collider other) {
		bool ignore = (other.GetComponent<Enemy> () != null && gameObject.layer == LayerMask.NameToLayer ("Player Bullet"))
			|| (GetComponent<Enemy> () != null && other.gameObject.layer == LayerMask.NameToLayer ("Player Bullet"))
			|| (gameObject.layer == LayerMask.NameToLayer("Enemy") && other.gameObject.layer == LayerMask.NameToLayer("Player"));
		return ignore;
	}
	
	void OnTriggerEnter(Collider other) {
		// Ignore collisions of still objects
		if (still) return;
		
		PE_Obj otherPEO = other.GetComponent<PE_Obj>();
		if (otherPEO == null || IgnoreCollision(other)) return;
		
		ResolveCollisionWith(otherPEO);
	}
	
	void OnTriggerStay(Collider other) {
		OnTriggerEnter(other);
	}
	
	void OnTriggerExit(Collider other) {
		// Ignore collisions of still objects
		if (still) return;
		
		PE_Obj otherPEO = other.GetComponent<PE_Obj>();
		if (otherPEO == null || IgnoreCollision(other)) return;
		
		// This sets ground to null if we fall off of the current ground
		// Jumping will also set ground to null
		if (ground == otherPEO) {
			ground = null;
		}
	}

	void OnDrawGizmos() {
		if (vel.magnitude != 0) {
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(a0,0.2f);
			Gizmos.color = Color.yellow;
			Gizmos.DrawLine(a0,a1);
			Gizmos.DrawWireSphere(a1,0.2f);
			Gizmos.color = Color.green;
			Gizmos.DrawLine(a1,posFinal);
			Gizmos.DrawWireSphere(posFinal,0.2f);
		}
	}

	public Vector3 a0, a1, b, delta, pU, posFinal; // a0-moving corner last frame, a1-moving corner now, b-comparison corner on other object

	void ResolveCollisionWith(PE_Obj that) {

		// Assumes that "that" is still
//		Vector3 posFinal;
		posFinal = pos1; // Sets a defaut value for posFinal

		switch (this.coll) {
		case PE_Collider.sphere:

			switch (that.coll) {
			case PE_Collider.sphere:
				// Sphere / Sphere collision
				float thisR, thatR, rad;
				// Note, this doesn't work with non-uniform or negative scales!
				thisR = Mathf.Max( this.transform.lossyScale.x, this.transform.lossyScale.y, this.transform.lossyScale.z ) / 2;
				thatR = Mathf.Max( that.transform.lossyScale.x, that.transform.lossyScale.y, that.transform.lossyScale.z ) / 2;
				rad = thisR + thatR;

				Vector3 delta = pos1 - that.transform.position;
				delta.Normalize();
				delta *= rad;

				posFinal = that.transform.position + delta;
				break;
			}

			break;

		case PE_Collider.aabb:

			switch (that.coll) {
			case PE_Collider.aabb:
				// AABB / AABB collision
				// Axis-Aligned Bounding Box
				// With AABB collisions, we're usually concerned with corners and deciding which corner to consider when making comparisons.
				// I believe that this corner should be determined by looking at the velocity of the moving body (this one)

				// Vector3 a0, a1, b, delta, pU; 
				// a0-moving corner last frame, a1-moving corner now, b-comparison corner on other object
				a0 = a1 = b = Vector3.zero;	 // Sets a default value to keep the compiler from complaining
				delta = pos1 - pos0;

				if (dir == PE_Dir.down) {
					// If a0 was above b and a1 is below b resolve to be on top
					a1 = pos1;
					a1.y -= transform.collider.bounds.size.y/2f;
					a0 = a1 - delta;
					b = that.pos1;
					b.y += that.transform.lossyScale.y/2f;
					if ( PhysEngine.GEQ( a0.y, b.y ) && b.y > a1.y) {
						posFinal.y += Mathf.Abs( a1.y - b.y );
						// Handle vel
						vel.y = 0;
						
						if (ground == null) ground = that;
					}
					break; // Exit this switch statement: switch (that.coll)
				}

				if (dir == PE_Dir.up) {
					// If a0 was below b and a1 is above b resolve to be below
					a1 = pos1;
					a1.y += transform.collider.bounds.size.y/2f;
					a0 = a1 - delta;
					b = that.pos1;
					b.y -= that.transform.lossyScale.y/2f;
					if ( PhysEngine.LEQ( a0.y, b.y ) && b.y < a1.y) {
						posFinal.y -= Mathf.Abs( a1.y - b.y );
						// Handle vel
						vel.y = 0;
						
						break; // Exit this switch statement: switch (that.coll)
					}
				}

				if (dir == PE_Dir.upRight) { // Bottom, Left is the comparison corner
					a1 = pos1;
					a1.x += transform.collider.bounds.size.x/2f;
					a1.y += transform.collider.bounds.size.y/2f;
					a0 = a1 - delta;
					b = that.pos1;
					b.x -= that.transform.lossyScale.x/2f;
					b.y -= that.transform.lossyScale.y/2f;
				}

				if (dir == PE_Dir.upLeft) { // Bottom, Right is the comparison corner
					a1 = pos1;
					a1.x -= transform.collider.bounds.size.x/2f;
					a1.y += transform.collider.bounds.size.y/2f;
					a0 = a1 - delta;
					b = that.pos1;
					b.x += that.transform.lossyScale.x/2f;
					b.y -= that.transform.lossyScale.y/2f;
				}

				if (dir == PE_Dir.downLeft) { // Top, Right is the comparison corner
 					a1 = pos1;
					a1.x -= transform.collider.bounds.size.x/2f;
					a1.y -= transform.collider.bounds.size.y/2f;
					a0 = a1 - delta;
					b = that.pos1;
					b.x += that.transform.lossyScale.x/2f;
					b.y += that.transform.lossyScale.y/2f;
				}

				if (dir == PE_Dir.downRight) { // Top, Left is the comparison corner
					a1 = pos1;
					a1.x += transform.collider.bounds.size.x/2f;
					a1.y -= transform.collider.bounds.size.y/2f;
					a0 = a1 - delta;
					b = that.pos1;
					b.x -= that.transform.lossyScale.x/2f;
					b.y += that.transform.lossyScale.y/2f;
				}

				// In the x dimension, find how far along the line segment between a0 and a1 we need to go to encounter b
				float u = (b.x - a0.x) / (a1.x - a0.x);

				// Determine this point using linear interpolation (see the appendix of the book)
				pU = (1-u)*a0 + u*a1;

				// Find distance we would have to offset in x or y
				float offsetX = Mathf.Abs(a1.x - b.x);
				float offsetY = Mathf.Abs(a1.y - b.y);

				// Use pU.y vs. b.y to tell which side of PE_Obj "that" PE_Obj "this" should be on
				switch (dir) {
				case PE_Dir.upRight:
					if (pU.y > b.y || u == 0) { // hit the left side
						posFinal.x -= offsetX;

						// Handle vel
						vel.x = 0;

					} else { // hit the bottom
						posFinal.y -= offsetY;
						
						// Handle vel
						vel.y = 0;

					}
					break;

				case PE_Dir.downRight:
					if (pU.y < b.y || u == 0) { // hit the left side
						posFinal.x -= offsetX;
						
						// Handle vel
						vel.x = 0;

					} else { // hit the top
						posFinal.y += offsetY;

						// Handle vel
						vel.y = 0;
						
						if (ground == null) ground = that;
					}
					break;
					
				case PE_Dir.upLeft:
					if (pU.y > b.y || u == 0) { // hit the right side
						posFinal.x += offsetX;
						
						// Handle vel
						vel.x = 0;

					} else { // hit the bottom
						posFinal.y -= offsetY;
						
						// Handle vel
						vel.y = 0;

					}
					break;
					
				case PE_Dir.downLeft:
					if (pU.y < b.y || u == 0) { // hit the right side
						posFinal.x += offsetX;
						
						// Handle vel
						vel.x = 0;

					} else { // hit the top
						posFinal.y += offsetY;
						
						// Handle vel
						vel.y = 0;
						
						if (ground == null) ground = that;
					}
					break;
				}

				break;
			}

			break;
		}

		transform.position = pos1 = posFinal;
	}


}
