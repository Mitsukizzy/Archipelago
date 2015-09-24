using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Character : MonoBehaviour {

    private InputManager m_Input;
    private Camera m_Cam;

    private bool m_FacingRight;

    private float speed = 5.0f;
    private float runSpeed = 10.0f;
    private float dodgeSpeed = 300.0f;

    // Audio declarations
    public AudioClip sndWalk;
    public AudioClip sndRun;
    private AudioSource m_AudioSource;

	//Dodge trail effect
	public GameObject dodgeTrail;
	private float originalSpeed;
	private bool dodgeReleased;

	//Gathering variables
	private bool isGathering;
	public GameObject gatherFrom;
	public float secondsGathering = 3.0f;
	private float gatherTime;
	public Image gatherBar;
	private float gatherFill;

    public enum PlayerState
    {
        Run,
        Walk,
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

		dodgeReleased = true;
		isGathering = false;
		gatherFrom = null;
		gatherTime = 0.0f;
		gatherFill = 0.0f;
		gatherBar.fillAmount = gatherFill;
	}
	
	// Update is called once per frame
	void Update () 
    {
		if(dodgeReleased){
			dodgeTrail.GetComponent<TrailRenderer>().material.SetColor("_TintColor", new Color(0,0,0,0));
		}

        // Move character
		if ( !isGathering )
        {
        	transform.Translate ( m_Input.GetHorizontalMovement() * speed );
        	transform.Translate ( m_Input.GetVerticalMovement() * speed );
		}
		else{ //if we are gathering
			gatherTime += Time.deltaTime;
			if(gatherTime >= secondsGathering){
				isGathering = false;
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
		}

		if( m_Input.DodgeButtonPressed() )
		{
			dodgeReleased = false;
		}

        if ( m_Input.DodgeButtonReleased () )
        {
			dodgeTrail.GetComponent<TrailRenderer>().material.SetColor("_TintColor", new Color(255,255,255,20));

			transform.Translate ( m_Input.GetHorizontalMovement () * dodgeSpeed );
            transform.Translate ( m_Input.GetVerticalMovement () * dodgeSpeed );
			dodgeReleased = true;		}

		if( m_Input.gatheringButtonPressed () )
		{
			GatherItem();
		}

        if ( m_Input.GetHorizontalMovement () == Vector3.zero && m_Input.GetVerticalMovement () == Vector3.zero )
        {
            m_State = PlayerState.Idle;
            m_AudioSource.Stop ();
        }
        else
        {
            if ( m_State != PlayerState.Run && m_State != PlayerState.Walk )
            {
                m_AudioSource.Play ();
            }
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

        FaceSpriteTowardDirection ();
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
		if (gatherFrom != null){
			isGathering = true;
			Debug.Log("Begin Gathering");
		}
	}
}