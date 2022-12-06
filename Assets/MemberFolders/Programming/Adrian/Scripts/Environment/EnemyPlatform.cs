using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlatform : MonoBehaviour
{
    [SerializeField] private GameObject _platform;

    [SerializeField] private bool _dropPlatform;

    private void Update()
    {
        if (_dropPlatform)
        {
            _dropPlatform = false;
            DestroyPlatform();
        }
    }

    public void DestroyPlatform()
    {
        Destroy(_platform);
    }
}
