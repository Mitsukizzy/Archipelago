using UnityEngine;
using System.Collections;


public enum InputType {
	Keyboard,
	Mouse,
	Controller,
};

public class InputManager : MonoBehaviour {

	public InputType controlScheme;

	// Use this for initialization
	void Start () 
    {
	}
	
	// Update is called once per frame
	void Update () 
    {
	}

    public Vector3 GetHorizontalMovement ()
    {
        switch ( controlScheme )
        {
            case InputType.Mouse:
                return Vector2.right * Input.GetAxis ( "Horizontal" ) * Time.deltaTime;
            case InputType.Keyboard:
                break;
            case InputType.Controller:
                break;
            default:
                break;
        }
        return Vector3.zero;
    }

    public Vector3 GetVerticalMovement ()
    {
        switch ( controlScheme )
        {
            case InputType.Mouse:
                return Vector2.up * Input.GetAxis ( "Vertical" ) * Time.deltaTime;
            case InputType.Keyboard:
                break;
            case InputType.Controller:
                break;
            default:
                break;
        }
        return Vector3.zero;
    }

    public bool RunButtonHeld ()
    {
        switch ( controlScheme )
        {
            case InputType.Mouse:
                if ( Input.GetKey ( KeyCode.LeftShift ) )
                {
                    return true;
                }
                break;
            case InputType.Keyboard:
                break;
            case InputType.Controller:
                break;
            default:
                break;
        }
        return false;
    }

	public bool DodgeButtonPressed ()
	{
		switch ( controlScheme )
		{
		case InputType.Mouse:
			if ( Input.GetKeyDown ( KeyCode.Space ) )
			{
				return true;
			}
			break;
		case InputType.Keyboard:
			break;
		case InputType.Controller:
			break;
		default:
			break;
		}
		return false;
	}
	
	public bool DodgeButtonReleased ()
	{
		switch ( controlScheme )
		{
		case InputType.Mouse:
			if ( Input.GetKeyUp ( KeyCode.Space ) )
			{
				return true;
			}
			break;
		case InputType.Keyboard:
                break;
            case InputType.Controller:
                break;
            default:
                break;
        }
        return false;
    }

    public bool AimButtonHeld()
    {
        switch ( controlScheme )
        {
            case InputType.Mouse:
                if ( Input.GetKey ( KeyCode.Q ) )
                {
                    return true;
                }
                break;
            case InputType.Keyboard:
                break;
            case InputType.Controller:
                break;
            default:
                break;
        }
        return false;
    }

    public bool ResetGameButtonPressed()
    {
        switch ( controlScheme )
        {
            case InputType.Mouse:
                if ( Input.GetKeyDown( KeyCode.Minus ) )
                {
                    return true;
                }
                break;
            case InputType.Keyboard:
                break;
            case InputType.Controller:
                break;
            default:
                break;
        }
        return false;
    }

    public bool ResetFromCampButtonPressed ()
    {
        switch ( controlScheme )
        {
            case InputType.Mouse:
                if ( Input.GetKeyDown ( KeyCode.Equals ) )
                {
                    return true;
                }
                break;
            case InputType.Keyboard:
                break;
            case InputType.Controller:
                break;
            default:
                break;
        }
        return false;
    }

    public bool NextLevelButtonPressed ()
    {
        switch ( controlScheme )
        {
            case InputType.Mouse:
                if ( Input.GetKeyDown ( KeyCode.L ) )
                {
                    return true;
                }
                break;
            case InputType.Keyboard:
                break;
            case InputType.Controller:
                break;
            default:
                break;
        }
        return false;
    }

    public bool SelectButtonPressed ()
    {
        switch ( controlScheme )
        {
            case InputType.Mouse:
                if ( Input.GetMouseButtonDown ( 0 ) ) // Left Click
                {
                    return true;
                }
                break;
            case InputType.Keyboard:
                break;
            case InputType.Controller:
                break;
            default:
                break;
        }
        return false;
    }

    public bool AltSelectButtonPressed ()
    {
        switch ( controlScheme )
        {
            case InputType.Mouse:
                if ( Input.GetMouseButtonDown ( 1 ) ) // Right Click
                {
                    return true;
                }
                break;
            case InputType.Keyboard:
                break;
            case InputType.Controller:
                break;
            default:
                break;
        }
        return false;
    }

    public bool SpecialSelectButtonPressed ()
    {
        switch ( controlScheme )
        {
            case InputType.Mouse:
                if ( Input.GetMouseButtonDown ( 2 ) ) // Middle Click
                {
                    return true;
                }
                break;
            case InputType.Keyboard:
                break;
            case InputType.Controller:
                break;
            default:
                break;
        }
        return false;
    }

    public bool InteractButtonPressed ()
    {
        switch ( controlScheme )
        {
            case InputType.Mouse:
                if ( Input.GetKeyDown ( KeyCode.E ) )
                {
                    return true;
                }
                break;
            case InputType.Keyboard:
                break;
            case InputType.Controller:
                break;
            default:
                break;
        }
        return false;
    }

    public bool HelpButtonPressed ()
    {
        switch ( controlScheme )
        {
            case InputType.Mouse:
                if ( Input.GetKeyDown( KeyCode.H ) )
                {
                    return true;
                }
                break;
            case InputType.Keyboard:
                break;
            case InputType.Controller:
                break;
            default:
                break;
        }
        return false;
    }

    public bool InventoryButtonPressed ()
    {
        switch ( controlScheme )
        {
            case InputType.Mouse:
                if ( Input.GetKeyDown ( KeyCode.I ) )
                {
                    return true;
                }
                break;
            case InputType.Keyboard:
                break;
            case InputType.Controller:
                break;
            default:
                break;
        }
        return false;
    }

    public bool JournalButtonPressed ()
    {
        switch ( controlScheme )
        {
            case InputType.Mouse:
                if ( Input.GetKeyDown ( KeyCode.J ) )
                {
                    return true;
                }
                break;
            case InputType.Keyboard:
                break;
            case InputType.Controller:
                break;
            default:
                break;
        }
        return false;
    }

    public bool PauseButtonPressed ()
    {
        switch ( controlScheme )
        {
            case InputType.Mouse:
                if ( Input.GetKeyDown ( KeyCode.Escape ) )
                {
                    return true;
                }
                break;
            case InputType.Keyboard:
                break;
            case InputType.Controller:
                break;
            default:
                break;
        }
        return false;
    }
}