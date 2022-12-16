using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlatform : MonoBehaviour
{
    [SerializeField] private GameObject _brokenTower;

    //[SerializeField] private bool _dropPlatform;

    [SerializeField] private GameObject[] _enemiesOnPlatform;
	
	[SerializeField] private bool _openDoor;
	[SerializeField] private GameObject _doorToOpen;

    [SerializeField] private GameObject _destructVFX;

    private void Update()
    {
        /*if (_dropPlatform)
        {
            _dropPlatform = false;
            DestroyPlatform();
        }*/		
    }

    public void DestroyPlatform()
    {
        if (_openDoor)
        {
            _doorToOpen.SetActive(false);
        }

        foreach (GameObject enemy in _enemiesOnPlatform)
        {
            enemy.transform.SetParent(null);
        }

        if (_destructVFX)
        {
            Instantiate(_destructVFX, transform.position, transform.rotation);
        }

        Instantiate(_brokenTower, transform.position, Quaternion.Euler(-90, 0, 0));
        Destroy(gameObject);
    }
}
