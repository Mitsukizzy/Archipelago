using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Character : MonoBehaviour {

    private InputManager m_Input;
    private AudioManager m_Audio;
    private Camera m_Cam;

    private bool m_FacingRight;

    public float speed = 5.0f;
    public float runSpeed = 10.0f;
    public float dodgeSpeed = 500.0f;
    public int health = 100;
    public int stamina = 500;

	//Dodge trail effect
	public GameObject dodgeTrail;
	private float originalSpeed;

	//Gathering variables
	public GameObject gatherFrom;
	public float secondsGathering = 3.0f;
	private float gatherTime;
    public GameObject gatherBarObj;
	private Slider gatherBar;

    //Health and Stamina sliders
    public Slider hpBar;
    public Slider staminaBar;
    public float timeToRegenStamina = 3.0f;
    private float staminaTimer = 0;
    private bool usingStamina = false;

    //Animator
    private Animator m_Animator;

    public enum PlayerState
    {
        Run,
        Walk,
        Dodge,
        Gather,
        Fight,
        Interact,
        Idle
    }
    private PlayerState m_State;

	// Use this for initialization
	void Start () 
    {
        m_FacingRight = false;
        m_State = PlayerState.Idle;
        originalSpeed = speed;

        m_Input = GameObject.Find ( "GameManager" ).GetComponent<InputManager> ();
        m_Audio = GameObject.Find ( "GameManager" ).GetComponent<AudioManager> ();
        m_Animator = GetComponent<Animator>();

		gatherFrom = null;
		gatherTime = 0.0f;
        gatherBar = gatherBarObj.GetComponent<Slider>();
        gatherBar.value = 0.0f;
        gatherBar.maxValue = 1;
	
        //health and stamina bar
        hpBar.maxValue = health;
        hpBar.value = health;
        staminaBar.maxValue = stamina;
        staminaBar.value = stamina;
    }
	
	// Update is called once per frame
	void Update () 
    {
        if (m_State == PlayerState.Interact || m_State == PlayerState.Gather)
        {
            m_Animator.SetBool("isWalking", false);
            if (m_State == PlayerState.Gather)
            {
                m_Animator.SetBool("isAtking", false);
            }
        }
        if (m_Input.gatheringButtonPressed() && gatherFrom != null)
        {
            m_State = PlayerState.Gather;    
        }
        if(m_State == PlayerState.Gather && gatherFrom != null)
        {
            gatherBarObj.SetActive(true);
            m_State = PlayerState.Gather;
			gatherTime += Time.deltaTime;
			if( gatherTime >= secondsGathering )
            {
                m_State = PlayerState.Idle;
				gatherBar.value = 0;
				gatherTime = 0.0f;
				Debug.Log("Finished Gathering");
				gatherFrom.GetComponent<Interactable>().ReceiveItem();
                gatherBarObj.SetActive(false);
			}
			else{
				gatherBar.value=gatherTime/secondsGathering;
			}
		}

        if ( m_State == PlayerState.Fight )
        {
            // Play battle music
        }

        if ( m_State != PlayerState.Dodge )
        {
            dodgeTrail.GetComponent<TrailRenderer> ().material.SetColor ( "_TintColor", new Color ( 0, 0, 0, 0 ) );
        }
        if(!usingStamina)
        {            
            staminaTimer += Time.deltaTime;
        }
        if (staminaTimer >= timeToRegenStamina && !usingStamina)
        {
            staminaBar.value += 100;
            staminaTimer = 0;
        }
        
        if ( m_State != PlayerState.Gather && m_State != PlayerState.Interact )
        {   
            // Move character
        	transform.Translate ( m_Input.GetHorizontalMovement() * speed );
        	transform.Translate ( m_Input.GetVerticalMovement() * speed );
            m_Animator.SetBool("isWalking", true);

            if( m_Input.DodgeButtonPressed() )
		    {
                m_State = PlayerState.Dodge;
		    }

            if ( m_Input.DodgeButtonReleased () )
            {
                if ( UseStamina ( 50 ) )
                {
                    dodgeTrail.GetComponent<TrailRenderer> ().material.SetColor ( "_TintColor", new Color ( 255, 255, 255, 20 ) );

                    transform.Translate ( m_Input.GetHorizontalMovement () * dodgeSpeed );
                    transform.Translate ( m_Input.GetVerticalMovement () * dodgeSpeed );
                    m_Audio.PlayOnce ( "dodge" );
                    m_State = PlayerState.Idle;
                }
            }

            if ( m_State != PlayerState.Gather && m_Input.GetHorizontalMovement () == Vector3.zero && m_Input.GetVerticalMovement () == Vector3.zero )
            {
                m_State = PlayerState.Idle;
                m_Animator.SetBool("isWalking", false);
            }
            else
            {
                if ( m_Input.RunButtonHeld () )
                {
                    if ( UseStamina ( 1 ) )
                    {
                        speed = runSpeed;
                        m_State = PlayerState.Run;
                        usingStamina = true;
                    }
                }
                else
                {
                    speed = originalSpeed;
                    usingStamina = false; 
                    m_State = PlayerState.Walk;
                }
            }
            FaceSpriteTowardDirection ();
		}
	}

    public void SetPlayerState ( PlayerState newState )
    {
        m_State = newState;
    }

    public PlayerState GetPlayerState ()
    {
        return m_State;
    }

    void FaceSpriteTowardDirection ()
    {
        if ( m_Input.GetHorizontalMovement ().x > 0 && !m_FacingRight )
        {
            m_FacingRight = true;
            Vector3 newScale = transform.localScale;
            newScale.x *= -1;
            transform.localScale = newScale;
        }
        else if ( m_Input.GetHorizontalMovement ().x < 0 && m_FacingRight )
        {
            m_FacingRight = false;
            Vector3 newScale = transform.localScale;
            newScale.x *= -1;
            transform.localScale = newScale;
        }
    }

    public bool UseStamina ( int cost )
    {
        if ( cost > staminaBar.value ) // Not enough stamina
        {
            m_Audio.PlayOnce ( "playerNoStamina" );
            return false;
        }
        staminaBar.value -= cost;  
        return true;
    }

    public void TakeDamage ( int damage )
    {
        hpBar.value -= damage;
        
        if ( hpBar.value <= 0 )
        {
            m_Audio.PlayOnce ( "playerDeath" );
        }
        else if ( hpBar.value <= 40 )
        {
            m_Audio.PlayOnce( "playerNearDeath" );
        }
        else
        {
            m_Audio.PlayOnce( "playerDamaged");
        }
    }

    public bool IsAlive()
    {
        if ( hpBar.value > 0 )
        {
            return true;
        }
        return false;
    }

    public void Revive ()
    {
        // Both back to their default, full values
        hpBar.value = health;
        staminaBar.value = stamina;
    }

    public void toggleInteract()
    {
        if (m_State != PlayerState.Interact)
        {
            m_State = PlayerState.Interact;
        }
        else
        {
            m_State = PlayerState.Idle;
        }
    }
}