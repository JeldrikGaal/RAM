using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatManager : MonoBehaviour
{
    [SerializeField] private StatTracker Stats;

    [SerializeField] private TMP_Text KillCountTextbox;
    [SerializeField] private TMP_Text KeyText;

    // Start is called before the first frame update
    void Start()
    {
        // Resets the Total time played each time you start
        Stats.TimePlayed = 0;
        Stats.Kills = 40;
    }

    // Update is called once per frame
    void Update()
    {
        // Update the time played in the Stat Scriptable Object
        Stats.TimePlayed += Time.deltaTime;

        // Sets the text of the killcount textbox
        KillCountTextbox.text = Stats.Kills + "/50 Kills";

        if (Stats.Kills >= 50)
        {
            KeyText.gameObject.SetActive(true);
        }
    }

    public void AddKill()
    {
        Stats.Kills++;
    }
}
