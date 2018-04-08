using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Boundry {
	public float xMin, xMax, zMin, zMax;
}

public class PlayerController : MonoBehaviour {

	private Rigidbody rb;
	public float acceleration;
	public float tilt;
	public SimpleTouchPad touchPad;
	public SimpleTouchAreaButton areaButton;

	public GameObject shot;
	public Transform shotSpawn;

	public Joystick joystick;

	public float fireRate;
	private float nextFire = 0.0f;
	private AudioSource audioSource;

	private Quaternion calibrationQuaternion;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();
		audioSource = GetComponent<AudioSource >();
		CalibrateAccelerometer ();
	}

	void Update () {
		// Input.GetButton("Fire1")
		bool canFire = areaButton.CanFire ();
		if (canFire && Time.time > nextFire) {
			nextFire = Time.time + fireRate;	
			FireShot ();
		}
	}

	public void FireShot () {
		Instantiate (shot, shotSpawn.position, shotSpawn.rotation); // as GameObject;
		audioSource.Play();
	}

	//Used to calibrate the Iput.acceleration input
	void CalibrateAccelerometer () {
		Vector3 accelerationSnapshot = Input.acceleration;
		Quaternion rotateQuaternion = Quaternion.FromToRotation (new Vector3 (0.0f, 0.0f, -1.0f), accelerationSnapshot);
		calibrationQuaternion = Quaternion.Inverse (rotateQuaternion);
	}

	//Get the 'calibrated' value from the Input
	Vector3 FixAcceleration (Vector3 acceleration) {
		Vector3 fixedAcceleration = calibrationQuaternion * acceleration;
		return fixedAcceleration;
	}
 
	// Update is called once per frame
	void FixedUpdate () {

		float moveHorizontal;
		float moveVertical;

//		// Input from keyboard
//		moveHorizontal = Input.GetAxis ("Horizontal");
//		moveVertical = Input.GetAxis ("Vertical");

//		// Input from accelerometer
//		Vector3 accelerationRaw = Input.acceleration;
//		Vector3 accelerationFixed = FixAcceleration (accelerationRaw);
//		moveHorizontal = accelerationFixed.x;
//		moveVertical = accelerationFixed.y;

//		// Input from touchPad
//		Vector2 direction = touchPad.GetDirection ();
//		moveHorizontal = direction.x;
//		moveVertical = direction.y;

//		// Input from joystick
		moveHorizontal = joystick.Horizontal;
		moveVertical = joystick.Vertical;

		// Add force to ship
		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical).normalized;
		rb.AddForce(movement * acceleration);

		float tiltAngle = 0.0f;
		if (movement.magnitude > 0) {
			tiltAngle = Vector3.SignedAngle (movement, transform.forward, Vector3.up);
			tiltAngle = Mathf.Clamp (tiltAngle, -tilt, tilt);
		}

		// Tilt the ship based on torque angle
		rb.rotation = Quaternion.Euler (0.0f, 0.0f, tiltAngle);

	}
}
