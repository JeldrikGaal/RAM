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
        _background.SetActive(true);
        _abilityDescription.enabled = true;
        _abilityIcon.sprite = icon;
        _abilityDescription.text = description;
    }

    public void DisableUI()
    {
        _abilityDescription.enabled = false;
        _background.SetActive(false);
    }
}
