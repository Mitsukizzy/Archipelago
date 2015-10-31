using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Journal : MonoBehaviour 
{
    public Canvas mJournal;
    private Character mChar;

    public List<TextAsset> pages = new List<TextAsset>();
    public TextAsset page1;
    public TextAsset page2;
    public TextAsset page3;

    private Text mLeftPage;
    private Text mRightPage;
    private GameObject mPrevPageBtn;
    private GameObject mNextPageBtn;

    private int curPage = 0; // Indicates the index of the left page

	// Use this for initialization
	void Start () 
    {
        mLeftPage = GameObject.Find ( "Left Page" ).GetComponent<Text> ();
        mRightPage = GameObject.Find ( "Right Page" ).GetComponent<Text> ();

        mPrevPageBtn = GameObject.Find ( "Prev Page Button" );
        mNextPageBtn = GameObject.Find ( "Next Page Button" );

        mChar = GameObject.FindGameObjectWithTag("Char").GetComponent<Character>();

        pages.Add ( page1 );
        pages.Add ( page2 );
        pages.Add ( page3 );
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    public void ToggleJournal()
    {
        mJournal.enabled = !mJournal.enabled;

        if ( mJournal.enabled )
        {
            mChar.SetPlayerState ( Character.PlayerState.Interact );
            UpdatePages ();
        }
        else
        {
            mChar.SetPlayerState ( Character.PlayerState.Idle );
        }
    }

    public void NextPage ()
    {
        if( pages.Count > ( curPage + 2) )
        {
            curPage += 2;
            UpdatePages ();
        }
    }

    public void PreviousPage()
    {
        if ( 0 <= ( curPage - 2 ) )
        {
            curPage -= 2;
            UpdatePages ();
        }
    }

    public void UpdatePages()
    {
        if ( curPage <= 0 )
        {
            // No previous page
            mPrevPageBtn.SetActive ( false );
        }
        else
        {
            mPrevPageBtn.SetActive ( true );
        }
        
        if ( ( curPage + 2 ) >= pages.Count )
        {
            // No next page
            mNextPageBtn.SetActive ( false );
        }
        else
        {
            mNextPageBtn.SetActive ( true );
        }

        // Set the text of the left and right journal pages
        mLeftPage.text = pages[curPage].text;
        if ( ( curPage + 1 ) >= pages.Count ) 
        {
            // Left page has content, but right page does not
            mRightPage.text = "";
        }
        else
        {
            // Left and Right page have content
            mRightPage.text = pages[curPage + 1].text;
        }
    }

    public void AddJournalPage( int index )
    {
        // TODO: Use a dictionary of all acquireable journal pages to index found pages
        //pages.Insert ( index, newPage );
    }
}
