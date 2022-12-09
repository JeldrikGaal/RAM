using System;
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

    private Transform _basicAbilityBlock;
    private Image _basicAbilityImage;
    private Image _basicAbilityCoolDownCircle;

    [SerializeField] private Image _dashCoolDownCircle;

    private List<float> _chargeInfo;

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
            _coolDownCircles.Add(t.GetComponent<Image>());  
            _abilityImages.Add(t.GetChild(0).GetComponent<Image>());
        }
        _abilityScripts = _controller.GetAbilityScripts();

        _basicAbilityBlock = transform.GetChild(5);
        _basicAbilityImage = _basicAbilityBlock.GetComponent<Image>();
        _basicAbilityCoolDownCircle = _basicAbilityBlock.GetChild(0).GetComponent<Image>();


    }

    // Update is called once per frame
    void Update()
    {
        bool anyAbilityInUse = false;
        float fillPercentage;
        // Loops through all abilities and updates the UI according to the status in the respective scripts
        for (int i = 0; i < _abilityScripts.Count; i++)
        {
            if (_abilityScripts[i].IsRunning()) 
            {
                AbilityBeingUsed(i);
                anyAbilityInUse = true;
            }

            fillPercentage = Mathf.Min(1, ((Time.time - _abilityScripts[i].GetStartingTime()) / _abilityScripts[i].Stats.Cooldown));
            //if (Time.time < _abilityScripts[i].Stats.Cooldown) fillPercentage = 1;
            if (_abilityScripts[i].GetStartingTime() == 0) fillPercentage = 1;
            SetAbilityClockToPercent(i, fillPercentage);
        }

        
      
        // Visualize Charge Cooldown
        fillPercentage = Mathf.Min(1, (Time.time - _controller.GetChargeStartTime()) / _controller.GetChargeCoolDown());
        if (_controller.GetChargeStartTime() == 0) fillPercentage = 1;
        _basicAbilityImage.fillAmount = fillPercentage;

        // Visualize Dash Cooldown
        fillPercentage = Mathf.Min(1, (Time.time - _controller.GetDashStartTime()) / _controller.GetDashCoolDown());
        if (_controller.GetDashStartTime() == 0) fillPercentage = 1;
        _dashCoolDownCircle.fillAmount = fillPercentage;

        /* // Read Input about charging from RammyController Script
        _chargeInfo = _controller.GetChargeInfo();
        if (_chargeInfo[0] > 0)
        {
            DisplayCharging();
            
        }
        else if (!_controller.GetDashing())
        {
            ResetDisplayCharging();
        } 
        
        // Display Dashing
        if (_controller.GetDashing())
        {
            _basicAbilityImage.color = Color.green;
        } */

        // Resets all abilities if none is being used
        if (!anyAbilityInUse)
        {
            for (int i = 0; i < 5; i++)
            {
                DisableAbility(i);
            }
        }

        for (int i = 0; i < _abilityImages.Count; i++)
        {
            if (!_controller.GetAbilitiesLearned()[i])
            {
                _abilityImages[i].gameObject.SetActive(false);

            }
            else
            {
                _abilityImages[i].gameObject.SetActive(true);
            }
        }


    }

    /// <summary>
    /// Displaying that the player is currently charging up his chargeattack and how far the charging progess has gone so far
    /// </summary>
    public void DisplayCharging()
    {
        _basicAbilityImage.color = Color.red;
        _basicAbilityCoolDownCircle.fillAmount = Mathf.Min(1, (_chargeInfo[0] / _chargeInfo[2]));

    }
    public void ResetDisplayCharging()
    {
        _basicAbilityImage.color = Color.white;
        _basicAbilityCoolDownCircle.fillAmount = 1f;
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
