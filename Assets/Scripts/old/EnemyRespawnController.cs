using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRespawnController : MonoBehaviour {


	public float respawnRate = 5f;
	public float respawnCounter;
	public Transform respawnArea;
	public GameObject [] newEnemy;
	public bool horizontalSpawn;

	public int numberOfEnemies = 1;

	public int levelControllerScore;


	void Start () {
		respawnArea = GetComponent<Transform> ();

	}


	void Update () {
		levelControllerScore = GameObject.FindGameObjectWithTag ("LevelController").GetComponent<ScoreController> ().score;
		respawnCounter -= Time.deltaTime;

		numberOfEnemies = (1 + levelControllerScore / 2000);

		if (respawnCounter <= 0) {
			respawnCounter = respawnRate;
			SpawnEnemy (horizontalSpawn, numberOfEnemies);
		}

	}

	void SpawnEnemy (bool horizontal, int amount){

		if (horizontal) {
			for(int i = 0; i< amount;i++)
				Instantiate (newEnemy [Random.Range (0, newEnemy.Length)], new Vector3 (Random.Range (-10, 10), respawnArea.position.y, 0), Quaternion.identity);
		} else {
			for(int i = 0; i<= amount;i++)
				Instantiate (newEnemy [Random.Range (0, newEnemy.Length)], new Vector3 (respawnArea.position.x, Random.Range (-10, 10), 0), Quaternion.identity);
		}
	}
			
}
