using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    /** MUSIC **/
    public AudioClip musicMain;
    public AudioClip musicBeach;
    public AudioClip musicWetlands;
    public AudioClip musicForest;
    public AudioClip musicDocks;
    public AudioClip musicSeaCave;
    public AudioClip musicPlains;
    public AudioClip musicTemp1;        // Coalescence
    public AudioClip musicTemp2;        // Monsoon

    /** SFX **/
    // Player
    public AudioClip sndWalk;
    public AudioClip sndRun;
    public AudioClip sndShoot;
    public AudioClip sndTransition;

    // UI
    public AudioClip sndMenuSelect;
    public AudioClip sndMenuHover;
    public AudioClip sndClick;

    // Other
    public AudioClip sndCampsite;
    public AudioClip sndGameOver;
    public AudioClip sndRefreshed;
    public AudioClip sndNewItem;
    public AudioClip sndFailed;
    public AudioClip sndRustle;
    public AudioClip sndPlayerDamaged;
    public AudioClip sndPlayerDeath;
    public AudioClip sndEnemyDamaged;
    public AudioClip sndEnemyDeath;

    private AudioSource m_AudioSource;

    private float sfxVolume = 1.0f;

	// Use this for initialization
	void Start ()
    {
        m_AudioSource.volume = 0.35f;
        m_AudioSource.loop = true;
	}

    public void SpecialInit()
    {
        m_AudioSource = GameObject.FindGameObjectWithTag ( "Manager" ).GetComponent<AudioSource> ();
    }

	// Update is called once per frame
	void Update () 
    {

	}

    public void Stop()
    {
        m_AudioSource.Stop ();
    }

    public void PlayLoop ( string soundToLoop )
    {
        Debug.Log ( "PLAY LOOP " + soundToLoop );
        switch( soundToLoop )
        {
            case "main":
                m_AudioSource.clip = musicMain;
                break;
            case "beach":
                m_AudioSource.clip = musicBeach;
                break;
            case "wetlands":
                m_AudioSource.clip = musicWetlands;
                break;
            case "forest":
                m_AudioSource.clip = musicForest;
                break;
            case "docks":
                m_AudioSource.clip = musicDocks;
                break;
            case "seacave":
                m_AudioSource.clip = musicSeaCave;
                break;
            case "plains":
                m_AudioSource.clip = musicPlains;
                break;
            case "temp1":
                m_AudioSource.clip = musicTemp1;
                break;
            case "temp2":
                m_AudioSource.clip = musicTemp2;
                break;
            default:
                break;
        }

        if ( !m_AudioSource.isPlaying )
        {
            m_AudioSource.Play ();
        }
    }

    public void PlayOnce ( string soundToPlay )
    {
        switch ( soundToPlay )
        {
            case "shoot":
                m_AudioSource.PlayOneShot ( sndShoot, sfxVolume );
                break;
            case "campsite":
                m_AudioSource.PlayOneShot ( sndCampsite, sfxVolume );
                break;
            case "gameOver":
                m_AudioSource.PlayOneShot ( sndGameOver, sfxVolume );
                break;
            case "refreshed":
                m_AudioSource.PlayOneShot ( sndRefreshed, sfxVolume );
                break;
            case "newItem":
                m_AudioSource.PlayOneShot ( sndNewItem, sfxVolume );
                break;
            case "failed":
                m_AudioSource.PlayOneShot ( sndFailed, sfxVolume );
                break;
            case "rustle":
                m_AudioSource.PlayOneShot ( sndRustle, sfxVolume );
                break;
            case "playerDamaged":
                m_AudioSource.PlayOneShot ( sndPlayerDamaged, sfxVolume );
                break;
            case "playerDeath":
                m_AudioSource.PlayOneShot ( sndPlayerDeath, sfxVolume );
                break;
            case "enemyDamaged":
                m_AudioSource.PlayOneShot ( sndEnemyDamaged, sfxVolume );
                break;
            case "enemyDeath":
                m_AudioSource.PlayOneShot ( sndEnemyDeath, sfxVolume );
                break;
            case "click":
                m_AudioSource.PlayOneShot ( sndClick, sfxVolume );
                break;
            case "menuSelect":
                m_AudioSource.PlayOneShot ( sndMenuSelect, sfxVolume );
                break;
            case "menuHover":
                m_AudioSource.PlayOneShot ( sndMenuHover, sfxVolume );
                break;
            case "transition":
                m_AudioSource.PlayOneShot ( sndTransition, sfxVolume );
                break;
            default:
                break;
        }
    }
}