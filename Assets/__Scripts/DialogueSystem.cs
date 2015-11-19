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
    public TextAsset watch;
    public TextAsset glasses;
    public TextAsset map;
    public TextAsset photo;

    public Dictionary<string, string> mDialogues;
    private string[] curDialogue;
    private int curIndex = 0;
    private string curKey;  // Keep track of the current dialogue to show special hints at the end of it
    
    private GameObject objective;
    private Color objTextColor;
    private GameObject objBackground;
    private GameObject rightArrow;

    private GameObject mDialogueBox;
    private Text mDialogueText;
    private Character mChar;
    private GameManager mGame;
    private Journal mJournal;

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
        mDialogues.Add("watch", watch.text);
        mDialogues.Add("glasses", glasses.text);
        mDialogues.Add("map", map.text);
        mDialogues.Add("photo", photo.text);

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

        if( Application.loadedLevel == 5 ) // sea cave is dark, hard to read obj text
        {
            objective.GetComponent<Text> ().color = Color.white;
        }
        else if( Application.loadedLevel != 0 && Application.loadedLevel != 7 )
        {
            objective.GetComponent<Text> ().color = objTextColor;
        }

        // Start dialogue if never been to beach
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
        mJournal = GameObject.FindGameObjectWithTag("Journal").GetComponent<Journal>();
        
        // able to find inactive objects
        mDialogueBox = GameObject.FindGameObjectWithTag( "Dialogue" ).transform.Find ( "DialogueBox" ).gameObject;
        mDialogueText = mDialogueBox.transform.Find ( "DialogueText" ).GetComponent<Text> ();

        objective = GameObject.FindGameObjectWithTag ( "Objective" ).transform.Find ( "ObjText" ).gameObject;
        objBackground = GameObject.FindGameObjectWithTag ( "Objective" ).transform.Find ( "ObjBackground" ).gameObject; 
        rightArrow = GameObject.FindGameObjectWithTag ( "Tutorial" ).transform.Find ( "Right" ).gameObject;
        objTextColor = objective.GetComponent<Text> ().color;
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
        int hintDuration = 8;
        switch( curKey )
        {
            case "beach1":  // spawned on island
                objective.GetComponent<Text> ().text = "Tip: Press <i>H</i> to view help\nTap <i>E</i> to interact with objects";
                break;
            case "beach2":  // backpack
                objective.GetComponent<Text> ().text = "Objective: Explore the Beach";
                break;
            case "beach3":  // boat
                mGame.SetHasVisitedBeach ( true );
                if ( !mGame.CheckItem( "Backpack" ) ) // If backpack has been interacted with
                {
                    StartDialogue( "afterBagBoat" );
                }
                break;
            case "afterBagBoat":  // after bag and boat             
                objective.GetComponent<Text> ().text = "Objective: Go inland and explore";
                rightArrow.SetActive ( true );
                StartCoroutine ( HideHint ( hintDuration ) );
                break;
            case "afterBow":  // shoot birds
                objective.GetComponent<Text> ().text = "Objective: Shoot a bird!\n<i>Q</i> to AIM and <i>LEFT CLICK</i> to SHOOT";
                break;
            case "wetlands2":
                objective.GetComponent<Text>().text = "Objective: Rest at the campsite";
                break;
            case "wetlands3":
				mJournal.AddJournalPage("JPWetlands");
                objective.GetComponent<Text>().text = "Objective: Find a way off the island";
                break;
            case "YouWin":
                mGame.SetHasJustWon ( true );
                GameObject.Find("WinCanvas").GetComponent<WinFade>().StartOverlay();
                StartCoroutine(WaitForFinish(5));
                break;
        }
		GameObject[] birds = GameObject.FindGameObjectsWithTag("Bird");
		foreach (GameObject b in birds)
		{
			b.GetComponent<BirdAI>().exitedSafeArea();
		}
    }

    IEnumerator HideHint( int seconds )
    {
        yield return new WaitForSeconds ( seconds );
        //objective.GetComponent<Text> ().text = "";
        rightArrow.SetActive(false);
    }

    IEnumerator WaitForFinish(int seconds)
    {
        yield return new WaitForSeconds(seconds);
        mGame.MainMenu();
    }
}
