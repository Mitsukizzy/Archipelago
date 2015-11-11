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
    public AudioClip sndCook;
    public AudioClip sndNewItem;
    public AudioClip sndFailed;
    public AudioClip sndRustle;
    public AudioClip sndPlayerDamaged;
    public AudioClip sndEnemyDamaged;
    public AudioClip sndEnemyDeath;

    private AudioSource m_AudioSource;
    private AudioSource m_MusicSource;

    private float sfxVolume = 0.5f;

	// Use this for initialization
	void Start ()
    {

	}

    public void SpecialInit()
    {
        m_AudioSource = GetComponent<AudioSource> ();

        m_MusicSource = GameObject.Find ( "Main Camera" ).GetComponent<AudioSource> ();
        m_MusicSource.volume = 0.4f;
        m_MusicSource.loop = true;
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
        Debug.Log ( "ONE SHOT " + soundToPlay );
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
                m_AudioSource.PlayOneShot ( sndRefreshed, 0.3f );
                break;
            case "cook":
                m_AudioSource.PlayOneShot ( sndCook, 0.4f );
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
            case "enemyDamaged":
                m_AudioSource.PlayOneShot ( sndEnemyDamaged, 0.1f );
                break;
            case "enemyDeath":
                m_AudioSource.PlayOneShot ( sndEnemyDeath, 0.3f );
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
                m_AudioSource.PlayOneShot ( sndTransition, 0.2f );
                break;
            default:
                break;
        }
    }
}