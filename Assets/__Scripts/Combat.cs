using UnityEngine;
using System.Collections;

public class Combat : MonoBehaviour 
{
    float multClickTimer = 0.0f;
    float delayBetweenMultClicks = 0.5f;
    int multClickCountLeft = 0;
    int multClickCountRight = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        // TODO: Replace with Input Manager hooks
        if ( Input.GetMouseButtonDown( 0 ) )
        {
            multClickTimer = Time.time;
            LeftClickCombo();
            Debug.Log( "Pressed left click." );
        }

        if ( Input.GetMouseButtonDown( 1 ) )
        {
            multClickTimer = Time.time;
            RightClickCombo();
            Debug.Log( "Pressed right click." );
        }

        if ( Input.GetMouseButtonDown( 2 ) )
        {
            Debug.Log( "Pressed middle click." );
        }
	}

    void LeftClickCombo ()
    {
        if( !isComboClick() )
        {
            multClickCountLeft = 0;
            multClickCountRight = 0;
        }

        switch ( multClickCountLeft )
        {
            case 0:
                // Perform basic move 1 left click
                break;
            case 1:
                // Perform basic move 2 left click
                break;
            case 2:
                // Perform basic move 3 left click
                break;
            case 3:
                // Perform basic move 4 left click
                break;
            default:
                break;
        }
        multClickCountLeft++;
    }

    void RightClickCombo ()
    {
        if ( !isComboClick() )
        {
            multClickCountLeft = 0;
            multClickCountRight = 0;
        }

        switch ( multClickCountLeft )
        {
            case 0:
                // Perform smash move 1 right click
                break;                  
            case 1:                     
                // Perform smash move 2 right click
                break;                  
            case 2:                     
                // Perform smash move 3 right click
                break;                  
            case 3:                     
                // Perform smash move 4 right click
                break;
            default:
                break;
        }
        multClickCountRight++;
    }

    bool isComboClick ()
    {
        return ( multClickTimer - Time.time ) <= delayBetweenMultClicks;
    }
}
