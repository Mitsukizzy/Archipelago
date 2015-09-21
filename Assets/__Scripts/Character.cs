using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour {

    private InputManager m_input;

    private bool m_facingRight;

    private float speed = 5.0f;
    private float runSpeed = 10.0f;
    private float dodgeSpeed = 300.0f;

    private float originalSpeed;

	// Use this for initialization
	void Start () 
    {
        m_facingRight = false;
        originalSpeed = speed;

        m_input = GameObject.Find ( "InputManager" ).GetComponent<InputManager> ();
	}
	
	// Update is called once per frame
	void Update () 
    {
        // Move character
        transform.Translate ( m_input.GetHorizontalMovement() * speed );
        transform.Translate ( m_input.GetVerticalMovement() * speed );

        if ( m_input.RunButtonPressed () )
        {
            speed = runSpeed;
        }
        else
        {
            speed = originalSpeed;
        }

        if ( m_input.DodgeButtonReleased () )
        {
            transform.Translate ( m_input.GetHorizontalMovement () * dodgeSpeed );
            transform.Translate ( m_input.GetVerticalMovement () * dodgeSpeed );
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
}