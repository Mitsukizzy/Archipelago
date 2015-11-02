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

    public GameObject knife;
    public GameObject journal;
    public GameObject objective;
    public GameObject arrow;
    public GameObject dialogueBox;
    public GameObject moveRight;

    private Intro intro;

    private DialogueSystem mDialogue;
    private GameObject backpack;
    private GameObject backpackUI;
    private GameObject journalUI;

    private Character mChar;
    private GameManager mGame;

    private GameObject currentTip;

    void Awake()
    {
        //SetHint("wasd");
    }

	// Use this for initialization
	void Start () 
    {
        //mChar = GameObject.FindGameObjectWithTag("Char").GetComponent<Character>();
        //mGame = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>();
        //mDialogue = mGame.GetDialogueSystem ();
        //backpack = GameObject.Find ( "Backpack" );
        //backpackUI = GameObject.Find( "Bag" );
        //journalUI = GameObject.Find ( "Journal" );
        //if (Application.loadedLevelName.Equals("1_Beach"))
        //{
        //    intro = GameObject.Find("Intro").GetComponent<Intro>();
        //}

        //Debug.Log ( "reached the close event" );
        //if ( Application.loadedLevelName == "1_Beach" )
        //{
        //    SetHint ( "wasd" );
        //}
        //else if ( Application.loadedLevelName == "2_Wetlands" )
        //{
        //    currentTip.SetActive ( false );
        //    objective.GetComponent<Text> ().text = "Objective: Go inland and search for food";
        //    objective.SetActive ( true );
        //}
	}

    // Update is called once per frame
    void Update ()
    {
        //journalUI.GetComponent<Image>().enabled = true;
        //journal.SetActive ( true );
        //knife.SetActive ( false );
        //Destroy ( backpack );
        //backpackUI.GetComponent<Image> ().enabled = true;
        //objective.GetComponent<Text> ().text = "Objective: Explore the Beach";
        //objective.SetActive ( true );
        //objective.GetComponent<Text> ().text = "Objective: Go inland and search for food";
        //objective.GetComponent<Text> ().text = "Objective: Rest at the Campsite";
        //objective.SetActive ( true );
        //arrow.SetActive ( true );
        //SetHint ( "moveRight" );
        //objective.SetActive(true);
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
