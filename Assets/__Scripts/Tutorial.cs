using UnityEngine;
using System.Collections;

public class Tutorial : MonoBehaviour 
{
    public GameObject wasd;
    public GameObject shift;
    public GameObject space;
    public GameObject interact;
    public GameObject leftClick;
    public GameObject rightClick;
    public GameObject staminaHelp;

    private GameObject currentTip;

	// Use this for initialization
	void Start () 
    {
        currentTip = wasd;
        currentTip.SetActive ( true );
        NextTutorial ();
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    void NextTutorial ()
    {
        StartCoroutine ( WaitToShow () );
    }

    IEnumerator WaitToShow ()
    {
        yield return new WaitForSeconds ( 5 );
        currentTip.SetActive ( false );
        currentTip = shift;
        currentTip.SetActive ( true );
    }
}
