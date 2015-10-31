using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Campfire : MonoBehaviour 
{    
    public Image screenFadeFill;
    public Image screenFadeCircle;

    private Character m_Char;
    private InputManager m_Input;
    private CameraFollow m_Camera;

    private Animator m_CharAnimator;

    public GameObject campPopup;
    public GameObject campMenu;

    private float fadeTime = 2.0f;
    private bool isFadingOut;
    private bool isFadingIn;
    private bool isFadingInCircle;
    private bool canCamp;

    private Inventory m_Inventory;

    void Awake()
    {
        screenFadeFill.rectTransform.sizeDelta = new Vector2 ( Screen.width, Screen.height );
        screenFadeCircle.rectTransform.sizeDelta = new Vector2 ( Screen.width, Screen.height );
    }

	// Use this for initialization
	void Start () 
    {
        m_Char = GameObject.FindGameObjectWithTag("Char").GetComponent<Character> ();
        m_Input = GameObject.FindGameObjectWithTag("Manager").GetComponent<InputManager>();
        m_Camera = GameObject.Find ( "Main Camera" ).GetComponent<CameraFollow> ();
        m_CharAnimator =m_Char.GetComponent<Animator>();
        m_Inventory = GameObject.Find("Inventory UI").transform.GetChild(0).GetComponent<Inventory>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        if ( m_Input.InteractButtonPressed () && canCamp )
        {
            campPopup.SetActive ( true );
            m_Char.SetPlayerState ( Character.PlayerState.Interact );
            m_CharAnimator.SetBool("isWalking", false);
        }

        if ( isFadingIn )
        {
            FadeIn ();
        }
        if ( isFadingInCircle )
        {
            FadeInCircle ();
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
        m_Char.SetPlayerState ( Character.PlayerState.Interact );

        // Fade out and fade in
        isFadingOut = true;

        // Center camera on campfire
        m_Camera.ChangeTarget ( transform );

        // Move MC next to campfire, sitting cross legged
    }

    public void NoCamp()
    {
        campPopup.SetActive ( false );
        m_Char.SetPlayerState ( Character.PlayerState.Idle );
    }

    public void EndCamp ()
    {
        isFadingInCircle = true;
        campMenu.SetActive ( false );
        m_Char.SetPlayerState ( Character.PlayerState.Idle );
        m_Inventory.CloseInventory();

        m_Char.setCampLocation(transform.position);

        // Center camera on player
        m_Camera.TargetPlayer ();
    }

    private void FadeInCircle()
    {
        screenFadeCircle.color = Color.Lerp ( screenFadeFill.color, Color.clear, fadeTime * Time.deltaTime );

        if ( screenFadeCircle.color.a <= 0.15f )
        {
            screenFadeCircle.color = Color.clear;
            isFadingInCircle = false;
        }
    }

    private void FadeIn ()
    {
        // Fades to clear
        screenFadeFill.color = Color.Lerp ( screenFadeFill.color, Color.clear, fadeTime * Time.deltaTime );

        if ( screenFadeFill.color.a <= 0.15f )
        {
            screenFadeFill.color = Color.clear;
            isFadingIn = false;
            
            // Open camp menu
            campMenu.SetActive ( true );
        }
    }

    private void FadeOut ()
    {
        // Fades to black
        screenFadeCircle.color = Color.Lerp ( screenFadeCircle.color, Color.black, fadeTime * Time.deltaTime );
        screenFadeFill.color = Color.Lerp ( screenFadeFill.color, Color.black, fadeTime * Time.deltaTime );

        if ( screenFadeFill.color.a >= 0.95f )
        {
            screenFadeFill.color = Color.black;
            isFadingOut = false;
            isFadingIn = true;
        }
    }

    public void CookandEat()
    {
        m_Inventory.OpenInventory();
        m_Inventory.SetInteractable(true);
    }

    void OnTriggerEnter2D ( Collider2D coll )
    {
        if ( coll.gameObject.tag == "Char" && m_Char.GetPlayerState () != Character.PlayerState.Interact )
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
