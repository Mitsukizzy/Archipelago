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
    private Combat m_Combat;

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
    private bool firstGatherDone = false;

    //Inventory
    private Inventory m_Inventory;

    //Arrows
    public int numArrows = 10;
    Text arrowUIText;

    //Health and Hunger sliders
    public Slider hpBar;
    public Slider hungerBar;
    private Image hungerFill;
    private Color hungerColorDefault;
    private GameObject hpBG;
    private GameObject hpFill;
    private float hpFillWidth;
    private float hpWidthOffset = 0.0f;
    private int timesStarved = 0;

    // Health starved bgs
    public Sprite hpBG_Starved1;
    public Sprite hpBG_Starved2;

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
        m_Combat = GetComponent<Combat> ();
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
        arrowUIText = GameObject.Find("ArrowCount").GetComponent<Text>();
        hungerColorDefault = hungerFill.color;
        hpFillWidth = hungerFill.rectTransform.rect.width;

        // Set max and starting value of health and hunger
        hpBar.maxValue = health;
        hpBar.value = health;
        hungerBar.maxValue = hunger;
        hungerBar.value = hunger;
    }
	
	// Update is called once per frame
	void Update () 
    {
        arrowUIText.text = numArrows.ToString();

		if(m_State == PlayerState.Dialogue || m_State == PlayerState.Gather) //Unaggro birds if the character is gathering or in dialogue
		{
			if(GameObject.FindGameObjectWithTag("Bird") != null) //if there are any birds in the scene
			{
				GameObject[] birds = GameObject.FindGameObjectsWithTag("Bird");
				foreach (GameObject b in birds)
				{
					if(b.GetComponent<BirdAI>().GetBirdState() == BirdAI.BirdState.Attack)
					{
						b.GetComponent<BirdAI>().enterSafeArea();
						b.GetComponent<BirdAI>().StopAttacking();
					}
				}
			}
		}

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
        if ( m_Input.InteractButtonPressed() && gatherFrom != null && m_State != PlayerState.Aim && m_State != PlayerState.Dialogue)
        {
            if (gatherFrom.tag != "arrow" && gatherFrom.GetComponent<Interactable>().GetCanGather() )
            {
                m_Audio.PlayOnce("rustle");
                SetPlayerState(PlayerState.Gather);
                BeginGather();
            }            
        }
        if ( m_Input.AimButtonHeld () && m_State != PlayerState.Gather && m_Combat.GetHasBow () )
        {
            m_Animator.SetBool("isWalking", false);
            SetPlayerState ( PlayerState.Aim );
            if (!m_Animator.GetBool("isAiming"))
            {
                m_Animator.SetBool("isAiming", true);
            }
            bow.SetActive(true);
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos.z = transform.position.z;
            //face player towards mouse
            Vector3 FacePos = transform.localScale;
            if (pos.x > transform.position.x)
            {
                m_FacingRight = true;
                FacePos.x = -Mathf.Abs(FacePos.x);
            }
            else
            {
                m_FacingRight = false;
                FacePos.x = Mathf.Abs(FacePos.x);
            }
            transform.localScale = FacePos;
            
            Vector3 dir = pos - bow.transform.position;

            if (transform.localScale.x < 0)
            {
                dir.x = -dir.x;
            }

            Quaternion rotateTo = new Quaternion();
          
            //TODO clamp the rotation so you cant go over her head
            rotateTo.SetFromToRotation(Vector3.down, dir);

            bow.transform.rotation = rotateTo;
			Vector3 ClampedRotation = new Vector3(
				bow.transform.rotation.eulerAngles.x,
				bow.transform.rotation.eulerAngles.y,
				Mathf.Clamp(bow.transform.rotation.eulerAngles.z, 210, 320)
				);
			Quaternion ClampedQuaternion = Quaternion.Euler(ClampedRotation);
			bow.transform.rotation = ClampedQuaternion;
        }
        else
        {
            bow.SetActive(false);
        }
        if (m_Input.AimButtonReleased() && m_State == PlayerState.Aim)
        {
            m_Animator.SetBool("isAiming", false);
        }
        if ( m_State == PlayerState.Gather && gatherFrom != null )
        {
            if (!m_Animator.GetBool("isGathering"))
            {
                m_Animator.SetBool("isGathering", true);
            }
            gatherBarObj.SetActive(true);
			gatherTime += Time.deltaTime;
			if( gatherTime >= secondsGathering )
            {
                SetPlayerState( PlayerState.Idle );
                m_Animator.SetBool("isGathering", false);
				gatherBar.value = 0;
				gatherTime = 0.0f;
                if( !firstGatherDone )
                {
                    firstGatherDone = true;
                    m_Dialogue.StartDialogue( "wetlands2" );
                }
                m_Audio.PlayOnce ( "newItem" );
				gatherFrom.GetComponent<Interactable>().ReceiveItem();
                gatherFrom.GetComponent<Interactable>().SwitchToGatheredSprite();
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
            if ( m_Input.GetHorizontalMovement () != Vector3.zero || m_Input.GetVerticalMovement () != Vector3.zero )
            {
                transform.Translate ( m_Input.GetHorizontalMovement () * speed );
                transform.Translate ( m_Input.GetVerticalMovement () * speed );
                m_Animator.SetBool ( "isWalking", true );
                m_Inventory.CloseInventory (); // Close inventory if moving
            }

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
            newScale.x = -Mathf.Abs(newScale.x);
            transform.localScale = newScale;
        }
        else if ( m_Input.GetHorizontalMovement ().x < 0 && m_FacingRight )
        {
            m_FacingRight = false;
            Vector3 newScale = transform.localScale;
            newScale.x = Mathf.Abs(newScale.x);
            transform.localScale = newScale;
        }
    }

    public void CollectArrow()
    {
        numArrows++;
        m_Audio.PlayOnce ( "arrowPickup" );
        if (numArrows == 1)
        {
            bow.GetComponent<BowScript>().swapSprite();
        }

    }

    public void TakeDamage ( int damage )
    {
        m_Animator.SetTrigger("Hit");
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
        if( speed == runSpeed )
        {
            amount *= 2; // If running, hunger runs out twice as fast
        }
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
            timesStarved++;
            hpBar.maxValue -= ( health / 5 ); // permanently lose a fifth of max health
            m_Game.IncreaseDaysStarved ();
            if( hpBar.value > hpBar.maxValue)
            {
                hpBar.value = hpBar.maxValue;
            }
            ChangeHPBarSprite ();

            // TODO: Implement better way to indicate permanent reduction of max health
            if ( hpWidthOffset < ( hpFillWidth * 0.8f ) )
            {
                // Don't lower max hp if it would make max hp 0
                hpWidthOffset += ( hpFillWidth / 5.0f );
                hpFill.GetComponent<RectTransform> ().sizeDelta = new Vector2 ( hpFillWidth - hpWidthOffset, 12.5f );
            }
        }
        // Since its a new day, replenish some hunger due to resting
        IncrementHunger ( 10 );
    }

    private void ChangeHPBarSprite()
    {
        switch( timesStarved )
        {
            case 1:
                hpBar.GetComponent<Image> ().sprite = hpBG_Starved1;
                break;
            case 2:
                hpBar.GetComponent<Image> ().sprite = hpBG_Starved1;
                break;
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
        hungerBar.value = hunger;
        hpBar.maxValue = health;
        hpWidthOffset = 0;
        hpFill.GetComponent<RectTransform> ().sizeDelta = new Vector2 ( hpFillWidth - hpWidthOffset, 12.5f );
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
        campLocation = GameObject.FindGameObjectWithTag( "Campsite" ).transform.position;
        if (campLocation != null)
        {
            transform.position = campLocation;
            m_Animator.SetBool("isAtking", false);
            m_Animator.SetBool("isWalking", false);
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