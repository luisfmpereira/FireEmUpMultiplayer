using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletController : MonoBehaviour {


	void Update () {

		Destroy (this.gameObject, 3);

	}

	void OnTriggerEnter2D(Collider2D hit){

		if (hit.gameObject.CompareTag ("Enemy")) {
			Destroy(this.gameObject);
		}
	}
}
