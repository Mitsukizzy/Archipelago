﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Combat : MonoBehaviour 
{
    private int damageToEnemy = 0;

    private GameManager m_Game;
    private GameObject m_Canvas;
    private Character m_Char;
    private Animator m_Animator;
    private InputManager m_Input;
    private AudioManager m_Audio;

    public GameObject arrow;

    bool animationDelay;

	// Use this for initialization
	void Start () {
        m_Canvas = GameObject.Find ( "Canvas" );
        m_Game = GameObject.Find ( "GameManager" ).GetComponent<GameManager>();
        m_Input = m_Game.GetInputManager();
        m_Audio = m_Game.GetAudioManager();
        m_Char = GameObject.Find ( "Character" ).GetComponent<Character>();
        m_Animator = GetComponent<Animator>();

        animationDelay = false;
	}
	
	// Update is called once per frame
	void Update () 
    {
        if ( m_Char.GetPlayerState () != Character.PlayerState.Gather &&
            m_Char.GetPlayerState () != Character.PlayerState.Interact &&
            m_Game.GetGameState () != GameManager.GameState.Tutorial )
        {
            if ( m_Input.SelectButtonPressed () && !animationDelay && !EventSystem.current.IsPointerOverGameObject () )
            {
                if ( m_Char.GetPlayerState () == Character.PlayerState.Aim )
                {
                    m_Audio.PlayOnce ( "shoot" );
                    ShootArrow ();
                }
            }
        }
	}

    void ShootArrow()
    {
        // Instantiate arrow, aiming code is in Arrow.cs
        GameObject arrowObj = ( GameObject )Instantiate ( arrow, transform.position, Quaternion.identity );
        m_Char.SetPlayerState ( Character.PlayerState.Idle );
    }

    public void finishedAttacking()
    {
        m_Animator.SetBool ( "isAtking", false );
    }

    void OnTriggerStay2D ( Collider2D coll )
    {
        if ( coll.gameObject.tag == "Enemy"  )
        {
            Enemy enemy = ( Enemy )coll.gameObject.GetComponent ( "Enemy" );

            if ( damageToEnemy != 0 )
            {
                // Play hit sound
                enemy.TakeDamage ( damageToEnemy );
                damageToEnemy = 0;
            }
        }
    }

    public void animationFinished()
    {
        animationDelay = false;
    }
}
