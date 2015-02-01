using UnityEngine;
using System.Collections;

public class Mine : MonoBehaviour {

	public Sprite normal, exploded;
	private float delay = 2f;
	private float destroyDelay = 3f;
	private float explodeTime;
	private bool exploding = false;
	private float damage = 10f;
	private bool dealtDamage = false;
	private float radius = 3f;

	// Use this for initialization
	void Start () {
		GetComponent<SpriteRenderer>().sprite = normal;
	}
	
	// Update is called once per frame
	void Update () {
		if(exploding){
			if(Time.time >= explodeTime + destroyDelay) {
				print ("destroy");
				Destroy(this.gameObject);
			} else if(Time.time >= explodeTime + delay){
				GetComponent<SpriteRenderer>().color = Color.white;
				GetComponent<SpriteRenderer>().sprite = exploded;
				foreach(Collider other in Physics.OverlapSphere(transform.position, radius)){
					if(!dealtDamage && other.gameObject.layer == LayerMask.NameToLayer("Player")){
						other.GetComponent<SamusMovement>().CauseDamage(damage);
						dealtDamage = true;
					} if(other.gameObject.layer == LayerMask.NameToLayer("Enemy")){
						other.GetComponent<Enemy>().CauseDamage(damage * 100000);
					}
				}
			} else {
				GetComponent<SpriteRenderer>().color = Color.red;
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
