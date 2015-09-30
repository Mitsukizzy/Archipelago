using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Campfire : MonoBehaviour 
{
    public GameObject campPopup;
    public Image screenFadeTexture;
    private Character m_Char;
    private InputManager m_Input;

    private float fadeTime = 2.0f;
    private bool isFadingOut;
    private bool isFadingIn;
    private bool canCamp;

    void Awake()
    {
        screenFadeTexture.rectTransform.sizeDelta = new Vector2( Screen.width, Screen.height );
    }

	// Use this for initialization
	void Start () 
    {
        m_Char = GameObject.Find ( "Character" ).GetComponent<Character> ();
        m_Input = GameObject.Find ( "InputManager" ).GetComponent<InputManager> ();
	}
	
	// Update is called once per frame
	void Update () 
    {
        if ( m_Input.gatheringButtonPressed () && canCamp )
        {
            campPopup.SetActive ( true );
            m_Char.SetPlayerState ( Character.PlayerState.Interact );
        }

        if ( isFadingIn )
        {
            FadeIn ();
        }
        if ( isFadingOut )
        {
            FadeOut ();
        }
	}

    public void MakeCamp ()
    {
        campPopup.SetActive ( false );
        canCamp = false;
        // Fade out
        isFadingOut = true;
        // Center camera on campfire
        // Move MC next to campfire, sitting cross legged
        // Fade in
    }

    public void NoCamp()
    {
        campPopup.SetActive ( false );
        m_Char.SetPlayerState ( Character.PlayerState.Idle );
    }

    private void FadeIn()
    {
        // Fades to clear
        screenFadeTexture.color = Color.Lerp ( screenFadeTexture.color, Color.clear, fadeTime * Time.deltaTime );

        if ( screenFadeTexture.color.a <= 0.05f )
        {
            screenFadeTexture.color = Color.clear;
            isFadingIn = false;
        }
    }

    private void FadeOut ()
    {
        // Fades to black
        screenFadeTexture.color = Color.Lerp ( screenFadeTexture.color, Color.black, fadeTime * Time.deltaTime );

        if ( screenFadeTexture.color.a >= 0.95f )
        {
            screenFadeTexture.color = Color.black;
            isFadingOut = false;
            isFadingIn = true;
        }
    }

    void OnTriggerEnter2D ( Collider2D coll )
    {
        if ( coll.gameObject.tag == "Char" )
        {
            canCamp = true;
        }
    }

    void OnTriggerExit2D ( Collider2D coll )
    {
        if ( coll.gameObject.tag == "Char" )
        {
            canCamp = false;
        }
    }
}
