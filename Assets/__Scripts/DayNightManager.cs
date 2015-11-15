using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DayNightManager : MonoBehaviour
{
    const float maxTime = 50.0f;
    public int timeOfDay = 1;
    int lightTimer = 1;

    // Number of seconds it takes to advance one hour
    public float secondsPerHour = 2.5f; // Overwritten by w/e is in inspector

    // Indicates which scene is the safe location (index)
    // Scenes 1-6
    int safeLocation = 0;

    // The Day Night Slider in the UI system. Goes 1 - 24
    private Slider mSlider;
    private Image mHandle;
    private GameManager mGame;
    private AudioManager mAudio;

    public Sprite sun;
    public Sprite moon;

	public Light daylight;

    private Character mChar;

	bool increasing = true;
    private bool isDay = false;

    float startTime;
    float curTime;
    public float numSecondsToChange = 1.0f;
    float curIntensity = 1.0f;
    float targetIntensity = 1.0f;
    public float maxIntensity = 1.0f;
    public float minIntensity = 0.5f;

    public Color dayColor;
    public Color nightColor;
    Color curColor;
    Color targetColor;

	void Start()
    {
        curColor = Color.white;
        targetColor = Color.white;
        mGame = GetComponent<GameManager> ();
        mAudio = GetComponent<AudioManager> ();
	}

    // Use this for initialization
    void OnLevelWasLoaded ()
    {
        if ( Application.loadedLevel != 0 && Application.loadedLevel != 7 )
        {
            ContinueDay ();
        }
        else
        {
            // If main menu or game over, reset the lights
            timeOfDay = 1;
            maxIntensity = 1.0f;
            targetIntensity = 1.0f;
            curIntensity = 1.0f;
            curColor = Color.white;
            targetColor = Color.white;
            lightTimer = 1;
            CheckTime ();
        }
    }

    // Update is called once per frame
    void Update ()
    {
		if ( Application.loadedLevel != 0 && Application.loadedLevel != 7 )
		{
            if (curTime - startTime < numSecondsToChange)
            {
                daylight.intensity = Mathf.Lerp(curIntensity, targetIntensity, (curTime-startTime)/numSecondsToChange);
                //Debug.Log(daylight.intensity);
                daylight.color = Color.Lerp(curColor, targetColor, (curTime - startTime) / numSecondsToChange);
                curTime += Time.deltaTime;
            }
        }
    }

    void RandomizeSafeLocation()
    {
        // Randomly choose one of the scenes that has a campfire
        safeLocation = Random.Range( 1, 6 );
    }

    int GetSafeLocation()
    {
        return safeLocation;
    }

    public void ContinueDay()
    {
        mSlider = GameObject.Find ( "DayNightSlider" ).GetComponent<Slider> ();
        mHandle = GameObject.Find ( "DayNightHandle" ).GetComponent<Image> ();

        mSlider.maxValue = maxTime;
        mSlider.value = timeOfDay;

        mChar = GameObject.FindGameObjectWithTag ( "Char" ).GetComponent<Character> ();

        if ( mGame.GetPreviousSceneIndex () == -1 ) // Is the first time this is loaded
        {
            StartCoroutine ( AdvanceHour ( 1 ) );
        }
        daylight = GameObject.Find ( "Directional Light" ).GetComponent<Light> ();
        daylight.intensity = targetIntensity; 
        CheckTime ();
    }

    public int GetTimeOfDay()
    {
        return timeOfDay;
    }

    IEnumerator AdvanceHour( int numHours )
    {
        yield return new WaitForSeconds( secondsPerHour );

        // Ensure the timer stops when on the menu screens
        if ( Application.loadedLevel == 0 || Application.loadedLevel == 7 )
        {
            yield break;
        }
        timeOfDay += numHours;

        // Deal with light intensity
        if ( increasing )
        {
            lightTimer += numHours;
        }
        else
        {
            lightTimer -= numHours;
        }
        //Debug.Log("Time of day: " + timeOfDay);
        //Debug.Log("Time of day out of 12: " + timeOfDay / 12.0f);
        startTime = Time.time;
        curTime = Time.time;
        curIntensity = daylight.intensity;
        targetIntensity = Mathf.Lerp ( maxIntensity, minIntensity, ( lightTimer / ( maxTime * 0.5f ) ) );
        curColor = daylight.color;
        targetColor = Color.Lerp(dayColor, nightColor, (lightTimer / (maxTime * 0.5f)));

        if( timeOfDay >= maxTime )
        {
            timeOfDay = 1; // Start new day
            mChar.CheckStarved ();
            RandomizeSafeLocation ();
        }
        if ( timeOfDay >= ( maxTime * 0.5) )
        {
			increasing = false;
        }
        if ( timeOfDay <= 1 )
        {
            increasing = true;
        }
        CheckTime ();
        mSlider.value = timeOfDay;
        mChar.IncrementHunger ( -1 ); // Get hungrier throughout the day
        
        StartCoroutine ( AdvanceHour ( 1 ) );
    }

    public void CheckTime()
    {
        Debug.Log ( "Checking time!" );
        if ( timeOfDay > ( maxTime * 0.35 ) && timeOfDay < ( maxTime * .75 ) && isDay )
        {
            StartNight ();
        }
        if ( timeOfDay <= ( maxTime * 0.35 ) && timeOfDay >= ( maxTime * .75 ) && !isDay )
        {
            StartDay ();
        }
    }

    public void StartDay()
    {
        isDay = true;
        mHandle.sprite = sun;
        mAudio.PlayLayer ( mGame.GetCurrentSceneIndex () );
    }

    public void StartNight()
    {
        isDay = false;
        mHandle.sprite = moon;
        mAudio.StopLayer ();
    }
}