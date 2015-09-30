using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.UI;

public class Combat : MonoBehaviour 
{
    float clickTimer = 0.0f;
    float prevClickTimer = 0.0f;
    float fadeTimer = 0.0f;
    float delayBetweenMultClicks = 0.75f;
    int multClickCountLeft = 0;
    int multClickCountRight = 0;

    private GameObject m_Canvas;
    private GameObject m_ComboTextObj;
    private Text m_ComboText;
    private Character m_Char;

	public GameObject CombatTrail;

    // Audio declarations
    public AudioClip sndLeft1;
    public AudioClip sndLeft2;
    public AudioClip sndLeft3;
    public AudioClip sndLeft4;
    public AudioClip sndRight0;
    public AudioClip sndRight1;
    public AudioClip sndRight2;
    public AudioClip sndRight3;
    public AudioClip sndRight4;
    private AudioSource m_AudioSource;
		
	private InputManager im;

	//trail renderer for attack effects
	public GameObject attackTrail;
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

		im = GameObject.Find ("InputManager").GetComponent<InputManager>();
        m_Char = GameObject.Find ( "Character" ).GetComponent<Character> ();
        m_AudioSource = GameObject.Find ("Main Camera").GetComponent<AudioSource> ();

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
            if ( im.normalAttackButtonPressed () )
            {
                clickTimer = Time.time;
                attackTrail.GetComponent<TrailRenderer> ().material.SetColor ( "_TintColor", Color.grey );
                LeftClickCombo ();
            }

            if ( im.smashAttackButtonPressed () )
            {
                clickTimer = Time.time;
                attackTrail.GetComponent<TrailRenderer> ().material.SetColor ( "_TintColor", Color.yellow );
                RightClickCombo ();
            }

            if ( im.specialAttackButtonPressed () )
            {
                Debug.Log ( "Pressed middle click." );
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
				attackTrail.transform.Translate(Vector3.right * 3.0f);
                m_AudioSource.PlayOneShot ( sndLeft1 );
				break;
            case 1:
                // Perform basic move 2 left click
                ShowComboText ( "LEFT 2!" );
				attackTrail.transform.Translate(Vector3.up * 3.0f);
				attackTrail.transform.Translate(Vector3.right * -3.0f);
                m_AudioSource.PlayOneShot ( sndLeft2 );
				break;
            case 2:
                // Perform basic move 3 left click
                ShowComboText ( "LEFT 3!" );
				attackTrail.transform.Translate(Vector3.right * 3.0f);
                m_AudioSource.PlayOneShot ( sndLeft3 );
				break;
            case 3:
                // Perform basic move 4 left click
				attackTrail.transform.Translate(Vector3.right * -3.0f);
                ShowComboText ( "LEFT 4!" );
                m_AudioSource.PlayOneShot ( sndLeft4 );
                break;
            default:
                // Reset to zero if over 4
                multClickCountLeft = 0;
                multClickCountRight = 0;
                // Perform basic move 1 left click
                ShowComboText ( "LEFT 1!" );
				attackTrail.transform.Translate(Vector3.up * -3.0f);
				attackTrail.transform.Translate(Vector3.right * 3.0f);
                m_AudioSource.PlayOneShot ( sndLeft1 );
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
                m_AudioSource.PlayOneShot ( sndRight0 );
			    break;
            case 1:
                // Perform smash move 1 right click
                ShowComboText ( "RIGHT 1!" );
			    attackTrail.transform.Translate(Vector3.up * -3.0f);
			    attackTrail.transform.Translate(Vector3.right * -3.0f);
                m_AudioSource.PlayOneShot ( sndRight1 );
			    break;
            case 2:
                // Perform smash move 2 right click
                ShowComboText ( "RIGHT 2!" );
                m_AudioSource.PlayOneShot ( sndRight2 );
                break;
            case 3:
                // Perform smash move 3 right click
                ShowComboText ( "RIGHT 3!" );
                m_AudioSource.PlayOneShot ( sndRight3 );
                break;
            case 4:
                // Perform smash move 4 right click
                ShowComboText ( "RIGHT 4!" );
                m_AudioSource.PlayOneShot ( sndRight4 );
                break;
            default:
                // Reset to zero if over 4
                multClickCountLeft = 0;
                multClickCountRight = 0;
                m_AudioSource.PlayOneShot ( sndRight0 );
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
        m_ComboText.text = text;
    }
}
