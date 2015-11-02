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



    private GameObject buttonsUI;
    private GameObject journalUI;

	// Use this for initialization
	void Start () 
    {
        mInput = GameObject.FindGameObjectWithTag ( "Manager" ).GetComponent<InputManager> ();
        mAudio = GameObject.FindGameObjectWithTag ( "Manager" ).GetComponent<AudioManager> ();
        mDialogue = GameObject.FindGameObjectWithTag ( "Manager" ).GetComponent<DialogueSystem> ();
        mJournal = GameObject.FindGameObjectWithTag ( "Journal" ).GetComponent<Journal> ();
        mGame = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>();

        buttonsUI = GameObject.Find ( "Buttons UI" );
        journalUI = GameObject.Find ( "Journal UI" );
	}
	
	// Update is called once per frame
	void Update () 
    {

	}

    void OnTriggerStay2D ( Collider2D coll )
    {
        if ( coll.gameObject.tag == "Char" && mInput.InteractButtonPressed () )
        {
            // Main Key Items
            if ( transform.tag.Equals ( "Backpack" ) )
            {
                mDialogue.StartDialogue ( "beach2" );
                buttonsUI.transform.Find ( "Bag" ).GetComponent<Image> ().enabled = true;
                Destroy(gameObject);
                mGame.DoNotSpawnOnLoad("Backpack");
            }
            else if ( transform.tag.Equals ( "Boat" ) )
            {
                mDialogue.StartDialogue ( "beach3" );
                buttonsUI.transform.Find ( "Journal" ).GetComponent<Image> ().enabled = true;
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
            if ( transform.tag.Equals ( "JPBeach" ) || transform.tag.Equals ( "JPSeaCave" ) || transform.tag.Equals ( "JPDocks" ) )
            {
                mJournal.AddJournalPage ( transform.tag );
            }
            Debug.Log("pressed interact button");

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
}
