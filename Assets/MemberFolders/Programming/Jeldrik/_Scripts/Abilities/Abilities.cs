using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Abilities : MonoBehaviour
{
    public float Cooldown;
    public float Duration;

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
            if (Time.time - _startingTime > Duration)
            {
                _started = false;
                _controller.EndUsingAbility();
            }
            
        }
    }

    // Checks if the respective Ability can be activated and activates in in that case
    public bool CheckActivate()
    {
        if (!_started)
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
        return Time.time - _startingTime > Cooldown;
    }
    
}
