using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Boundry {
	public float xMin, xMax, zMin, zMax;
}

public class PlayerController : MonoBehaviour {

	private Rigidbody rb;
	public float acceleration;
	public float maxSpeed;
	public float tilt;
	public Boundry boundry;
	public SimpleTouchPad touchPad;
	public SimpleTouchAreaButton areaButton;

	public GameObject shot;
	public Transform shotSpawn;

	public Joystick joystick;

	public float fireRate;
	private float nextFire = 0.0f;
	private AudioSource audioSource;
	private float rotationY = 0.0f;

	private Quaternion calibrationQuaternion;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();
		audioSource = GetComponent<AudioSource >();
		rotationY = rb.rotation.y;
		CalibrateAccelerometer ();
	}

	void Update () {
		// Input.GetButton("Fire1")
		bool canFire = areaButton.CanFire ();
		if (canFire && Time.time > nextFire) {
			nextFire = Time.time + fireRate;	
			// Game/Object clone = 
			Instantiate (shot, shotSpawn.position, shotSpawn.rotation); // as GameObject;
			audioSource.Play();
		}
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
//		float moveHorizontal = Input.GetAxis ("Horizontal");
//		float moveVertical = Input.GetAxis ("Vertical");

//		Vector3 accelerationRaw = Input.acceleration;
//		Vector3 accelerationFixed = FixAcceleration (accelerationRaw);
//		float moveHorizontal = accelerationFixed.x;
//		float moveVertical = accelerationFixed.y;

//		Vector3 moveVector = (transform.right * joystick.Horizontal + transform.forward * joystick.Vertical).normalized;
//		transform.Translate(moveVector * moveSpeed * Time.deltaTime);

//		Vector2 direction = touchPad.GetDirection ();
//		float moveHorizontal = direction.x;
//		float moveVertical = direction.y;

		float moveHorizontal = joystick.Horizontal;
		float moveVertical = joystick.Vertical;

		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
//		rb.velocity = movement * speed;
//		float forwardForce = transform.forward * moveVertical * speed;
//		float rightForce = transform.right * moveHorizontal * speed;
//		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
//		Vector3 force = transform.forward * moveVertical;
//		Vector3 force = transform.forward * moveVertical;
		rb.AddForce(movement * acceleration);


		rb.velocity = new Vector3 (
			Mathf.Clamp (rb.velocity.x, -maxSpeed, maxSpeed),
			0.0f,
			Mathf.Clamp (rb.velocity.z, -maxSpeed, maxSpeed)
		);



		float positionX = rb.position.x;
		positionX = positionX < boundry.xMin ? boundry.xMax : positionX;
		positionX = positionX > boundry.xMax ? boundry.xMin : positionX;

		float positionZ = rb.position.z;
		positionZ = positionZ < boundry.zMin ? boundry.zMax : positionZ;
		positionZ = positionZ > boundry.zMax ? boundry.zMin : positionZ;

		rb.position = new Vector3 (
			positionX,
			0.0f,
			positionZ
		);

	
		Quaternion rotation = Quaternion.LookRotation (rb.velocity);

//		rb.rotation = Quaternion.Euler (0.0f, rotation.y, rb.velocity.x * -tilt);
		rb.rotation = rotation;
	}
}
