using UnityEngine;
using System.Collections;

public class Paralax : MonoBehaviour {

	Character mChar;
	InputManager m_Input;
	CameraFollow m_Cam;

	// Use this for initialization
	void Start () {
		mChar = GameObject.FindGameObjectWithTag("Char").GetComponent<Character>();
		m_Input = GameObject.FindGameObjectWithTag("Manager").GetComponent<InputManager>();
		m_Cam = Camera.main.GetComponent<CameraFollow>();
	}
	
	// Update is called once per frame
	void Update () {
		if ( mChar.GetPlayerState() != Character.PlayerState.Gather && mChar.GetPlayerState() != Character.PlayerState.Interact && mChar.GetPlayerState() != Character.PlayerState.Aim && mChar.GetPlayerState() != Character.PlayerState.Dialogue && !m_Cam.isBounded)
		{   
			// Move character
			if ( m_Input.GetHorizontalMovement () != Vector3.zero || m_Input.GetVerticalMovement () != Vector3.zero )
			{
				transform.Translate ( m_Input.GetHorizontalMovement () * -mChar.speed/12 );

			}
		}
	}
}
