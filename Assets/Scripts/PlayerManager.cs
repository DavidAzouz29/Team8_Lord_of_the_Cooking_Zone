/// <summary>
/// Author: 		David Azouz
/// Date Created: 	12/04/16
/// Date Modified: 	12/04/16
/// --------------------------------------------------
/// Brief: A Player Manager class that handles players.
/// viewed https://drive.google.com/drive/folders/0B67Mvyh-0w-RTlhLX1lsOHRfdDA
/// https://unity3d.com/learn/tutorials/projects/survival-shooter/more-enemies
/// 
/// ***EDIT***
/// - 	- David Azouz 11/04/16
/// -  - David Azouz 11/04/16
/// -  - David Azouz 12/04/16
/// 
/// TODO:
/// - change remove const from MAX_PLAYERS
/// </summary>

using UnityEngine;
using System.Collections;

//#define MAX_PLAYERS 4

public class PlayerManager : MonoBehaviour
{
    //----------------------------------
    // PUBLIC VARIABLES
    //----------------------------------
    public const uint MAX_PLAYERS = 4; // TODO: change in Player Controller
    public GameObject r_Player; // Referance to a player.
    public Vector3 v3PlayerPosition = Vector3.zero;

    public Color[] colorsArray = new Color[MAX_PLAYERS];
    public PlayerController[] uiPlayerArray = new PlayerController[MAX_PLAYERS]; //TODO: private
    public PlayerController r_PlayerController; // Referance to a player.

    //----------------------------------
    // PRIVATE VARIABLES
    //----------------------------------
    private float fTerrRadius = 0.66f;

    // Use this for initialization
    void Start ()
    {
        v3PlayerPosition.y = 0.39f;

        float fTransparency = 0.20f;

        // Array of colors
        colorsArray[0] = new Vector4(Color.red.r,   Color.red.g,    Color.red.b,    fTransparency);
        colorsArray[1] = new Vector4(Color.yellow.r,Color.yellow.g, Color.yellow.b, fTransparency);
        colorsArray[2] = new Vector4(Color.green.r, Color.green.g,  Color.green.b,  fTransparency);
        colorsArray[3] = new Vector4(Color.blue.r,  Color.blue.g,   Color.blue.b,   fTransparency);

        
        //Loop through and create our players.
        for (uint i = 0; i < MAX_PLAYERS; ++i)
        {
            v3PlayerPosition.x = Random.Range(-fTerrRadius, fTerrRadius); //
            v3PlayerPosition.z = Random.Range(-fTerrRadius, fTerrRadius); //
            Object j = Instantiate(r_Player, v3PlayerPosition, r_Player.transform.rotation);
            MeshRenderer[] mesh = ((GameObject)j).GetComponentsInChildren<MeshRenderer>();
            // Set Color to color in array
            for(ushort k = 0; k < MAX_PLAYERS; ++k)
            {
                mesh[k].material.SetColor("_Color", colorsArray[i]);
            }
            r_PlayerController = ((GameObject)j).GetComponent<PlayerController>();
            r_PlayerController.SetPlayerID(i);

            uiPlayerArray[i] = r_PlayerController;
            Debug.Log(v3PlayerPosition);
            // Select a player to assign the bomb to
            /*if (i == randSelection)
            {
                r_PlayerController.m_eCurrentPlayerState = r_PlayerController.ChangeStateBomb();
            } //*/
            /* r_PlayerController = GetComponent<PlayerController>();
            //r_PlayerController.SetPlayerID(i); */
        }
        //uiPlayerArray[(int)randSelection].ChangeStateBomb();
    }
	
	// Update is called once per frame
	void Update ()
    {
	    
	}

    /*uint GetRandSelection()
    {
        return randSelection;
    }*/
}
