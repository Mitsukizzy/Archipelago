using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DayNightManager : MonoBehaviour
{
    // Time will be 3/4 day and 1/4 night
    // Daytime: 1-18, Nighttime: 19-24
    int timeOfDay = 1;

    // Number of seconds it takes to advance one hour
    float secondsPerHour = 5;

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
    float numSecondsToChange = 2.0f;
    float curIntensity = 2.0f;
    float targetIntensity = 2.0f;


	void Start(){
		if ( Application.loadedLevel != 0 && Application.loadedLevel != 7 )
		{
			mSlider = GameObject.Find ( "DayNightSlider" ).GetComponent<Slider> ();
			mHandle = GameObject.Find ( "DayNightHandle" ).GetComponent<Image> ();
			
			mSlider.maxValue = 12;
			mSlider.value = timeOfDay;
			
			mChar = GameObject.FindGameObjectWithTag("Char").GetComponent<Character>();
			
			daylight = GameObject.Find("Directional Light").GetComponent<Light>();

            if (startOnce)
            {
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

            mSlider.maxValue = 12;
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

    IEnumerator AdvanceHour( int numHours )
    {
        yield return new WaitForSeconds( secondsPerHour );
		if(increasing){
        	timeOfDay += numHours;
            startTime = Time.time;
            curIntensity = daylight.intensity;
            targetIntensity = Mathf.Lerp(2.0f, 0.5f, (timeOfDay / 12.0f));
		}
		else{
			timeOfDay -= numHours;
            startTime = Time.time;
            curIntensity = daylight.intensity;
            targetIntensity = Mathf.Lerp(2.0f, 0.5f, (timeOfDay / 12.0f));
		}
        if( timeOfDay >= 12 )
        {
            //timeOfDay -= 24; // Start new day
			increasing = false;
            mChar.CheckStarved ();
            RandomizeSafeLocation();
        }
		if(timeOfDay <= 0){
			increasing = true;
		}
        if( timeOfDay > 8 )
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