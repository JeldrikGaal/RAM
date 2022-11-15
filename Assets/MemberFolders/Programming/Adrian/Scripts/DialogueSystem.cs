using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueSystem : MonoBehaviour
{
    public TMP_Text DialogueBox;
    public TMP_Text Name1;
    public TMP_Text Name2;

    [HideInInspector] public bool PlayingAudio;

    private float _timeSpent;


    /// Plays an audioclip at a location with a set volume, then waits for the audioclip is over plus another variable of time in seconds to play the next clip in the array
    // Input: array of dialogue lines, array of audioclips, time in seconds to wait after the clip is over to play the next clip, 
    // world position to play the clip at, volume of the clip (0 - 1), names of the two people talking
    public IEnumerator Dialogue(string[] lines, AudioClip[] clips, float timeBetweenLines, Vector3 position, float volume, string name1, string name2)
    {
        // Enables the first nameplate and disbles the second
        Name1.gameObject.SetActive(false);
        Name2.gameObject.SetActive(true);

        // Sets the name of the two people in the dialogue
        Name1.text = name1;
        Name2.text = name2;

        // Enable a bool so the audio can't be played on top of itself
        PlayingAudio = true;

        // For loop to play all the audio lines
        for (int i = 0; i < clips.Length; i++)
        {
            // Reverses the active nameplate
            Name1.gameObject.SetActive(!Name1.gameObject.activeSelf);
            Name2.gameObject.SetActive(!Name2.gameObject.activeSelf);

            // Set the max visible characters of the textbox to 0
            DialogueBox.maxVisibleCharacters = 0;

            // Set the text in the textbox to be the full dialogue line
            DialogueBox.text = lines[i];

            // Play the dialogue audio clip at a certain specified location with a specified volume
            AudioSource.PlayClipAtPoint(clips[i], position, volume);
            _timeSpent = 0;

            // Foreach loop to go over each character in the line and increase the max visible character count by one and delay a bit to get a typewriter effect, also adds to 
            // total time spent on displaying the text
            foreach (char c in lines[i])
            {
                DialogueBox.maxVisibleCharacters++;
                yield return new WaitForSeconds(0.03f);
                _timeSpent += 0.1f;
            }

            // Wait the length of the audio clip minus the time already spent displaying the text
            yield return new WaitForSeconds(clips[i].length - _timeSpent);

            // Wait the set time between dialogue lines
            yield return new WaitForSeconds(timeBetweenLines);
        }

        // Set the bool to false again so the audio can be triggered again
        PlayingAudio = false;
        DialogueBox.transform.parent.gameObject.SetActive(false);
    }
}
