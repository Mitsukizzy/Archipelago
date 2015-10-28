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
    private int hunger = 70;  // Starting hunger will be 70/100

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

    //Animator
    private Animator m_Animator;

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

        m_Game = GameObject.Find ( "GameManager" ).GetComponent<GameManager> ();
        m_Input = m_Game.GetInputManager ();
        m_Audio = m_Game.GetAudioManager ();
        m_Dialogue = m_Game.GetDialogueSystem ();
        m_Animator = GetComponent<Animator>();

		gatherFrom = null;
		gatherTime = 0.0f;
        gatherBar = gatherBarObj.GetComponent<Slider>();
        gatherBar.value = 0.0f;
        gatherBar.maxValue = 1;
	
        // Set max and starting value of health and hunger
        hpBar.maxValue = health;
        hpBar.value = health;
        hungerBar.maxValue = health;  // Starting hunger is 70/100
        hungerBar.value = hunger;

        m_Inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
        hpBar = GameObject.Find ( "HealthSlider" ).GetComponent<Slider> ();
        hungerBar = GameObject.Find ( "HungerSlider" ).GetComponent<Slider> ();
    }

    void OnLevelWasLoaded()
    {
        GameObject spawn = GameObject.Find ( "SpawnPoint" );
        // TODO: Keep track of visited locations
        // Beach initial spawn is in middle of map, spawn point changes to right side after that
        if ( spawn != null )
        {
            transform.position = spawn.transform.position;
        }
    }
	
	// Update is called once per frame
	void Update () 
    {
        if( m_State == PlayerState.Dialogue )
        {
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
            SetPlayerState ( PlayerState.Gather );
        }
        if ( m_Input.AimButtonHeld() )
        {
            SetPlayerState ( PlayerState.Aim );
        }
        if(m_State == PlayerState.Gather && gatherFrom != null)
        {
            gatherBarObj.GetComponent<RectTransform>().localPosition = Camera.main.WorldToScreenPoint(transform.position);
            gatherBarObj.SetActive(true);
            m_State = PlayerState.Gather;
			gatherTime += Time.deltaTime;
			if( gatherTime >= secondsGathering )
            {
                SetPlayerState( PlayerState.Idle );
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
    }

    // Starving for a day lowers max health
    // Gets called on a new day
    public void CheckStarved()
    {
        if( hungerBar.value < 50 )
        {
            hpBar.maxValue -= 20;
            Debug.Log ( "Starved" );
            if( hpBar.value > hpBar.maxValue)
            {
                hpBar.value = hpBar.maxValue;
            }
        }
        // Since its a new day, replenish some hunger due to resting
        hungerBar.value += 20;
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

    public void setCampLocation(Vector3 lastCamp)
    {
        campLocation = lastCamp;
    }

}