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
    public AudioClip sndClick;

    // Background Music
    public AudioClip musicMystery;
    public AudioClip musicMain;

    // Other
    public AudioClip sndPlayerDamaged;
    public AudioClip sndPlayerDeath;
    public AudioClip sndEnemyDamaged;
    public AudioClip sndEnemyDeath;

    private AudioSource m_SFXSource;
    public AudioSource m_MusicSource;	


	// Use this for initialization
	void Start () 
    {
        m_SFXSource = GameObject.Find ( "Main Camera" ).GetComponent<AudioSource> ();
        m_MusicSource.volume = 0.5f;
        m_SFXSource.loop = false;

        m_MusicSource.volume = 0.35f;
        m_MusicSource.loop = true;
	}
	
	// Update is called once per frame
	void Update () 
    {
	
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
            case "mystery":
                m_MusicSource.clip = musicMystery;
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
            case "transition":
                m_SFXSource.PlayOneShot ( sndTransition );
                break;
            default:
                break;
        }
    }
}
