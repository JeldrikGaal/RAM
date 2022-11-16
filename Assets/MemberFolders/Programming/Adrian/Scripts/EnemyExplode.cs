using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyExplode : MonoBehaviour
{
    /*
    public int Test { get { return test; } private set { test = value; } }
    [SerializeField]
    private int _test = 0;
    */
    [Header("Kill Count Stuff")]
    [SerializeField] private StatManager _statManager;
    [SerializeField] private GameObject Key;

    [Header("Guts Stuff")]
    [SerializeField] private PiecesManager PiecesManager;
    [SerializeField] private int GutsAmount;
    [SerializeField] private GameObject[] OnDestructionObject;

    [SerializeField] private Vector2 XZDirection;
    [SerializeField] private Vector2 YDirection;

    [SerializeField] private float ForceMultiplier;

    [SerializeField] private float Lifespan;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            PiecesManager.SpawnPieces(OnDestructionObject, transform.position, XZDirection, YDirection, XZDirection, ForceMultiplier, GutsAmount, Lifespan);
            if (_statManager.Stats.Kills == 49)
            {
                Instantiate(Key, transform.position, Quaternion.identity);
            }
            _statManager.AddKill();
        }
    }
}
