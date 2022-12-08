using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _comboTimerText;

    [SerializeField] private bool _letterLevel;

    [SerializeField] private GameObject _journal;
    [SerializeField] private TMP_Text _journalText;
    [SerializeField] private GameObject _swords;

    public StatTracker Stats;

    [SerializeField] private float _comboTimer;
    [SerializeField] private float _scoreTimer;

    [SerializeField] private int _comboKills;
    [SerializeField] private float _score;

    private int _maxKills;

    [SerializeField] private TMP_Text KillCountTextbox;
    [SerializeField] private TMP_Text KeyText;
    [SerializeField] private TMP_Text ScoreTextbox;

    [SerializeField] private RammyController _playerController;


    // Start is called before the first frame update
    void Start()
    {
        // Resets the Total time played each time you start
        Stats.TimePlayed = 0;

        // Sets the accumulated kills to 40 for testing purposes
        Stats.Kills = 0;

        if (!_letterLevel)
        {
            _swords.transform.position = _journal.transform.position;
            _journal.SetActive(false);
        }

        _maxKills = GameObject.FindGameObjectsWithTag("wolf").Length;
    }

    // Update is called once per frame
    void Update()
    {
        // Update the time played in the Stat Scriptable Object
        Stats.TimePlayed += Time.deltaTime;

        //Keep track of rammy health
        //_health = _playerController.Health;


        // Sets the text of the killcount textbox
        KillCountTextbox.text = Stats.Kills + "/" + _maxKills + " Kills";

        // Updates the Score textbox with the current score
        ScoreTextbox.text = "Score: " + _score.ToString("F0");

        _journalText.text = _playerController.lettersCollected + "/3";

        // Actibate key after you get 50 or more kills
        if (Stats.Kills >= 50)
        {
            KeyText.gameObject.SetActive(true);
        }
        #region Combo
        // If the combo timer has started
        if (_comboTimer > 0)
        {
            // Decrease the timer by real time
            _comboTimer -= Time.deltaTime;
        }
        else
        {
            EndCombo();
        }

        // If you get more than 4 kils in a row
        if (_comboKills > 4)
        {
            // Enable the kombo timer canvas
            _comboTimerText.gameObject.SetActive(true);
            //_comboCanvas.gameObject.SetActive(true);

            // Display the current combo timer with two decimals
            //_comboTimerText.text = "Combo Timer:\n" + _comboTimer.ToString("F2");
            _comboTimerText.text = "COMBO X " + ((int)(_comboKills / 4)).ToString();
        }
        #endregion

        #region Score

        // If the timer has started
        if (_scoreTimer > 0)
        {
            // Decrease the timer by real time
            _scoreTimer -= Time.deltaTime;
        }
        else
        {
            // When the timer is up reduce the score by 10 per second
            _score -= 10 * Time.deltaTime;

            // Clamp the score so it doesn't go negative
            _score = Mathf.Clamp(_score, 0, 100000000);
        }

        #endregion

        // Multiply damage based on score
        if (_score < 20)
        {
            return;
        }
        else if (_score < 40)
        {
            // Increase damage by 1.5
        }
        else if (_score < 70)
        {
            // Increase damage by 2
        }
        else
        {
            // Increase damage by 2.5
        }
    }

    public void AddKill()
    {
        // Add a kill to the total amount of kills
        Stats.Kills++;

        // Add a kill to the combo counter
        _comboKills++;

        // Add 5 points to the score
        AddScore(5);

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

    // Adds a given amount to the total Score
    public void AddScore(int score)
    {
        // Adds a given amount to the total score
        _score += score;

        // Sets the score timer to 5 seconds
        _scoreTimer = 3;

        print("Added score");
    }

    public void EndCombo()
    {
        // If the timer stopped reset the combo kill counter
        _comboKills = 0;
        _comboTimerText.gameObject.SetActive(false);
        //_comboCanvas.gameObject.SetActive(false);
    }
}
