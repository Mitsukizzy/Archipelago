using UnityEngine;
using System.Collections;

public class SeaCaveScript : MonoBehaviour
{

    private GameManager mGame;

    // Use this for initialization
    void Start ()
    {
        //Beach will have the boat that needs to be fixed and the bag to pick up
        mGame = GameObject.FindGameObjectWithTag ( "Manager" ).GetComponent<GameManager> ();

        if ( mGame.CheckItem ( "JournalPageSeaCave" ) )
        {
            GameObject JournalPage = Instantiate ( Resources.Load ( "JournalPageSeaCave", typeof ( GameObject ) ) ) as GameObject;
        }
        if (mGame.CheckItem("hammer"))
        {
            GameObject Backpack = Instantiate(Resources.Load("hammer", typeof(GameObject))) as GameObject;
        }
    }

    // Update is called once per frame
    void Update ()
    {

    }
}