using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Character : MonoBehaviour {

    private InputManager m_input;

    private bool m_facingRight;

    private float speed = 5.0f;
    private float runSpeed = 10.0f;
    private float dodgeSpeed = 300.0f;
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

	// Use this for initialization
	void Start () 
    {
        m_facingRight = false;
        originalSpeed = speed;

        m_input = GameObject.Find ( "InputManager" ).GetComponent<InputManager> ();

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
		if(!isGathering){
        	transform.Translate ( m_input.GetHorizontalMovement() * speed );
        	transform.Translate ( m_input.GetVerticalMovement() * speed );
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

        if ( m_input.RunButtonPressed () )
        {
            speed = runSpeed;
        }
        else
        {
            speed = originalSpeed;
        }

		if( m_input.DodgeButtonPressed() )
		{
			dodgeReleased = false;
		}

        if ( m_input.DodgeButtonReleased () )
        {
			dodgeTrail.GetComponent<TrailRenderer>().material.SetColor("_TintColor", new Color(255,255,255,20));

			transform.Translate ( m_input.GetHorizontalMovement () * dodgeSpeed );
            transform.Translate ( m_input.GetVerticalMovement () * dodgeSpeed );
			dodgeReleased = true;		}

		if( m_input.gatheringButtonPressed () )
		{
			GatherItem();
		}

        FaceSpriteTowardDirection ();
	}

    void FaceSpriteTowardDirection ()
    {
        if ( m_input.GetHorizontalMovement ().x > 0 && !m_facingRight )
        {
            m_facingRight = true;
            Vector3 newScale = transform.localScale;
            newScale.x *= -1;
            transform.localScale = newScale;
        }
        else if ( m_input.GetHorizontalMovement ().x < 0 && m_facingRight )
        {
            m_facingRight = false;
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