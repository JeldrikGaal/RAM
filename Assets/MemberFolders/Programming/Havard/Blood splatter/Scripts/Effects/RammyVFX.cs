//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RammyVFX : MonoBehaviour
{
    [SerializeField] private GameObject _bloodBomb;
    [SerializeField] private GameObject _bloodSpreadCalculator;
    [SerializeField] private BloodySteps _stepScript;

    [Header("Ram attack")]
    [Range(0.0f, 2.0f)] [SerializeField] private float _bloodSpread = 0.5f;
    [Range(-90f, 90f)] public float _heightAngle = 1;
    [Range(0f, 10.0f)] [SerializeField] private float _bloodForceMin;
    [Range(0f, 10.0f)] [SerializeField] private float _bloodForceMax;
    [Range(0, 15)] public int _bloodAmount = 5;
    [Range(0.1f, 2)] public float _bloodSizeMin = 1;
    [Range(0.1f, 2)] public float _bloodSizeMax = 1;

    [Header("Normal attack")]
    [Range(0.0f, 2.0f)] [SerializeField] private float _bloodSpreadNormal = 0.5f;
    [Range(-90f, 90f)] public float _heightAngleNormal = 1;
    [Range(0f, 10.0f)] [SerializeField] private float _bloodForceMinNormal;
    [Range(0f, 10.0f)] [SerializeField] private float _bloodForceMaxNormal;
    [Range(0, 15)] public int _bloodAmountNormal = 5;
    [Range(0.1f, 2)] public float _bloodSizeMinNormal = 1;
    [Range(0.1f, 2)] public float _bloodSizeMaxNormal = 1;

    public void RamAttack(GameObject enemy)
    {
        SpawnBlood(_bloodSizeMin, _bloodSizeMax, _bloodSpread, _bloodAmount, _bloodForceMin, _bloodForceMax, enemy);
    }
    public void NormalAttack(GameObject enemy)
    {
        SpawnBlood(_bloodSizeMin, _bloodSizeMax, _bloodSpread, _bloodAmount, _bloodForceMin, _bloodForceMax, enemy);
    }

    private void SpawnBlood(float bloodSizeMin, float bloodSizeMax, float bloodSpread, float bloodAmount, float bloodForceMin, float bloodForceMax, GameObject rammedObject)
    {
        var _bloodPrefab = Instantiate(_bloodBomb, rammedObject.transform.position, Quaternion.Euler(new Vector3(0, 0, 0)));
        _bloodPrefab.transform.localScale *= Random.Range(bloodSizeMin, bloodSizeMax);


        // Vector3 _enemyDirection = rammedObject.transform.position - this.transform.position;
        _bloodSpreadCalculator.transform.localRotation = Quaternion.Euler(_heightAngle, 0, 0);

        _bloodSpreadCalculator.transform.GetChild(0).transform.localScale = new Vector3(bloodSpread, 1, 1);

        var _bloodDir1 = _bloodSpreadCalculator.transform.GetChild(0).transform.GetChild(0).transform.position - _bloodSpreadCalculator.transform.position;
        var _bloodDir2 = _bloodSpreadCalculator.transform.GetChild(0).transform.GetChild(1).transform.position - _bloodSpreadCalculator.transform.position;

        var i = 0;
        foreach (Transform child in _bloodPrefab.transform)
        {
            if (i >= bloodAmount)
            {
                Destroy(child.gameObject);
            }
            i++;
            child.GetComponent<StickyBlood>().BloodStepScript = _stepScript;
            child.GetComponent<StickyBlood>().BloodSize = Random.Range(bloodSizeMin, bloodSizeMax);
            child.GetComponent<InitVelocity>().CalcDirLeft = _bloodDir1;
            child.GetComponent<InitVelocity>().CalcDirRight = _bloodDir2;
            child.GetComponent<InitVelocity>().BloodForceMin = bloodForceMin;
            child.GetComponent<InitVelocity>().BloodForceMax = bloodForceMax;
        }
    }
}
