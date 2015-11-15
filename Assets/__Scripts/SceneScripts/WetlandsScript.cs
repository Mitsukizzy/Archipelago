using UnityEngine;
using System.Collections;

public class WetlandsScript : MonoBehaviour 
{
    private GameManager mGame;
	// Use this for initialization
	void Start () 
    {
        mGame = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>();
        if ( mGame.CheckItem("Bow") )
        {
            GameObject Bow = Instantiate(Resources.Load("Bow", typeof(GameObject))) as GameObject;
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
