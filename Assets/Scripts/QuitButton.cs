using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class QuitButton : MonoBehaviour
{
    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Application.Quit();
        }
    }

    void OnMouseEnter()
    {
        GetComponentInChildren<Light>().enabled = true;
    }

    void OnMouseExit()
    {
        GetComponentInChildren<Light>().enabled = false;
    }
}