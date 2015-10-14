using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenu : MonoBehaviour 
{
    private Canvas m_QuitMenu;
    private Button m_Start;
    private Button m_Exit;

	// Use this for initialization
	void Start () 
    {
        m_QuitMenu = GameObject.Find ( "QuitMenu" ).GetComponent<Canvas> ();
        m_Start = GameObject.Find ( "Play" ).GetComponent<Button> ();
        m_Exit = GameObject.Find ( "Exit" ).GetComponent<Button> ();
        m_QuitMenu.enabled = false;
	}

    public void PlayPress ()
    {
        Application.LoadLevel ( "Beach" );
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

    public void NoPress ()
    {
        m_QuitMenu.enabled = false;
        m_Start.enabled = true;
        m_Exit.enabled = true;
    }
}
