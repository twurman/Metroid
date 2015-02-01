using UnityEngine;
using System.Collections;

public class Mine : MonoBehaviour {

	public Sprite normal, exploded;
	private float delay = 2f;
	private float explodeTime;
	private bool exploding = false;
	private float damage = 10f;

	// Use this for initialization
	void Start () {
		GetComponent<SpriteRenderer>().sprite = normal;
	}
	
	// Update is called once per frame
	void Update () {
		if(exploding){
			if(Time.time >= explodeTime + delay){
				GetComponent<SpriteRenderer>().sprite = exploded;
				foreach(Collider other in Physics.OverlapSphere(transform.position, 3f)){
					if(other.gameObject.layer == LayerMask.NameToLayer("Player")){
						other.GetComponent<SamusMovement>().CauseDamage(damage);
						print ("Player in range");
					} if(other.gameObject.layer == LayerMask.NameToLayer("Enemy")){
						other.GetComponent<Enemy>().CauseDamage(damage);
						print("enemy hit");
					}
				}
			}
		}
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.layer == LayerMask.NameToLayer ("Player")) {
			exploding = true;
			explodeTime = Time.time;
		}
	}
}
