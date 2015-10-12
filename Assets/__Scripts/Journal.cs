using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Journal : MonoBehaviour 
{
    public Canvas mJournal;

    public List<TextAsset> pages = new List<TextAsset>();
    public TextAsset page1;
    public TextAsset page2;
    public TextAsset page3;

    private Text mLeftPage;
    private Text mRightPage;

    private int curPage = 0; // Indicates the index of the left page

	// Use this for initialization
	void Start () 
    {
        mLeftPage = GameObject.Find ( "Left Page" ).GetComponent<Text> ();
        mRightPage = GameObject.Find ( "Right Page" ).GetComponent<Text> ();

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
            UpdatePages ();
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
        mLeftPage.text = pages[curPage].text;
        if( ( curPage + 1 ) >= pages.Count )
        {
            mRightPage.text = "";
        }
        else
        {
            mRightPage.text = pages[curPage + 1].text;
        }
    }
}
