using System.ComponentModel;
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

    [SerializeField] private Image _speedBuff;
    [SerializeField] private Image _damageBuff;
    [SerializeField] private Image _damageReductionBuff;
    [SerializeField] private Image _stunBuff;
    [SerializeField] private Texture2D _cursor;
    private CursorMode cursorMode = CursorMode.Auto;
    private Vector2 hotSpot = Vector2.zero;



    private float _killSplatAlpha;

    public StatTracker Stats;

    public int MaxKills;


    public RammyController PlayerController;


    // Start is called before the first frame update
    void Start()
    {
        Cursor.SetCursor(_cursor, hotSpot, cursorMode);
        // Resets the Total time played each time you start
        Stats.TimePlayed = 0;
        Stats.Kills = 0;

        // If there are no letters in the area disable the letter tracker
        if (!_letterLevel)
        {
            _journal.SetActive(false);
        }
        else
        {
            _swords.SetActive(false);
        }

        PlayerController = FindObjectOfType<RammyController>();

        // if (GameObject.FindGameObjectsWithTag("wolf").Length != 0)
        // {
        //     MaxKills = GameObject.FindGameObjectsWithTag("wolf").Length;
        // }
    }

    // Update is called once per frame
    void Update()
    {
        //Keep track of rammy health
        //_health = _playerController.Health;


        // Sets the text of the killcount textbox
        if (Stats.Kills <= MaxKills)
        {
            _swordTextbox.text = Stats.Kills + "/" + MaxKills;
        }

        // Sets the text of the total killcount textbox
        _killCountTextbox.text = Stats.Kills + "";

        // Sets the text of the letters collected textbox
        _journalText.text = PlayerController.lettersCollected + "/3";


        // Scales all the textboxes and the bloodsplat down to their default value
        _totalKills.transform.localScale = Vector3.Lerp(_totalKills.transform.localScale, Vector3.one, Time.deltaTime);
        _totalKills.transform.localScale = Vector3.ClampMagnitude(_totalKills.transform.localScale, 3f);


        _killCountTextbox.transform.localScale = Vector3.Lerp(_killCountTextbox.transform.localScale, Vector3.one, Time.deltaTime);
        _killCountTextbox.transform.localScale = Vector3.ClampMagnitude(_killCountTextbox.transform.localScale, 3f);

        _killSplat.transform.localScale = Vector3.Lerp(_killSplat.transform.localScale, new Vector3(2.91f, 2.91f, 2.91f), Time.deltaTime);
        _killSplat.transform.localScale = Vector3.ClampMagnitude(_killSplat.transform.localScale, 10f);

        // Sets the transparency of the bloodsplat image
        _killSplat.color = new Color(255, 0, 0, _killSplatAlpha);

        _killSplatAlpha = Mathf.Lerp(_killSplatAlpha, 0, Time.deltaTime);

        // Sets the images to be enabled if the player has a powerup
        _damageBuff.enabled = PlayerController.HasDamageBuff;
        _speedBuff.enabled = PlayerController.HasSpeedBuff;
        _stunBuff.enabled = PlayerController.HasStunBuff;
        _damageReductionBuff.enabled = PlayerController.HasDamageReductionBuff;
    }

    public void AddKill()
    {
        // Add a kill to the total amount of kills
        Stats.Kills++;

        // Scales the ui elements up a little
        _totalKills.transform.localScale += (Vector3.one / 4);
        _killCountTextbox.transform.localScale += (Vector3.one / 4);
        _killSplat.transform.localScale += (Vector3.one);

        // Makes the bloodsplat less transparent
        _killSplatAlpha += 20f;
    }
}
