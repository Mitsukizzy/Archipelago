using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour 
{
    private Character m_Char;
    public GameObject gameOverOverlay;
    private GameState m_GameState;

    private AudioManager m_audio;
    private bool isPlaying = false; 

    public enum GameState
    {
        Tutorial,
        Normal
    }

	// Use this for initialization
	void Start () 
    {        
        m_audio = GetComponent<AudioManager> ();

        if ( Application.loadedLevelName.Equals ( "MainMenu" ) )
        {
            m_audio.PlayLoop ( "mystery" );
            isPlaying = false; 
        }
        else
        {
            m_Char = GameObject.Find ( "Character" ).GetComponent<Character> ();
            isPlaying = true;


            if ( Application.loadedLevelName.Equals ( "Beach" ) )
            {
                m_GameState = GameState.Tutorial;
                m_audio.PlayLoop ( "main" );
            }
            else
            {
                m_GameState = GameState.Normal;
                m_audio.PlayLoop ( "main" );
            }  
        }      
	}
	
	// Update is called once per frame
	void Update () 
    {
        if ( isPlaying )
        {
            if ( !m_Char.IsAlive () )
            {
                // Move to Game Over screen
                gameOverOverlay.SetActive ( true );
                m_Char.Revive ();
            }
        }
	}

    public GameManager GetGameManager ()
    {
        return GetComponent<GameManager> ();
    }

    public InputManager GetInputManager ()
    {
        return GetComponent<InputManager> ();
    }

    public AudioManager GetAudioManager ()
    {
        return GetComponent<AudioManager> ();
    }

    public void SetGameState ( GameState newState )
    {
        m_GameState = newState;
    }

    public GameState GetGameState()
    {
        return m_GameState;
    }

    public void MainMenu ()
    {
        Application.LoadLevel ( 0 );
    }

    public void RetryFromCampfire ()
    {
        gameOverOverlay.SetActive ( false );
    }
}
