using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObjectPoolManager))]
public class FloatingDamageManager : MonoBehaviour
{

    [SerializeField] private FDOProperties _instanceProperties;
    static FDOProperties _fDOProperties;
    private void Awake()
    {
        if (_fDOProperties != null)
        {
            Destroy(this);
        }
        else
        {
            _fDOProperties = _instanceProperties;
        }
        
    }
    /// <summary>
    /// Spawns text that display the amount of damage, in the spesified location.
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="worldPosition"></param>
    public static void DisplayDamage(float damage, Vector3 worldPosition)
    {

        _fDOProperties.ToBeDisplayed = damage.ToString();
        ObjectPoolManager.RequestObject(typeof(FloatingDamageObject), worldPosition, Vector3.zero, Vector3.forward + Vector3.up, _fDOProperties.FadeTime, _fDOProperties);
    }
}








