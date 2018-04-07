using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleRotation : MonoBehaviour {

	ParticleSystem particleSystem;

	// Use this for initialization
	void Start () {
		particleSystem = GetComponent<ParticleSystem> ();
	}

	void FixedUpdate () {
		particleSystem.startRotation = gameObject.transform.rotation.eulerAngles.y * Mathf.Deg2Rad;
	}
}
