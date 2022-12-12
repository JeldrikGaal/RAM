using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatManager : MonoBehaviour
{
    [SerializeField] private bool _letterLevel;

    [SerializeField] private GameObject _journal;
    [SerializeField] private TMP_Text _journalText;
    [SerializeField] private GameObject _swords;
    [SerializeField] private TMP_Text _swordTextbox;

    public StatTracker Stats;

    public int MaxKills;


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

        MaxKills = GameObject.FindGameObjectsWithTag("wolf").Length;
    }

    // Update is called once per frame
    void Update()
    {
        // Update the time played in the Stat Scriptable Object
        Stats.TimePlayed += Time.deltaTime;

        //Keep track of rammy health
        //_health = _playerController.Health;


        // Sets the text of the killcount textbox
        _swordTextbox.text = Stats.Kills + "/" + MaxKills + " Kills";

        _journalText.text = _playerController.lettersCollected + "/3";

        _swords.transform.localScale = Vector3.Lerp(_swords.transform.localScale, new Vector3(0.25f, 0.25f, 0.25f), Time.deltaTime);
        _swords.transform.localScale = Vector3.ClampMagnitude(_swords.transform.localScale, 1.5f);


        _swordTextbox.transform.localScale = Vector3.Lerp(_swordTextbox.transform.localScale, Vector3.one, Time.deltaTime);
        _swordTextbox.transform.localScale = Vector3.ClampMagnitude(_swordTextbox.transform.localScale, 1.5f);
    }

    public void AddKill()
    {
        // Add a kill to the total amount of kills
        Stats.Kills++;

        _swords.transform.localScale += (Vector3.one / 5);
        _swordTextbox.transform.localScale += (Vector3.one / 5);
    }
}
