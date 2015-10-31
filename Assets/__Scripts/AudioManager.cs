using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour 
{
    // Player
    public AudioClip sndWalk;
    public AudioClip sndRun;
    public AudioClip sndShoot;
    public AudioClip sndTransition;

    // UI
    public AudioClip sndMenuSelect;
    public AudioClip sndMenuHover;
    public AudioClip sndClick;

    // Background Music
    public AudioClip musicMain;
    public AudioClip musicBeach;
    public AudioClip musicWetlands;
    public AudioClip musicForest;
    public AudioClip musicDocks;
    public AudioClip musicSeaCave;
    public AudioClip musicPlains;
    public AudioClip musicTemp1;        // Coalescence
    public AudioClip musicTemp2;        // Monsoon

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

    private AudioSource m_MusicSource;
    private AudioSource m_SFXSource;

	// Use this for initialization
	void Start ()
    {
       
	}

	// Update is called once per frame
	void Update () 
    {

	}

    // Called from SpawnPersistant due to dependency issues
    public void SpecialInit()
    {
        m_MusicSource = GameObject.FindGameObjectWithTag ( "Manager" ).GetComponents<AudioSource> ()[0];
        m_MusicSource.volume = 0.35f;
        m_MusicSource.loop = true;

        m_SFXSource = GameObject.FindGameObjectWithTag ( "Manager" ).GetComponents<AudioSource> ()[0];
        m_SFXSource.volume = 0.75f;
        m_SFXSource.loop = false;
    }

    public void StopSFX ()
    {
        m_SFXSource.Stop ();
    }

    public void StopMusic ()
    {
        m_MusicSource.Stop ();
    }

    public void PlayLoop ( string soundToLoop )
    {
        switch( soundToLoop )
        {
            case "main":
                m_MusicSource.clip = musicMain;
                break;
            case "beach":
                m_MusicSource.clip = musicBeach;
                break;
            case "wetlands":
                m_MusicSource.clip = musicWetlands;
                break;
            case "forest":
                m_MusicSource.clip = musicForest;
                break;
            case "docks":
                m_MusicSource.clip = musicDocks;
                break;
            case "seacave":
                m_MusicSource.clip = musicSeaCave;
                break;
            case "plains":
                m_MusicSource.clip = musicPlains;
                break;
            case "temp1":
                m_MusicSource.clip = musicTemp1;
                break;
            case "temp2":
                m_MusicSource.clip = musicTemp2;
                break;
            default:
                break;
        }

        if ( !m_MusicSource.isPlaying )
        {
            m_MusicSource.Play ();
        }
    }

    public void PlayOnce ( string soundToPlay )
    {
        switch ( soundToPlay )
        {
            case "shoot":
                m_SFXSource.PlayOneShot ( sndShoot );
                break;
            case "campsite":
                m_SFXSource.PlayOneShot( sndCampsite );
                break;
            case "gameOver":
                m_SFXSource.PlayOneShot( sndGameOver );
                break;
            case "refreshed":
                m_SFXSource.PlayOneShot( sndRefreshed );
                break;
            case "newItem":
                m_SFXSource.PlayOneShot ( sndNewItem );
                break;
            case "failed":
                m_SFXSource.PlayOneShot ( sndFailed );
                break;
            case "rustle":
                m_SFXSource.PlayOneShot ( sndRustle );
                break;
            case "playerDamaged":
                m_SFXSource.PlayOneShot ( sndPlayerDamaged );
                break;
            case "playerDeath":
                m_SFXSource.PlayOneShot ( sndPlayerDeath );
                break;
            case "enemyDamaged":
                m_SFXSource.PlayOneShot ( sndEnemyDamaged );
                break;
            case "enemyDeath":
                m_SFXSource.PlayOneShot ( sndEnemyDeath );
                break;
            case "click":
                m_SFXSource.PlayOneShot ( sndClick );
                break;
            case "menuSelect":
                m_SFXSource.PlayOneShot ( sndMenuSelect );
                break;
            case "menuHover":
                m_SFXSource.PlayOneShot ( sndMenuHover );
                break;
            case "transition":
                m_SFXSource.PlayOneShot ( sndTransition );
                break;
            default:
                break;
        }
    }
}