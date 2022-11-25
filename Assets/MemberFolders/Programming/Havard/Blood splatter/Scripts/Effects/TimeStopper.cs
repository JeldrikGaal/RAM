using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeStopper : MonoBehaviour
{
    // Function to pause time. Takes the input of how much time is paused to set the time to that for as long as the other input wants it to be

    private bool _pausing = false;
    public void PauseTime(float timeScaleWhilePaused, float timeTilNotPaused)
    {
        StartCoroutine(TimeFunction(timeScaleWhilePaused, timeTilNotPaused));
    }

    private IEnumerator TimeFunction(float timeWhilePaused, float timeTilNotPaused)
    {
        if (!_pausing)
        {
            Time.timeScale = timeWhilePaused;
            _pausing = true;
            yield return new WaitForSecondsRealtime(timeTilNotPaused);
            Time.timeScale = 1;
            _pausing = false;
        }
    }


}
