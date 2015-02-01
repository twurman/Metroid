using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//struct RemovedEnemy {
//	public PE_Obj enemy;
//	public float time;
//
//	public RemovedEnemy(PE_Obj e, float t) {
//		enemy = e;
//		time = t;
//}

public class Enemy : MonoBehaviour {

	public Sprite normal, hit;
	private int hitTimer;

	public float health = 9999f;

	// See http://deanyd.net/sm/index.php?title=Enemy_item_drops
	
	public float HealthDropChance_1 = .25f;
	public GameObject HealthDrop1;

	public float HealthDropChance_2 = .25f;
	public GameObject HealthDrop2;

//	private List<RemovedEnemy> removed_enemies;

	public float frozen_duration = 5f;

	public bool frozen = false;
	public float frozen_until = 0f;
	
	// Use this for initialization
	void Start () {
//		removed_enemies = new List<RemovedEnemy>();
	}
	
	// Update is called once per frame
	void Update () {
		if (frozen && Time.time > frozen_until) {
			frozen = false;
			PhysEngine.objs.Add(GetComponent<PE_Obj>());
		}
	}

	void FixedUpdate(){
		if(hitTimer == 10){
			ChangeSprite(false);
		}
		hitTimer++;
	}

	public void CauseDamage(float amount) {
		health -= amount;
		if (health <= 0) {
			Debug.Log ("died");
			Destroy (this.gameObject);
			PhysEngine.objs.Remove(GetComponent<PE_Obj>());

			if(Random.value <= HealthDropChance_1) {
				GameObject clone = Instantiate(HealthDrop1, transform.position, transform.rotation) as GameObject;
			} else if(Random.value <= HealthDropChance_2) {
				GameObject clone = Instantiate(HealthDrop2, transform.position, transform.rotation) as GameObject;
			}
		}
	}

	void ChangeSprite(bool gotHit){
		if(gotHit){
			GetComponent<SpriteRenderer>().sprite = hit;
			hitTimer = 0;
		} else {
			GetComponent<SpriteRenderer>().sprite = normal;
		}
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.layer == LayerMask.NameToLayer ("Player Bullet")) {
			CauseDamage(other.GetComponent<Bullet>().damage);
			ChangeSprite(true);

			if (!frozen) {
				Debug.Log("FROZEN!");
				frozen = true;
				frozen_until = Time.time + frozen_duration;
				PhysEngine.objs.Remove(this.GetComponent<PE_Obj>());
			}
			//removed_enemies.Add(new RemovedEnemy(other.GetComponent<PE_Obj>(), Time.time + 
		}
	}

}
