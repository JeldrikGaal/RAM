//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class RammyVFX : MonoBehaviour
{
    [System.Serializable]
    public class GoreValues
    {
        public string GoreNames;
        [Range(0.0f, 2.0f)] public float Spread;
        [Range(0f, 90f)] public float Angle;
        [Range(0f, 10.0f)] public float MinForce;
        [Range(0f, 10.0f)] public float MaxForce;
        [Range(0, 10)] public int MinAmount;
        [Range(0, 10)] public int MaxAmount;
    }
    // Some essential things for the script:
    [SerializeField] private GameObject _bloodBomb;
    [SerializeField] private GameObject _bloodSpreadCalculator;
    [SerializeField] private BloodySteps _stepScript;
    [SerializeField] private TimeStopper _timeEffectScript;

    [Header("Gore prefabs")]
    [SerializeField] private GameObject _skullObject;
    [SerializeField] private GameObject _heartObject;
    [SerializeField] private GameObject _intestineObject;
    [SerializeField] private GameObject _spineObject;
    [SerializeField] private GameObject _brainObject;
    [SerializeField] private GameObject _eyeballPrefab;
    [SerializeField] private GameObject[] _meatPrefabs;


    // Graphics settings:
    [Header("Graphics settings")]
    public bool IsBlue = false;

    // These values are completely random, and helps variate the type of blood
    [Header("Blood variations")]
    [SerializeField] private Material[] _bloodVariations;
    [SerializeField] private Material[] _blueBloodVariations;

    #region blood settings

    // Here you can customize the values for every type of attack!
    [Header("Ram attack")]
    [Range(0.0f, 2.0f)] [SerializeField] private float _bloodSpread = 0.5f;
    [Range(0f, 90f)] public float _heightAngle = 20;
    [Range(0f, 10.0f)] [SerializeField] private float _bloodForceMin;
    [Range(0f, 10.0f)] [SerializeField] private float _bloodForceMax;
    [Range(0, 15)] public int _bloodAmount = 5;
    [Range(0.1f, 2)] public float _bloodSizeMin = 1;
    [Range(0.1f, 2)] public float _bloodSizeMax = 1;
    // Death gore variables:
    [SerializeField] private GoreValues[] _goreValues;

    [Header("Normal attack")]
    [Range(0.0f, 2.0f)] [SerializeField] private float _bloodSpreadNormal = 0.5f;
    [Range(0f, 90f)] public float _heightAngleNormal = 20;
    [Range(0f, 10.0f)] [SerializeField] private float _bloodForceMinNormal;
    [Range(0f, 10.0f)] [SerializeField] private float _bloodForceMaxNormal;
    [Range(0, 15)] public int _bloodAmountNormal = 5;
    [Range(0.1f, 2)] public float _bloodSizeMinNormal = 1;
    [Range(0.1f, 2)] public float _bloodSizeMaxNormal = 1;

    #endregion


    #region blood functions

    // These functions are called from the RammyController script, and invokes the main blood function with the values in this script, but with the enemy from the rammy script
    public void RamAttack(GameObject enemy)
    {
        SpawnBlood(_bloodSizeMin, _bloodSizeMax, _bloodSpread, _heightAngle, _bloodAmount, _bloodForceMin, _bloodForceMax, enemy);
        
        if(enemy.GetComponent<EnemyTesting>()._health <= 0)
        {
            print("gore");
        }
    }
    public void NormalAttack(GameObject enemy)
    {
        SpawnBlood(_bloodSizeMinNormal, _bloodSizeMaxNormal, _bloodSpreadNormal, _heightAngleNormal, _bloodAmountNormal, _bloodForceMinNormal, _bloodForceMaxNormal, enemy);
    }

    #endregion

    // This big function essentially sets up everything we need to make blood!
    private void SpawnBlood(float bloodSizeMin, float bloodSizeMax, float bloodSpread, float angle, float bloodAmount, float bloodForceMin, float bloodForceMax, GameObject rammedObject)
    {
        // Here we instantiate the bloodprojectiles. The way I have it set up, lets it use one prefab with 15 projectiles inside it.
        var _bloodPrefab = Instantiate(_bloodBomb, rammedObject.transform.position, Quaternion.Euler(new Vector3(0, 0, 0)));
        
        // Here we're inserting some settings to change and calculate the direction the blood should move in.
        _bloodPrefab.transform.localScale *= Random.Range(bloodSizeMin, bloodSizeMax);
        angle = -angle;
        _bloodSpreadCalculator.transform.localRotation = Quaternion.Euler(angle, 0, 0);
        _bloodSpreadCalculator.transform.GetChild(0).transform.localScale = new Vector3(bloodSpread, 1, 1);
        var _bloodDir1 = _bloodSpreadCalculator.transform.GetChild(0).transform.GetChild(0).transform.position - _bloodSpreadCalculator.transform.position;
        var _bloodDir2 = _bloodSpreadCalculator.transform.GetChild(0).transform.GetChild(1).transform.position - _bloodSpreadCalculator.transform.position;

        // This foreach lets us access all projectiles inside the prefab.
        var i = 0;
        foreach (Transform child in _bloodPrefab.transform)
        {
            int randomMaterialNum = Random.Range(0, _bloodVariations.Length);

            // If we want less than 15, it deletes the rest! Probably not the best to spawn and delete the ones we don't need, but hey, that's bad programming for you.
            if (i >= bloodAmount)
            {
                Destroy(child.gameObject);
            }
            i++;

            // Here we're accessing the projectile scripts to change the final splat's settings and the projectile direction and force.
            child.GetComponent<StickyBlood>().BloodStepScript = _stepScript;
            child.GetComponent<StickyBlood>().BloodSize = Random.Range(bloodSizeMin, bloodSizeMax);
            child.GetComponent<InitVelocity>().CalcDirLeft = _bloodDir1;
            child.GetComponent<InitVelocity>().CalcDirRight = _bloodDir2;
            child.GetComponent<InitVelocity>().BloodForceMin = bloodForceMin;
            child.GetComponent<InitVelocity>().BloodForceMax = bloodForceMax;
            // Here we just check if it's supposed to be blue, and assign the correct materials
            if (!IsBlue)
            {
                child.GetComponent<StickyBlood>().BloodMaterial = _bloodVariations[randomMaterialNum];
            }
            else if (IsBlue)
            {
                child.GetComponent<StickyBlood>().BloodMaterial = _blueBloodVariations[randomMaterialNum];
            }
        }
    }
}
