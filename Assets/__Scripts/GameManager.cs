using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class GameManager : MonoBehaviour 
{
    public static GameManager instance;

    private Character m_Char;
    private AudioManager m_audio;
    private InputManager m_input;
    private DayNightManager m_daynight;
    private DialogueSystem m_dialogue;

    private GameObject uiObj;

    //dictionary to hold all key items and if they have been picked up yet
    private Dictionary<string, bool> KeyItems;
    //TRUE = has not been found
    //FALSE = has been found

    // Metrics trackers
    public bool trackMetrics = false;
    private string metricsText = "";
    private Dictionary<string, float> timeSpentPerLevel = new Dictionary<string, float> ();
    private List<string> locationTimestamps = new List<string> ();
    private int daysStarved = 0;
    private int deaths = 0;
    private int numPlays = 1;

    private bool hasVisitedBeach = false;
    private bool hasVisitedWetlands = false;
    private bool hasPlayedIntroSplash = false;
    private bool hasJustWon = false; // trigger for credits

	private int CurrentSceneIndex;
	private int PreviousSceneIndex;

    // Persistent (Singleton)
    void Awake ()
    {
        if ( instance == null )
        {
            instance = this;
            DontDestroyOnLoad ( gameObject );
        }
        else
        {
            Destroy ( this );
        }
    }

	// Use this for initialization
	void Start () 
    {        
        KeyItems = new Dictionary<string,bool>();
        m_audio = GetComponent<AudioManager> ();
        m_input = GetComponent<InputManager> ();
        m_daynight = GetComponent<DayNightManager> ();
        m_dialogue = GetComponent<DialogueSystem> ();

        KeyItems.Add ( "Backpack", true );
        KeyItems.Add ( "Boat", true );
        KeyItems.Add ( "Bow", true );
        KeyItems.Add("wood", true);
        KeyItems.Add("rope", true);
        KeyItems.Add("hammer", true);
        KeyItems.Add ( "JournalPageBeach", true );
        KeyItems.Add ( "JournalPageSeaCave", true );
        KeyItems.Add ( "JournalPageDocks", true );
		KeyItems.Add ( "JournalPagePlains", true);

		CurrentSceneIndex = 0;
		PreviousSceneIndex = -1; // On purpose for day night check

        m_audio.PlayLoop ( "main" ); // Start with main theme music
        locationTimestamps.Add ( "Main Menu - " + Time.time );
        if (Application.loadedLevelName != "0_MainMenu")
        {
            m_Char = GameObject.FindGameObjectWithTag("Char").GetComponent<Character>();
        }
	}

    void OnLevelWasLoaded()
    {
        if ( Application.loadedLevel != 0 && Application.loadedLevel != 7 )
        {
            m_Char = GameObject.FindGameObjectWithTag( "Char" ).GetComponent<Character>();
            uiObj = GameObject.FindGameObjectWithTag ( "UI" ).gameObject;
            uiObj.transform.localScale = new Vector3( 1, 1, 1 ); // unhide the UI
        }

        // Move the character to the proper location
        // Beach initial spawn is in middle of map, spawn point changes to right side after that
        // Don't spawn if on main menu(0) or game over(7)
        if ( !Application.loadedLevelName.Equals ( "1_Beach" ) && Application.loadedLevel != 0 && Application.loadedLevel != 7 )
        {
            Vector3 spawnLoc;
            PreviousSceneIndex = CurrentSceneIndex;
            CurrentSceneIndex = Application.loadedLevel;

            if ( CurrentSceneIndex < PreviousSceneIndex )
            {
                spawnLoc = GameObject.Find ( "SpawnPoint2" ).GetComponent<Transform> ().position;
            }
            else
            {
                spawnLoc = GameObject.Find ( "SpawnPoint" ).GetComponent<Transform> ().position;
            }
            m_Char.transform.position = spawnLoc;

            if( !hasVisitedWetlands )
            {
                m_dialogue.StartDialogue ( "wetlands1" );
                //hasVisitedWetlands = true; //moved to campfire
            }
        }
        else if ( Application.loadedLevel == 1 && CurrentSceneIndex > 1 ) // Coming from wetlands to beach
        {
            m_Char.transform.position = GameObject.Find ( "SpawnPoint2" ).GetComponent<Transform> ().position;
            hasVisitedBeach = true;
        }

        // Chooses music being played in each level
        m_audio.SpecialInit ();
        if ( Application.loadedLevel == 0 )
        {
            m_audio.PlayLoop( "main" );
            locationTimestamps.Add("Main Menu - " + Time.time);
            hasPlayedIntroSplash = true;
        }
        else if ( Application.loadedLevel == 1 )
        {
            m_audio.PlayLoop ( "beach" ); 
            m_audio.PlayLayer( "beachDay" );
            locationTimestamps.Add ( "Beach - " + Time.time );
        }
        else if ( Application.loadedLevel == 2 )
        {
            m_audio.PlayLoop( "wetlands" );
            m_audio.PlayLayer( "wetlandsDay" );
            locationTimestamps.Add ( "Wetlands - " + Time.time );
        }
        else if ( Application.loadedLevel == 3 )
        {
            m_audio.PlayLoop ( "forest" );
            m_audio.PlayLayer( "forestDay" );
            locationTimestamps.Add ( "Forest - " + Time.time );
        }
        else if ( Application.loadedLevel == 4 )
        {
            m_audio.PlayLoop ( "docks" );
            locationTimestamps.Add ( "Docks - " + Time.time );
        }
        else if ( Application.loadedLevel == 5 )
        {
            m_audio.PlayLoop ( "seacave" );
            locationTimestamps.Add ( "Sea Cave - " + Time.time );
        }
        else if ( Application.loadedLevel == 6 )
        {
            m_audio.PlayLoop ( "plains" );
            m_audio.PlayLayer ( "plainsDay" );
            locationTimestamps.Add ( "Plains - " + Time.time );
        }
        else if ( Application.loadedLevel == 7 )
        {
            m_audio.PlayLoop ( "main" );
            locationTimestamps.Add ( "Game Over - " + Time.time );
        }
    }

    // Write metrics on game quit
    // In Editor, this will show up in the project folder root (with Library, Assets, etc.)
    // In Standalone, this will show up in the same directory as your executable
    void OnApplicationQuit ()
    {
        if ( trackMetrics )
        {
            GenerateMetricsString ();
            string time = System.DateTime.UtcNow.ToString ();
            string dateTime = System.DateTime.Now.ToString (); //Get the time to tack on to the file name
            time = time.Replace ( "/", "-" ); // Replace slashes with dashes, because Unity thinks they are directories..
            time = time.Replace ( ":", "." ); // Replace colons with periods for time
            string reportFile = "Archipelago_Metrics_" + time + ".txt";
            File.WriteAllText ( reportFile, metricsText );
        }
    }

	// Update is called once per frame
	void Update () 
    {
        // Help
        if ( m_input.HelpButtonPressed() )
        {
            ToggleSplash();
        }

        if ( Application.loadedLevel != 0 && Application.loadedLevel != 7 ) //Not the main menu or game over screen
        {
            // Check if alive
            if ( !m_Char.IsAlive () )
            {
                // Move to Game Over screen
                m_Char.Revive ();
                m_audio.PlayOnce ( "playerDeath" );
                deaths++;
                uiObj.transform.localScale = Vector3.zero; // hide the UI
                Application.LoadLevel ( 7 ); // to game over screen
            }
            
            // Pause
            if ( m_input.PauseButtonPressed () )
            {
                Pause ();
            }

            // Inventory
            if( m_input.InventoryButtonPressed() )
            {
                GameObject.Find ( "Inventory" ).GetComponent<Inventory> ().ToggleInventory ();
            }

            // Journal
            if ( m_input.JournalButtonPressed () )
            {
                Journal journal = GameObject.FindGameObjectWithTag ( "Journal" ).GetComponent<Journal> ();
                if( journal.GetIsOpen() || ( m_Char.GetPlayerState () != Character.PlayerState.Dialogue && m_Char.GetPlayerState () != Character.PlayerState.Interact ) )
                {
                    // Toggle if journal is already open OR player is not in dialogue or interact mode
                    journal.ToggleJournal ();
                }
            }
        }
        else
        {
            // Pause press in main menu
            if ( m_input.PauseButtonPressed () )
            {
                ShowSplash ( false ); // close splash
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

    public bool CheckHasVisitedBeach()
    {
        return hasVisitedBeach;
    }

    public void Pause()
    {
        GameObject pauseOverlay = GameObject.FindGameObjectWithTag ( "UI" ).transform.Find ( "Pause UI" ).gameObject; // my way of finding inactive gameobject

        if ( Time.timeScale == 0 )
        {
            GameObject splashOverlay = GameObject.FindGameObjectWithTag ( "UI" ).transform.Find ( "Splash UI" ).gameObject; 

            if ( splashOverlay.activeSelf )
            {
                ShowSplash ( false );
            }
            else
            {
                pauseOverlay.SetActive ( false );
                ShowSplash ( false );
                Time.timeScale = 1;
            }
        }
        else
        {
            pauseOverlay.SetActive ( true );
            Time.timeScale = 0;
        }
    }

    public void ShowSplash( bool shouldShow )
    {
        GameObject splashOverlay = GameObject.FindGameObjectWithTag ( "UI" ).transform.Find ( "Splash UI" ).gameObject; // my way of finding inactive gameobject
        splashOverlay.SetActive ( shouldShow );
    }

    public void ToggleSplash ()
    {
        GameObject splashOverlay = GameObject.FindGameObjectWithTag("UI").transform.Find("Splash UI").gameObject; // my way of finding inactive gameobject
        if ( !splashOverlay.activeSelf )
        {
            Pause (); // order matters
            splashOverlay.SetActive ( !splashOverlay.activeSelf ); 
        }
        else
        {
            splashOverlay.SetActive ( !splashOverlay.activeSelf ); 
            Pause ();
        }
    }

    public void ExitGame ()
    {
        Application.Quit ();
    }

    public void SetHasJustWon( bool won )
    {
        hasJustWon = won;
    }

    public bool GetHasJustWon()
    {
        return hasJustWon;
    }

    public void MainMenu ()
    {
        // Make sure it's not paused
        GameObject pauseOverlay = GameObject.FindGameObjectWithTag ( "UI" ).transform.Find ( "Pause UI" ).gameObject; // my way of finding inactive gameobject
        if( pauseOverlay )
        {
            pauseOverlay.SetActive ( false );
            Time.timeScale = 1;
        }

        // Going to MM indicates end of one playthrough
        GenerateMetricsString ();

        // Delete persistent objects that will be spawned again in Main Menu
        Destroy ( GameObject.FindGameObjectWithTag ( "UI" ).gameObject );
        Destroy ( GameObject.FindGameObjectWithTag ( "Char" ).gameObject );
        Application.LoadLevel ( 0 );
    }

    public void RetryFromCampfire ()
    {
        // TODO: Replace with code to load last level of campsite
        Application.LoadLevel(2);
        m_Char = GameObject.FindGameObjectWithTag ( "Char" ).GetComponent<Character> ();
        m_daynight = GetComponent<DayNightManager> ();
        m_Char.ReturnToCamp ();
        m_daynight.ContinueDay ();
    }

    public void LoadNextLevel()
    {
        if( Application.loadedLevel < 6 )
        {
            Application.LoadLevel ( Application.loadedLevel + 1 );
        }
    }

    void GenerateMetricsString ()
    {
        metricsText += "-------- Playthrough " + numPlays + " --------";
        metricsText += "Days Starved: " + daysStarved + '\n';
        metricsText += '\n' + "Player Deaths: " + deaths + '\n';

        metricsText += '\n' + "Timestamps of visits to each location. (in seconds since game start)" + '\n';
        for ( int i = 0; i < locationTimestamps.Count; i++ )
        {
            metricsText += locationTimestamps[i] + '\n';
        }
        metricsText += '\n';

        numPlays++;

        // Clear these out in case there is another playthrough
        timeSpentPerLevel = new Dictionary<string, float> ();
        locationTimestamps = new List<string> ();
        daysStarved = 0;
        deaths = 0;
    }

    public void IncreaseDaysStarved()
    {
        daysStarved++;
    }

    public bool CheckItem ( string ItemName )
    {
        if (KeyItems == null)
        {
            Debug.Log("why is this null");
        }
        if ( !KeyItems.ContainsKey ( ItemName ) )
        {
            Debug.Log ( "Checking an item that doesnt exist" );
            return false;
        }
        else
        {
            return KeyItems[ItemName];
        }
    }

    public void DoNotSpawnOnLoad ( string itemName )
    {
        if ( KeyItems.ContainsKey ( itemName ) )
        {
            KeyItems[itemName] = false;
        }
    }

	public bool GetHasVisitedWetlands()
    {
		return hasVisitedWetlands;
	}

	public void SetHasVisitedWetlands(bool hasVisited)
    {
		hasVisitedWetlands = hasVisited;
	}

    public int GetCurrentSceneIndex ()
    {
        return CurrentSceneIndex;
    }

    public int GetPreviousSceneIndex ()
    {
        return PreviousSceneIndex;
    }

    public bool GetHasPlayedIntroSplash ()
    {
        return hasPlayedIntroSplash;
    }
}
