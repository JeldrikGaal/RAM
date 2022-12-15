using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CloseButtonAbilityUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _abilityDescription;
    [SerializeField] private GameObject _button;
    [SerializeField] private GameObject _background;


    public void DisableUI()
    {
        Time.timeScale = 1;
        _abilityDescription.enabled = false;
        _background.SetActive(false);
        _button.SetActive(false);
    }
}
