/// ----------------------------------
/// <summary>
/// Name: PlayerController.cs
/// Author: David Azouz
/// Date Created: 20/06/2016
/// Date Modified: 6/2016
/// ----------------------------------
/// Brief: Player Controller class that controls the player.
/// viewed: https://unity3d.com/learn/tutorials/projects/roll-a-ball/moving-the-player
/// http://wiki.unity3d.com/index.php?title=Xbox360Controller
/// *Edit*
/// - Player state machine - 20/06/2016
/// TODO:
/// - More than one player added - /6/2016
/// - 
/// </summary>
/// ----------------------------------

using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public const uint MAX_PLAYERS = 2; //TODO: more than two players?

    // PRIVATE VARIABLES [MenuItem ("MyMenu/Do Something")]
    [Header("Movement")]
    public float playerSpeed;
    public float speedBoost = 6;
    [Tooltip("This will change at runtime.")]
    public string verticalAxis = "Vertical";
    public string horizontalAxis = "Horizontal";
    public string rotationAxisX = "Rotation_X";
    public string rotationAxisY = "Rotation_X";

    [Header("Weapon")]
    public GameObject r_weapon;
    //public GameObject r_gameOverPanel;
    //public GameObject r_bombExplosionParticleEffect;
    //public Camera r_camera;

    //choosing player states
    [HideInInspector]
    public enum E_PLAYER_STATE
    {
        E_PLAYER_STATE_ALIVE,
        E_PLAYER_STATE_WEAPON, //TODO: add more?
        E_PLAYER_STATE_DEAD,

        E_PLAYER_STATE_COUNT,
    };

    public E_PLAYER_STATE m_eCurrentPlayerState;

    // PRIVATE VARIABLES
    // A way to identidy players
    [SerializeField] private uint m_playerID = 0;
    //Rigidbody m_rigidBody;
    private float fRot = 0.2f;

    // Use this for initialization
    void Start ()
    {
        //setting our current state to alive
        m_eCurrentPlayerState = E_PLAYER_STATE.E_PLAYER_STATE_ALIVE;

        // Loops through our players and assigns variables for input from different controllers
        for (uint i = 0; i < MAX_PLAYERS; ++i)
        {
            if (m_playerID == i)
            {
                verticalAxis = "P" + (i + 1) + "_Vertical";
                horizontalAxis = "P" + (i + 1) + "_Horizontal";
                rotationAxisX = "P" + (i + 1) + "_Rotation_X";
                rotationAxisY = "P" + (i + 1) + "_Rotation_Y";
            }
        }        
    }
	
	// Update is called once per frame
	void Update ()
    {
        //creating a variable that gets the input axis
        float moveHorizontal = Input.GetAxis(horizontalAxis);
        float moveVertical = Input.GetAxis(verticalAxis);
        float moveRotationX = Input.GetAxis(rotationAxisX);
        float moveRotationY = Input.GetAxis(rotationAxisY);

        // Movement
        if (moveHorizontal < -fRot || moveHorizontal > fRot ||
            moveVertical < -fRot || moveVertical > fRot)
        {
            Vector3 movementDirection = new Vector3(moveHorizontal, 0.0f, moveVertical);
            Vector3 pos = transform.position + movementDirection * playerSpeed * Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, pos, 0.2f);
        }

        // If we are rotating
        // Rotation/ Direction with the (right) analog stick
        if (moveRotationX < -fRot || moveRotationX > fRot ||
            moveRotationY < -fRot || moveRotationY > fRot)
        {
            transform.forward = new Vector3(moveRotationX, 0.0f, moveRotationY);
        }

        // if we topple over
        if (Input.GetButton("Reset"))
        {
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.identity, 0.2f);
        }

        //m_rigidBody.AddForce(movementDirection * playerSpeed * Time.deltaTime);

        //Switches between player states
        #region Player States
        switch (m_eCurrentPlayerState)
        {
            //checks if the player is alive
            case E_PLAYER_STATE.E_PLAYER_STATE_ALIVE:
                {
                    r_weapon.SetActive(false);
                    //Debug.Log("Alive!");
                    break;
                }
            case E_PLAYER_STATE.E_PLAYER_STATE_WEAPON:
                {
                    r_weapon.SetActive(true);
                    //Debug.Log("Bomb!");
                    break;
                }
            //if player is dead
            case E_PLAYER_STATE.E_PLAYER_STATE_DEAD:
                {
                    // Particle effect bomb (explosion)
                    //r_bombExplosionParticleEffect.SetActive(true);
                    // actions to perform after a certain time
                    uint uiBombEffectTimer = 2;
                    Invoke("BombEffectDead", uiBombEffectTimer);
                    Debug.Log("Dead :(");
                    break;
                }
            default:
                {
                    Debug.LogError("No State Chosen!");
                    break;
                }
        }
        #endregion
	}

    void BombEffectDead()
    {
        r_weapon.SetActive(false);
        Destroy(this.gameObject);
        //r_gameOverPanel.SetActive(true); 
        Time.timeScale = 0;
        // After three seconds, return to menu
        Invoke("ReturnToMenu", 1);
        Debug.Log("Bomb Effect");
    }

    void ReturnToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public E_PLAYER_STATE ChangeStateWeapon()
    {
        return E_PLAYER_STATE.E_PLAYER_STATE_WEAPON;
    }

    public E_PLAYER_STATE ChangeStateDead()
    {
        return E_PLAYER_STATE.E_PLAYER_STATE_DEAD;
    }

    public void SetPlayerStateDead(uint a_uiPlayerStateDead)
    {
        m_eCurrentPlayerState = (E_PLAYER_STATE)a_uiPlayerStateDead;
    }

    public uint GetPlayerID()
    {
        return m_playerID;
    }

    public void SetPlayerID(uint a_uiPlayerID)
    {
        m_playerID = a_uiPlayerID; 
    }

    // Upon Collision TODO: is this still needed?
    /*void OnCollisionEnter()
    {
        Vector3 v3PreviousPos = transform.localPosition;
        transform.parent.position = transform.localPosition;
        transform.position = v3PreviousPos;
    } */

    /*void OnCollisionStay(Collision a_collision)
    {
        //if current players state is "BOMB"
        if (m_eCurrentPlayerState == E_PLAYER_STATE.E_PLAYER_STATE_BOMB &&
            a_collision.collider.GetComponent<PlayerController>().m_eCurrentPlayerState == E_PLAYER_STATE.E_PLAYER_STATE_ALIVE)
        {
            //And is colliding with another player
            if (a_collision.collider.tag == "Player")
            {
                // change the other players state
                a_collision.collider.GetComponent<PlayerController>().m_eCurrentPlayerState = E_PLAYER_STATE.E_PLAYER_STATE_BOMB;
                //change the state of ourselves to alive
                m_eCurrentPlayerState = E_PLAYER_STATE.E_PLAYER_STATE_ALIVE;
                //r_camera.fieldOfView = Mathf.Lerp(70, 60, Time.time);
                Debug.Log("CHECK IF COLLIDED: OnCollExit");
            }
        }
        else
        {
            Debug.Log("GLHF");
        }
    } */

}
