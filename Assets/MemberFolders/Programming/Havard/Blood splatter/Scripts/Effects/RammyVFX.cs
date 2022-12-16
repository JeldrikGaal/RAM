//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Sirenix.OdinInspector;

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
    [FoldoutGroup("Essentials")][SerializeField] private GameObject _bloodBomb;
    [FoldoutGroup("Essentials")][SerializeField] private GameObject _bloodSpreadCalculator;
    [FoldoutGroup("Essentials")][SerializeField] private BloodySteps _stepScript;
    [FoldoutGroup("Essentials")][SerializeField] private TimeStopper _timeEffectScript;
    [FoldoutGroup("Essentials")][SerializeField] private GameObject _gorePrefab;
    [FoldoutGroup("Essentials")][SerializeField] private GameObject _bloodParticle;
    [FoldoutGroup("Essentials")][SerializeField] private DoubleArrayPooling _goreSmudgeArrayPool;
    [FoldoutGroup("Essentials")][SerializeField] private DoubleArrayPooling _goreArrayPool;
    [FoldoutGroup("Essentials")][SerializeField] private float _spawnHeightOffset = 0.5f;
    [FoldoutGroup("Essentials")][SerializeField] private GameObject _nonDirectionBlood;

    //[Header("Gore prefabs")]
    [FoldoutGroup("Gore Prefabs")][SerializeField] private GameObject _skullObject;
    [FoldoutGroup("Gore Prefabs")][SerializeField] private GameObject _heartObject;
    [FoldoutGroup("Gore Prefabs")][SerializeField] private GameObject _intestineObject;
    [FoldoutGroup("Gore Prefabs")][SerializeField] private GameObject _spineObject;
    [FoldoutGroup("Gore Prefabs")][SerializeField] private GameObject _brainObject;
    [FoldoutGroup("Gore Prefabs")][SerializeField] private GameObject _eyeballObject;
    [FoldoutGroup("Gore Prefabs")][SerializeField] private GameObject[] _meatPrefabs;

    //[Header("Gore Array Pool Scripts")]
    [FoldoutGroup("Gore Array Pools")][SerializeField] private DoubleArrayPooling _skullArray;
    [FoldoutGroup("Gore Array Pools")][SerializeField] private DoubleArrayPooling _heartArray;
    [FoldoutGroup("Gore Array Pools")][SerializeField] private DoubleArrayPooling _intestineArray;
    [FoldoutGroup("Gore Array Pools")][SerializeField] private DoubleArrayPooling _spineArray;
    [FoldoutGroup("Gore Array Pools")][SerializeField] private DoubleArrayPooling _brainArray;
    [FoldoutGroup("Gore Array Pools")][SerializeField] private DoubleArrayPooling _eyeballArray;
    [FoldoutGroup("Gore Array Pools")][SerializeField] private DoubleArrayPooling _meatArray;

    //[Header("Max Gore Items")]
    [FoldoutGroup("Max Gore Items")][SerializeField] private Vector2 _skullArrayAmount;
    [FoldoutGroup("Max Gore Items")][SerializeField] private Vector2 _heartArrayAmount;
    [FoldoutGroup("Max Gore Items")][SerializeField] private Vector2 _intestineArrayAmount;
    [FoldoutGroup("Max Gore Items")][SerializeField] private Vector2 _spineArrayAmount;
    [FoldoutGroup("Max Gore Items")][SerializeField] private Vector2 _brainArrayAmount;
    [FoldoutGroup("Max Gore Items")][SerializeField] private Vector2 _eyeballArrayAmount;
    [FoldoutGroup("Max Gore Items")][SerializeField] private Vector2 _meatArrayAmount;

    // Graphics settings:
    [Header("Graphics settings")]
    public bool IsBlue = false;

    // These values are completely random, and helps variate the type of blood
    [Header("Blood variations")]
    [SerializeField] private Material[] _bloodVariations;
    [SerializeField] private Material[] _blueBloodVariations;
    [SerializeField] private Material[] _bloodProjectileVariations;
    [SerializeField] private Material[] _blueBloodProjectileVariations;

    #region blood and gore settings for each ability

    // Here you can customize the values for every type of attack!
    //[Header("Ram attack")]
    [TabGroup("Ram")][Range(0.0f, 2.0f)][SerializeField] private float _bloodSpread = 0.5f;
    [TabGroup("Ram")][Range(0f, 90f)] public float _heightAngle = 20;
    [TabGroup("Ram")][Range(0f, 10.0f)][SerializeField] private float _bloodForceMin;
    [TabGroup("Ram")][Range(0f, 10.0f)][SerializeField] private float _bloodForceMax;
    [TabGroup("Ram")][Range(0, 15)] public int _bloodAmount = 5;
    [TabGroup("Ram")][Range(0.1f, 2)] public float _bloodSizeMin = 1;
    [TabGroup("Ram")][Range(0.1f, 2)] public float _bloodSizeMax = 1;
    // Death gore variables:
    [TabGroup("Ram")][SerializeField] private GoreValues[] _goreValuesRam;

    //[Header("Normal attack")]
    [TabGroup("Normal")][Range(0.0f, 2.0f)][SerializeField] private float _bloodSpreadNormal = 0.5f;
    [TabGroup("Normal")][Range(0f, 90f)] public float _heightAngleNormal = 20;
    [TabGroup("Normal")][Range(0f, 10.0f)][SerializeField] private float _bloodForceMinNormal;
    [TabGroup("Normal")][Range(0f, 10.0f)][SerializeField] private float _bloodForceMaxNormal;
    [TabGroup("Normal")][Range(0, 15)] public int _bloodAmountNormal = 5;
    [TabGroup("Normal")][Range(0.1f, 2)] public float _bloodSizeMinNormal = 1;
    [TabGroup("Normal")][Range(0.1f, 2)] public float _bloodSizeMaxNormal = 1;

    //[Header("Stomp attack")]
    [TabGroup("Stomp")][Range(0.0f, 2.0f)][SerializeField] private float _bloodSpreadAb1 = 0.5f;
    [TabGroup("Stomp")][Range(0f, 90f)] public float _heightAngleAb1 = 20;
    [TabGroup("Stomp")][Range(0f, 10.0f)][SerializeField] private float _bloodForceMinAb1;
    [TabGroup("Stomp")][Range(0f, 10.0f)][SerializeField] private float _bloodForceMaxAb1;
    [TabGroup("Stomp")][Range(0, 15)] public int _bloodAmountAb1 = 5;
    [TabGroup("Stomp")][Range(0.1f, 2)] public float _bloodSizeMinAb1 = 1;
    [TabGroup("Stomp")][Range(0.1f, 2)] public float _bloodSizeMaxAb1 = 1;
    [TabGroup("Stomp")][SerializeField] private GoreValues[] _goreValuesAb1;

    //[Header("Spin attack")]
    [TabGroup("Spin")][Range(0.0f, 2.0f)][SerializeField] private float _bloodSpreadAb2 = 0.5f;
    [TabGroup("Spin")][Range(0f, 90f)] public float _heightAngleAb2 = 20;
    [TabGroup("Spin")][Range(0f, 10.0f)][SerializeField] private float _bloodForceMinAb2;
    [TabGroup("Spin")][Range(0f, 10.0f)][SerializeField] private float _bloodForceMaxAb2;
    [TabGroup("Spin")][Range(0, 15)] public int _bloodAmountAb2 = 5;
    [TabGroup("Spin")][Range(0.1f, 2)] public float _bloodSizeMinAb2 = 1;
    [TabGroup("Spin")][Range(0.1f, 2)] public float _bloodSizeMaxAb2 = 1;
    [TabGroup("Spin")][SerializeField] private GoreValues[] _goreValuesAb2;

    //[Header("Pull attack")]
    [TabGroup("Pull")][Range(0.0f, 2.0f)][SerializeField] private float _bloodSpreadAb3 = 0.5f;
    [TabGroup("Pull")][Range(0f, 90f)] public float _heightAngleAb3 = 20;
    [TabGroup("Pull")][Range(0f, 10.0f)][SerializeField] private float _bloodForceMinAb3;
    [TabGroup("Pull")][Range(0f, 10.0f)][SerializeField] private float _bloodForceMaxAb3;
    [TabGroup("Pull")][Range(0, 15)] public int _bloodAmountAb3 = 5;
    [TabGroup("Pull")][Range(0.1f, 2)] public float _bloodSizeMinAb3 = 1;
    [TabGroup("Pull")][Range(0.1f, 2)] public float _bloodSizeMaxAb3 = 1;
    [TabGroup("Pull")][SerializeField] private GoreValues[] _goreValuesAb3;

    //[Header("Sweep attack")]
    [TabGroup("Sweep")][Range(0.0f, 2.0f)][SerializeField] private float _bloodSpreadAb4 = 0.5f;
    [TabGroup("Sweep")][Range(0f, 90f)] public float _heightAngleAb4 = 20;
    [TabGroup("Sweep")][Range(0f, 10.0f)][SerializeField] private float _bloodForceMinAb4;
    [TabGroup("Sweep")][Range(0f, 10.0f)][SerializeField] private float _bloodForceMaxAb4;
    [TabGroup("Sweep")][Range(0, 15)] public int _bloodAmountAb4 = 5;
    [TabGroup("Sweep")][Range(0.1f, 2)] public float _bloodSizeMinAb4 = 1;
    [TabGroup("Sweep")][Range(0.1f, 2)] public float _bloodSizeMaxAb4 = 1;
    [TabGroup("Sweep")][SerializeField] private GoreValues[] _goreValuesAb4;

    //[Header("Bodyslam attack")]
    [TabGroup("Bodyslam")][Range(0.0f, 2.0f)][SerializeField] private float _bloodSpreadAb5 = 0.5f;
    [TabGroup("Bodyslam")][Range(0f, 90f)] public float _heightAngleAb5 = 20;
    [TabGroup("Bodyslam")][Range(0f, 10.0f)][SerializeField] private float _bloodForceMinAb5;
    [TabGroup("Bodyslam")][Range(0f, 10.0f)][SerializeField] private float _bloodForceMaxAb5;
    [TabGroup("Bodyslam")][Range(0, 15)] public int _bloodAmountAb5 = 5;
    [TabGroup("Bodyslam")][Range(0.1f, 2)] public float _bloodSizeMinAb5 = 1;
    [TabGroup("Bodyslam")][Range(0.1f, 2)] public float _bloodSizeMaxAb5 = 1;
    [TabGroup("Bodyslam")][SerializeField] private GoreValues[] _goreValuesAb5;

    #endregion

    #region max amounts of each type of blood;

    [Header("Max amounts of gore and blood")]
    [SerializeField] private int _maxGoreSmudge;
    [SerializeField] private int _maxGoreSmudgeBackup;

    #endregion

    [Header("Honey Variables")]
    public bool InHoney = false;
    [SerializeField] private float _rammyHoneySpeedDif;
    private float _rammyOriginalSpeed;

    private void Start()
    {
        #region Setting all the double array scripts' value at the beginning:
        _goreSmudgeArrayPool.Array1 = new GameObject[_maxGoreSmudge];
        _goreSmudgeArrayPool.Array2 = new GameObject[_maxGoreSmudgeBackup];
        _goreSmudgeArrayPool.FullArray1 = false;
        _goreSmudgeArrayPool.FullArray2 = false;
        _goreSmudgeArrayPool.CurrentArray1 = 0;
        _goreSmudgeArrayPool.CurrentArray2 = 0;

        _skullArray.Array1 = new GameObject[(int)_skullArrayAmount.x];
        _skullArray.Array2 = new GameObject[(int)_skullArrayAmount.y];
        _skullArray.FullArray1 = false;
        _skullArray.FullArray2 = false;
        _skullArray.CurrentArray1 = 0;
        _skullArray.CurrentArray2 = 0;
        _heartArray.Array1 = new GameObject[(int)_heartArrayAmount.x];
        _heartArray.Array2 = new GameObject[(int)_heartArrayAmount.y];
        _heartArray.FullArray1 = false;
        _heartArray.FullArray2 = false;
        _heartArray.CurrentArray1 = 0;
        _heartArray.CurrentArray2 = 0;
        _intestineArray.Array1 = new GameObject[(int)_intestineArrayAmount.x];
        _intestineArray.Array2 = new GameObject[(int)_intestineArrayAmount.y];
        _intestineArray.FullArray1 = false;
        _intestineArray.FullArray2 = false;
        _intestineArray.CurrentArray1 = 0;
        _intestineArray.CurrentArray2 = 0;
        _spineArray.Array1 = new GameObject[(int)_spineArrayAmount.x];
        _spineArray.Array2 = new GameObject[(int)_spineArrayAmount.y];
        _spineArray.FullArray1 = false;
        _spineArray.FullArray2 = false;
        _spineArray.CurrentArray1 = 0;
        _spineArray.CurrentArray2 = 0;
        _brainArray.Array1 = new GameObject[(int)_brainArrayAmount.x];
        _brainArray.Array2 = new GameObject[(int)_brainArrayAmount.y];
        _brainArray.FullArray1 = false;
        _brainArray.FullArray2 = false;
        _brainArray.CurrentArray1 = 0;
        _brainArray.CurrentArray2 = 0;
        _eyeballArray.Array1 = new GameObject[(int)_eyeballArrayAmount.x];
        _eyeballArray.Array2 = new GameObject[(int)_eyeballArrayAmount.y];
        _eyeballArray.FullArray1 = false;
        _eyeballArray.FullArray2 = false;
        _eyeballArray.CurrentArray1 = 0;
        _eyeballArray.CurrentArray2 = 0;
        _meatArray.Array1 = new GameObject[(int)_meatArrayAmount.x];
        _meatArray.Array2 = new GameObject[(int)_meatArrayAmount.y];
        _meatArray.FullArray1 = false;
        _meatArray.FullArray2 = false;
        _meatArray.CurrentArray1 = 0;
        _meatArray.CurrentArray2 = 0;

        #endregion

        #region Rammy speed for honey
        _rammyOriginalSpeed = GetComponent<RammyController>().MovementSpeed;
        _rammyHoneySpeedDif = _rammyOriginalSpeed * _rammyHoneySpeedDif;
        #endregion
    }

    #region blood functions for each attack

    // These functions are called from the RammyController script, and invokes the main blood function with the values in this script, but with the enemy from the rammy script
    public void RamAttack(GameObject enemy)
    {
        SpawnBlood(_bloodSizeMin, _bloodSizeMax, _bloodSpread, _heightAngle, _bloodAmount, _bloodForceMin, _bloodForceMax, enemy);

        if (enemy.GetComponent<EnemyController>().Health <= 0)
        {
            SpawnGore(_goreValuesRam[0], _skullObject, enemy, _skullArray);
            SpawnGore(_goreValuesRam[1], _heartObject, enemy, _heartArray);
            SpawnGore(_goreValuesRam[2], _intestineObject, enemy, _intestineArray);
            SpawnGore(_goreValuesRam[3], _spineObject, enemy, _spineArray);
            SpawnGore(_goreValuesRam[4], _brainObject, enemy, _brainArray);
            SpawnGore(_goreValuesRam[5], _eyeballObject, enemy, _eyeballArray);
            SpawnGore(_goreValuesRam[6], _meatPrefabs[0], enemy, _meatArray);
        }

        // Intantiates the blood particle at the correct location and direction
        Instantiate(_bloodParticle, enemy.transform.position, this.transform.rotation);


    }
    public void NormalAttack(GameObject enemy)
    {
        SpawnBlood(_bloodSizeMinNormal, _bloodSizeMaxNormal, _bloodSpreadNormal, _heightAngleNormal, _bloodAmountNormal, _bloodForceMinNormal, _bloodForceMaxNormal, enemy);
    }

    public void Ab1Attack(GameObject enemy)
    {
        var dir = (enemy.transform.position - transform.position).normalized;

        SpawnBlood(_bloodSizeMinAb1, _bloodSizeMaxAb1, _bloodSpreadAb1, _heightAngleAb1, _bloodAmountAb1, _bloodForceMinAb1, _bloodForceMaxAb1, enemy, dir);

        if (enemy.GetComponent<EnemyController>().Health <= 0)
        {
            SpawnGore(_goreValuesAb1[0], _skullObject, enemy, _skullArray, dir);
            SpawnGore(_goreValuesAb1[1], _heartObject, enemy, _heartArray, dir);
            SpawnGore(_goreValuesAb1[2], _intestineObject, enemy, _intestineArray, dir);
            SpawnGore(_goreValuesAb1[3], _spineObject, enemy, _spineArray, dir);
            SpawnGore(_goreValuesAb1[4], _brainObject, enemy, _brainArray, dir);
            SpawnGore(_goreValuesAb1[5], _eyeballObject, enemy, _eyeballArray, dir);
            SpawnGore(_goreValuesAb1[6], _meatPrefabs[0], enemy, _meatArray, dir);
        }
    }

    public void Ab2Attack(GameObject enemy)
    {
        var dir = (enemy.transform.position - transform.position).normalized;

        SpawnBlood(_bloodSizeMinAb2, _bloodSizeMaxAb2, _bloodSpreadAb2, _heightAngleAb2, _bloodAmountAb2, _bloodForceMinAb2, _bloodForceMaxAb2, enemy, dir);

        if (enemy.GetComponent<EnemyController>().Health <= 0)
        {
            SpawnGore(_goreValuesAb2[0], _skullObject, enemy, _skullArray, dir);
            SpawnGore(_goreValuesAb2[1], _heartObject, enemy, _heartArray, dir);
            SpawnGore(_goreValuesAb2[2], _intestineObject, enemy, _intestineArray, dir);
            SpawnGore(_goreValuesAb2[3], _spineObject, enemy, _spineArray, dir);
            SpawnGore(_goreValuesAb2[4], _brainObject, enemy, _brainArray, dir);
            SpawnGore(_goreValuesAb2[5], _eyeballObject, enemy, _eyeballArray, dir);
            SpawnGore(_goreValuesAb2[6], _meatPrefabs[0], enemy, _meatArray, dir);
        }
    }

    public void Ab3Attack(GameObject enemy, Vector3 point)
    {
        var dir = (point - enemy.transform.position).normalized;

        SpawnBlood(_bloodSizeMinAb3, _bloodSizeMaxAb3, _bloodSpreadAb3, _heightAngleAb3, _bloodAmountAb3, _bloodForceMinAb3, _bloodForceMaxAb3, enemy, dir);

        if (enemy.GetComponent<EnemyController>().Health <= 0)
        {
            SpawnGore(_goreValuesAb3[0], _skullObject, enemy, _skullArray, dir);
            SpawnGore(_goreValuesAb3[1], _heartObject, enemy, _heartArray, dir);
            SpawnGore(_goreValuesAb3[2], _intestineObject, enemy, _intestineArray, dir);
            SpawnGore(_goreValuesAb3[3], _spineObject, enemy, _spineArray, dir);
            SpawnGore(_goreValuesAb3[4], _brainObject, enemy, _brainArray, dir);
            SpawnGore(_goreValuesAb3[5], _eyeballObject, enemy, _eyeballArray, dir);
            SpawnGore(_goreValuesAb3[6], _meatPrefabs[0], enemy, _meatArray, dir);
        }
    }

    public void Ab4Attack(GameObject enemy, Vector3 normal)
    {
        // var dir = (enemy.transform.position - transform.position).normalized;
        var dir = -normal;

        SpawnBlood(_bloodSizeMinAb4, _bloodSizeMaxAb4, _bloodSpreadAb4, _heightAngleAb4, _bloodAmountAb4, _bloodForceMinAb4, _bloodForceMaxAb4, enemy, dir);

        if (enemy.GetComponent<EnemyTesting>()._health <= 0)
        {
            SpawnGore(_goreValuesAb4[0], _skullObject, enemy, _skullArray, dir);
            SpawnGore(_goreValuesAb4[1], _heartObject, enemy, _heartArray, dir);
            SpawnGore(_goreValuesAb4[2], _intestineObject, enemy, _intestineArray, dir);
            SpawnGore(_goreValuesAb4[3], _spineObject, enemy, _spineArray, dir);
            SpawnGore(_goreValuesAb4[4], _brainObject, enemy, _brainArray, dir);
            SpawnGore(_goreValuesAb4[5], _eyeballObject, enemy, _eyeballArray, dir);
            SpawnGore(_goreValuesAb4[6], _meatPrefabs[0], enemy, _meatArray, dir);
        }
    }

    public void Ab5Attack(GameObject enemy, Vector3 rotation)
    {

        SpawnBlood(_bloodSizeMinAb5, _bloodSizeMaxAb5, _bloodSpreadAb5, _heightAngleAb5, _bloodAmountAb5, _bloodForceMinAb5, _bloodForceMaxAb5, enemy, rotation);

        if (enemy.GetComponent<EnemyController>().Health <= 0)
        {
            var dir = (enemy.transform.position - transform.position).normalized;

            SpawnGore(_goreValuesAb5[0], _skullObject, enemy, _skullArray, dir);
            SpawnGore(_goreValuesAb5[1], _heartObject, enemy, _heartArray, dir);
            SpawnGore(_goreValuesAb5[2], _intestineObject, enemy, _intestineArray, dir);
            SpawnGore(_goreValuesAb5[3], _spineObject, enemy, _spineArray, dir);
            SpawnGore(_goreValuesAb5[4], _brainObject, enemy, _brainArray, dir);
            SpawnGore(_goreValuesAb5[5], _eyeballObject, enemy, _eyeballArray, dir);
            SpawnGore(_goreValuesAb5[6], _meatPrefabs[0], enemy, _meatArray, dir);
        }

    }

    #endregion

    // This big function essentially sets up everything we need to make blood!
    private void SpawnBlood(float bloodSizeMin, float bloodSizeMax, float bloodSpread, float angle, float bloodAmount, float bloodForceMin, float bloodForceMax, GameObject rammedObject, Vector3 direction = default(Vector3))
    {
        RaycastHit hit;
        var layer = 1 << 10;
        if (Physics.Raycast(transform.position + new Vector3(0, 100, 0), -Vector3.up, out hit, Mathf.Infinity, layer))
        {
            var initialBloodEffect = Instantiate(_nonDirectionBlood, rammedObject.transform.position, Quaternion.Euler(90, 0, Random.Range(0,360)));
            Destroy(initialBloodEffect, 200f);
        }

        // Here we instantiate the bloodprojectiles. The way I have it set up, lets it use one prefab with 15 projectiles inside it.
        for (int i = 0; i < bloodAmount; i++)
        {
            var _bloodPrefab = Instantiate(_bloodBomb, rammedObject.transform.position += _spawnHeightOffset * Vector3.up, Quaternion.Euler(new Vector3(0, 0, 0)));
            _bloodPrefab.transform.localScale *= Random.Range(bloodSizeMin, bloodSizeMax);
            if (!IsBlue)
            {
                _bloodPrefab.transform.GetChild(0).GetComponent<Renderer>().material = _bloodProjectileVariations[Random.Range(0, _bloodProjectileVariations.Length)];
            } else if (IsBlue)
            {
                _bloodPrefab.transform.GetChild(0).GetComponent<Renderer>().material = _blueBloodProjectileVariations[Random.Range(0, _blueBloodProjectileVariations.Length)];
            }


            // Here we're inserting some settings to change and calculate the direction the blood should move in.
            Vector3 bloodDir1;
            Vector3 bloodDir2;
            CalculateDirections(out bloodDir1, out bloodDir2, direction, angle, bloodSpread);

            //// This foreach lets us access all projectiles inside the prefab.
            //var i = 0;
            //foreach (Transform child in _bloodPrefab.transform)
            //{
            int randomMaterialNum = Random.Range(0, _bloodVariations.Length);

            //    // If we want less than 15, it deletes the rest! Probably not the best to spawn and delete the ones we don't need, but hey, that's bad programming for you.
            //    if (i >= bloodAmount)
            //    {
            //        Destroy(child.gameObject);
            //    }
            //    i++;

            // Here we're accessing the projectile scripts to change the final splat's settings and the projectile direction and force.
            _bloodPrefab.GetComponent<StickyBlood>().BloodStepScript = _stepScript;
            _bloodPrefab.GetComponent<StickyBlood>().BloodSize = Random.Range(bloodSizeMin, bloodSizeMax);
            _bloodPrefab.GetComponent<InitVelocity>().CalcDirLeft = bloodDir1;
            _bloodPrefab.GetComponent<InitVelocity>().CalcDirRight = bloodDir2;
            _bloodPrefab.GetComponent<InitVelocity>().BloodForceMin = bloodForceMin;
            _bloodPrefab.GetComponent<InitVelocity>().BloodForceMax = bloodForceMax;
            // Here we just check if it's supposed to be blue, and assign the correct materials
            if (!IsBlue)
            {
                _bloodPrefab.GetComponent<StickyBlood>().BloodMaterial = _bloodVariations[randomMaterialNum];
            }
            else if (IsBlue)
            {
                _bloodPrefab.GetComponent<StickyBlood>().BloodMaterial = _blueBloodVariations[randomMaterialNum];
            }

            // }
        }
    }

    // This function sets up everything we want for the gore to be!
    private void SpawnGore(GoreValues goreSettings, GameObject spawnObject, GameObject enemy, DoubleArrayPooling arrayScript = default(DoubleArrayPooling), Vector3 direction = default(Vector3))
    {
        // Randomize the amount of gore piece we want
        var amountOfGore = Random.Range(goreSettings.MinAmount, goreSettings.MaxAmount + 1);
        for (int i = 0; i < amountOfGore; i++)
        {
            // Here we set the gore piece as an empty so we can adjust it in seperate if statements
            GameObject gorePiece = null;

            // If the array script does exist, we can do array pooling
            if (arrayScript != null)
            {
                // If the arrays have not filled up yet, we create new ones and add them
                if (!arrayScript.FullArray2)
                {
                    gorePiece = Instantiate(spawnObject, enemy.transform.position, Quaternion.Euler(0, 0, 0));
                    // If they have filled up, we reuse the ones that exist
                }
                else if (arrayScript.FullArray2)
                {
                    gorePiece = arrayScript.NextToTake;
                    gorePiece.transform.position = enemy.transform.position;

                    if (gorePiece.GetComponent<RagdollVelocity>())
                    {
                        // This will only affect the ragdolls

                        /*
                        Transform[] allChildren = gorePiece.GetComponentsInChildren<Transform>();
                        foreach (Transform child in allChildren)
                        {
                            if (child.GetComponent<CapsuleCollider>())
                            {
                                child.GetComponent<CapsuleCollider>().enabled = true;
                            }
                            if (child.GetComponent<Rigidbody>())
                            {
                                child.GetComponent<Rigidbody>().isKinematic = false;
                            }

                        }
                        gorePiece.GetComponent<RagdollVelocity>().Quiet = false;*/

                        Destroy(gorePiece);
                        gorePiece = Instantiate(spawnObject, enemy.transform.position, Quaternion.Euler(0, 0, 0));
                    }
                    else
                    {
                        // This resets all the components
                        gorePiece.GetComponent<Rigidbody>().isKinematic = false;
                        gorePiece.GetComponent<Collider>().enabled = true;
                        var goreBlood = gorePiece.GetComponent<GoreBlood>();
                        goreBlood.HitFloor = false;
                        goreBlood.HasSmudge = false;
                        goreBlood.Smudge = null;
                    }
                }
                arrayScript.AddPoint(gorePiece);
                // If the array script does not exist, we do simple things with it
            }
            else if (arrayScript == null)
            {
                gorePiece = Instantiate(spawnObject, enemy.transform.position, Quaternion.Euler(0, 0, 0));
            }


            // Here we check if it has the ragdoll script. If it does, we add the settings to that instead of the rigidbody velocity script.
            if (gorePiece.GetComponent<RagdollVelocity>())
            {
                gorePiece.transform.rotation = Quaternion.Euler(Random.Range(0,360), Random.Range(0, 360), Random.Range(0, 360));
                var gorePieceVel = gorePiece.GetComponent<RagdollVelocity>();

                Vector3 bloodDir1;
                Vector3 bloodDir2;
                CalculateDirections(out bloodDir1, out bloodDir2, direction, goreSettings.Angle, goreSettings.Spread);
                gorePieceVel.BloodForceMin = goreSettings.MinForce;
                gorePieceVel.BloodForceMax = goreSettings.MaxForce;
                gorePieceVel.CalcDirLeft = bloodDir1;
                gorePieceVel.CalcDirRight = bloodDir2;
            }
            else
            {
                // Adds the velocity script:
                var gorePieceVel = gorePiece.GetComponent<InitVelocity>();
                // Adds the blood splat creator:
                var bloodScript = gorePiece.GetComponent<GoreBlood>();

                // Just sets the splat prefab to be the one we want:
                bloodScript.SplatObject = _gorePrefab;
                // Tell the gore that it has a double array scriptableobject:
                bloodScript.DoubleArrayScript = _goreSmudgeArrayPool;

                // Applies gore settings to velocity:
                Vector3 bloodDir1;
                Vector3 bloodDir2;
                CalculateDirections(out bloodDir1, out bloodDir2, direction, goreSettings.Angle, goreSettings.Spread);
                gorePieceVel.BloodForceMin = goreSettings.MinForce;
                gorePieceVel.BloodForceMax = goreSettings.MaxForce;
                gorePieceVel.CalcDirLeft = bloodDir1;
                gorePieceVel.CalcDirRight = bloodDir2;

                gorePieceVel.InitializeVelocity();
            }
        }
    }

    // Function used everywhere in this script to get two rotations from one direction, an angle and a width
    private void CalculateDirections(out Vector3 leftDirection, out Vector3 rightDirection, Vector3 inDirection, float angle, float spread)
    {

        if (inDirection == new Vector3(0, 0, 0))
        {
            angle = -angle;
            _bloodSpreadCalculator.transform.localRotation = Quaternion.Euler(angle, 0, 0);
        }
        else
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

    private void Update()
    {
        #region Honey slows

        if (InHoney)
        {
            GetComponent<RammyController>().MovementSpeed = _rammyHoneySpeedDif;
        } else
        {
            GetComponent<RammyController>().MovementSpeed = _rammyOriginalSpeed;
        }

        #endregion
    }


    // Helpful function to remap values:
    float map(float s, float a1, float a2, float b1, float b2)
    {
        return b1 + (s - a1) * (b2 - b1) / (a2 - a1);
    }

}
