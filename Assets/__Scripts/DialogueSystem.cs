using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DialogueSystem : MonoBehaviour
{
    public TextAsset beach1; // Intro
    public TextAsset beach2; // Bag + Journal
    public TextAsset beach3; // Boat
    public TextAsset foundWood; // wood for the boat
    public TextAsset foundRope; // rope for the boat
    public TextAsset foundHammer; //hammer for boat
    public TextAsset boatNotDone; //interact with the boat when you dont have all the objects
    public TextAsset boatDone; //interact with the boat when you have all the objects

    public Dictionary<string, string> mDialogues;
    private string[] curDialogue;
    private int curIndex = 0;

    private GameObject mDialogueBox;
    private Text mDialogueText;
    private Character mChar;
    private GameManager mGame;

    private bool visitedBeach = false;

    // Needs to be init before Start
    void Start()
    {
        // Add all the TextAssets to the dicionary, use their .text to convert to string
        mDialogues = new Dictionary<string, string> ();
        mDialogues.Add ( "beach1", beach1.text );
        mDialogues.Add ( "beach2", beach2.text );
        mDialogues.Add ( "beach3", beach3.text );
        mDialogues.Add ( "wood", foundWood.text );
        mDialogues.Add ( "rope", foundRope.text );
        mDialogues.Add("hammer", foundHammer.text);
        mDialogues.Add("NotYet", boatNotDone.text);
        mDialogues.Add("YouWin", boatDone.text);


        mGame = GetComponent<GameManager> ();

        if ( mChar == null && Application.loadedLevel != 0 )
        {
            SpecialInit (); // Initialize character and dialogue objects
        }
    }

    void OnLevelWasLoaded()
    {
        if ( mChar == null && Application.loadedLevel != 0 )
        {
            SpecialInit (); // Initialize character and dialogue objects
        }

        // Start dialogue if never been to beach, assume picked up backpack
        if ( Application.loadedLevelName == "1_Beach" && !visitedBeach )
        {
            StartDialogue ( "beach1" );
            visitedBeach = true;
        }
    }

    // Update is called once per frame
    void Update ()
    {

    }

    // Custom init method
    void SpecialInit()
    {
        mChar = GameObject.FindGameObjectWithTag("Char").GetComponent<Character>();

        mDialogueText = GameObject.Find ( "DialogueText" ).GetComponent<Text> ();
        mDialogueBox = GameObject.Find ( "DialogueBox" );
    }

    public void StartDialogue( string key )
    {
        mChar.SetPlayerState ( Character.PlayerState.Dialogue );
        mDialogueBox.SetActive ( true );

        // Get the full text, save in a large string
        string fullText;
        mDialogues.TryGetValue ( key, out fullText );

        // Separate fullText into individual strings and load them into curDialogue
        Debug.Log(fullText);
        curDialogue = fullText.Split ( '\n' ); // using newline as delimiter
        curIndex = 0;
        mDialogueText.text = curDialogue[curIndex];
    }

    public void Advance()
    {
        if( curIndex < curDialogue.Length - 1 )
        {
            curIndex++;
            mDialogueText.text = curDialogue[curIndex];
        }
        else
        {
            // Close dialogue
            mDialogueText.text = "";
            mChar.SetPlayerState ( Character.PlayerState.Idle );
            mDialogueBox.SetActive ( false );
        }
    }
}
