using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffRefresh : MonoBehaviour
{
    [SerializeField] private List<Vector3> _buffPositions = new List<Vector3>();

    [SerializeField] private GameObject[] _buffs;
    [SerializeField] private GameObject[] _currentBuffs;
    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject buff in _currentBuffs)
        {
            _buffPositions.Add(buff.transform.position);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        for (int i = 0; i < 4; i++)
        {
            if (_currentBuffs[i] == null)
            {
                var newBuff = Instantiate(_buffs[i], _buffPositions[i], Quaternion.identity);
                _currentBuffs[i] = newBuff;
            }
        }
    }
}
