using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldManager : MonoBehaviour {

	public GameObject levelController;
	public int shieldLife = 3;


	void Awake(){

		levelController = GameObject.FindGameObjectWithTag ("LevelController");
		shieldLife = 3;
	}


	void Update(){
		if(shieldLife <= 0) {
			levelController.GetComponent<ScoreController> ().shieldActive = false;
		}

	}

	void OnTriggerEnter2D(Collider2D hit){

		if (hit.gameObject.CompareTag ("Enemy")) {
			Destroy (hit.gameObject);
			shieldLife--;
		}
			
	}

}
