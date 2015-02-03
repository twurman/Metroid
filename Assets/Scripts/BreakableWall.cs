using UnityEngine;
using System.Collections;

public class BreakableWall : MonoBehaviour {

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.layer == LayerMask.NameToLayer ("Player Bullet")) {
			PhysEngine.objs.Remove(other.GetComponent<PE_Obj>());
			Destroy(other.gameObject);
		}
	}
}
