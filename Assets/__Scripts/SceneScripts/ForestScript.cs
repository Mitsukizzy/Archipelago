﻿using UnityEngine;
using System.Collections;

public class ForestScript : MonoBehaviour {

    //Scene scripts ask GameManager what the state of the scene should be.
    //Checks GameManager's KeyItems and Events dictionary to know what items or events need to be spawned into the scene

    private GameManager mGame;

    // Use this for initialization
    void Start()
    {
        //The Forest has wood as a key item for fixing the boat
        mGame = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>();

        if (mGame.CheckItem("wood"))
        {
            GameObject Backpack = Instantiate(Resources.Load("wood", typeof(GameObject))) as GameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
