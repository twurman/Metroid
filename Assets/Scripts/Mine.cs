using UnityEngine;
using System.Collections;

public class Mine : MonoBehaviour {

	public Sprite normal, exploded;
	private float delay = 2f;
	private float destroyDelay = 3f;
	private float explodeTime;
	private bool exploding = false;
	private float damageInner = 5f;
	private float damageOuter = 3f;
	private bool dealtDamage = false;
	private float outerHitRadius = 2.5f;
	private float innerHitRadius = 2f;

	// Use this for initialization
	void Start () {
		GetComponent<SpriteRenderer>().sprite = normal;
	}
	
	// Update is called once per frame
	void Update () {
		if(exploding){
			if(Time.time >= explodeTime + destroyDelay) {
				Destroy(this.gameObject);
			} else if(Time.time >= explodeTime + delay){
				GetComponent<SpriteRenderer>().color = Color.white;
				GetComponent<SpriteRenderer>().sprite = exploded;
				transform.Find ("Halo").renderer.material.color = Color.red;
				foreach(Collider other in Physics.OverlapSphere(transform.position, innerHitRadius)){
					if(!dealtDamage && other.gameObject.layer == LayerMask.NameToLayer("Player")){
						other.GetComponent<SamusMovement>().CauseDamage(damageInner);
						dealtDamage = true;
					} if(other.gameObject.layer == LayerMask.NameToLayer("Enemy")){
						other.GetComponent<Enemy>().CauseDamage(damageInner * 100000);
					}
				}
				foreach(Collider other in Physics.OverlapSphere(transform.position, outerHitRadius)){
					if(!dealtDamage && other.gameObject.layer == LayerMask.NameToLayer("Player")){
						other.GetComponent<SamusMovement>().CauseDamage(damageOuter);
						dealtDamage = true;
					} if(other.gameObject.layer == LayerMask.NameToLayer("Enemy")){
						other.GetComponent<Enemy>().CauseDamage(damageOuter * 100000);
					}
				}

//				Time.timeScale = 0;  //pause for debug
			} else {
				GetComponent<SpriteRenderer>().color = Color.red;
			}
		}
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.layer == LayerMask.NameToLayer ("Player")) {
			if(!exploding){
				explodeTime = Time.time;
			}
			exploding = true;
		} if(other.gameObject.layer == LayerMask.NameToLayer("Player Bullet")){
			if(!exploding){
				explodeTime = Time.time;
			}
			exploding = true;
		}
	}
}
