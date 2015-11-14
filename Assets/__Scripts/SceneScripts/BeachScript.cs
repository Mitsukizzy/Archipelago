using UnityEngine;
using System.Collections;

public class BeachScript : MonoBehaviour
{
    //Scene scripts ask GameManager what the state of the scene should be.
    //Checks GameManager's KeyItems and Events dictionary to know what items or events need to be spawned into the scene

    private GameManager mGame;

    // Use this for initialization
    void Start ()
    {
        //Beach will have the boat that needs to be fixed and the bag to pick up
        mGame = GameObject.FindGameObjectWithTag ( "Manager" ).GetComponent<GameManager> ();

        if ( mGame.CheckItem ( "Backpack" ) )
        {
            GameObject Backpack = Instantiate ( Resources.Load ( "Backpack", typeof ( GameObject ) ) ) as GameObject;
        }
        if ( mGame.CheckItem ( "JournalPageBeach" ) )
        {
            GameObject JournalPage = Instantiate ( Resources.Load ( "JournalPageBeach", typeof ( GameObject ) ) ) as GameObject;
            mGame.DoNotSpawnOnLoad ( "JournalPageBeach" );
        }
    }

    // Update is called once per frame
    void Update ()
    {

    }
}