using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Abilities : MonoBehaviour
{
    public RammyAttack Stats;
    protected bool _upgraded;

    private float _startingTime;
    private bool _started;

    public RammyController _controller;

    public virtual void Start()
    {
        _controller = GetComponent<RammyController>();
    }

    public virtual void Update()
    {
        // Resetting variables when the duration of the ability has passed after activation
        if (_started)
        {
            if (Time.time - _startingTime > (_upgraded ? Stats.UAttackTime : Stats.AttackTime))
            {
                _started = false;
                _controller.EndUsingAbility();
            }
            
        }
    }

    // Checks if the respective Ability can be activated and activates in in that case
    public bool CheckActivate()
    {
        if (!_started && IsReady())
        {
            _started = true;
            _startingTime = Time.time;
            _controller.StartUsingAbility();
            Activate();
            return true;
        }
        return false;
    }
    // Abstract function for the actual implementation of the abilities
    abstract public void Activate();

    // Checking if the ability is ready to be used again after the cooldown period
    public bool IsReady()
    {
        if (_startingTime == 0) return true;
        return Time.time - _startingTime > Stats.Cooldown;
    }

    public bool IsRunning()
    {
        return _started;
    }

    public float GetStartingTime()
    {
        return _startingTime;
    }
    
}
