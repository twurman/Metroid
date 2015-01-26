using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SamusMovement : MonoBehaviour
{
		public float BulletOffset_x = 0f;
		public float BulletOffset_y = 0f;
		public float BulletOffset_y_vertical = 0f;
		public float BulletDamage = 0f;
		public Text  HealthCounter;
		public float fireRate = 0.5F;
		private float nextFire = 0.0F;
		public GameObject projectile;
		public float health = 0f;
		public bool crouching = false;

		private void UpdateHealthCounter ()
		{
				HealthCounter.text = "EN..";
				if(health < 10){
					HealthCounter.text += "0";
				}
				HealthCounter.text += health;
		}
		
		void Start ()
		{
				UpdateHealthCounter ();
		}

		void Update ()
		{
				if (Input.GetButton ("Fire") && Time.time > nextFire) {
						nextFire = Time.time + fireRate;

						GameObject clone;
						if (Input.GetButton ("Vertical")) {
								Vector3 bullet_pos = new Vector3 (
											transform.position.x,
			                                transform.position.y + BulletOffset_y_vertical,
											transform.position.z);
								clone = Instantiate (projectile, bullet_pos, transform.rotation) as GameObject;
								PE_Obj pe = clone.GetComponent<PE_Obj> ();
								pe.vel = new Vector3 (0, 1, 0);
						} else {
								Vector3 bullet_pos = new Vector3 (
											transform.position.x + (BulletOffset_x * transform.localScale.x),
			                                transform.position.y + BulletOffset_y,
											transform.position.z);
								clone = Instantiate (projectile, bullet_pos, transform.rotation) as GameObject;
								PE_Obj pe = clone.GetComponent<PE_Obj> ();
								pe.vel = new Vector3 (transform.localScale.x, 0, 0);
						}

						clone.layer = LayerMask.NameToLayer ("Player Bullet");
						clone.GetComponent<Bullet> ().damage = BulletDamage;
				}
		}

		void OnTriggerEnter (Collider other)
		{
				if (other.gameObject.layer == LayerMask.NameToLayer ("Enemy Bullet")) {
						CauseDamage (other.GetComponent<Bullet> ().damage);
						// TODO : destory bullet
				}
		}

		public void CauseDamage (float amount)
		{
				health -= amount;
				if (health <= 0) {
						//Destroy(this.gameObject);
						Debug.Log ("player died");
				}
				UpdateHealthCounter ();
		}

		void FixedUpdate ()
		{
				float hSpeed = Input.GetAxis ("Horizontal");
				float hScale = transform.localScale.x;
				float vScale = transform.localScale.y;

				if (hSpeed > 0) {
						hScale = 1;
				} else if (hSpeed < 0) {
						hScale = -1;
				}

				if (Input.GetAxis ("Vertical") < 0 && GetComponent<PE_Controller>().grounded) {
						vScale = .5f;
						Vector3 pos = GetComponent<PE_Obj>().pos0;
						pos.y += vScale * transform.collider.bounds.size.y;
						GetComponent<PE_Obj>().pos0 = pos;
						crouching = true;
				} else if (Input.GetAxis ("Vertical") > 0) {
						vScale = 1;
						Vector3 pos = GetComponent<PE_Obj>().pos0;
						pos.y += vScale * transform.collider.bounds.size.y;
						GetComponent<PE_Obj>().pos0 = pos;
						crouching = false;
				}
			
				transform.localScale = new Vector3 (hScale, vScale, 1);
		}
}
