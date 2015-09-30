using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Character : MonoBehaviour {

    private InputManager m_Input;
    private Camera m_Cam;

    private bool m_FacingRight;

    private float speed = 5.0f;
    private float runSpeed = 10.0f;
    private float dodgeSpeed = 500.0f;
    private int health = 100;

    // Audio declarations
    public AudioClip sndWalk;
    public AudioClip sndRun;
    public AudioClip sndDodge;
    private AudioSource m_AudioSource;

	//Dodge trail effect
	public GameObject dodgeTrail;
	private float originalSpeed;

	//Gathering variables
	public GameObject gatherFrom;
	public float secondsGathering = 3.0f;
	private float gatherTime;
	public Image gatherBar;
	private float gatherFill;

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

        m_Input = GameObject.Find ( "InputManager" ).GetComponent<InputManager> ();
        m_AudioSource = GameObject.Find ( "Main Camera" ).GetComponent<AudioSource> ();
        m_AudioSource.loop = true;

		gatherFrom = null;
		gatherTime = 0.0f;
		gatherFill = 0.0f;
		gatherBar.fillAmount = gatherFill;
	}
	
	// Update is called once per frame
	void Update () 
    {
        if ( m_State != PlayerState.Dodge )
        {
            dodgeTrail.GetComponent<TrailRenderer> ().material.SetColor ( "_TintColor", new Color ( 0, 0, 0, 0 ) );
        }
        
        if ( m_State != PlayerState.Gather && m_State != PlayerState.Interact )
        {
            // Move character
        	transform.Translate ( m_Input.GetHorizontalMovement() * speed );
        	transform.Translate ( m_Input.GetVerticalMovement() * speed );

            if ( m_Input.DodgeButtonPressed () )
            {
                m_State = PlayerState.Dodge;
            }
            if ( m_Input.DodgeButtonReleased () )
            {
                dodgeTrail.GetComponent<TrailRenderer> ().material.SetColor ( "_TintColor", new Color ( 255, 255, 255, 20 ) );

                transform.Translate ( m_Input.GetHorizontalMovement () * dodgeSpeed );
                transform.Translate ( m_Input.GetVerticalMovement () * dodgeSpeed );
                m_AudioSource.PlayOneShot ( sndDodge );
                m_State = PlayerState.Idle;
            }

            if ( m_Input.gatheringButtonPressed () )
            {
                GatherItem ();
            }

            if ( m_Input.GetHorizontalMovement () == Vector3.zero && m_Input.GetVerticalMovement () == Vector3.zero )
            {
                m_State = PlayerState.Idle;
            }
            else
            {
                if ( m_Input.RunButtonHeld () )
                {
                    speed = runSpeed;
                    m_State = PlayerState.Run;
                    m_AudioSource.clip = sndRun;
                }
                else
                {
                    speed = originalSpeed;
                    m_State = PlayerState.Walk;
                    m_AudioSource.clip = sndWalk;
                }
            }

            if ( m_State == PlayerState.Run || m_State == PlayerState.Walk )
            {
                if ( !m_AudioSource.isPlaying )
                {
                    m_AudioSource.PlayOneShot ( m_AudioSource.clip );
                    m_AudioSource.Play ();
                }
            }
            else
            {
                m_AudioSource.Stop ();
            }

            FaceSpriteTowardDirection ();
		}
        else if ( m_State != PlayerState.Gather )
        {   
            //if we are gathering
			gatherTime += Time.deltaTime;
			if( gatherTime >= secondsGathering )
            {
                m_State = PlayerState.Idle;
				gatherFill = 0.0f;
				gatherBar.fillAmount = gatherFill;
				gatherTime = 0.0f;
				Debug.Log("Finished Gathering");
				gatherFrom.GetComponent<Interactable>().ReceiveItem();
			}
			else{
				gatherFill=gatherTime/secondsGathering;
				gatherBar.fillAmount = gatherFill;
			}
            m_AudioSource.Stop (); // temp solution to stop walking audio
		}
        else
        {
            m_AudioSource.Stop (); // temp solution to stop walking audio
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

	void GatherItem(){
		if ( gatherFrom != null )
        {
            m_State = PlayerState.Gather;
			Debug.Log("Begin Gathering");
		}
	}

    public bool IsAlive()
    {
        if ( health > 0 )
        {
            return true;
        }
        return false;
    }
}