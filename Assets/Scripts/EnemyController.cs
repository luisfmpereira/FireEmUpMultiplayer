using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {


	public GameObject levelController;
	public int enemyScore = 100;

	//move variables
	public GameObject player;
	private Transform enemyTransf;
	public float moveSpeed;

	//shooting variables
	private float cooldown = 2f;
	private float shootingCounter;
	public Rigidbody2D bulletPrefab;
	public bool allowShooting;

	//health and drops
	public int enemyHealth = 1;
	public float dropChance;
	public GameObject [] foodPrefabs;

	void Start () {
		enemyTransf = GetComponent<Transform> ();
		player = GameObject.FindGameObjectWithTag ("Player");
		levelController = GameObject.FindGameObjectWithTag ("LevelController");

		shootingCounter = 0f;
	}
	

	void Update () {
		
		MoveEnemy ();

		if(allowShooting)
			ShootEnemy ();
		
		if (enemyHealth <= 0)
			KillEnemy ();
	}


	void MoveEnemy(){
		
		enemyTransf.position = Vector2.MoveTowards (enemyTransf.position, player.transform.position, moveSpeed*Time.deltaTime);

		//rotate enemy sprite towards player
		var direction =  player.transform.position - enemyTransf.position;
		var angle = Mathf.Atan2 (direction.y, direction.x) * Mathf.Rad2Deg;

		enemyTransf.rotation = Quaternion.Euler (0f, 0f, angle);
	}

	void ShootEnemy(){

		shootingCounter += Time.deltaTime;

		var direction =  (player.transform.position - enemyTransf.position);

		if (shootingCounter >= cooldown) {
			
			shootingCounter = 0;

			var bullet = Instantiate (bulletPrefab, enemyTransf.position, enemyTransf.rotation) as Rigidbody2D;

			bullet.AddForce (300 * new Vector2(direction.x,direction.y).normalized);

		}

	}


	void KillEnemy(){
		
		float drop = Random.Range (0, 10);
		var foodOfChoice = Random.Range (0, foodPrefabs.Length);

		if (drop <= dropChance) {
			GameObject food = Instantiate (foodPrefabs [foodOfChoice], this.transform.position, Quaternion.identity);

			Destroy (food.gameObject, 5);

		}

		Destroy (this.gameObject);

		levelController.GetComponent<ScoreController> ().score += enemyScore;
		levelController.GetComponent<ScoreController> ().enemyCount++;

		if(!player.GetComponent<PlayerController>().usingUlt)
			levelController.GetComponent<ScoreController> ().ultCount++;

	}



	void OnTriggerEnter2D(Collider2D hit){

		if (hit.gameObject.CompareTag ("PlayerBullet")) {
			enemyHealth--;
		}
		if (hit.gameObject.CompareTag ("UltKillzone")) {
			enemyHealth=0;
		}


	}

}
