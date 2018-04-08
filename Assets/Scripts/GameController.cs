using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

	public GameObject[] hazards;
	public Vector3 spawnValues;
	public int hazardCount;
	public float spawnWait;
	public float startWait;
	public float waveWait;

	public GameObject restartButton;

	public Text scoreText;
	public Text highscoreText;
	public Text healthText;
	public GUIText gameOverText;
	private int score;
	private int highscore;
	private int healthValue = 100;

	private bool gameOver;

	// Use this for initialization
	void Start () {
		gameOver = false;
		restartButton.SetActive (false);
		gameOverText.text = "";
		score = 0;
		highscore = PlayerPrefs.GetInt ("highscore", highscore);
		UpdateScore ();
		UpdateHealth ();
		StartCoroutine (SpawnWaves ());
	}

	void Update () {
//		if (restart) {
//			if (Input.GetButton("Fire1")) {
//				Application.LoadLevel (Application.loadedLevel);
//			}
// 		}
	}
	
	IEnumerator SpawnWaves () {
		Vector3 spawnPosition;
		Quaternion spawnRotation;
		yield return new WaitForSeconds (startWait);

		while (true) {
			for (int i = 0; i < hazardCount; i++) {
				GameObject hazard = hazards [Random.Range (0, hazards.Length)];
				spawnPosition = new Vector3 (Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
				spawnRotation = Quaternion.identity;

				if (hazard.name == "Attacking Enemy Ship") {
					spawnPosition = new Vector3 (0, spawnValues.y, -spawnValues.z);
					spawnRotation = Quaternion.Euler(0, 180, 0);
				}

				Instantiate (hazard, spawnPosition, spawnRotation);
				yield return new WaitForSeconds (spawnWait);

				if (gameOver) {
					yield return new WaitForSeconds (2);
					restartButton.SetActive (true);
					break;
				}
			}
			yield return new WaitForSeconds (waveWait);
		}
	}

	public void AddScore (int newScoreValue) {
		score += newScoreValue;
		UpdateScore ();
	}

	public void HitPlayer(int hitValue) {
		healthValue -= hitValue;
		healthValue = Mathf.Max (0, healthValue);
		if (healthValue == 0) {
			GameOver ();
		}
		UpdateHealth ();
	}

	public void GameOver() {
		gameOverText.text = "Game Over";
		gameOver = true;
	}

	public void RestartGame() {
		Application.LoadLevel (Application.loadedLevel);
	}

	void UpdateScore () {
		scoreText.text = "Score " + score;
		if (score > highscore) {
			highscore = score;
			PlayerPrefs.SetInt ("highscore", highscore);
		}
		highscoreText.text = "Highscore " + highscore;
	}

	public int GetHealth() {
		return healthValue;
	}

	void UpdateHealth() {
		healthText.text = "Health " + healthValue;
	}
}
