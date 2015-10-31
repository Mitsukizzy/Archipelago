using UnityEngine;
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

    //dictionary to hold all key items and if they have been picked up yet
    private Dictionary<string, bool> KeyItems;
    //TRUE = has not been found
    //FALSE = has been found

	private int CurrentSceneIndex;
	private int PreviousSceneIndex;

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

        KeyItems.Add("Backpack", true);
        KeyItems.Add("Boat", true);

		CurrentSceneIndex = 0;
		PreviousSceneIndex = 0;
        if (Application.loadedLevelName != "0_MainMenu")
        {
            m_Char = GameObject.FindGameObjectWithTag("Char").GetComponent<Character>();
        }
	}

    void OnLevelWasLoaded()
    {
        if ( Application.loadedLevel != 0 && Application.loadedLevel != 7 )
        {
            m_Char = GameObject.FindGameObjectWithTag("Char").GetComponent<Character>();
        }

        // TODO: Keep track of visited locations
        // Move the character to the proper location
        // Beach initial spawn is in middle of map, spawn point changes to right side after that
        // Don't spawn if on main menu(0) or game over(7)
        if ( !Application.loadedLevelName.Equals ( "1_Beach" ) && Application.loadedLevel != 0 && Application.loadedLevel != 7 ) 
        {
			PreviousSceneIndex = CurrentSceneIndex;
			CurrentSceneIndex = Application.loadedLevel;
			Vector3 spawnLoc;
			if(CurrentSceneIndex < PreviousSceneIndex){
				spawnLoc = GameObject.Find("SpawnPoint2").GetComponent<Transform>().position;
			}
			else{
				spawnLoc = GameObject.Find("SpawnPoint").GetComponent<Transform>().position;
			}
            m_Char.transform.position = spawnLoc;
        }

        // Chooses music being played in each level
        if ( Application.loadedLevel == 0 )
        {
            m_audio.PlayLoop( "main" );
        }
        else if ( Application.loadedLevelName.Equals( "1_Beach" ) )
        {
            Debug.Log( "Entered Beach" );
            m_GameState = GameState.Tutorial;
            m_audio.PlayLoop( "beach" );
        }
        else if ( Application.loadedLevelName.Equals( "2_Wetlands" ) )
        {
            m_GameState = GameState.Normal;
            m_audio.PlayLoop( "wetlands" );
        }  
    }

	public bool CheckItem(string ItemName){
		if(!KeyItems.ContainsKey(ItemName)){
			Debug.Log ("Checking an item that doesnt exist");
			return false;
		}
		else{
			return KeyItems[ItemName];
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
        if ( Application.loadedLevel != 0 && Application.loadedLevel != 7 ) //Not the main menu or game over screen
        {
            // Check if alive
            if ( !m_Char.IsAlive () )
            {
                // Move to Game Over screen
                Debug.Log ( "GAME OVER" );
                m_Char.Revive ();
                Application.LoadLevel ( 7 ); // to game over screen
            }
            
            // Pause
            if ( m_input.PauseButtonPressed () )
            {
                Pause ();
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
        if ( m_input.NextLevelButtonPressed () )
        {
            LoadNextLevel ();
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

    public void Pause()
    {
        GameObject pauseOverlay = GameObject.Find ( "Status UI" ).transform.Find ( "Paused Overlay" ).gameObject; // my way of finding inactive gameobject
        if ( Time.timeScale == 0 )
        {
            pauseOverlay.SetActive ( false );
            Time.timeScale = 1;
        }
        else
        {
            pauseOverlay.SetActive ( true );
            Time.timeScale = 0;
        }
    }

    public void MainMenu ()
    {
        Application.LoadLevel ( 0 );
    }

    public void RetryFromCampfire ()
    {
        // TODO: Replace with code to load last level of campsite
        Application.LoadLevel ( 2 );
        m_Char.ReturnToCamp();
    }

    public void LoadNextLevel()
    {
        if( Application.loadedLevel < 6 )
        {
            Application.LoadLevel ( Application.loadedLevel + 1 );
        }
    }
}
