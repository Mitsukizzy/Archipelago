﻿using UnityEngine;
using System.Collections;

public class SpawnPersistant : MonoBehaviour
{
	public Texture2D mouseCursor;

    // Use this for initialization
    void Awake ()
    {
		if(mouseCursor != null){
			Cursor.SetCursor(mouseCursor, new Vector2(0,0), CursorMode.ForceSoftware);
		}
        GameObject manager = GameObject.FindGameObjectWithTag ( "Manager" );
        if ( manager == null )
        {
            GameObject GameManager = Instantiate ( Resources.Load ( "GameManager", typeof ( GameObject ) ) ) as GameObject;
            GameManager.GetComponent<AudioManager> ().SpecialInit ();
        }
        if ( Application.loadedLevelName != "0_MainMenu" )
        {
            GameObject ui = GameObject.FindGameObjectWithTag ( "UI" );
            if ( ui == null )
            {
                GameObject UI = Instantiate ( Resources.Load ( "UI", typeof ( GameObject ) ) ) as GameObject;
            }
            GameObject chara = GameObject.FindGameObjectWithTag ( "Char" );
            if ( chara == null )
            {
                GameObject Character = Instantiate ( Resources.Load ( "Character", typeof ( GameObject ) ) ) as GameObject;
            }
        }
    }

    // Update is called once per frame
    void Update ()
    {

    }
}