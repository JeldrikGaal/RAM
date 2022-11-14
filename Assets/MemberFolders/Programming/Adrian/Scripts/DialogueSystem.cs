using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueSystem : MonoBehaviour
{

    public bool PlayingAudio;

    /// Plays an audioclip at a location with a set volume, then waits for the audioclip is over plus another variable of time in seconds to play the next clip in the array
    // Input: array of audioclips, time in seconds to wait after the clip is over to play the next clip, position to play the clip at, volume of the clip
    public IEnumerator Dialogue(AudioClip[] clips, float time, Vector3 position, float volume)
    {
        PlayingAudio = true;
        for (int i = 0; i < clips.Length; i++)
        {
            AudioSource.PlayClipAtPoint(clips[i], position, volume);
            print("playing clip: " + clips[i]);
            yield return new WaitForSeconds(clips[i].length + time);
        }
        PlayingAudio = false;
    }
}
