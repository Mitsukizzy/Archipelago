using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour 
{
    private Vector3 m_StartingPosition;
    private Character m_Char;

    public float aggroDistance = 20.0f;         // Distance from player that prompts movement
    public float attackDistance = 10.0f;        // Distance from player that prompts attacks
    public float moveSpeed = 20.0f;             // Movement speed
    public float attackSpeed = 2.0f;            // Time between attacks in seconds
    public int health = 250;
    private bool isAggroed = false;
    private bool canAttack = true;             // The enemy's cooldown control

	// Use this for initialization
	void Start () 
    {
        m_StartingPosition = transform.position;
        m_Char = GameObject.Find ( "Character" ).GetComponent<Character> ();
	}
	
	// Update is called once per frame
	void Update () 
    {
        float distFromCharAggro = ( m_StartingPosition - m_Char.transform.position ).magnitude;
        float distFromCharAttack = ( transform.position - m_Char.transform.position ).magnitude;

        Debug.Log ( "Enemy Health: " + health );
        if ( distFromCharAttack <= attackDistance )
        {
            // Attack player 
            if ( canAttack )
            {
                StartCoroutine ( AttackPlayer () );
            }
        }
        else if ( distFromCharAggro <= aggroDistance )
        {
            // Follow character
            isAggroed = true;
            m_Char.SetPlayerState ( Character.PlayerState.Fight );
            transform.position = Vector3.MoveTowards ( transform.position, m_Char.transform.position, moveSpeed * Time.deltaTime );
        }
        else
        {
            if ( isAggroed )
            {
                // Go back to starting position
                transform.position = Vector3.MoveTowards ( transform.position, m_StartingPosition, moveSpeed * Time.deltaTime );
                
                if ( transform.position == m_StartingPosition )
                {
                    isAggroed = false;
                }
            }            
        }

        // Check if dead
        if ( health <= 0 )
        {
            // Play death animation and sound, then destroy
            Destroy ( gameObject );
        }
	}

    IEnumerator AttackPlayer ()
    {
        m_Char.TakeDamage ( 40 );
        canAttack = false;
        yield return new WaitForSeconds ( attackSpeed );
        canAttack = true;
    }

    public void TakeDamage ( int damage )
    {
        health -= damage;
    }
}
