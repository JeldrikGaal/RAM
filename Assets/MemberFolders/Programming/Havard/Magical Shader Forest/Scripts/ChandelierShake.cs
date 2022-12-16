using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChandelierShake : MonoBehaviour
{

    [SerializeField] private CinemachineTopDown shakeScript;
    [SerializeField] private GameObject cube;

    // Start is called before the first frame update
    void Start()
    {
        shakeScript = FindObjectOfType<CinemachineTopDown>();
    }

    // Update is called once per frame
    void Update()
    {
        if(shakeScript && shakeScript._shakeStart)
        {
            cube.transform.position += new Vector3(Random.Range(0, 01f), 0, Random.Range(0, 1f));
        }
    }
}
