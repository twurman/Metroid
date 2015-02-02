using UnityEngine;
using System.Collections;

public class FollowingCamera2 : MonoBehaviour {
	
	//from http://answers.unity3d.com/questions/29183/2d-camera-smooth-follow.html
	public float dampTime = 0.15f;
	private Vector3 velocity = Vector3.zero;
	public Transform target;
	public float y = 0;
	public float x = 0;
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		if (target)
		{
			Vector3 point = camera.WorldToViewportPoint(target.position);
			
			Vector3 delta = target.position - camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z)); //(new Vector3(0.5, 0.5, point.z));
			Vector3 destination = transform.position + delta;
			
			if (target.position.x < x) {
				destination.y = y;
			} else {
				destination.x = x + 9;
				destination.y = target.position.y > y ? target.position.y : y;
			}
			
			transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
		}
		
		if(Input.GetKeyDown(KeyCode.R)){
			Application.LoadLevel(Application.loadedLevel);
		}
		
	}
}