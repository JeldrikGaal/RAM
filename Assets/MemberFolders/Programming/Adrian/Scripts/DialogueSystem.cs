using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueSystem : MonoBehaviour
{
    public TMP_Text TextBox;

    public bool PlayingAudio;

    /// Plays an audioclip at a location with a set volume, then waits for the audioclip is over plus another variable of time in seconds to play the next clip in the array
    // Input: array of audioclips, time in seconds to wait after the clip is over to play the next clip, position to play the clip at, volume of the clip
    public IEnumerator Dialogue(string[] lines, AudioClip[] clips, float time, Vector3 position, float volume)
    {
        PlayingAudio = true;
        for (int i = 0; i < clips.Length; i++)
        {
            TextBox.maxVisibleCharacters = 0;
            TextBox.text = lines[i];
            AudioSource.PlayClipAtPoint(clips[i], position, volume);
            foreach (char c in lines[i])
            {
                TextBox.maxVisibleCharacters++;
                yield return new WaitForSeconds(0.1f);
            }
            print("playing clip: " + clips[i]);
            yield return new WaitForSeconds(clips[i].length + time);
        }
        PlayingAudio = false;
    }
}
