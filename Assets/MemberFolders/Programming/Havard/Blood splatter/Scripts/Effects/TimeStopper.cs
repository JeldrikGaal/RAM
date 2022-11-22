using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeStopper : MonoBehaviour
{
    // Function to pause time. Takes the input of how much time is paused to set the time to that. Then it calculates the time for the fixed time timer.
    public void PauseTime(float timeWhilePaused, float timeTilNotPaused)
    {
        Time.timeScale = timeWhilePaused;
        print(timeWhilePaused);
        print(Time.timeScale);
        print(timeTilNotPaused);
        print(timeTilNotPaused * Time.timeScale);
        Invoke("FixTime", timeTilNotPaused*Time.timeScale);
    }

    private void FixTime()
    {
        Time.timeScale = 1;
    }

}
