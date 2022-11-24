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
    [SerializeField] private GameObject _eyeballObject;
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
    [SerializeField] private GoreValues[] _goreValuesRam;

    [Header("Normal attack")]
    [Range(0.0f, 2.0f)] [SerializeField] private float _bloodSpreadNormal = 0.5f;
    [Range(0f, 90f)] public float _heightAngleNormal = 20;
    [Range(0f, 10.0f)] [SerializeField] private float _bloodForceMinNormal;
    [Range(0f, 10.0f)] [SerializeField] private float _bloodForceMaxNormal;
    [Range(0, 15)] public int _bloodAmountNormal = 5;
    [Range(0.1f, 2)] public float _bloodSizeMinNormal = 1;
    [Range(0.1f, 2)] public float _bloodSizeMaxNormal = 1;

    [Header("Stomp attack")]
    [Range(0.0f, 2.0f)] [SerializeField] private float _bloodSpreadAb1 = 0.5f;
    [Range(0f, 90f)] public float _heightAngleAb1 = 20;
    [Range(0f, 10.0f)] [SerializeField] private float _bloodForceMinAb1;
    [Range(0f, 10.0f)] [SerializeField] private float _bloodForceMaxAb1;
    [Range(0, 15)] public int _bloodAmountAb1 = 5;
    [Range(0.1f, 2)] public float _bloodSizeMinAb1 = 1;
    [Range(0.1f, 2)] public float _bloodSizeMaxAb1 = 1;
    [SerializeField] private GoreValues[] _goreValuesAb1;


    [Header("Pull attack")]
    [Range(0.0f, 2.0f)] [SerializeField] private float _bloodSpreadAb3 = 0.5f;
    [Range(0f, 90f)] public float _heightAngleAb3 = 20;
    [Range(0f, 10.0f)] [SerializeField] private float _bloodForceMinAb3;
    [Range(0f, 10.0f)] [SerializeField] private float _bloodForceMaxAb3;
    [Range(0, 15)] public int _bloodAmountAb3 = 5;
    [Range(0.1f, 2)] public float _bloodSizeMinAb3 = 1;
    [Range(0.1f, 2)] public float _bloodSizeMaxAb3 = 1;
    [SerializeField] private GoreValues[] _goreValuesAb3;

    #endregion


    #region blood functions

    // These functions are called from the RammyController script, and invokes the main blood function with the values in this script, but with the enemy from the rammy script
    public void RamAttack(GameObject enemy)
    {
        SpawnBlood(_bloodSizeMin, _bloodSizeMax, _bloodSpread, _heightAngle, _bloodAmount, _bloodForceMin, _bloodForceMax, enemy);
        
        if(enemy.GetComponent<EnemyTesting>()._health <= 0)
        {
            SpawnGore(_goreValuesRam[0], _skullObject, enemy);
            SpawnGore(_goreValuesRam[1], _heartObject, enemy);
            SpawnGore(_goreValuesRam[2], _intestineObject, enemy);
            SpawnGore(_goreValuesRam[3], _spineObject, enemy);
            SpawnGore(_goreValuesRam[4], _brainObject, enemy);
            SpawnGore(_goreValuesRam[5], _eyeballObject, enemy);
            SpawnGore(_goreValuesRam[6], _meatPrefabs[0], enemy);
        }
    }
    public void NormalAttack(GameObject enemy)
    {
        SpawnBlood(_bloodSizeMinNormal, _bloodSizeMaxNormal, _bloodSpreadNormal, _heightAngleNormal, _bloodAmountNormal, _bloodForceMinNormal, _bloodForceMaxNormal, enemy);
    }

    public void Ab1Attack(GameObject enemy)
    {

        var dir = (enemy.transform.position - transform.position).normalized;

        SpawnBlood(_bloodSizeMinAb1, _bloodSizeMaxAb1, _bloodSpreadAb1, _heightAngleAb1, _bloodAmountAb1, _bloodForceMinAb1, _bloodForceMaxAb1, enemy, dir);
    }

    public void Ab3Attack(GameObject enemy, Vector3 point)
    {

        var dir = (point - enemy.transform.position).normalized;

        SpawnBlood(_bloodSizeMinAb3, _bloodSizeMaxAb3, _bloodSpreadAb3, _heightAngleAb3, _bloodAmountAb3, _bloodForceMinAb3, _bloodForceMaxAb3, enemy, dir);
    }

    #endregion

    // This big function essentially sets up everything we need to make blood!
    private void SpawnBlood(float bloodSizeMin, float bloodSizeMax, float bloodSpread, float angle, float bloodAmount, float bloodForceMin, float bloodForceMax, GameObject rammedObject, Vector3 direction = default(Vector3))
    {
        // Here we instantiate the bloodprojectiles. The way I have it set up, lets it use one prefab with 15 projectiles inside it.
        var _bloodPrefab = Instantiate(_bloodBomb, rammedObject.transform.position, Quaternion.Euler(new Vector3(0, 0, 0)));
        _bloodPrefab.transform.localScale *= Random.Range(bloodSizeMin, bloodSizeMax);


        // Here we're inserting some settings to change and calculate the direction the blood should move in.
        Vector3 bloodDir1;
        Vector3 bloodDir2;
        CalculateDirections(out bloodDir1, out bloodDir2, direction, angle, bloodSpread);

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
            child.GetComponent<InitVelocity>().CalcDirLeft = bloodDir1;
            child.GetComponent<InitVelocity>().CalcDirRight = bloodDir2;
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

    private void SpawnGore(GoreValues goreSettings, GameObject spawnObject, GameObject enemy, Vector3 direction = default(Vector3))
    {
        var amountOfGore = Random.Range(goreSettings.MinAmount, goreSettings.MaxAmount+1);
        for (int i = 0; i < amountOfGore; i++)
        {
            var gorePiece = Instantiate(spawnObject, enemy.transform.position, Quaternion.Euler(0,0,0));
            var gorePieceVel = gorePiece.AddComponent<InitVelocity>();

            Vector3 bloodDir1;
            Vector3 bloodDir2;
            CalculateDirections(out bloodDir1, out bloodDir2, direction, goreSettings.Angle, goreSettings.Spread);
            gorePieceVel.BloodForceMin = goreSettings.MinForce;
            gorePieceVel.BloodForceMax = goreSettings.MaxForce;
            gorePieceVel.CalcDirLeft = bloodDir1;
            gorePieceVel.CalcDirRight = bloodDir2;
        }
    }

    // Function used everywhere in this script to get two rotations from one direction, an angle and a width
    private void CalculateDirections(out Vector3 leftDirection, out Vector3 rightDirection, Vector3 inDirection, float angle, float spread)
    {

        if(inDirection == new Vector3(0, 0, 0))
        {
            angle = -angle;
            _bloodSpreadCalculator.transform.localRotation = Quaternion.Euler(angle, 0, 0);
        } else
        {
            var lookDir = inDirection;
            angle = -map(angle, 90, 0, 0, 90);
            lookDir.y = angle;
            _bloodSpreadCalculator.transform.rotation = Quaternion.LookRotation(lookDir);
        }

        _bloodSpreadCalculator.transform.GetChild(0).transform.localScale = new Vector3(spread, 1, 1);
        leftDirection = _bloodSpreadCalculator.transform.GetChild(0).transform.GetChild(0).transform.position - _bloodSpreadCalculator.transform.position;
        rightDirection = _bloodSpreadCalculator.transform.GetChild(0).transform.GetChild(1).transform.position - _bloodSpreadCalculator.transform.position;
    }

    float map(float s, float a1, float a2, float b1, float b2)
    {
        return b1 + (s - a1) * (b2 - b1) / (a2 - a1);
    }

}
