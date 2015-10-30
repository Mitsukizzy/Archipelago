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

	}

    void OnLevelWasLoaded()
    {

        if ( Application.loadedLevel != 0 )
        {
            m_Char = GameObject.Find( "Character" ).GetComponent<Character>();
        }

        // Move the character to the proper location
        // TODO: Keep track of visited locations
        // Beach initial spawn is in middle of map, spawn point changes to right side after that
        if (!Application.loadedLevelName.Equals("1_Beach")) //the character is spawned on the beach
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
        //use this function to change what music is being played on in each level
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
        if ( Application.loadedLevel != 0 ) //Not the main menu
        {
            if ( !m_Char.IsAlive () )
            {
                // Move to Game Over screen
                Debug.Log ( "GAME OVER" );
                Application.LoadLevel ( 7 ); // to game over screen
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

    public void MainMenu ()
    {
        Application.LoadLevel ( 0 );
    }

    public void RetryFromCampfire ()
    {
        m_Char.Revive ();
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
