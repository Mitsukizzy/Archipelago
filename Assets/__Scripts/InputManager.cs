using UnityEngine;
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

	public GameObject currentCharacter; //The character that we are controlling at this time.
	public float speed = 5.0f;
	public float runSpeed = 10.0f;
	public InputType controlScheme;
	float originalSpeed;

	// Use this for initialization
	void Start () {
		originalSpeed = speed;
	}
	
	// Update is called once per frame
	void Update () {
		if(controlScheme == InputType.Mouse){

			//move character
			currentCharacter.transform.Translate(Vector2.right * Input.GetAxis("Horizontal") * Time.deltaTime * speed);
			currentCharacter.transform.Translate(Vector2.up * Input.GetAxis("Vertical") * Time.deltaTime * speed);


			if( Input.GetKey( KeyCode.Q ) )
			{
				Debug.Log("Run");
				speed = runSpeed;
			}

			if( Input.GetKeyUp( KeyCode.Q ) )
			{
				Debug.Log("Stopped Running");
				speed = originalSpeed;
			}

			if( Input.GetKey( KeyCode.Space ) )
			{
				Debug.Log("Dodge");
			}
		}
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
