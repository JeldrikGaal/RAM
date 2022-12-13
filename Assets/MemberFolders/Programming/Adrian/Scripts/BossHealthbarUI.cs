using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthbarUI : MonoBehaviour
{
    [SerializeField] private EnemyController _controller;
    [SerializeField] private Image _healthBarImage;

    void Update()
    {
        _healthBarImage.fillAmount = _controller.Health / _controller.Stats.GetHealth(4);
    }
}
