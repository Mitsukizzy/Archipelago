using UnityEngine;
using System.Collections;

// Backpack and Boat
public class TutorialItems : MonoBehaviour 
{
    public Tutorial tutorial;
    private InputManager mInput;
    private AudioManager mAudio;
    private bool canPlayBackpack = false;
    private bool canPlayBoat = false;
    private bool canPlayBush = false;
    private bool canPlayCampfire = false;

    private bool boatPlayOnce = true;
    private bool bushPlayOnce = true;
    private bool campfirePlayOnce = true;

	// Use this for initialization
	void Start () 
    {
	    mInput = GameObject.Find ( "GameManager" ).GetComponent<InputManager> ();
        mAudio = GameObject.Find ( "GameManager" ).GetComponent<AudioManager> ();
	}
	
	// Update is called once per frame
	void Update () 
    {
        if ( canPlayBackpack )
        {
            tutorial.ShowDialogue ( 0, 2 );
            canPlayBackpack = false;
        }
        if ( canPlayBoat && boatPlayOnce )
        {
            tutorial.ShowDialogue ( 3, 6 );
            canPlayBoat = false;
            boatPlayOnce = false;
        }
        if( canPlayBush && bushPlayOnce )
        {
            tutorial.ShowDialogue(7, 8);
            canPlayBush = false;
            bushPlayOnce = false;
        }
        if( canPlayCampfire && campfirePlayOnce )
        {
            tutorial.ShowDialogue(9, 10);
            canPlayCampfire = false;
            campfirePlayOnce = false;
        }

	}

    IEnumerator Transition ( int seconds = 1 )
    {        
        mAudio.PlayOnce ( "transition" );
        yield return new WaitForSeconds ( seconds );
        Application.LoadLevel ( 2 ); // Load the Wetlands
    }

    void OnTriggerEnter2D ( Collider2D coll )
    {
        if ( coll.gameObject.tag == "Char" )
        {            
            if ( transform.name.Equals ( "Arrow" ) )
            {
                StartCoroutine ( Transition () );
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
            if (transform.name.Equals("Backpack"))
            {
                canPlayBackpack = true;
            }
            else if (transform.name.Equals("Boat"))
            {
                canPlayBoat = true;
            }
            else if (transform.name.Equals("Bush"))
            {
                canPlayBush = true;
            }
            else if (transform.name.Equals("Campfire"))
            {
                canPlayCampfire = true;
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
