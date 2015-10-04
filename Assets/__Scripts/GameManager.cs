﻿using UnityEngine;
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
            m_Char.Revive ();
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

    public void MainMenu ()
    {
        Application.LoadLevel ( 0 );
    }

    public void RetryFromCampfire ()
    {
        gameOverOverlay.SetActive ( false );
    }
}
