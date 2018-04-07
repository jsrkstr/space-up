using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Boundry {
	public float xMin, xMax, zMin, zMax;
}

public class PlayerController : MonoBehaviour {

	private Rigidbody rb;
	public float speed;
	public float tilt;
	public Boundry boundry;
	public SimpleTouchPad touchPad;
	public SimpleTouchAreaButton areaButton;

	public GameObject shot;
	public Transform shotSpawn;

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

		Vector3 accelerationRaw = Input.acceleration;
		Vector3 accelerationFixed = FixAcceleration (accelerationRaw);
		float moveHorizontal = accelerationFixed.x;
		float moveVertical = accelerationFixed.y;

//		Vector2 direction = touchPad.GetDirection ();
//		float moveHorizontal = direction.x;
//		float moveVertical = direction.y;

		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
		rb.velocity = movement * speed;
		rb.position = new Vector3 (
			Mathf.Clamp (rb.position.x, boundry.xMin, boundry.xMax),
			0.0f,
			Mathf.Clamp (rb.position.z, boundry.zMin, boundry.zMax)
		);

		rb.rotation = Quaternion.Euler (0.0f, 0.0f, rb.velocity.x * -tilt);
	}
}
