using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
 

public class MainMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Play(){

		SceneManager.LoadScene (1);
	}

	public void Controls(){

		SceneManager.LoadScene (2);

	}

	public void Credits(){

		SceneManager.LoadScene (3);

	}

	public void Back(){

		SceneManager.LoadScene (0);

	}

	public void Quit(){

		Application.Quit();
	}
}