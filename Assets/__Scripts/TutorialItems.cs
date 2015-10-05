using UnityEngine;
using System.Collections;

// Backpack and Boat
public class TutorialItems : MonoBehaviour 
{
    public Tutorial tutorial;
    private InputManager mInput;
    private bool canPlayBackpack = false;
    private bool canPlayBoat = false;

	// Use this for initialization
	void Start () 
    {
	    mInput = GameObject.Find ( "GameManager" ).GetComponent<InputManager> ();
	}
	
	// Update is called once per frame
	void Update () 
    {
        if ( canPlayBackpack )
        {
            tutorial.ShowDialogue ( 0, 2 );
            canPlayBackpack = false;
        }
        if ( canPlayBoat )
        {
            tutorial.ShowDialogue ( 3, 6 );
            canPlayBoat = false;
        }
	}

    void OnTriggerEnter2D ( Collider2D coll )
    {
        if ( coll.gameObject.tag == "Char" )
        {            
            if ( transform.name.Equals ( "Arrow" ) )
            {
                Application.LoadLevel ( 2 ); // Load the Wetlands
            }
            else
            {
                tutorial.SetHint ( "interact" );
            }
        }
    }

    void OnTriggerStay2D ( Collider2D coll )
    {
        if ( coll.gameObject.tag == "Char" && mInput.gatheringButtonPressed () )
        {
            if ( transform.name.Equals ( "Backpack" ) )
            {
                canPlayBackpack = true;
            }
            else if ( transform.name.Equals ( "Boat" ) )
            {
                canPlayBoat = true;
            }            
            tutorial.CloseHint ();
        }
    }

    void OnTriggerExit2D ( Collider2D coll )
    {
        if ( coll.gameObject.tag == "Char" )
        {
            tutorial.CloseHint ();
        }
    }
}
