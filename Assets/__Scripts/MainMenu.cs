using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenu : MonoBehaviour 
{
    private Canvas m_QuitMenu;
    private Button m_Start;
    private Button m_Exit;

    private GameObject m_Credits;

	// Use this for initialization
	void Start () 
    {
        m_QuitMenu = GameObject.Find ( "QuitMenu" ).GetComponent<Canvas> ();
        m_Start = GameObject.Find ( "Play" ).GetComponent<Button> ();
        m_Exit = GameObject.Find ( "Exit" ).GetComponent<Button> ();
        m_QuitMenu.enabled = false;

        m_Credits = GameObject.Find ( "UI" ).transform.Find ( "Credits UI" ).gameObject;
	}

    public void PlayPress ()
    {
        Application.LoadLevel ( "1_Beach" );
    }

    public void ExitPress ()
    {
        m_QuitMenu.enabled = true;
        m_Start.enabled = false;
        m_Exit.enabled = false;
    }

    public void ExitGame ()
    {
        Application.Quit ();
    }

    public void OpenCredits()
    {
        m_Credits.SetActive ( true );
    }

    public void CloseCredits ()
    {
        m_Credits.SetActive ( false );
    }

    public void NoPress ()
    {
        m_QuitMenu.enabled = false;
        m_Start.enabled = true;
        m_Exit.enabled = true;
    }
}
