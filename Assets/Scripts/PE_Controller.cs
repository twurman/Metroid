using UnityEngine;
using System.Collections;

public class PE_Controller : MonoBehaviour {
	private PE_Obj peo;
	
	public Vector3	vel;
	public bool		grounded = false;
	
	public float	hSpeed = 10;
	public float	acceleration = 10;
	public float	jumpVel = 10;
	public float	airSteeringAmt = 1f;
	
	public float	airMomentumX = 1; // 0 for no momentum (i.e. 100% drag), 1 for total momentum
	public float	groundMomentumX = 0.1f;
	
	public Vector2	maxSpeed = new Vector2( 10, 15 ); // Different x & y to limit maximum falling velocity
	
	// Use this for initialization
	void Start () {
		peo = GetComponent<PE_Obj>();
	}
	
	// Update is called once per frame
	// Note that we use Update for input but FixedUpdate for physics. This is because Unity input is handled based on Update
	void Update () {
		Debug.Log ("in PE_Controller::Update()");
		vel = peo.vel; // Pull velocity from the PE_Obj
		grounded = (peo.ground != null);
		
		// Horizontal movement
		float vX = Input.GetAxis("Horizontal"); // Returns a number [-1..1]
		vel.x = vX * hSpeed;
		
		//		if (vX == 0) { // No Horizontal input from keyboard
		//		}
		//		Vector3 accMult = Vector3.one;
		//		if (!grounded) accMult.x = airSteeringAmt;
		//
		//		vel.x += vX * accMult.x * acceleration * Time.deltaTime;
		
		// Jumping with A (which is x or .)
		if (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.Period)) {
			// Jump if you're grounded
			if (grounded) {
				vel.y = jumpVel;
				peo.ground = null; // Jumping will set ground = null
			}
		}
		
		peo.vel = vel;
	}
}
