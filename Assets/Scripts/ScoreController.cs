using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour {

	//scoring
	public Text scoreText;
	public int score;
	public int enemyCount;
	public float ultCount;
	public float ultMax = 30;
	private int randomSpecial;

	public bool shieldActive = false;
	public float shieldMax = 10f;
	public float shieldTime;

	private bool rapidFire = false;
	private float rapidFireCooldown = 10;
	private float rapidFireTimer;

	public GameObject player;
	public GameObject shield;
	public GameObject PowerUp;

	public Image shieldIm;
	public Image bulletIm;

	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		score = 0;
		shieldIm.enabled = false;
		bulletIm.enabled = false;
	}
	

	void Update () {
		scoreText.text = "Score : \n" + score;


		//add power up to player
		if (enemyCount >= 10) {

			var PU = Instantiate (PowerUp, player.transform.position,Quaternion.identity);
			Destroy (PU, 3);


			//randomize chances
			randomSpecial = Random.Range (0, 10);

			//activate shield
			if (randomSpecial <= 5) {
				shield.gameObject.SetActive (true);
				shieldIm.enabled = true;
				shieldActive = true;
				shieldTime = shieldMax;

			}
			//activate rapid fire
			if (randomSpecial > 5) {
				player.gameObject.GetComponent<PlayerController> ().fireCooldown = 0.1f;
				bulletIm.enabled = true;
				rapidFire = true; 
				rapidFireTimer = rapidFireCooldown;
			}

			enemyCount = 0;
			
		}


		if (ultCount >= ultMax) {
			//allow ultimate
			player.GetComponent<PlayerController>().allowUltimate = true;
		}


		//shield
		if(shieldActive)
			shieldTime -= Time.deltaTime;

		if (shieldTime <= 0 || !shieldActive) {
			shield.gameObject.SetActive (false);
			shieldIm.enabled = false;
		}



		//rapid fire
		if (rapidFire)
			rapidFireTimer -= Time.deltaTime;

		if (rapidFireTimer <= 0) {
			rapidFire = false;
			bulletIm.enabled = false;
			player.gameObject.GetComponent<PlayerController> ().fireCooldown = 0.3f;
		}


	}
}
