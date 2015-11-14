using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DialogueSystem : MonoBehaviour
{
    public TextAsset beach1; // Intro
    public TextAsset beach2; // Bag + Journal
    public TextAsset beach3; // Boat
    public TextAsset wetlands1; // Enter wetlands, shoot birds
    public TextAsset wetlands2; // Berries
    public TextAsset wetlands3; // Camp
    public TextAsset afterBagBoat; // On beach after interacting with bag and boat
    public TextAsset afterBow;  // After interacting with bow
    public TextAsset foundWood; // wood for the boat
    public TextAsset foundRope; // rope for the boat
    public TextAsset foundHammer; //hammer for boat
    public TextAsset boatNotDone; //interact with the boat when you dont have all the objects
    public TextAsset boatDone; //interact with the boat when you have all the objects
	public TextAsset afterPlainsJP;

    public Dictionary<string, string> mDialogues;
    private string[] curDialogue;
    private int curIndex = 0;
    private string curKey;  // Keep track of the current dialogue to show special hints at the end of it
    
    private GameObject objective;
    private GameObject objBackground;
    private GameObject rightArrow;

    private GameObject mDialogueBox;
    private Text mDialogueText;
    private Character mChar;
    private GameManager mGame;

    // Needs to be init before Start
    void Start()
    {
        // Add all the TextAssets to the dicionary, use their .text to convert to string
        mDialogues = new Dictionary<string, string> ();
        mDialogues.Add ( "beach1", beach1.text );
        mDialogues.Add ( "beach2", beach2.text );
        mDialogues.Add ( "beach3", beach3.text );
        mDialogues.Add ( "wetlands1", wetlands1.text );
        mDialogues.Add ( "wetlands2", wetlands2.text );
        mDialogues.Add ( "wetlands3", wetlands3.text );
        mDialogues.Add ( "afterBagBoat", afterBagBoat.text );
        mDialogues.Add ( "afterBow", afterBow.text );
        mDialogues.Add ( "wood", foundWood.text );
        mDialogues.Add ( "rope", foundRope.text );
        mDialogues.Add ( "hammer", foundHammer.text );
        mDialogues.Add ( "NotYet", boatNotDone.text );
        mDialogues.Add ( "YouWin", boatDone.text );
		mDialogues.Add ( "afterPlains", afterPlainsJP.text);

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
        if ( Application.loadedLevelName == "1_Beach" && !mGame.CheckHasVisitedBeach() )
        {
            StartDialogue ( "beach1" );
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
        
        // able to find inactive objects
        mDialogueBox = GameObject.FindGameObjectWithTag( "Dialogue" ).transform.Find ( "DialogueBox" ).gameObject;
        mDialogueText = mDialogueBox.transform.Find ( "DialogueText" ).GetComponent<Text> ();

        objective = GameObject.FindGameObjectWithTag ( "Objective" ).transform.Find ( "ObjText" ).gameObject;
        objBackground = GameObject.FindGameObjectWithTag ( "Objective" ).transform.Find ( "ObjBackground" ).gameObject; 
        rightArrow = GameObject.FindGameObjectWithTag ( "Tutorial" ).transform.Find ( "Right" ).gameObject; 
    }

    public void StartDialogue( string key )
    {
        curKey = key;
        mChar.SetPlayerState ( Character.PlayerState.Dialogue );
        mDialogueBox.SetActive ( true );

        // Get the full text, save in a large string
        string fullText;
        mDialogues.TryGetValue ( key, out fullText );

        // Separate fullText into individual strings and load them into curDialogue
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
            ShowHint ();
        }
    }

    // Handle special hints or tutorials
    private void ShowHint()
    {
        //objBackground.SetActive ( true );
        switch( curKey )
        {
            case "beach1":  // spawned on island
                objective.GetComponent<Text> ().text = "Tip: Press H to view help";
                break;
            case "beach2":  // backpack
                objective.GetComponent<Text> ().text = "Objective: Explore the Beach";
                break;
            case "beach3":  // boat
                if ( !mGame.CheckItem( "Backpack" ) ) // If backpack has been interacted with
                {
                    StartDialogue( "afterBagBoat" );
                }
                break;
            case "afterBagBoat":  // after bag and boat             
                objective.GetComponent<Text> ().text = "Objective: Go inland and search for food";
                rightArrow.SetActive ( true );
                StartCoroutine ( HideHint () );
                break;
            case "afterBow":  // shoot birds
                objective.GetComponent<Text> ().text = "Objective: Shoot a bird!\n\n(Q to AIM and LEFT CLICK to SHOOT)";
                break;
        }
        //StartCoroutine ( HideHint () );
    }

    IEnumerator HideHint( int seconds = 5 )
    {
        yield return new WaitForSeconds ( seconds );
        //objective.GetComponent<Text> ().text = "";
        //objBackground.SetActive ( false );
        rightArrow.SetActive ( false );
    }
}
