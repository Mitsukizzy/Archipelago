using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour 
{
    private Vector3 m_StartingPosition;
    private Character m_Char;
    private AudioManager m_Audio;

    public float aggroDistance = 20.0f;         // Distance from player that prompts movement
    public float attackDistance = 10.0f;        // Distance from player that prompts attacks
    public float moveSpeed = 20.0f;             // Movement speed
    public float attackSpeed = 2.0f;            // Time between attacks in seconds
    public int health = 250;
    private bool isAggroed = false;
    private bool canAttack = true;             // The enemy's cooldown control

    private bool isDead = false;

    //item drops
    public float dropRng = 1;

	// Use this for initialization
	void Start () 
    {
        m_StartingPosition = transform.position;
        m_Char = GameObject.Find ( "Character" ).GetComponent<Character> ();
        m_Audio = GameObject.Find ( "GameManager" ).GetComponent<AudioManager> ();
	}
	
	// Update is called once per frame
	void Update () 
    {
        float distFromCharAggro = ( m_StartingPosition - m_Char.transform.position ).magnitude;
        float distFromCharAttack = ( transform.position - m_Char.transform.position ).magnitude;

        if ( m_Char.IsAlive () )
        {
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
                // Set player state to Fight
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
        }

        // Check if dead
        if ( health <= 0 && !isDead )
        {
            // Play death animation and sound, then destroy
            m_Audio.PlayOnce ( "enemyDeath" );
            //obtain dropable item!
            if (GetComponent<Interactable>().gatherableItem != null)
            {
                if (Random.value <= dropRng)
                {
                    GetComponent<Interactable>().ReceiveItem();
                    GetComponent<Interactable>().gatherableItem = null;
                }
            }
            GetComponent<Animator>().SetTrigger("Death");
            isDead = true;
        }
	}

    public void OnDeath()
    {
        Destroy(gameObject);
    }

    IEnumerator AttackPlayer ()
    {
        GetComponent<Animator>().SetTrigger("Attack");
        m_Char.TakeDamage ( 40 );
        canAttack = false;
        yield return new WaitForSeconds ( attackSpeed );
        canAttack = true;
    }

    public void TakeDamage ( int damage )
    {
        health -= damage;
        if ( health > 0 ) 
        {
            m_Audio.PlayOnce ( "enemyDamaged" );
        }
        GetComponent<Animator>().SetTrigger("Hit");
    }
}
