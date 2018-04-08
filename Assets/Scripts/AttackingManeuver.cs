using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackingManeuver : MonoBehaviour {

	public Vector2 startWait;
	public float maneuverWait;
	public float acceleration;
	public float tilt;
	public Text debugText;

	private Rigidbody rb;
	private GameObject player;
	private Vector3 movement;

	void Start () {
		debugText = FindObjectOfType<Text> ();
		rb = GetComponent<Rigidbody> ();
		player = GameObject.FindGameObjectWithTag ("Player");
		if (player != null) {
			StartCoroutine (Attack ());
		}
	}

	IEnumerator Attack () {
		yield return new WaitForSeconds (Random.Range(startWait.x, startWait.y));

		while (true) {
			if (player == null || player.transform == null) {
				StopCoroutine ("Attack");
				break;
			}

			float moveHorizontal = player.transform.position.x - transform.position.x;
			float moveVertical = player.transform.position.z - transform.position.z;

			// Add force to ship
			movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
			rb.AddForce(movement * acceleration);


			yield return new WaitForSeconds (maneuverWait);
		}
	}


	void FixedUpdate () {
		float tiltAngle = 0.0f;
		if (movement.magnitude > 0) {
			tiltAngle = Vector3.SignedAngle (movement, transform.forward, Vector3.up);
			tiltAngle = Mathf.Clamp (tiltAngle, -tilt, tilt);
		}

		// Tilt the ship based on torque angle
		rb.rotation = Quaternion.Euler (0.0f, 0.0f, tiltAngle);
		debugText.text = "Angle - " + tiltAngle;
	}

}
