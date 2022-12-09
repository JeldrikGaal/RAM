using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepsEmitter : MonoBehaviour
{
    enum Surface {Dirt, Grass, Wood}
    [SerializeField] AudioAddIn _audio;
    [SerializeField] Surface _defaultSurface;
    [SerializeField] float _stepMinTimeGlobal;
    [SerializeField] float _stepMinTime;
    private bool _stepped;

    private static float _StepMinTimeGlobal;
    private static bool _SteppedGlobal;

    private void Awake()
    {
        _audio.SetTransform(transform);
    }

    // Start is called before the first frame update
    void Start()
    {
        SetGlobal();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {

        if (!_stepped && !_SteppedGlobal)
        {
            (string name, float value)[] ps = new (string name, float value)[1];
            ps[0] = ("Surface", (int)_defaultSurface);
            _audio.Play(ps);

            _stepped = true;
            StartCoroutine(Wait());
            _SteppedGlobal = true;
            StartCoroutine(WaitGlobal());

#if UNITY_EDITOR
            SetGlobal();
#endif

        }

        


    }
    private void SetGlobal()
    {
        _StepMinTimeGlobal = _stepMinTimeGlobal;
    }

    private IEnumerator WaitGlobal()
    {
        yield return new WaitForSeconds(_StepMinTimeGlobal);
        _SteppedGlobal = false;
    }
    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(_stepMinTime);
        _stepped = false;
    }



}
