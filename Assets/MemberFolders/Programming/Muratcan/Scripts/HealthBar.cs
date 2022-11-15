using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    Transform _cameraTransform;
    GameObject _parentCharacter;
    [SerializeField] Image _healthBarImage;
    float _health = 1f;
    [SerializeField] Color32 _playerHealthColor;
    [SerializeField] Color32 _enemyHealthColor;
    // Start is called before the first frame update
    void Start()
    {
        _cameraTransform = Camera.main.transform;
        _parentCharacter = transform.parent.gameObject;

        if (_parentCharacter.CompareTag("Player"))
        {
            _healthBarImage.color = _playerHealthColor;
        }
        else
        {
            _healthBarImage.color = _enemyHealthColor;
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        LookAtCamera();
        UpdateHealthBar(0f);
    }

    void LookAtCamera()
    {
        transform.LookAt(_cameraTransform.position + transform.position);
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
