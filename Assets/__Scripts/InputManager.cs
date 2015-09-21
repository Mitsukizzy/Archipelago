﻿using UnityEngine;
using System.Collections;


public enum InputType {
	Keyboard,
	Mouse,
	Controller,
};

public class InputManager : MonoBehaviour {
	
	float clickTimer = 0.0f;
	float prevClickTimer = 0.0f;
	float delayBetweenMultClicks = 0.75f;
	int multClickCountLeft = 0;
	int multClickCountRight = 0;

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
                break;
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
                break;
            case InputType.Keyboard:
                break;
            case InputType.Controller:
                break;
            default:
                break;
        }
        return Vector3.zero;
    }

    public bool RunButtonPressed ()
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

	public bool normalAttackButtonPressed(){
		switch(controlScheme){
		case InputType.Mouse:
			if(Input.GetMouseButtonDown( 0 )){
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

	public bool smashAttackButtonPressed(){
		switch(controlScheme){
		case InputType.Mouse:
			if(Input.GetMouseButtonDown( 1 )){
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

	public bool specialAttackButtonPressed(){
		switch(controlScheme){
		case InputType.Mouse:
			if(Input.GetMouseButtonDown( 2 )){
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
