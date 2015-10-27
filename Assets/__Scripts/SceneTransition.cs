using UnityEngine;
using System.Collections;

// USAGE: Attach to invisible object with 2D Collider set to trigger

public class SceneTransition : MonoBehaviour
{
    private GameManager mGame;

    // Use this for initialization
    void Start ()
    {
        mGame = GameObject.Find ( "GameManager" ).GetComponent<GameManager> ();
    }

    // Update is called once per frame
    void Update ()
    {

    }

    void OnTriggerEnter2D ( Collider2D coll )
    {
        if ( coll.gameObject.tag == "Char" )
        {
            mGame.LoadNextLevel ();
        }
    }
}
