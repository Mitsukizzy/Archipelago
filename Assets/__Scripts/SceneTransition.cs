using UnityEngine;
using System.Collections;

// USAGE: Attach to invisible object with 2D Collider set to trigger

public class SceneTransition : MonoBehaviour
{
    private GameManager mGame;
    private AudioManager mAudio;
    public string SceneName;

    // Use this for initialization
    void Start ()
    {
        mGame = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>();
        mAudio = mGame.GetAudioManager ();
    }

    // Update is called once per frame
    void Update ()
    {

    }

    void OnTriggerEnter2D ( Collider2D coll )
    {
        if ( coll.gameObject.tag == "Char" )
        {
            if( Application.loadedLevel == 1 ) // special condition for beach
            {
                Debug.Log(mGame.CheckItem( "Backpack" ) + " " + mGame.CheckItem( "Boat" ) );
                if( !mGame.CheckItem( "Backpack" ) && !mGame.CheckItem( "Boat" ) ) // Can only leave if they've been interacted with
                {
                    mAudio.PlayOnce( "transition" );
                    Application.LoadLevel( SceneName );
                }
            }
            else
            {
                mAudio.PlayOnce( "transition" );
                Application.LoadLevel( SceneName );
            }
        }
    }
}