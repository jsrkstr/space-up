using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvasiveManeuver : MonoBehaviour {

	public Boundry boundry;
	public float tilt;
	public float dodge;
	public float smoothing;
	public Vector2 startWait;
	public Vector2 maneuverTime;
	public Vector2 maneuverWait;

	private float currentSpeed;
	private float targetManeuver;

	private Rigidbody rb;

	void Start () {
		rb = GetComponent<Rigidbody>();
		currentSpeed = rb.velocity.z;
		StartCoroutine (Evade ());
	}

	IEnumerator Evade () {
		yield return new WaitForSeconds (Random.Range(startWait.x, startWait.y));

		while (true) {
			targetManeuver = Random.Range (1, dodge) * -Mathf.Sign(transform.position.x);
			yield return new WaitForSeconds (Random.Range(maneuverTime.x, maneuverTime.y));
			targetManeuver = 0;
			yield return new WaitForSeconds (Random.Range(maneuverWait.x, maneuverWait.y));
		}
	}
 	
	void FixedUpdate () {
		float newManeuver = Mathf.MoveTowards (GetComponent<Rigidbody>().velocity.x, targetManeuver, Time.deltaTime * smoothing);
		GetComponent<Rigidbody>().velocity = new Vector3 (newManeuver, 0.0f, currentSpeed);
		GetComponent<Rigidbody>().position = new Vector3 (
			Mathf.Clamp (GetComponent<Rigidbody>().position.x, boundry.xMin, boundry.xMax),
			0.0f,
			Mathf.Clamp (GetComponent<Rigidbody>().position.z, boundry.zMin, boundry.zMax)
		);

		GetComponent<Rigidbody>().rotation = Quaternion.Euler (0.0f, 0.0f, GetComponent<Rigidbody>().velocity.x * -tilt);
	}

//	IEnumerator Evade () {
//		yield return new WaitForSeconds (Random.Range (startWait.x, startWait.y));
//		while (true)
//		{
//			targetManeuver = Random.Range (1, dodge) * -Mathf.Sign (transform.position.x);
//			yield return new WaitForSeconds (Random.Range (maneuverTime.x, maneuverTime.y));
//			targetManeuver = 0;
//			yield return new WaitForSeconds (Random.Range (maneuverWait.x, maneuverWait.y));
//		}
//	}
//
//	void FixedUpdate () {
//		float newManeuver = Mathf.MoveTowards (GetComponent<Rigidbody>().velocity.x, targetManeuver, smoothing * Time.deltaTime);
//		GetComponent<Rigidbody>().velocity = new Vector3 (newManeuver, 0.0f, currentSpeed);
//		GetComponent<Rigidbody>().position = new Vector3
//			(
//				Mathf.Clamp(GetComponent<Rigidbody>().position.x, boundry.xMin, boundry.xMax), 
//				0.0f, 
//				Mathf.Clamp(GetComponent<Rigidbody>().position.z, boundry.zMin, boundry.zMax)
//			);
//
//		GetComponent<Rigidbody>().rotation = Quaternion.Euler (0, 0, GetComponent<Rigidbody>().velocity.x * -tilt);
//	}
}
