using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatManager : MonoBehaviour
{
    [SerializeField] private Canvas _comboCanvas;

    [SerializeField] private TMP_Text _comboTimerText;

    public StatTracker Stats;

    [SerializeField] private float _comboTimer;

    [SerializeField] private int _comboKills;

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

        // Actibate key after you get 50 or more kills
        if (Stats.Kills >= 50)
        {
            KeyText.gameObject.SetActive(true);
        }

        // If the combo timer has started
        if (_comboTimer > 0)
        {
            // Decrease the timer by real time
            _comboTimer -= Time.deltaTime;
        }
        else
        {
            // If the timer stopped reset the combo kill counter
            _comboKills = 0;
            _comboCanvas.gameObject.SetActive(false);
        }

        // If you get more than 4 kils in a row
        if (_comboKills > 4)
        {
            // Enable the kombo timer canvas
            _comboCanvas.gameObject.SetActive(true);

            // Display the current combo timer with two decimals
            _comboTimerText.text = "Combo Timer:\n" + _comboTimer.ToString("F2");
        }
    }

    public void AddKill()
    {
        // Add a kill to the total amount of kills
        Stats.Kills++;

        // Add a kill to the combo counter
        _comboKills++;

        // if you have less 5 or less current combo kills add 5/(current amount of combo kills) to the combo timer
        if (_comboKills <= 5)
        {
            _comboTimer += 5 / _comboKills;
        }
        else
        {
            // If you have more than 5 add 0.6 seconds to the combo timer
            _comboTimer += 0.6f;
        }
    }
}
