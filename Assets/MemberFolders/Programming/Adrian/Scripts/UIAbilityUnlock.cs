using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIAbilityUnlock : MonoBehaviour
{
    [SerializeField] private GameObject _background;
    [SerializeField] private Image _abilityIcon;
    [SerializeField] private TMP_Text _abilityDescription;
    [SerializeField] private GameObject _button;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void EnableUI(Sprite icon, string description)
    {
        Time.timeScale = 0;
        _background.SetActive(true);
        _button.SetActive(true);
        _abilityDescription.enabled = true;
        _abilityIcon.sprite = icon;
        _abilityDescription.text = description;
    }
}
