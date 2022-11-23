using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BuildSceneUtility : MonoBehaviour
{
    [SerializeField] GameObject _enemy;

    [SerializeField] bool _lookAtCamera;
    Transform _cameraTransform;

    // Start is called before the first frame update
    void Start()
    {
        _cameraTransform = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (_lookAtCamera)
        {
            transform.rotation = Quaternion.LookRotation(_cameraTransform.forward, _cameraTransform.up);
        }
    }

    public void Respawn(GameObject g, Vector3 pos, Quaternion rot)
    {
        Debug.Log("TEST");
        StartCoroutine(RespawnAfterTime(g, pos, rot));
    }

    IEnumerator RespawnAfterTime(GameObject g, Vector3 pos, Quaternion rot)
    {

        yield return new WaitForSeconds(3);
        Debug.Log("TEST");
        GameObject temp =  Instantiate(_enemy, pos, rot);
        temp.GetComponent<EnemyTesting>()._health = 100;
        temp.GetComponent<EnemyTesting>()._respawnAfterDeath = true;
        temp.GetComponent<EnemyTesting>()._utilScript = this;
    }

}
