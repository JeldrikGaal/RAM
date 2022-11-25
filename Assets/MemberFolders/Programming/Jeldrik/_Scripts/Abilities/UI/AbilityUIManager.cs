using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class AbilityUIManager : MonoBehaviour
{
    [SerializeField] private RammyController _controller;

    private List<Transform> _abilityBlocks = new List<Transform>();
    private List<Image> _abilityImages = new List<Image>();
    private List<Image> _coolDownCircles = new List<Image>();

    private List<Abilities> _abilityScripts;

    // Start is called before the first frame update
    void Awake()
    {
        // Fills some needed Lists with Values
        for (int i = 0; i < 5; i++)
        {
            _abilityBlocks.Add(transform.GetChild(i));
        }
        foreach (Transform t in _abilityBlocks)
        {
            _coolDownCircles.Add(t.GetChild(0).GetComponent<Image>());  
            _abilityImages.Add(t.GetComponent<Image>());
        }
        _abilityScripts = _controller.GetAbilityScripts();
    }

    // Update is called once per frame
    void Update()
    {
        bool anyAbilityInUse = false;
        // Loops through all abilities and updates the UI according to the status in the respective scripts
        for (int i = 0; i < _abilityScripts.Count; i++)
        {
            if (_abilityScripts[i].IsRunning()) 
            {
                AbilityBeingUsed(i);
                anyAbilityInUse = true;
            }
            
            float fillPercentage = Mathf.Min(1, ((Time.time - _abilityScripts[i].GetStartingTime()) / _abilityScripts[i].Cooldown));
            if (Time.time < _abilityScripts[i].Cooldown) fillPercentage = 1;
            SetAbilityClockToPercent(i, fillPercentage);
        }

        // Resets all abilities if none is being used
        if (!anyAbilityInUse)
        {
            for (int i = 0; i < 5; i++)
            {
                DisableAbility(i);
            }
        }
    }

    /// <summary>
    /// Set the specified timing circle to a specified percentage
    /// </summary>
    /// <param name="index"></param>
    /// <param name="percentage"></param>
    public void SetAbilityClockToPercent(int index, float percentage)
    {
        _coolDownCircles[index].fillAmount = percentage;
    }

    /// <summary>
    /// Sets the ability UI to display that the ability is currently being used and every other ability to display that it is not being used
    /// </summary>
    /// <param name="index"></param>
    public void AbilityBeingUsed(int index)
    {
        for (int i = 0; i < _abilityBlocks.Count; i++)
        {
            if (i == index)
            {
                EnableAbility(i);
            }
            else 
            {
                DisableAbility(i);
            }
        }
    }

    private void EnableAbility(int index)
    {
        _abilityImages[index].color = Color.white;
    }

    private void DisableAbility(int index)
    {
        _abilityImages[index].color = Color.gray;
    }
}
