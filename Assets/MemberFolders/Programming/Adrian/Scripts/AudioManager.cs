using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Public float value for volume of the audio clips
    //public float Volume;

    /// Plays an audio clip at a certain position in the world with at a specific volume
    // Input: Audio clip to be played, world position to play the audio clip at, volume for the clip
    public void PlayAudio(AudioClip clip, Vector3 position, float volume)
    {
        AudioSource.PlayClipAtPoint(clip, position, volume);
    }
}
