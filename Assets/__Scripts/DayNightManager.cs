using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DayNightManager : MonoBehaviour
{
    const float maxTime = 50.0f;
    public int timeOfDay = 1;
    int lightTimer = 1;

    // Number of seconds it takes to advance one hour
    public float secondsPerHour = 4.0f;

    // Indicates which scene is the safe location (index)
    // Scenes 1-6
    int safeLocation = 0;

    // The Day Night Slider in the UI system. Goes 1 - 24
    private Slider mSlider;
    private Image mHandle;

    public Sprite sun;
    public Sprite moon;

	public Light daylight;

    private Character mChar;

	bool increasing = true;
    bool startOnce = true;

    float startTime;
    float curTime;
    public float numSecondsToChange = 1.0f;
    float curIntensity = 2.0f;
    float targetIntensity = 2.0f;


	void Start(){
		if ( Application.loadedLevel != 0 && Application.loadedLevel != 7 )
		{
			mSlider = GameObject.Find ( "DayNightSlider" ).GetComponent<Slider> ();
			mHandle = GameObject.Find ( "DayNightHandle" ).GetComponent<Image> ();

            mSlider.maxValue = maxTime;
			mSlider.value = timeOfDay;
			
			mChar = GameObject.FindGameObjectWithTag("Char").GetComponent<Character>();
			
			daylight = GameObject.Find("Directional Light").GetComponent<Light>();

            if (startOnce)
            {
                Debug.Log("this should never happen in the main menu");
                StartCoroutine(AdvanceHour(1));
                startOnce = false;
            }
		}
	}

    // Use this for initialization
    void OnLevelWasLoaded ()
    {
        if ( Application.loadedLevel != 0 && Application.loadedLevel != 7 )
        {
            mSlider = GameObject.Find ( "DayNightSlider" ).GetComponent<Slider> ();
            mHandle = GameObject.Find ( "DayNightHandle" ).GetComponent<Image> ();

            mSlider.maxValue = maxTime;
            mSlider.value = timeOfDay;

            mChar = GameObject.FindGameObjectWithTag("Char").GetComponent<Character>();

			daylight = GameObject.Find("Directional Light").GetComponent<Light>();
            if (startOnce)
            {
                StartCoroutine(AdvanceHour(1));
                startOnce = false;
            }
        }
        daylight.intensity = targetIntensity;

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

    public int GetTimeOfDay()
    {
        return timeOfDay;
    }

    IEnumerator AdvanceHour( int numHours )
    {
        yield return new WaitForSeconds( secondsPerHour );
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
        targetIntensity = Mathf.Lerp ( 2.0f, 0.5f, ( lightTimer / ( maxTime * 0.5f ) ) );
        
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
        if( timeOfDay > ( maxTime * 0.35 ) && timeOfDay < ( maxTime * .75 ) )
        {
            mHandle.sprite = moon;
        }
        else
        {
            mHandle.sprite = sun;
        }
        mSlider.value = timeOfDay;
        mChar.IncrementHunger ( -1 ); // Get hungrier throughout the day
        StartCoroutine( AdvanceHour( 1 ) );
    }
}