using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
 

public class WinState : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Play(){

		SceneManager.LoadScene (1);
	}


	public void MainMenu(){

		SceneManager.LoadScene (0);

	}

	public void Quit(){

		Application.Quit();
	}
}