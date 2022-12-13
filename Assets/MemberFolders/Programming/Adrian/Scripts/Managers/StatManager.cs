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
    [SerializeField] private TMP_Text _totalKills;
    [SerializeField] private TMP_Text _killCountTextbox;
    [SerializeField] private Image _killSplat;
    private float _killSplatAlpha;

    public StatTracker Stats;

    public int MaxKills;


    [SerializeField] private RammyController _playerController;


    // Start is called before the first frame update
    void Start()
    {
        // Resets the Total time played each time you start
        Stats.TimePlayed = 0;
        Stats.Kills = 0;


        if (!_letterLevel)
        {
            _journal.SetActive(false);
        }

        // if (GameObject.FindGameObjectsWithTag("wolf").Length != 0)
        // {
        //     MaxKills = GameObject.FindGameObjectsWithTag("wolf").Length;
        // }
    }

    // Update is called once per frame
    void Update()
    {
        // Update the time played in the Stat Scriptable Object
        Stats.TimePlayed += Time.deltaTime;

        //Keep track of rammy health
        //_health = _playerController.Health;


        // Sets the text of the killcount textbox
        if (Stats.Kills <= MaxKills)
        {
            print("wo");
            _swordTextbox.text = Stats.Kills + "/" + MaxKills;
        }

        _killCountTextbox.text = Stats.Kills + "";

        _journalText.text = _playerController.lettersCollected + "/3";

        _totalKills.transform.localScale = Vector3.Lerp(_totalKills.transform.localScale, Vector3.one, Time.deltaTime);
        _totalKills.transform.localScale = Vector3.ClampMagnitude(_totalKills.transform.localScale, 3f);


        _killCountTextbox.transform.localScale = Vector3.Lerp(_killCountTextbox.transform.localScale, Vector3.one, Time.deltaTime);
        _killCountTextbox.transform.localScale = Vector3.ClampMagnitude(_killCountTextbox.transform.localScale, 3f);

        _killSplat.transform.localScale = Vector3.Lerp(_killSplat.transform.localScale, new Vector3(2.91f, 2.91f, 2.91f), Time.deltaTime);
        _killSplat.transform.localScale = Vector3.ClampMagnitude(_killSplat.transform.localScale, 10f);

        _killSplat.color = new Color(255, 0, 0, _killSplatAlpha);

        _killSplatAlpha = Mathf.Lerp(_killSplatAlpha, 0, Time.deltaTime);
    }

    public void AddKill()
    {
        // Add a kill to the total amount of kills
        Stats.Kills++;

        _totalKills.transform.localScale += (Vector3.one / 4);
        _killCountTextbox.transform.localScale += (Vector3.one / 4);
        _killSplat.transform.localScale += (Vector3.one);
        _killSplatAlpha += 20f;
    }
}
