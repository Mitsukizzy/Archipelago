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

    // Other
    public AudioClip sndNoStamina;
    public AudioClip sndPlayerNearDeath;
    public AudioClip sndPlayerDamaged;
    public AudioClip sndPlayerDeath;
    public AudioClip sndEnemyDamaged;
    public AudioClip sndEnemyDeath;

    private AudioSource m_AudioSource;		

	// Use this for initialization
	void Start () 
    {
        m_AudioSource = GameObject.Find ( "Main Camera" ).GetComponent<AudioSource> ();
        m_AudioSource.loop = false;
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    public void Stop ()
    {
        m_AudioSource.Stop ();
    }

    public void Loop ( string soundToLoop )
    {

    }

    public void PlayOnce ( string soundToPlay )
    {
        switch ( soundToPlay )
        {
            case "left1":
                m_AudioSource.PlayOneShot ( sndLeft1 );
                break;
            case "left2":
                m_AudioSource.PlayOneShot ( sndLeft2 );
                break;
            case "left3":
                m_AudioSource.PlayOneShot ( sndLeft3 );
                break;
            case "left4":
                m_AudioSource.PlayOneShot ( sndLeft4 );
                break;
            case "right0":
                m_AudioSource.PlayOneShot ( sndRight0 );
                break;
            case "right1":
                m_AudioSource.PlayOneShot ( sndRight1 );
                break;
            case "right2":
                m_AudioSource.PlayOneShot ( sndRight2 );
                break;
            case "right3":
                m_AudioSource.PlayOneShot ( sndRight3 );
                break;
            case "right4":
                m_AudioSource.PlayOneShot ( sndRight4 );
                break;
            case "dodge":
                m_AudioSource.PlayOneShot ( sndDodge );
                break;
            case "playerNoStamina":
                m_AudioSource.PlayOneShot ( sndNoStamina );
                break;
            case "playerNearDeath":
                m_AudioSource.PlayOneShot ( sndPlayerNearDeath );
                break;
            case "playerDamaged":
                m_AudioSource.PlayOneShot ( sndPlayerDamaged );
                break;
            case "playerDeath":
                m_AudioSource.PlayOneShot ( sndPlayerDeath );
                break;
            case "enemyDamaged":
                m_AudioSource.PlayOneShot ( sndEnemyDamaged );
                break;
            case "enemyDeath":
                m_AudioSource.PlayOneShot ( sndEnemyDeath );
                break;
            default:
                break;
        }
    }
}
