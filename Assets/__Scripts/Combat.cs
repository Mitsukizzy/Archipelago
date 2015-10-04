using UnityEngine;
using System.Collections;
using UnityEngine.UI;

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

	//trail renderer for attack effects
    public GameObject attackTrail;
    public GameObject CombatTrail;
	Vector3 trailDefaultPos;

	// Use this for initialization
	void Start () {
        m_Canvas = GameObject.Find ( "Canvas" );
        m_ComboTextObj = ( GameObject )Instantiate ( Resources.Load ( "ComboText" ) );
        m_ComboTextObj.SetActive ( false );
		CombatTrail.SetActive(false);
	    m_ComboText = m_ComboTextObj.GetComponent<Text>();
        m_ComboText.rectTransform.parent = m_Canvas.transform;
        m_ComboText.rectTransform.localPosition = transform.position;

        m_Game = GameObject.Find ( "GameManager" ).GetComponent<GameManager>();
        m_Input = m_Game.GetInputManager();
        m_Audio = m_Game.GetAudioManager();
        m_Char = GameObject.Find ( "Character" ).GetComponent<Character>();
        m_Animator = GetComponent<Animator>();

		trailDefaultPos = attackTrail.transform.position;
	}
	
	// Update is called once per frame
	void Update () 
    {
        fadeTimer -= Time.deltaTime;
        if ( fadeTimer < 0 )
        {
	        m_ComboTextObj.SetActive( false );
			CombatTrail.SetActive(false);
	    }

        if ( m_Char.GetPlayerState () != Character.PlayerState.Gather && m_Char.GetPlayerState () != Character.PlayerState.Interact )
        {
            if ( m_Input.normalAttackButtonPressed () )
            {
                if ( m_Char.UseStamina ( 30 ) )
                {
                    clickTimer = Time.time;
                    attackTrail.GetComponent<TrailRenderer> ().material.SetColor ( "_TintColor", Color.grey );
                    damageToEnemy = 10;
                    LeftClickCombo ();
                }
            }

            if ( m_Input.smashAttackButtonPressed () )
            {
                if ( m_Char.UseStamina ( 50 ) )
                {
                    clickTimer = Time.time;
                    attackTrail.GetComponent<TrailRenderer> ().material.SetColor ( "_TintColor", Color.yellow );
                    damageToEnemy = 20;
                    RightClickCombo ();
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
				attackTrail.transform.Translate(Vector3.up * -3.0f);
                attackTrail.transform.Translate ( Vector3.right * 3.0f );
                m_Audio.PlayOnce ( "left1" );
                m_Animator.SetTrigger("atkL1");
				break;
            case 1:
                // Perform basic move 2 left click
                ShowComboText ( "LEFT 2!" );
				attackTrail.transform.Translate(Vector3.up * 3.0f);
				attackTrail.transform.Translate(Vector3.right * -3.0f);
                m_Audio.PlayOnce ( "left2" );
				break;
            case 2:
                // Perform basic move 3 left click
                ShowComboText ( "LEFT 3!" );
				attackTrail.transform.Translate(Vector3.right * 3.0f);
                m_Audio.PlayOnce ( "left3" );
				break;
            case 3:
                // Perform basic move 4 left click
				attackTrail.transform.Translate(Vector3.right * -3.0f);
                ShowComboText ( "LEFT 4!" );
                m_Audio.PlayOnce ( "left4" );
                break;
            default:
                // Reset to zero if over 4
                multClickCountLeft = 0;
                multClickCountRight = 0;
                // Perform basic move 1 left click
                ShowComboText ( "LEFT 1!" );
                m_Animator.SetTrigger("atkL1");
				attackTrail.transform.Translate(Vector3.up * -3.0f);
				attackTrail.transform.Translate(Vector3.right * 3.0f);
                m_Audio.PlayOnce ( "left1" );
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
			    attackTrail.transform.Translate(Vector3.up * 3.0f);
			    attackTrail.transform.Translate(Vector3.right * 3.0f);
                m_Audio.PlayOnce ( "right0" );
                m_Animator.SetTrigger("atkR1");
			    break;
            case 1:
                // Perform smash move 1 right click
                ShowComboText ( "RIGHT 1!" );
			    attackTrail.transform.Translate(Vector3.up * -3.0f);
			    attackTrail.transform.Translate(Vector3.right * -3.0f);
                m_Audio.PlayOnce ( "right1" );
			    break;
            case 2:
                // Perform smash move 2 right click
                ShowComboText ( "RIGHT 2!" );
                m_Audio.PlayOnce ( "right2" );
                break;
            case 3:
                // Perform smash move 3 right click
                ShowComboText ( "RIGHT 3!" );
                m_Audio.PlayOnce ( "right3" );
                break;
            case 4:
                // Perform smash move 4 right click
                ShowComboText ( "RIGHT 4!" );
                m_Audio.PlayOnce ( "right4" );
                break;
            default:
                // Reset to zero if over 4
                multClickCountLeft = 0;
                multClickCountRight = 0;
                m_Audio.PlayOnce ( "right0" );
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

    void ShowComboText( string text )
    {
        fadeTimer = 0.75f;
        m_ComboTextObj.SetActive( true );
		CombatTrail.SetActive(true);
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
}
