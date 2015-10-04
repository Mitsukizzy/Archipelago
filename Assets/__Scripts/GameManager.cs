using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour 
{
    private Character m_Char;
    public GameObject gameOverOverlay;

	// Use this for initialization
	void Start () 
    {
        m_Char = GameObject.Find ( "Character" ).GetComponent<Character> ();
	}
	
	// Update is called once per frame
	void Update () 
    {
        if ( !m_Char.IsAlive () )
        {
            // Move to Game Over screen
            gameOverOverlay.SetActive ( true );
        }
	}

    public void MainMenu ()
    {
        Application.LoadLevel ( 0 );
    }

    public void RetryFromCampfire ()
    {
        m_Char.Revive ();
        gameOverOverlay.SetActive ( false );
    }
}
