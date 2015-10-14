using UnityEngine;
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
    public GameObject objective;
    public GameObject arrow;
    public GameObject dialogueBox;
    public GameObject moveRight;

    private Intro intro;

    private Text dialogueText;
    private GameObject backpack;
    private GameObject backpackUI;
    private GameObject journalUI;

    private Character mChar;
    private GameManager mGame;

    private GameObject currentTip;
    private bool canAdvanceDialogue;
    private int curDialogue = 0;
    private int endDialogue = 0;
    List<string> DialogueList = new List<string> ();

    void Awake()
    {
        SetHint("wasd");
    }

	// Use this for initialization
	void Start () 
    {
        dialogueText = dialogueBox.transform.FindChild ( "DialogueText" ).GetComponent<Text> ();
        mChar = GameObject.Find ( "Character" ).GetComponent<Character>();
        mGame = GameObject.Find ( "GameManager" ).GetComponent<GameManager> ();
        backpack = GameObject.Find ( "Backpack" );
        backpackUI = GameObject.Find( "Bag" );
        journalUI = GameObject.Find ( "Journal" );
        if (Application.loadedLevelName.Equals("Beach"))
        {
            intro = GameObject.Find("Intro").GetComponent<Intro>();
        }

        CloseDialogue ();
        //SetHint ( "wasd" );

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

        //This is for the wetlands map
        //upon gathering a berry for the first time
        DialogueList.Add("These berries look safe to eat, I'll put one in my bag.");
        DialogueList.Add("I'm getting kind of tired I wonder if there's anywhere I can rest.");
        //upon finding the campfire
        DialogueList.Add("Well this campsite is convienient. I think I'll take a rest");
        DialogueList.Add("Looks like there's another journal page here too. -adds to journal-");

        //Intro Dialog index starting at index 11
        DialogueList.Add("...");
        DialogueList.Add("......");
        DialogueList.Add("...Where am I?");

        dialogueText.text = DialogueList[curDialogue];
        curDialogue++;
	}

    void OnLevelWasLoaded()
    {
        mChar = GameObject.Find("Character").GetComponent<Character>();
        Debug.Log("reached the close event");
        CloseDialogue();
        if (Application.loadedLevelName == "Beach")
        {
            SetHint("wasd");
        }
        else if (Application.loadedLevelName == "Wetlands")
        {
            currentTip.SetActive(false);
            objective.GetComponent<Text>().text = "Objective: Go inland and search for food";
            objective.SetActive(true);
        }
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
                journalUI.GetComponent<Image>().enabled = true;
                journal.SetActive ( true );
                knife.SetActive ( false );
            }
            else
            {
                if (knife != null)
                {
                    knife.SetActive(false);
                    journal.SetActive(false);
                }
            }

            if ( curDialogue == 2 )
            {
                Destroy ( backpack );
                backpackUI.GetComponent<Image>().enabled = true;
                objective.GetComponent<Text> ().text = "Objective: Explore the Beach";
                objective.SetActive ( true );
            }
            if ( curDialogue == 3 )
            {
                objective.SetActive ( false );
            }
            if ( curDialogue == 6)
            {
                objective.GetComponent<Text> ().text = "Objective: Go inland and search for food";
                objective.SetActive ( true );
                arrow.SetActive ( true );
                SetHint("moveRight");
            }
            if(curDialogue == 8)
            {
                objective.GetComponent<Text>().text = "Objective: Rest at the Campsite";
                objective.SetActive(true);
            }
            if (curDialogue == 10)
            {
                objective.SetActive(false);
            }

            if(curDialogue == 13)
            {
                intro.isFadingIn = true;
            }

        }
    }

    public void AdvanceDialogue ( )
    {
        if ( curDialogue == endDialogue )
        {
            canAdvanceDialogue = false;
            CloseDialogue ();
            if ( curDialogue == 2 )
            {
                SetHint ( "shift" );
            }
            if(curDialogue == 10)
            {
                mChar.SetPlayerState(Character.PlayerState.Interact);
            }
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
        if(mChar.GetPlayerState() != Character.PlayerState.Gather)
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
                StartCoroutine ( TimedHintShow ( 10 ) );
                StartCoroutine ( CustomHintShow ( staminaHelp, 10 ) );
                break;
            case "space":
                currentTip = wasd;
                StartCoroutine ( TimedHintShow () );
                break;
            case "moveRight":
                StartCoroutine(CustomHintShow(moveRight, 10));
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

    IEnumerator CustomHintShow ( GameObject tip, int seconds = 5 )
    {
        tip.SetActive ( true );
        yield return new WaitForSeconds ( seconds );
        tip.SetActive ( false );
    }
}
