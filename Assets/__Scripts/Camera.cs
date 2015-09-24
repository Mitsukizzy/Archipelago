using UnityEngine;
using System.Collections;

public class Camera : MonoBehaviour 
{
    private Character m_Char;

    public float leftLimit = -1.0f;
    public float rightLimit = -1.0f;

	// Use this for initialization
	void Start () 
    {
        m_Char = GameObject.Find ( "Character" ).GetComponent<Character> ();
	}
	
	// Update is called once per frame
	void Update () 
    {
        // Center camera on player, clamp to limits
        transform.position = new Vector3 ( m_Char.transform.position.x, m_Char.transform.position.y, -50 );
        transform.position = new Vector3 ( Mathf.Clamp ( transform.position.x, leftLimit, rightLimit ), transform.position.y, transform.position.z );
	}
}
