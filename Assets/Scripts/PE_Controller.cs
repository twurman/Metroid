using UnityEngine;
using System.Collections;

public class PE_Controller : MonoBehaviour {
	private PE_Obj peo;
	
	public Vector3	vel;
	public bool		grounded = false;
	public float 	maxJumpHeight = 5f;
	public float	jumpStart = 0f;
	public bool 	falling;
	
	public float	hSpeed = 10f;
	public float	acceleration = 10f;
	public float	jumpVel = 2f;
	public float	gravity = 0.1f;
	public float	airSteeringAmt = 1f;
	
	public float	airMomentumX = 1; // 0 for no momentum (i.e. 100% drag), 1 for total momentum
	public float	groundMomentumX = 0.1f;
	
	public Vector2	maxSpeed = new Vector2( 10, 15 ); // Different x & y to limit maximum falling velocity
	private bool 	adjustPos = false;
	
	// Use this for initialization
	void Start () {
		peo = GetComponent<PE_Obj>();
	}
	
	// Update is called once per frame
	// Note that we use Update for input but FixedUpdate for physics. This is because Unity input is handled based on Update
	void Update () {
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

		// handle jump
		if(grounded){
			jumpStart = peo.pos0.y;
			falling = false;
			if(Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.Period)){
				if(!GetComponent<SamusMovement>().crouching){
					peo.ground = null;
					vel.y = jumpVel;
					peo.acc.y = 9.8f;
				} else {
					float hScale = transform.localScale.x;
					transform.localScale = new Vector3 (hScale, 1, 1);
					GetComponent<SamusMovement>().crouching = false;
					adjustPos = true;
				}
			}
		} else if(!falling && !(Input.GetButton("Jump"))){
			falling = true;
			vel.y = 0;
			peo.acc.y = 0;
		} if(!falling && peo.pos0.y >= jumpStart + maxJumpHeight){
			falling = true;
			peo.acc.y = -9.8f;
		}
		
		peo.vel = vel;
	}

	void FixedUpdate(){
		if(adjustPos){
			Vector3 pos = GetComponent<PE_Obj>().pos0;
			pos.y += transform.collider.bounds.size.y;
			GetComponent<PE_Obj>().pos0 = pos;
			adjustPos = false;
		}
	}
}
