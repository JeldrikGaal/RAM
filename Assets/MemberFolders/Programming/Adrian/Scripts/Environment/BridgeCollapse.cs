using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeCollapse : MonoBehaviour
{
    [SerializeField] private Transform _bridge;

    [SerializeField] private bool _collapseBridge;

    [SerializeField] private float _lerpSpeed;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (_collapseBridge)
        {
            _bridge.position = Vector3.Lerp(_bridge.position, new Vector3(_bridge.position.x, _bridge.position.y - 30, _bridge.position.z), _lerpSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            _collapseBridge = true;
        }
    }

}
