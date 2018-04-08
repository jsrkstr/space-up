using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByContact : MonoBehaviour {

	public GameObject explosion;
	public GameObject playerExplosion;
	public int hitValue;
	public int scoreValue;
	private GameController gameController;

	void Start () {
		GameObject gameControllerObject = GameObject.FindWithTag ("GameController");
		if (gameControllerObject != null) {
			gameController = gameControllerObject.GetComponent <GameController> ();
		}
		if (gameControllerObject == null) {
			Debug.Log ("Cannnot find 'GameController' script");
		}
	}

	void OnTriggerEnter(Collider other) {
		if (other.CompareTag ("Boundary") || other.CompareTag ("Enemy")) {
			return;
		}

		Destroy (gameObject);

		if (explosion != null) {
			Instantiate (explosion, transform.position, transform.rotation);
		}

		if (other.CompareTag ("Player")) {
			gameController.HitPlayer (hitValue);

			if (gameController.GetHealth () == 0) {
				Destroy (other.gameObject);
				if (playerExplosion != null) {
					Instantiate (playerExplosion, transform.position, transform.rotation);
				}
			}
			return;
		}

		gameController.AddScore (scoreValue);
		Destroy (other.gameObject);

	}
}
