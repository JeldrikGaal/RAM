using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarBig : MonoBehaviour
{
    [SerializeField] Image _healthBarImage;
    float _health = 1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Use this to increase or decrease the health bar in situations such as damage or health pick-up.
    /// </summary>
    /// <param name="changeAmount"></param>
    public void UpdateHealthBar(float changeAmount)
    {
        _health += changeAmount;
        _healthBarImage.fillAmount = _health;
    }
}
