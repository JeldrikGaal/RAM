using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyExplode : MonoBehaviour
{
    public PiecesManager PiecesManager;
    public int GutsAmount;
    public GameObject[] OnDestructionObject;

    public Vector2 XZDirection;
    public Vector2 YDirection;

    public float ForceMultiplier;

    public float Lifespan;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            PiecesManager.SpawnPieces(OnDestructionObject, transform.position, XZDirection, YDirection, XZDirection, ForceMultiplier, GutsAmount, Lifespan);
        }
    }
}
