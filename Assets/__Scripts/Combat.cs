using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Combat : MonoBehaviour 
{
    private float clickTimer = 0.0f;
    private float prevClickTimer = 0.0f;
    private float fadeTimer = 0.0f;
    private float delayBetweenMultClicks = 0.75f;
    private int multClickCountLeft = 0;
    private int multClickCountRight = 0;
    private int damageToEnemy = 0;

    private GameManager m_Game;
    private GameObject m_Canvas;
    private GameObject m_ComboTextObj;
    private Text m_ComboText;
    private Character m_Char;
    private Animator m_Animator;
    private InputManager m_Input;
    private AudioManager m_Audio;

    public GameObject arrow;

    bool animationDelay;

	// Use this for initialization
	void Start () {
        m_Canvas = GameObject.Find ( "Canvas" );
        m_ComboTextObj = ( GameObject )Instantiate ( Resources.Load ( "ComboText" ) );
        m_ComboTextObj.SetActive ( false );
	    m_ComboText = m_ComboTextObj.GetComponent<Text>();
        m_ComboText.rectTransform.parent = m_Canvas.transform;
        m_ComboText.rectTransform.localPosition = transform.position;

        m_Game = GameObject.Find ( "GameManager" ).GetComponent<GameManager>();
        m_Input = m_Game.GetInputManager();
        m_Audio = m_Game.GetAudioManager();
        m_Char = GameObject.Find ( "Character" ).GetComponent<Character>();
        m_Animator = GetComponent<Animator>();

        animationDelay = false;
	}
	
	// Update is called once per frame
	void Update () 
    {
        fadeTimer -= Time.deltaTime;
        if ( fadeTimer < 0 )
        {
	        m_ComboTextObj.SetActive( false );
	    }

        if ( m_Char.GetPlayerState () != Character.PlayerState.Gather && 
            m_Char.GetPlayerState () != Character.PlayerState.Interact &&
            m_Game.GetGameState() != GameManager.GameState.Tutorial )
        {
            if (m_Input.normalAttackButtonPressed() && !animationDelay && !EventSystem.current.IsPointerOverGameObject())
            {
                if( m_Char.GetPlayerState() == Character.PlayerState.Aim )
                {
                    m_Audio.PlayOnce ( "click" );
                    ShootArrow ();                    
                }
                else if ( m_Char.UseStamina ( 30 ) )
                {
                    clickTimer = Time.time;
                    damageToEnemy = 10;
                    LeftClickCombo ();
                    //animationDelay = true; //uncomment when we have all animations
                }
            }

            if (m_Input.smashAttackButtonPressed() && !animationDelay && !EventSystem.current.IsPointerOverGameObject())
            {
                if ( m_Char.UseStamina ( 50 ) )
                {
                    clickTimer = Time.time;
                    damageToEnemy = 20;
                    RightClickCombo ();
                    animationDelay = true; //uncomment when we have all animations
                }
            }
        }
	}

    void LeftClickCombo ()
    {
        if ( !IsComboClick() )
        {
            multClickCountLeft = 0;
            multClickCountRight = 0;
        }

        switch ( multClickCountLeft )
        {
            case 0:
                // Perform basic move 1 left click
                ShowComboText ( "LEFT 1!" );
                m_Audio.PlayOnce ( "left1" );
                m_Animator.SetTrigger("atkL1");
                animationDelay = true; //take this line out once we get all animations
				break;
            case 1:
                // Perform basic move 2 left click
                ShowComboText ( "LEFT 2!" );
                m_Audio.PlayOnce ( "left2" );
                m_Animator.SetTrigger("atkL1");
                animationDelay = true; //take this line out once we get all animations
				break;
            case 2:
                // Perform basic move 3 left click
                ShowComboText ( "LEFT 3!" );
                m_Audio.PlayOnce ( "left3" );
                m_Animator.SetTrigger("atkL1");
                animationDelay = true; //take this line out once we get all animations
				break;
            case 3:
                // Perform basic move 4 left click
                ShowComboText ( "LEFT 4!" );
                m_Audio.PlayOnce ( "left4" );
                m_Animator.SetTrigger("atkL1");
                animationDelay = true; //take this line out once we get all animations
                break;
            default:
                // Reset to zero if over 4
                multClickCountLeft = 0;
                multClickCountRight = 0;
                // Perform basic move 1 left click
                ShowComboText ( "LEFT 1!" );
                m_Animator.SetTrigger("atkL1");
                m_Audio.PlayOnce ( "left1" );
                animationDelay = true; //take this line out once we get all animations
				break;
        } 
        multClickCountRight = 0;
        multClickCountLeft++;
    }

    void RightClickCombo ()
    {
        if ( !IsComboClick() )
        {
            multClickCountLeft = 0;
            multClickCountRight = 0;
        }

        switch ( multClickCountLeft )
        {
            case 0:
                // Perform smash move 0 right click
                ShowComboText( "RIGHT 0!");
                m_Audio.PlayOnce ( "right0" );
                m_Animator.SetTrigger("atkR1");
                //animationDelay = true; //take this line out once we get all animations
			    break;
            case 1:
                // Perform smash move 1 right click
                ShowComboText ( "RIGHT 1!" );
                m_Audio.PlayOnce ( "right1" );
                m_Animator.SetTrigger("atkR1");
			    break;
            case 2:
                // Perform smash move 2 right click
                ShowComboText ( "RIGHT 2!" );
                m_Audio.PlayOnce ( "right2" );
                m_Animator.SetTrigger("atkR1");
                break;
            case 3:
                // Perform smash move 3 right click
                ShowComboText ( "RIGHT 3!" );
                m_Audio.PlayOnce ( "right3" );
                m_Animator.SetTrigger("atkR1");
                break;
            case 4:
                // Perform smash move 4 right click
                ShowComboText ( "RIGHT 4!" );
                m_Audio.PlayOnce ( "right4" );
                m_Animator.SetTrigger("atkR1");
                break;
            default:
                // Reset to zero if over 4
                multClickCountLeft = 0;
                multClickCountRight = 0;
                m_Audio.PlayOnce ( "right0" );
                m_Animator.SetTrigger("atkR1");
                break;
        }
        multClickCountLeft = 0;
        multClickCountRight++;
    }

    bool IsComboClick ()
    {
        if ( ( clickTimer - prevClickTimer ) <= delayBetweenMultClicks )
        {
            prevClickTimer = clickTimer;
            return true;
        }
        prevClickTimer = clickTimer;
        return false;
    }

    void ShootArrow()
    {
        // Instantiate arrow, aiming code is in Arrow.cs
        GameObject arrowObj = ( GameObject )Instantiate ( arrow, transform.position, Quaternion.identity );
        m_Char.SetPlayerState ( Character.PlayerState.Idle );
    }
    
    void ShowComboText( string text )
    {
        fadeTimer = 0.75f;
        m_ComboTextObj.SetActive( true );
        m_Animator.SetBool("isAtking", true);
        m_ComboText.text = text;
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
