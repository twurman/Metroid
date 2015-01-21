using UnityEngine;
using System.Collections;

public class FollowingCamera : MonoBehaviour {
	
	public GameObject camTarget;
	
	public float xOffset = 0;
	public float yOffset = 0;
	
	void LateUpdate() {
		Debug.Log (this.camTarget.transform.position);
		this.transform.position = new Vector3(camTarget.transform.position.x + xOffset,
		                                      camTarget.transform.position.y + yOffset,
		                                      this.transform.position.z);
	}
}
