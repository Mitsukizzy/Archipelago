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
        mInput = GameObject.FindGameObjectWithTag("Manager").GetComponent<InputManager>();
        mAudio = GameObject.FindGameObjectWithTag("Manager").GetComponent<AudioManager>();
        tutorial = GameObject.Find("Tutorial").GetComponent<Tutorial>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        if ( canPlayBackpack )
        {
            canPlayBackpack = false;
        }
        if ( canPlayBoat && boatPlayOnce )
        {
            canPlayBoat = false;
            boatPlayOnce = false;
        }
        if( canPlayBush && bushPlayOnce )
        {
            canPlayBush = false;
            bushPlayOnce = false;
        }
        if( canPlayCampfire && campfirePlayOnce )
        {
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
        if ( coll.gameObject.tag == "Char" && mInput.InteractButtonPressed () )
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
