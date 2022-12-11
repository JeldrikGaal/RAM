using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AI_ManualCallComponent : MonoBehaviour
{
    [SerializeField]private UnityEvent<EnemyController>[] _unityEvents = new UnityEvent<EnemyController>[0];

    [SerializeField] private EnemyController _controller;

    

    private void Awake()
    {
        if (_controller == null)
        {
            _controller = GetComponent<EnemyController>();
        }
        
    }

    public void Call(int _event)
    {
        if (_unityEvents.Length > _event)
        {
            if (_unityEvents[_event] != null )
            {
                _unityEvents[_event].Invoke(_controller);
            }
            else
            {
                Debug.LogWarning("there where noting at Event" + _event);
            }
        }
        else
        {
            Debug.LogWarning($"Event{_event} does not exist");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
