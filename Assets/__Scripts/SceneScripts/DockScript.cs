using UnityEngine;
using System.Collections;

public class DockScript : MonoBehaviour {

    //Scene scripts ask GameManager what the state of the scene should be.
    //Checks GameManager's KeyItems and Events dictionary to know what items or events need to be spawned into the scene

    private GameManager mGame;

    // Use this for initialization
    void Start()
    {
        //The Forest has wood as a key item for fixing the boat
        mGame = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>();

        if ( mGame.CheckItem ( "JournalPageDocks" ) )
        {
            GameObject JournalPage = Instantiate ( Resources.Load ( "JournalPageDocks", typeof ( GameObject ) ) ) as GameObject;
        }
        if (mGame.CheckItem("rope"))
        {
            GameObject Backpack = Instantiate(Resources.Load("rope", typeof(GameObject))) as GameObject;
        } 
    }

    // Update is called once per frame
    void Update()
    {

    }
}
