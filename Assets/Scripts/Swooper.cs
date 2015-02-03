using UnityEngine;
using System.Collections;

public class Swooper : MonoBehaviour {

	public GameObject player;

	private PE_Obj player_peo;

	private PE_Obj peo;

	public float start_pos = 97.8f;
	
	public float end_pos = 118.5f;

	public float GroundHorizontalVelocity = 1f;

	public float SwoopHorizontalVelocity = 1.5f;

	public float SwoopVerticalAcceleration = 120f;

	public float SwoopStopVelocityThreshold = 0.1f;

	public float SwoopHalfwayFudgeFactor = 0.1f;

	public float TimeBetweenSwoops = 0.25f;
	public float hitDamage;

	private bool chasing = false;
	private bool swoop_in_progress = false;
	private bool swoop_right;
	private bool hit_wall_swoop;
	private float swoop_half_point;
	private float next_swoop_time = 0f;

	// Use this for initialization
	void Start () {
		peo = this.GetComponent<PE_Obj>();
		player_peo = player.GetComponent<PE_Obj>();
		//peo.still = true;
	}

	void setHorizontalVelocity() {
		if (player.transform.position.x < this.transform.position.x) {
			peo.vel.x = -1 * GroundHorizontalVelocity;
		} else {
			peo.vel.x =  1 * GroundHorizontalVelocity;
		}
	}

	void swoop(bool right) {
		peo.acc.y = SwoopVerticalAcceleration;
		float h = this.transform.position.y - player.transform.position.y;

		// using conservation of energy: kinematic -> gravitational potential
		float v_y_0 = Mathf.Sqrt(2 * SwoopVerticalAcceleration * h);
		if (float.IsNaN(v_y_0)) {
			v_y_0 = 0;
		}

		peo.vel.y = -1 * v_y_0;

		if (right) {
			peo.vel.x = SwoopHorizontalVelocity;
		} else {
			peo.vel.x = -1 * SwoopHorizontalVelocity;
		}

		swoop_in_progress = true;
		hit_wall_swoop = false;
		swoop_right = right;
		Debug.Log(v_y_0);
		swoop_half_point = (v_y_0 * peo.vel.x) / SwoopVerticalAcceleration + this.transform.position.x;
		Debug.Log(swoop_half_point);

	}

	void OnTriggerEnter(Collider other) {
		// Ignore bullet collisions, as they are handled by Enemy
		if (other.gameObject.layer == LayerMask.NameToLayer ("Player")) {
			other.GetComponent<SamusMovement>().CauseDamage(hitDamage);
		}
	}

	// Update is called once per frame
	void Update () {
		if (!(player.transform.position.x >= start_pos && player.transform.position.x <= end_pos)) {
			return;
		}
		float cur_x = this.transform.position.x;

		if (!swoop_in_progress && !chasing && Time.time > next_swoop_time) {
			if (cur_x < ((end_pos - start_pos) / 2f) + start_pos) {
				swoop(true);
			} else {
				swoop(false);
			}
		}

		if (swoop_in_progress) {
			if (float.IsNaN(swoop_half_point)) {
				hit_wall_swoop = true;
			}

			if ((swoop_right && cur_x > (swoop_half_point + SwoopHalfwayFudgeFactor) ||
			      !swoop_right && cur_x < (swoop_half_point - SwoopHalfwayFudgeFactor))
			    && Mathf.Abs(player_peo.vel.x) > .5f
			    && Mathf.Abs(player.transform.position.y - this.transform.position.y) < .25f) {

				chasing = true;
				swoop_in_progress = false;
				peo.vel.y = 0;
				peo.acc = Vector3.zero;
				
				//setHorizontalVelocity();
			} else if (Mathf.Abs(peo.vel.y) < SwoopStopVelocityThreshold &&
			    	((swoop_right && cur_x > (swoop_half_point + SwoopHalfwayFudgeFactor) ||
			 		!swoop_right && cur_x < (swoop_half_point - SwoopHalfwayFudgeFactor)) ||
		 			hit_wall_swoop)) {
				peo.vel = Vector3.zero;
				peo.acc = Vector3.zero;

				swoop_in_progress = false;
				next_swoop_time = Time.time + TimeBetweenSwoops;
			} else if (Mathf.Abs(peo.vel.x) < SwoopHorizontalVelocity - SwoopStopVelocityThreshold) {
				Debug.Log("hit wall");
				hit_wall_swoop = true;
				swoop_right = !swoop_right;
				
				if (swoop_right) {
					peo.vel.x = SwoopHorizontalVelocity;
				} else {
					peo.vel.x = -1 * SwoopHorizontalVelocity;
				}
			}
		}

		if (chasing) {
			if (Mathf.Abs(peo.vel.x) < GroundHorizontalVelocity - SwoopStopVelocityThreshold) {
				//peo.vel.x = -1 * peo.vel.x;
				setHorizontalVelocity();
			} else if (player_peo.ground == null) {
				//swoop (false);
			
				swoop (peo.vel.x > 0);

//				if (cur_x < ((end_pos - start_pos) / 2f) + start_pos) {
//					swoop(true);
//				} else {
//					swoop(false);
//				}
				chasing = false;
				swoop_in_progress = true;
				//hit_wall_swoop = true;
				swoop_half_point = this.transform.position.x;
				//peo.vel.y = 0;
			}
		}
	}


}
