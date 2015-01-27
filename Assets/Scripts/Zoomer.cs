﻿using UnityEngine;
using System.Collections;

public enum Directions {
	up, down, left, right
}

public class Zoomer : MonoBehaviour {

	private PE_Obj peo;

	private bool established = false;

	public Directions GroundDirection;

	public float GroundAcceleration = 10f;

	public float Velocity = .5f;

	public float DirectionChangeVelocity = 1f;

	// Use this for initialization
	void Start () {
		peo = this.GetComponent<PE_Obj>();
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log(peo.vel + "," + peo.acc + "," + GroundDirection);

		switch (GroundDirection) {
		case Directions.up:
			peo.acc = new Vector3(0f, GroundAcceleration, 0f);
			peo.vel.x = Velocity;

			if (peo.vel.y < DirectionChangeVelocity) {
				established = true;
			}

			if (established && peo.vel.y > DirectionChangeVelocity) {
				GroundDirection = Directions.left;
				established = false;
				peo.acc = Vector3.zero;
				peo.vel = Vector3.zero;
			}
			break;

		case Directions.left:
			peo.acc = new Vector3(-1*GroundAcceleration, 0f, 0f);
			peo.vel.y = Velocity;

			if (peo.vel.x > -1 * DirectionChangeVelocity) {
				established = true;
			}

			if (established && peo.vel.x < -1 * DirectionChangeVelocity) {
				GroundDirection = Directions.down;
				established = false;
				peo.acc = Vector3.zero;
				peo.vel = Vector3.zero;
			}
			break;

		case Directions.down:
			peo.acc = new Vector3(0f, -1*GroundAcceleration, 0f);
			peo.vel.x = -1 * Velocity;

			if (peo.vel.y > -1 * DirectionChangeVelocity) {
				established = true;
			}

			if (established && peo.vel.y < -1 * DirectionChangeVelocity) {
				GroundDirection = Directions.right;
				established = false;
				peo.acc = Vector3.zero;
				peo.vel = Vector3.zero;
			}
			
			break;

		case Directions.right:
			peo.acc = new Vector3(GroundAcceleration, 0f, 0f);
			peo.vel.y = -1 * Velocity;

			if (peo.vel.x > DirectionChangeVelocity) {
				established = true;
			}

			if (established && peo.vel.x > DirectionChangeVelocity) {
				GroundDirection = Directions.up;
				established = false;
				peo.vel = Vector3.zero;
				peo.acc = Vector3.zero;
			}
			break;


		}
	}
}