using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletController : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D hit){

		if (hit.gameObject.CompareTag ("Shield")) {

			Destroy (this.gameObject);
		}

		if (hit.gameObject.CompareTag ("UltKillzone")) {

			Destroy (this.gameObject);
		}
	}


	void Update(){

		Destroy (this.gameObject, 3);

	}
}
