using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour 
{
    // Combat
    public AudioClip sndLeft1;
    public AudioClip sndLeft2;
    public AudioClip sndLeft3;
    public AudioClip sndLeft4;
    public AudioClip sndRight0;
    public AudioClip sndRight1;
    public AudioClip sndRight2;
    public AudioClip sndRight3;
    public AudioClip sndRight4;

    // Movement
    public AudioClip sndWalk;
    public AudioClip sndRun;
    public AudioClip sndDodge;
    public AudioClip sndTransition;

    // UI
    public AudioClip sndMenuSelect;
    public AudioClip sndClick;

    // Background Music
    public AudioClip musicMystery;
    public AudioClip musicMain;

    // Other
    public AudioClip sndNoStamina;
    public AudioClip sndPlayerNearDeath;
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
                Debug.Log ( "main" );
                m_MusicSource.clip = musicMain;
                break;
            case "mystery":
                Debug.Log ( "mystery" );
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
            case "left1":
                m_SFXSource.PlayOneShot ( sndLeft1 );
                break;
            case "left2":
                m_SFXSource.PlayOneShot ( sndLeft2 );
                break;
            case "left3":
                m_SFXSource.PlayOneShot ( sndLeft3 );
                break;
            case "left4":
                m_SFXSource.PlayOneShot ( sndLeft4 );
                break;
            case "right0":
                m_SFXSource.PlayOneShot ( sndRight0 );
                break;
            case "right1":
                m_SFXSource.PlayOneShot ( sndRight1 );
                break;
            case "right2":
                m_SFXSource.PlayOneShot ( sndRight2 );
                break;
            case "right3":
                m_SFXSource.PlayOneShot ( sndRight3 );
                break;
            case "right4":
                m_SFXSource.PlayOneShot ( sndRight4 );
                break;
            case "dodge":
                m_SFXSource.PlayOneShot ( sndDodge );
                break;
            case "playerNoStamina":
                m_SFXSource.PlayOneShot ( sndNoStamina );
                break;
            case "playerNearDeath":
                m_SFXSource.PlayOneShot ( sndPlayerNearDeath );
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
