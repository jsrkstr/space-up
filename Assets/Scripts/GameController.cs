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
	public GUIText gameOverText;
	private int score;
	private int highscore;

	private bool gameOver;
	private bool restart;

	// Use this for initialization
	void Start () {
		gameOver = false;
		restart = false;
		restartButton.SetActive (false);
		gameOverText.text = "";
		score = 0;
		highscore = PlayerPrefs.GetInt ("highscore", highscore);
		UpdateScore ();
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
		yield return new WaitForSeconds (startWait);
		while (true) {
			for (int i = 0; i < hazardCount; i++) {
				GameObject hazard = hazards [Random.Range (0, hazards.Length)];
				Vector3 spawnPosition = new Vector3 (Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
				Quaternion spawnRotation = Quaternion.identity;
				Instantiate (hazard, spawnPosition, spawnRotation);
				yield return new WaitForSeconds (spawnWait);
			}
			yield return new WaitForSeconds (waveWait);

			if (gameOver) {
				restartButton.SetActive (true);
				//restartText.text = "Tap to Restart";
				//restart = true;
				break;
			}
		}
	}

	public void AddScore (int newScoreValue) {
		score += newScoreValue;
		UpdateScore ();
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
}
