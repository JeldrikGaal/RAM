using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFallFromTree : MonoBehaviour
{
    public bool DropItem;

    [SerializeField] private GameObject _object;

    [SerializeField] private float _dropDistance;
    [SerializeField] private float _dropSpeed;

    private Vector3 _startpos;

    // Start is called before the first frame update
    void Start()
    {
        _startpos = _object.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (DropItem)
        {
            _object.transform.position = Vector3.Lerp(_object.transform.position, new Vector3(_startpos.x, _startpos.y - _dropDistance, _startpos.z), _dropSpeed * Time.deltaTime);
        }
    }
}
