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

	public GameObject CombatTrail;
		
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

		trailDefaultPos = attackTrail.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        // TODO: Replace with Input Manager hooks
        fadeTimer -= Time.deltaTime;
        if ( fadeTimer < 0 )
        {
	        m_ComboTextObj.SetActive( false );
			CombatTrail.SetActive(false);
	    }

        if ( im.normalAttackButtonPressed() )
        {
            clickTimer = Time.time;
            LeftClickCombo();
        }

        if ( im.smashAttackButtonPressed() )
        {
            clickTimer = Time.time;
            RightClickCombo();
        }

        if ( im.specialAttackButtonPressed() )
        {
            Debug.Log( "Pressed middle click." );
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
				break;
            case 1:
                // Perform basic move 2 left click
                ShowComboText ( "LEFT 2!" );
				attackTrail.transform.Translate(Vector3.up * 3.0f);
				attackTrail.transform.Translate(Vector3.right * -3.0f);
				break;
            case 2:
                // Perform basic move 3 left click
                ShowComboText ( "LEFT 3!" );
				attackTrail.transform.Translate(Vector3.right * 3.0f);
				break;
            case 3:
                // Perform basic move 4 left click
				attackTrail.transform.Translate(Vector3.right * -3.0f);
                ShowComboText ( "LEFT 4!" );
                break;
            default:
                // Reset to zero if over 4
                multClickCountLeft = 0;
                multClickCountRight = 0;
                // Perform basic move 1 left click
                ShowComboText ( "LEFT 1!" );
				attackTrail.transform.Translate(Vector3.up * -3.0f);
				attackTrail.transform.Translate(Vector3.right * 3.0f);
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
			break;
            case 1:
                // Perform smash move 1 right click
                ShowComboText ( "RIGHT 1!" );
			attackTrail.transform.Translate(Vector3.up * -3.0f);
			attackTrail.transform.Translate(Vector3.right * -3.0f);
			break;
            case 2:
                // Perform smash move 2 right click
                ShowComboText ( "RIGHT 2!" );
                break;
            case 3:
                // Perform smash move 3 right click
                ShowComboText ( "RIGHT 3!" );
                break;
            case 4:
                // Perform smash move 4 right click
                ShowComboText ( "RIGHT 4!" );
                break;
            default:
                // Reset to zero if over 4
                multClickCountLeft = 0;
                multClickCountRight = 0;
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
