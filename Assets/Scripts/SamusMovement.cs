using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SamusMovement : MonoBehaviour
{
		public bool CrouchEnabled = false;
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
		public Sprite standing, upwards, crouch1, crouch2, crouch3, crouch4;
		private int crouchTimer = 0;
		public bool gravitySwap = false;
		private PhysEngine physEngine;

		public enum Samus_Dir { // The direction in which the PE_Obj is moving
			standing,
			up,
			c1,
			c2,
			c3,
			c4
		}

		private void UpdateHealthCounter ()
		{
				HealthCounter.text = "EN..";
				if(health < 10){
					HealthCounter.text += "0";
				} else if(health > 99){
					health = 99;
				}
				HealthCounter.text += health;
		}
		
		void Start ()
		{
				UpdateHealthCounter ();
				physEngine = Camera.main.GetComponent<PhysEngine>();
		}

		void Update ()
		{
				if(transform.position.y > 12.5 && gravitySwap){
						physEngine.gravity = new Vector3(0, 1f, 0);
						GetComponent<PE_Controller>().maxSpeed = new Vector2(10f, 1f);
				} 
		
				if (Input.GetButton ("Fire") && Time.time > nextFire && !crouching) {
						nextFire = Time.time + fireRate;

						GameObject clone;
						if (Input.GetAxis("Vertical") > 0) {
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

		void ChangeSprite(Samus_Dir dir){
				switch(dir){
				case(Samus_Dir.standing):
					GetComponent<SpriteRenderer>().sprite = standing;
					break;
				case(Samus_Dir.up):
					GetComponent<SpriteRenderer>().sprite = upwards;
					break;
				case(Samus_Dir.c1):
					GetComponent<SpriteRenderer>().sprite = crouch1;
					break;
				case(Samus_Dir.c2):
					GetComponent<SpriteRenderer>().sprite = crouch2;
					break;
				case(Samus_Dir.c3):
					GetComponent<SpriteRenderer>().sprite = crouch3;
					break;
				case(Samus_Dir.c4):
					GetComponent<SpriteRenderer>().sprite = crouch4;
					break;
				}
		}

		void OnTriggerEnter (Collider other)
		{
				if (other.gameObject.layer == LayerMask.NameToLayer ("Enemy Bullet")) {
						CauseDamage (other.GetComponent<Bullet> ().damage);
						PhysEngine.objs.Remove(other.GetComponent<PE_Obj>());
						Destroy(other.gameObject);
				}
				if(other.gameObject.layer == LayerMask.NameToLayer("Ground")){
					GetComponent<PE_Controller>().falling = true;
				}
		}

		public void CauseDamage (float amount)
		{
				health -= amount;
				if (health <= 0) {
						//Destroy(this.gameObject);
						Debug.Log ("player died");
						Application.LoadLevel(Application.loadedLevel);
				}
				UpdateHealthCounter ();
		}

		void FixedUpdate ()
		{
				crouchTimer++;
				if(crouching){
					if(crouchTimer % 4 == 0){
						ChangeSprite(Samus_Dir.c1);
					} else if(crouchTimer % 4 == 1){
						ChangeSprite(Samus_Dir.c2);
					} else if(crouchTimer % 4 == 2){
						ChangeSprite(Samus_Dir.c3);
					} else if(crouchTimer % 4 == 3){
						ChangeSprite(Samus_Dir.c4);
					}
				} else if(Input.GetAxis ("Vertical") > 0){
					ChangeSprite(Samus_Dir.up);
				} else {
					ChangeSprite(Samus_Dir.standing);
				}


				float hSpeed = Input.GetAxis ("Horizontal");
				float hScale = transform.localScale.x;
				float vScale = transform.localScale.y;

				if (hSpeed > 0) {
						hScale = 1;
				} else if (hSpeed < 0) {
						hScale = -1;
				}

				if (CrouchEnabled && Input.GetAxis ("Vertical") < 0 && GetComponent<PE_Controller>().grounded && !crouching) {
						vScale = .5f;
						Vector3 pos = GetComponent<PE_Obj>().pos0;
						pos.y += vScale * transform.collider.bounds.size.y;
						GetComponent<PE_Obj>().pos0 = pos;
						crouching = true;
				} else if (Input.GetAxis ("Vertical") > 0 && crouching) {
						vScale = 1;
						
						Vector3 pos = GetComponent<PE_Obj>().pos0;
						pos.y += vScale * transform.collider.bounds.size.y;

						if (CanStand()) {
							GetComponent<PE_Obj>().pos0 = pos;
							crouching = false;
						} else {
							vScale = .5f;
						}
				}
			
				transform.localScale = new Vector3 (hScale, vScale, 1);
				
		}

	public bool CanStand() {
		Vector3 pos = GetComponent<PE_Obj>().pos0;
		pos.y += transform.collider.bounds.size.y;
		
		Vector3 StandingCheck = new Vector3(transform.position.x, pos.y, pos.z);

		return Physics.OverlapSphere(new Vector3(transform.position.x - transform.collider.bounds.size.x / 2f, pos.y, pos.z), 0.1f).Length == 0 &&
			Physics.OverlapSphere(new Vector3(transform.position.x + transform.collider.bounds.size.x / 2f, pos.y, pos.z), 0.1f).Length == 0;
	}
}
