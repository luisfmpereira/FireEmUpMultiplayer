using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SplashScreenController : MonoBehaviour {

	public void StartGame(){

		SceneManager.LoadScene ("MainMenu");
	}

}
