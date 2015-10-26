using UnityEngine;
using System.Collections;

public class DayNightManager : MonoBehaviour
{
    // Time will be 3/4 day and 1/4 night
    // Daytime: 0-18, Nighttime: 19-24
    int timeOfDay = 0;

    // Number of seconds it takes to advance one hour
    int secondsPerHour = 30;

    // Indicates which scene is the safe location (index)
    // Scenes 1-6
    int safeLocation = 0;

    // Use this for initialization
    void Start ()
    {
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
        StartCoroutine( AdvanceHour( 1 ) );
    }
}