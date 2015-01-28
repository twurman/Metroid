using UnityEngine;
using System.Collections;

public class FollowingCamera : MonoBehaviour {

	//from http://answers.unity3d.com/questions/29183/2d-camera-smooth-follow.html
	public float dampTime = 0.15f;
	private Vector3 velocity = Vector3.zero;
	public Transform target;
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		if (target)
		{
			Vector3 point = camera.WorldToViewportPoint(target.position);

			Vector3 delta = target.position - camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z)); //(new Vector3(0.5, 0.5, point.z));
			Vector3 destination = transform.position + delta;

			if (target.position.x < 120f) {
				destination.y = 6.9f;
			} else {
				destination.x = 127f;
			}

			transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
		}

		if(Input.GetKeyDown(KeyCode.R)){
			Application.LoadLevel(Application.loadedLevel);
		}
		
	}
}