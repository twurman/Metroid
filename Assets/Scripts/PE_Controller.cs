using UnityEngine;
using UnityEngine.UI;
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
	public bool		floating = false;
	public bool		thrustEnabled = false;
	public Text		fuelCounter;
	public int 		fuel = 100;
	public float	fuelRegenTime;
	public bool		jeremyMode = false;

	private PhysEngine physEngine;
	
	// Use this for initialization
	void Start () {
		peo = GetComponent<PE_Obj>();
		physEngine = Camera.main.GetComponent<PhysEngine>();
	}

	private void UpdateFuelCounter ()
	{
		if(!fuelCounter) return;
		if(thrustEnabled){
			if(fuel < 100){
				fuelCounter.text = "FUEL..0" + fuel / 10;
			} else {
				fuelCounter.text = "FUEL.." + fuel / 10;
			}
		} else {
			fuelCounter.text = "";
		}

	}
	
	// Update is called once per frame
	// Note that we use Update for input but FixedUpdate for physics. This is because Unity input is handled based on Update
	void Update () {
		vel = peo.vel; // Pull velocity from the PE_Obj
		grounded = (peo.ground != null);
		
		// Horizontal movement
		if(floating){
			if(Time.time > fuelRegenTime + .5f){
				fuel += 1;
				fuelRegenTime = Time.time;
				
			}

			if(fuel > 999){
				fuel = 999;
			} else if(fuel < 0) {
				fuel = 0;
			}



			if(fuel > 0){
				if(Input.GetAxis("Horizontal") > 0){
					vel.x += 0.1f;
					if(vel.x < maxSpeed.x){
						fuel--;
					}
				} else if(Input.GetAxis("Horizontal") < 0){
					vel.x -= 0.1f;
					if(vel.x > -maxSpeed.x){
						fuel--;
					}
				}
				
				if(Input.GetAxis("Vertical") < 0){
					vel.y -= 0.1f;
					fuel -= 2;
				}
			}

		} else {
			float vX = Input.GetAxis("Horizontal"); // Returns a number [-1..1]
			vel.x = vX * hSpeed;
		}

		// handle jump
		if(grounded){
			peo.acc.x = 0;
			jumpStart = peo.pos0.y;
			falling = false;
			if(Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.Period)){
				if(!GetComponent<SamusMovement>().crouching){
					peo.ground = null;
					vel.y = jumpVel;
					peo.acc.y = -physEngine.gravity.y;
				} else {
					if (GetComponent<SamusMovement>().CanStand()) {
						float hScale = transform.localScale.x;
						transform.localScale = new Vector3 (hScale, 1, 1);
						GetComponent<SamusMovement>().crouching = false;
						adjustPos = true;
					}
				}
			}
		} else if(!falling && !(Input.GetButton("Jump"))){
			falling = true;
			vel.y = 0;
			peo.acc.y = 0;
		} if(!falling && peo.pos0.y >= jumpStart + maxJumpHeight){
			falling = true;
		} if(falling){
			peo.acc.y = physEngine.gravity.y;
		}
		
		peo.vel = vel;
		if(jeremyMode){
			fuel = 1000;
		}
		UpdateFuelCounter();
	}

	void FixedUpdate(){
		if(adjustPos){
			Vector3 pos = GetComponent<PE_Obj>().pos0;
			pos.y += transform.collider.bounds.size.y;
			GetComponent<PE_Obj>().pos0 = pos;
			adjustPos = false;
		}
		peo.vel.x = Mathf.Sign(peo.vel.x) * Mathf.Min(Mathf.Abs(peo.vel.x), Mathf.Abs(maxSpeed.x));
		peo.vel.y = Mathf.Sign(peo.vel.y) * Mathf.Min(Mathf.Abs(peo.vel.y), Mathf.Abs(maxSpeed.y));
	}
}
