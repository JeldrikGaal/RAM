using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObjectPoolManager))]
public class FloatingDamageManager : MonoBehaviour
{

    [SerializeField] private FDOProperties _instanceProperties;
    static FDOProperties fDOProperties;
    private void Awake()
    {
        fDOProperties = _instanceProperties;
    }
    public static void DisplayDamage(float damage, Vector3 worldPosition)
    {

        fDOProperties.ToBeDisplayed = damage.ToString();
        ObjectPoolManager.RequestObject(typeof(FloatingDamageObject), worldPosition, Vector3.zero, Vector3.up, fDOProperties.FadeTime, fDOProperties);
    }
}








