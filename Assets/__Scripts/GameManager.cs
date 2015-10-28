﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class GameManager : MonoBehaviour 
{
    private Character m_Char;
    public GameObject gameOverOverlay;
    private GameState m_GameState;

    private AudioManager m_audio;
    private InputManager m_input;
    private bool isPlaying = false;

    //dictionary to hold all key items and if they have been picked up yet
    private Dictionary<string, bool> KeyItems;
    //TRUE = has not been found
    //FALSE = has been found

    public enum GameState
    {
        Tutorial,
        Normal
    }

	// Use this for initialization
	void Start () 
    {        
        KeyItems = new Dictionary<string,bool>();
        m_audio = GetComponent<AudioManager> ();
        m_input = GetComponent<InputManager> ();

        if ( Application.loadedLevelName.Equals ( "0_MainMenu" ) )
        {
            m_audio.PlayLoop ( "mystery" );
            isPlaying = false; 
        }
        else
        {
            m_Char = GameObject.Find ( "Character" ).GetComponent<Character> ();
            isPlaying = true;


            if ( Application.loadedLevelName.Equals ( "1_Beach" ) )
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

        KeyItems.Add("Backpack", true);
        KeyItems.Add("Boat", true);
	}

    void OnLevelWasLoaded(int level)
    {
        m_Char = GameObject.Find("Character").GetComponent<Character>();

        //use this function to change what music is being played on in each level


        //Move the character to the proper location
        if (!Application.loadedLevelName.Equals("1_Beach")) //the character is spawned on the beach
        {
            Vector3 spawnLoc = GameObject.Find("SpawnPoint").GetComponent<Transform>().position;
            m_Char.transform.position = spawnLoc;
        }

        //spawn in any key items for the level that have not been picked up yet
        if (Application.loadedLevelName.Equals("1_Beach"))
        {
            Debug.Log("Entered Beach");
            //Check if key items have been found, if not, spawn them in the proper x,y location
            if (KeyItems["Backpack"])
            {
                GameObject Backpack = Instantiate(Resources.Load("Backpack", typeof(GameObject))) as GameObject;
            }
            if (KeyItems["Boat"])
            {
                GameObject Boat = Instantiate(Resources.Load("Boat", typeof(GameObject))) as GameObject;
            }
        }
    }

    void DoNotSpawnOnLoad(string itemName)
    {
        if (KeyItems.ContainsKey(itemName))
        {
            KeyItems[itemName] = false;
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
        
        // Our cheat reset keys
        if ( m_input.ResetGameButtonPressed () )
        {
            Application.LoadLevel ( 0 ); // back to main menu
        }
        if ( m_input.ResetFromCampButtonPressed () )
        {
            // TODO (this will just load wetlands atm)
            Application.LoadLevel ( 2 ); // back to latest campfire save
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

    public DialogueSystem GetDialogueSystem ()
    {
        return GetComponent<DialogueSystem> ();
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
        m_Char.ReturnToCamp();
    }
}
