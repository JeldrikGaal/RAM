using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueSystem : MonoBehaviour
{
    public TMP_Text TextBox;

    public bool PlayingAudio;

    private float timeSpent;

    /// Plays an audioclip at a location with a set volume, then waits for the audioclip is over plus another variable of time in seconds to play the next clip in the array
    // Input: array of audioclips, time in seconds to wait after the clip is over to play the next clip, position to play the clip at, volume of the clip
    public IEnumerator Dialogue(string[] lines, AudioClip[] clips, float timeBetweenLines, Vector3 position, float volume)
    {
        // Enable a bool so the audio can't be played on top of itself
        PlayingAudio = true;

        // For loop to play all the audio lines
        for (int i = 0; i < clips.Length; i++)
        {
            // Set the max visible characters of the textbox to 0
            TextBox.maxVisibleCharacters = 0;

            // Set the text in the textbox to be the full dialogue line
            TextBox.text = lines[i];

            // Play the dialogue audio clip at a certain specified location with a specified volume
            AudioSource.PlayClipAtPoint(clips[i], position, volume);
            timeSpent = 0;

            // Foreach loop to go over each character in the line and increase the max visible character count by one and delay a bit to get a typewriter effect
            foreach (char c in lines[i])
            {
                TextBox.maxVisibleCharacters++;
                yield return new WaitForSeconds(0.05f);
                timeSpent += 0.1f;
            }

            // Wait the length of the audio clip plus a specified length in seconds for the next audio to be played
            yield return new WaitForSeconds(clips[i].length - timeSpent);
            yield return new WaitForSeconds(timeBetweenLines);
        }

        // Set the bool to false again so the audio can be triggered again
        PlayingAudio = false;
    }
}
