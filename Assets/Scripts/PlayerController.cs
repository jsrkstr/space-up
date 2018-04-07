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

	public GameObject shot;
	public Transform shotSpawn;

	public float fireRate;
	private float nextFire = 0.0f;
	private AudioSource audioSource;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();
		audioSource = GetComponent<AudioSource >();
	}

	void Update () {
		if (Input.GetButton("Fire1") && Time.time > nextFire) {
			nextFire = Time.time + fireRate;	
			// Game/Object clone = 
			Instantiate (shot, shotSpawn.position, shotSpawn.rotation); // as GameObject;
			audioSource.Play();
		}
	}
 
	// Update is called once per frame
	void FixedUpdate () {
//		float moveHorizontal = Input.GetAxis ("Horizontal");
//		float moveVertical = Input.GetAxis ("Vertical");
		float moveHorizontal = Input.acceleration.x;
		float moveVertical = Input.acceleration.y;

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
