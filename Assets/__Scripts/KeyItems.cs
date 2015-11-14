using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// Backpack and Boat
public class KeyItems : MonoBehaviour 
{
    private InputManager mInput;
    private AudioManager mAudio;
    private DialogueSystem mDialogue;
    private GameManager mGame;
    private Journal mJournal;
    private Inventory mInvent;

    private GameObject buttonsUI;
    private GameObject journalUI;

    private bool canInteract = true;

	// Use this for initialization
	void Start ()
    {
        mInput = GameObject.FindGameObjectWithTag ( "Manager" ).GetComponent<InputManager> ();
        mAudio = GameObject.FindGameObjectWithTag ( "Manager" ).GetComponent<AudioManager> ();
        mDialogue = GameObject.FindGameObjectWithTag ( "Manager" ).GetComponent<DialogueSystem> ();
        mJournal = GameObject.FindGameObjectWithTag ( "Journal" ).GetComponent<Journal> ();
        mGame = GameObject.FindGameObjectWithTag ( "Manager" ).GetComponent<GameManager> ();

        buttonsUI = GameObject.Find ( "Buttons UI" );
        journalUI = GameObject.Find ( "Journal UI" );
	}
	
	// Update is called once per frame
	void Update () 
    {

	}

    void OnTriggerStay2D ( Collider2D coll )
    {
        if ( coll.gameObject.tag == "Char" && mInput.InteractButtonPressed () && canInteract )
        {
            canInteract = false; // prevent double interactions from happening on accident

            // Main Key Items
            if ( transform.tag.Equals ( "Backpack" ) )
            {
                mDialogue.StartDialogue ( "beach2" );
                buttonsUI.transform.Find ( "Bag" ).GetComponent<Image> ().enabled = true;
                buttonsUI.transform.Find("Journal").GetComponent<Image>().enabled = true;
                Destroy ( gameObject );                
                mGame.DoNotSpawnOnLoad ( "Backpack" );

                if ( !mGame.CheckItem( "Boat" ) ) // If boat has been interacted with
                {
                    mDialogue.StartDialogue( "AfterBagBoat" );
                }
            }
            else if ( transform.tag.Equals ( "Boat" ) )
            {
                if ( !mGame.CheckItem ( "wood" ) && !mGame.CheckItem ( "rope" ) && !mGame.CheckItem ( "hammer" ) )
                {
                    mDialogue.StartDialogue ( "YouWin" );
                }
                else if ( !mGame.CheckHasVisitedBeach() )
                {
                    mDialogue.StartDialogue ( "beach3" );
                    mGame.DoNotSpawnOnLoad( "Boat" );
                }
                else
                {
                    mDialogue.StartDialogue ( "NotYet" );
                }
            }
            else if ( transform.tag.Equals ( "Bush" ) )
            {
                mAudio.PlayOnce ( "rustle" );
            }
            else if ( transform.tag.Equals ( "Campsite" ) )
            {
                //mAudio.PlayOnce ( "campsite" );
            }

            // Journal Pages
            if ( transform.tag.Equals ( "JPBeach" ) || transform.tag.Equals ( "JPSeaCave" ) || transform.tag.Equals ( "JPDocks" ) || transform.tag.Equals("JPPlains") )
            {
                Debug.Log ( "tag " + transform.tag );
                GameObject.Find ( "NewPage" ).GetComponent<Animator> ().SetTrigger ( "becameActive" );
                mJournal.AddJournalPage ( transform.tag );
                Destroy ( gameObject ); //picked up, not needed anymore
            }

            if (gameObject.name.Contains("wood"))
            {
                //Debug.Log("interacted with wood");
                mDialogue.StartDialogue("wood");
                mGame.DoNotSpawnOnLoad("wood");
                gameObject.GetComponent<Interactable>().ReceiveItem();
                Destroy(gameObject);
            }

            if (gameObject.name.Contains("rope"))
            {
                //Debug.Log("interacted with wood");
                mDialogue.StartDialogue("rope");
                mGame.DoNotSpawnOnLoad("rope");
                gameObject.GetComponent<Interactable>().ReceiveItem();
                Destroy(gameObject);
            }
            if (gameObject.name.Contains("hammer"))
            {
                //Debug.Log("interacted with wood");
                mDialogue.StartDialogue("hammer");
                mGame.DoNotSpawnOnLoad("hammer");
                gameObject.GetComponent<Interactable>().ReceiveItem();
                Destroy(gameObject);
            }
        }
    }

    void OnTriggerExit2D ( Collider2D coll )
    {
        if ( coll.gameObject.tag == "Char" )
        {
            canInteract = true;
        }
    }
}