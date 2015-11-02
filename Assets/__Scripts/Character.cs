using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Character : MonoBehaviour 
{

    private GameManager m_Game;
    private InputManager m_Input;
    private AudioManager m_Audio;
    private DialogueSystem m_Dialogue;
    private Camera m_Cam;

    private bool m_FacingRight;

    public float speed = 5.0f;
    public float runSpeed = 10.0f;
    private float originalSpeed;
    private int health = 100;
    private int hunger = 100;

	//Gathering variables
	public GameObject gatherFrom;
	public float secondsGathering = 3.0f;
	private float gatherTime;
    public GameObject gatherBarObj;
	private Slider gatherBar;

    //Inventory
    private Inventory m_Inventory;

    //Health and Hunger sliders
    public Slider hpBar;
    public Slider hungerBar;
    private Image hungerFill;
    private Color hungerColorDefault;
    private GameObject hpBG;
    private GameObject hpFill;
    private int hpWidthOffset = 0;

    //Animator
    private Animator m_Animator;
    public GameObject bow;

    //Campfire Checkpoint
    private Vector3 campLocation;
    //add char stats here as well

    public enum PlayerState
    {
        Idle,
        Gather,
        Aim,
        Interact,
        Dialogue
    }
    private PlayerState m_State;

	// Use this for initialization
	void Start () 
    {
        m_FacingRight = false;
        originalSpeed = speed;

        m_Game = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>();
        m_Input = m_Game.GetInputManager ();
        m_Audio = m_Game.GetAudioManager ();
        m_Dialogue = m_Game.GetDialogueSystem ();
        m_Animator = GetComponent<Animator>();

		gatherFrom = null;

        m_Inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
        hpBar = GameObject.Find ( "HealthSlider" ).GetComponent<Slider> ();
        hpBG = hpBar.transform.Find( "Background" ).gameObject;
        hpFill = hpBar.transform.Find( "Fill Area" ).gameObject;
        hungerBar = GameObject.Find ( "HungerSlider" ).GetComponent<Slider> ();
        hungerFill = hungerBar.transform.Find ( "Fill Area" ).transform.Find ( "Fill" ).GetComponent<Image> ();
        hungerColorDefault = hungerFill.color;

        // Set max and starting value of health and hunger
        hpBar.maxValue = health;
        hpBar.value = health;
        hungerBar.maxValue = hunger;
        hungerBar.value = hunger;
    }
	
	// Update is called once per frame
	void Update () 
    {
        if( m_State == PlayerState.Dialogue && Time.timeScale != 0 ) // Make sure the game isn't paused
        {
            m_Animator.SetBool("isWalking", false);
            if( m_Input.InteractButtonPressed() || m_Input.SelectButtonPressed() )
            {
                m_Dialogue.Advance ();
            }
        }
        if (m_State == PlayerState.Interact || m_State == PlayerState.Gather )
        {
            m_Animator.SetBool("isWalking", false);
            if (m_State == PlayerState.Gather)
            {
                m_Animator.SetBool("isAtking", false);
            }
        }
        if ( m_Input.InteractButtonPressed() && gatherFrom != null )
        {
            m_Audio.PlayLoop ( "rustle" );
            SetPlayerState ( PlayerState.Gather );
            BeginGather();
        }
        if ( m_Input.AimButtonHeld() )
        {
            m_Animator.SetBool("isWalking", false);
            SetPlayerState ( PlayerState.Aim );
            bow.SetActive(true);
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos.z = transform.position.z;
            
            Vector3 dir = pos - bow.transform.position;

            if (transform.localScale.x < 0)
            {
                dir.x = -dir.x;
            }

            Quaternion rotateTo = new Quaternion();
          
            //TODO clamp the rotation so you cant go over her head
            rotateTo.SetFromToRotation(Vector3.left, dir);

            bow.transform.rotation = rotateTo;
        }
        else
        {
            bow.SetActive(false);
        }
        if(m_State == PlayerState.Gather && gatherFrom != null)
        {
            //gatherBar = gatherBarObj.GetComponent<Slider>();
            gatherBarObj.SetActive(true);
            m_State = PlayerState.Gather;
			gatherTime += Time.deltaTime;
			if( gatherTime >= secondsGathering )
            {
                SetPlayerState( PlayerState.Idle );
				gatherBar.value = 0;
				gatherTime = 0.0f;
				Debug.Log("Finished Gathering");
                m_Audio.PlayLoop ( "newItem" );
				gatherFrom.GetComponent<Interactable>().ReceiveItem();
                gatherBarObj.SetActive(false);
			}
			else
            {
				gatherBar.value = gatherTime/secondsGathering;
			}
		}

        if ( m_State != PlayerState.Gather && m_State != PlayerState.Interact && m_State != PlayerState.Aim && m_State != PlayerState.Dialogue )
        {   
            // Move character
        	transform.Translate ( m_Input.GetHorizontalMovement() * speed );
        	transform.Translate ( m_Input.GetVerticalMovement() * speed );
            m_Animator.SetBool("isWalking", true);

            if ( m_State != PlayerState.Gather && m_Input.GetHorizontalMovement () == Vector3.zero && m_Input.GetVerticalMovement () == Vector3.zero )
            {
                SetPlayerState ( PlayerState.Idle );
                m_Animator.SetBool( "isWalking", false);
            }
            else
            {
                if ( m_Input.RunButtonHeld () )
                {
                    speed = runSpeed;
                }
                else
                {
                    speed = originalSpeed;
                }
            }
            FaceSpriteTowardDirection ();
		}

        if ( !m_Input.AimButtonHeld() && m_State == PlayerState.Aim )
        {
            SetPlayerState ( PlayerState.Idle );
        }
	}

    public void SetPlayerState ( PlayerState newState )
    {
        //Debug.Log ( newState );
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

    public void TakeDamage ( int damage )
    {
        hpBar.value -= damage;
        
        if ( hpBar.value <= 0 )
        {
            m_Audio.PlayOnce ( "playerDeath" );
        }
        else
        {
            m_Audio.PlayOnce( "playerDamaged");
        }
    }

    // Will add the parameter to hunger value
    // If eating, want a positive number
    // If time passes and you don't eat, want a negative number
    public void IncrementHunger( int amount )
    {
        hungerBar.value += amount;
        if ( hungerBar.value < 60 )
        {
            hungerFill.color = new Color ( 0.88627f, 0.20784f, 0.20784f, 0.7843f );
        }
        else
        {
            hungerFill.color = hungerColorDefault;
        }
    }

    // Starving for a day lowers max health
    // Gets called on a new day
    public void CheckStarved()
    {
        if( hungerBar.value < 60 )
        {
            hpBar.maxValue -= ( health / 5 ); // permanently lose a fifth of max health
            m_Game.IncreaseDaysStarved ();
            Debug.Log ( "Starved" );
            if( hpBar.value > hpBar.maxValue)
            {
                hpBar.value = hpBar.maxValue;
            }

            if ( hpWidthOffset < 120 ) 
            {
                // Don't lower max hp if it would make max hp 0
                hpWidthOffset += 30;
                hpBG.GetComponent<RectTransform> ().sizeDelta = new Vector2 ( 150 - hpWidthOffset, 16 );
                hpFill.GetComponent<RectTransform> ().sizeDelta = new Vector2 ( 150 - hpWidthOffset, 16 );
            }
        }
        // Since its a new day, replenish some hunger due to resting
        IncrementHunger ( 10 );
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
        hungerBar.value = hunger;
        hpBar.maxValue = health; 
        hpWidthOffset = 0;
        hpBG.GetComponent<RectTransform> ().sizeDelta = new Vector2 ( 150 - hpWidthOffset, 16 );
        hpFill.GetComponent<RectTransform> ().sizeDelta = new Vector2 ( 150 - hpWidthOffset, 16 );
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

    public void GetItem(GameObject item)
    {
        m_Inventory.AddItem(item);
    }

    public void useItem(GameObject item)
    {
        ItemData data = item.GetComponent<ItemData>();
        hpBar.value += data.healthIncrease;
        hungerBar.value += data.hungerIncrease;
    }

    public void ReturnToCamp()
    {
        //just move the character back to the campfire for the playtest, this needs to also reset stats later on
        if (campLocation != null)
        {
            transform.position = campLocation;
            m_Animator.SetBool("isAtking", false);
            m_Animator.SetBool("isWalking", false);
        }
        else
        {
            Application.LoadLevel("MainMenu");
        }
    }

    public void BeginGather()
    {
        if (m_State != PlayerState.Gather)
        {
            gatherTime = 0.0f;
            gatherBar = gatherBarObj.GetComponent<Slider>();
            gatherBar.value = 0.0f;
            gatherBar.maxValue = 1;
        }
    }

    public void setCampLocation(Vector3 lastCamp)
    {
        campLocation = lastCamp;
    }

}