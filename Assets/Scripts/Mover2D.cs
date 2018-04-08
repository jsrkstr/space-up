using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover2D : MonoBehaviour {

	private Rigidbody rb;
	public float maxSpeed;
	public Boundry boundry;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();
	}

	// Update is called once per frame
	void FixedUpdate () {

		// Limit velocity of ship
		rb.velocity = new Vector3 (
			Mathf.Clamp (rb.velocity.x, -maxSpeed, maxSpeed),
			0.0f,
			Mathf.Clamp (rb.velocity.z, -maxSpeed, maxSpeed)
		);

		// Make ship appear on other side of wall
		float positionX = rb.position.x;
		positionX = positionX < boundry.xMin ? boundry.xMax : positionX;
		positionX = positionX > boundry.xMax ? boundry.xMin : positionX;

		// Make ship appear on other side of wall
		float positionZ = rb.position.z;
		positionZ = positionZ < boundry.zMin ? boundry.zMax : positionZ;
		positionZ = positionZ > boundry.zMax ? boundry.zMin : positionZ;

		// Make ship appear on other side of wall
		rb.position = new Vector3 (positionX, 0.0f, positionZ);

		// Make the ship face towards the direction of movement
		if (rb.velocity.magnitude != 0.0f) { // fixes warning
			Quaternion newRotation = Quaternion.LookRotation (rb.velocity);
			// keep z rotations as same, since it is set by other scripts
			newRotation *= Quaternion.Euler (0, 0, rb.rotation.eulerAngles.z);
			rb.rotation = newRotation;
		}

	}
}
