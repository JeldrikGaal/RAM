using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HawkBossVFX : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] _wingFlaps;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void WingFlapIdle()
    {
        for (int i = 0; i < _wingFlaps.Length; i++)
        {
            _wingFlaps[i].Emit(2);
        }
    }
    public void WingFlap()
    {
        for (int i = 0; i < _wingFlaps.Length; i++)
        {
            _wingFlaps[i].Emit(2);
        }
    }

    public void PrintEvent(string s)
    {
        Debug.Log("PrintEvent: " + s + " called at: " + Time.time);
    }

    public void PrintEvent()
    {
        Debug.Log("PrintEvent");
    }
}
