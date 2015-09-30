using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour 
{
    private Vector3 m_StartingPosition;
    private Character m_Char;

    public float aggroDistance = 20.0f;
    public float attackDistance = 10.0f;
    public float speed = 20.0f;
    private bool isAggroed = false;

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

        if ( distFromCharAttack <= attackDistance )
        {
            // Attack character
            Debug.Log ( "ATTACK" );
        }
        else if ( distFromCharAggro <= aggroDistance )
        {
            // Follow character
            isAggroed = true;
            transform.position = Vector3.MoveTowards ( transform.position, m_Char.transform.position, speed * Time.deltaTime );
        }
        else
        {
            if ( isAggroed )
            {
                // Go back to starting position
                transform.position = Vector3.MoveTowards ( transform.position, m_StartingPosition, speed * Time.deltaTime );
                
                if ( transform.position == m_StartingPosition )
                {
                    isAggroed = false;
                }
            }            
        }
	}
}
