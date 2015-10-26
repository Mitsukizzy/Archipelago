﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DayNightManager : MonoBehaviour
{
    // Time will be 3/4 day and 1/4 night
    // Daytime: 1-18, Nighttime: 19-24
    int timeOfDay = 1;

    // Number of seconds it takes to advance one hour
    int secondsPerHour = 5;

    // Indicates which scene is the safe location (index)
    // Scenes 1-6
    int safeLocation = 0;

    // The Day Night Slider in the UI system. Goes 1 - 24
    private Slider mSlider;
    private Image mHandle;

    public Sprite sun;
    public Sprite moon;

    // Use this for initialization
    void Start ()
    {
        mSlider = GameObject.Find ( "DayNightSlider" ).GetComponent<Slider>();
        mHandle = GameObject.Find ( "DayNightHandle" ).GetComponent<Image>();


        StartCoroutine( AdvanceHour( 1 ) );
    }

    // Update is called once per frame
    void Update ()
    {

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
        timeOfDay += numHours;
        if( timeOfDay > 24 )
        {
            timeOfDay -= 24; // Start new day
            RandomizeSafeLocation();
        }
        if( timeOfDay > 18 )
        {
            mHandle.sprite = moon;
        }
        else
        {
            mHandle.sprite = sun;
        }
        mSlider.value = timeOfDay;
        StartCoroutine( AdvanceHour( 1 ) );
    }
}