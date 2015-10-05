﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour 
{
    public GameObject wasd;
    public GameObject shift;
    public GameObject space;
    public GameObject interact;
    public GameObject leftClick;
    public GameObject rightClick;
    public GameObject staminaHelp;

    public GameObject knife;
    public GameObject journal;
    public GameObject dialogueBox;
    private Text dialogueText;
    private GameObject backpack;

    private Character mChar;
    private GameManager mGame;

    private GameObject currentTip;
    private bool canAdvanceDialogue;
    private int curDialogue = 0;
    private int endDialogue = 0;
    List<string> DialogueList = new List<string> ();

	// Use this for initialization
	void Start () 
    {
        dialogueText = dialogueBox.transform.FindChild ( "DialogueText" ).GetComponent<Text> ();
        mChar = GameObject.Find ( "Character" ).GetComponent<Character>();
        mGame = GameObject.Find ( "GameManager" ).GetComponent<GameManager> ();
        backpack = GameObject.Find ( "Backpack" );

        CloseDialogue ();
        SetHint ( "wasd" );

        knife.SetActive ( false );
        journal.SetActive ( false );

        // Dialogue
        //DialogueList.Add ( "-Player is lying on the shoreline, face up-" );
        //DialogueList.Add ( "-Eyes open, slowly gets up-" );
        // Upon finding backpack
        DialogueList.Add ( "A hunting knife. This should come in handy. -puts knife away-" );
        DialogueList.Add ( "This looks like someone's journal. I wonder what it says." );
        DialogueList.Add ( "-Picks up backpack- \n Hmm..Now to figure out, where am I?" );
        // Upon finding boat
        DialogueList.Add ( "This boat isn't in any shape to leave land." );
        DialogueList.Add ( "What's this? Looks like a page torn out of a notebook. -sticks page in journal-" );
        // After interacting with boat and journal page
        DialogueList.Add ( "Shoot, it's getting dark and I don't know how long it's been since I've eaten anything." );
        DialogueList.Add ( "This beach doesn't seem to have much. I better go inland and see if I can find something edible." );

        dialogueText.text = DialogueList[curDialogue];
        curDialogue++;
	}

    // Update is called once per frame
    void Update ()
    {
        if ( dialogueBox.activeSelf )
        {
            if ( curDialogue == 0 )
            {
                knife.SetActive ( true );
            }
            else if ( curDialogue == 1 )
            {
                journal.SetActive ( true );
                knife.SetActive ( false );
            }
            else
            {
                knife.SetActive ( false );
                journal.SetActive ( false );
            }

            if ( curDialogue == 2 )
            {
                Destroy ( backpack );
            }
        }
    }

    public void AdvanceDialogue ( )
    {
        if ( curDialogue == endDialogue )
        {
            canAdvanceDialogue = false;
            CloseDialogue ();
        }
        if ( canAdvanceDialogue )
        {
            curDialogue++;
            dialogueText.text = DialogueList[curDialogue];            
        }        
    }

    public void ShowDialogue ( int start, int end )
    {
        if ( start > end )
        {
            Debug.Log ( "Start index can't be greater than end index." );
            return;
        }
        mChar.SetPlayerState ( Character.PlayerState.Interact );
        curDialogue = start;
        endDialogue = end;
        dialogueText.text = DialogueList[curDialogue];
        dialogueBox.SetActive ( true );
        canAdvanceDialogue = true;
    }

    public void CloseDialogue ()
    {
        dialogueBox.SetActive ( false );
        mChar.SetPlayerState ( Character.PlayerState.Idle );
    }

    public void SetHint ( string hint )
    {
        if ( currentTip != null )
        {
            currentTip.SetActive ( false ); // close any already open hints
        }
        switch ( hint )
        {
            case "wasd":
                currentTip = wasd;
                StartCoroutine ( TimedHintShow () );
                break;
            case "interact":
                currentTip = interact;
                currentTip.SetActive ( true ); // closes on exit trigger
                break;
            case "shift":
                currentTip = shift;
                StartCoroutine ( TimedHintShow () );
                break;
            case "space":
                currentTip = wasd;
                StartCoroutine ( TimedHintShow () );
                break;
            default:
                break;
        }
    }

    public void CloseHint( )
    {
        currentTip.SetActive ( false );
    }

    IEnumerator TimedHintShow ( int seconds = 5 )
    {
        currentTip.SetActive ( true );
        yield return new WaitForSeconds ( seconds );
        currentTip.SetActive ( false );
    }
}
