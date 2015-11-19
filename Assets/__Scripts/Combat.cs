using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Combat : MonoBehaviour 
{
    private int damageToEnemy = 0;

    private GameManager m_Game;
    private GameObject m_Canvas;
    private Character m_Char;
    private Animator m_Animator;
    private InputManager m_Input;
    private AudioManager m_Audio;

    public GameObject arrow;
    public GameObject bow;

    bool animationDelay;
    private bool hasBow = false; // only allow shooting once bow is obtained

	// Use this for initialization
	void Start () {
        m_Canvas = GameObject.Find ( "Canvas" );
        m_Game = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>();
        m_Input = m_Game.GetInputManager();
        m_Audio = m_Game.GetAudioManager();
        m_Char = GameObject.FindGameObjectWithTag("Char").GetComponent<Character>();
        m_Animator = GetComponent<Animator>();

        animationDelay = false;
	}
	
	// Update is called once per frame
	void Update () 
    {
        if ( m_Char.GetPlayerState () != Character.PlayerState.Gather &&
            m_Char.GetPlayerState () != Character.PlayerState.Interact )
        {
            if ( m_Input.SelectButtonPressed () && !animationDelay )
            {
                if ( m_Char.GetPlayerState () == Character.PlayerState.Aim && hasBow )
                {
                    ShootArrow ();
                }
            }
        }
        
	}

    public void GiveBow()
    {
        hasBow = true; // you can shoot now!
    }

    public bool GetHasBow()
    {
        return hasBow;
    }

    void ShootArrow()
    {
        // Instantiate arrow, aiming code is in Arrow.cs
        if (m_Char.numArrows > 0)
        {
            m_Audio.PlayOnce("shoot");
			Quaternion arrowRotation = Quaternion.Euler(new Vector3(bow.transform.rotation.eulerAngles.x,
			                                                        bow.transform.rotation.eulerAngles.y,
			                                                        bow.transform.rotation.eulerAngles.z * (transform.localScale.x/Mathf.Abs(transform.localScale.x))));
            GameObject arrowObj = (GameObject)Instantiate(arrow, bow.transform.position, arrowRotation);
			arrowObj.transform.localScale =new Vector3( arrowObj.transform.localScale.x *  -(transform.localScale.x/Mathf.Abs(transform.localScale.x)),
			                                            arrowObj.transform.localScale.y,
			                                            arrowObj.transform.localScale.z);
            arrowObj.GetComponent<Arrow>().mouseDir();
            m_Char.SetPlayerState(Character.PlayerState.Idle);
            bow.SetActive(false);
            m_Char.numArrows--;
            m_Game.IncreaseArrowsShot();
            if (m_Char.numArrows <= 0)
            {
                m_Char.bow.GetComponent<BowScript>().swapSprite();
            }
        }
    }

    public void finishedAttacking()
    {
        m_Animator.SetBool ( "isAtking", false );
    }

    void OnTriggerStay2D ( Collider2D coll )
    {
        if ( coll.gameObject.tag == "Enemy"  )
        {
            Enemy enemy = ( Enemy )coll.gameObject.GetComponent ( "Enemy" );

            if ( damageToEnemy != 0 )
            {
                // Play hit sound
                enemy.TakeDamage ( damageToEnemy );
                damageToEnemy = 0;
            }
        }
    }

    public void animationFinished()
    {
        animationDelay = false;
    }
}
