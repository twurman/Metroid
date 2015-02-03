using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {
	public Sprite normal, left_unlocked, right_unlocked, both_unlocked;

	private SpriteRenderer spriteRenderer;

	public float unlock_time = 3f;

	public int num_hits_needed = 3;

	public int num_hits_left = 3;

	public bool left_side_unlocked = false;
	public float left_unlocked_until;
	public int num_hits_left_left;

	public bool right_side_unlocked = false;
	public float right_unlocked_until;
	public int num_hits_left_right;

	private bool player_inside = false;

	// Use this for initialization
	void Start () {
		spriteRenderer = this.GetComponent<SpriteRenderer>();
		spriteRenderer.sprite = normal;
		num_hits_left_left = num_hits_left_right = num_hits_needed;
	}
	
	// Update is called once per frame
	void Update () {
//		if (Time.time > start) {
//			spriteRenderer.sprite = left_unlocked;
//		}

		if (left_side_unlocked && Time.time > left_unlocked_until) {
			left_side_unlocked = false;
			num_hits_left_left = num_hits_needed;
		}
		if (right_side_unlocked && Time.time > right_unlocked_until) {
			right_side_unlocked = false;
			num_hits_left_right = num_hits_needed;
		}

		if (player_inside || left_side_unlocked || right_side_unlocked) {
			this.gameObject.tag = "Door";
		} else {
			this.gameObject.tag = "Untagged";
		}

		if (!left_side_unlocked && !right_side_unlocked) {
			spriteRenderer.sprite = normal;
		} else if (left_side_unlocked && !right_side_unlocked) {
			spriteRenderer.sprite = left_unlocked;
		} else if (!left_side_unlocked && right_side_unlocked) {
			spriteRenderer.sprite = right_unlocked;
		} else if (left_side_unlocked && right_side_unlocked) {
			spriteRenderer.sprite = both_unlocked;
		}
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.layer == LayerMask.NameToLayer ("Player Bullet")) {
			PE_Obj peo = other.GetComponent<PE_Obj>();

			if (peo.vel.x > 0) {
				num_hits_left_left -= 1;
				if (num_hits_left_left <= 0) {
					left_side_unlocked = true;
					left_unlocked_until = Time.time + unlock_time;
				}
			} else {
				num_hits_left_right -= 1;
				if (num_hits_left_right <= 0) {
					right_side_unlocked = true;
					right_unlocked_until = Time.time + unlock_time;
				}
			}

			PhysEngine.objs.Remove(other.GetComponent<PE_Obj>());
			Destroy(other.gameObject);
		}

		if (other.gameObject.layer == LayerMask.NameToLayer ("Player") && (left_side_unlocked || right_side_unlocked))  {
			player_inside = true;
		}
	}

	void OnTriggerExit(Collider other) {
		if (other.gameObject.layer == LayerMask.NameToLayer ("Player"))  {
			player_inside = false;
		}
	}


}
