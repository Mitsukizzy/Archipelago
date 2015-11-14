using UnityEngine;
using System.Collections;

public class PlainsScript : MonoBehaviour {

	private GameManager mGame;
	// Use this for initialization
	void Start () {
		mGame = GameObject.FindGameObjectWithTag ( "Manager" ).GetComponent<GameManager> ();
		if ( mGame.CheckItem ( "JournalPagePlains" ) )
		{
			GameObject JournalPage = Instantiate ( Resources.Load ( "JournalPagePlains", typeof ( GameObject ) ) ) as GameObject;
			mGame.DoNotSpawnOnLoad ( "JournalPagePlains" );
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
