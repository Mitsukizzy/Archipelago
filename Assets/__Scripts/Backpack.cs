using UnityEngine;
using System.Collections;

public class Backpack : MonoBehaviour 
{
    public Tutorial tutorial;
    private InputManager mInput;
    private bool canPlayBackpack = false;

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
	}

    void OnTriggerEnter2D ( Collider2D coll )
    {
        if ( coll.gameObject.tag == "Char" )
        {
            tutorial.SetHint ( "interact" );
        }
    }

    void OnTriggerStay2D ( Collider2D coll )
    {
        if ( coll.gameObject.tag == "Char" && mInput.gatheringButtonPressed () )
        {
            canPlayBackpack = true;
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
