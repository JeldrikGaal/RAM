using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FadeWithSeconds : MonoBehaviour
{
    public DecalProjector Decal;
    private float _timer = 0;
    [SerializeField] private float _maxTime = 1f;
    void Awake()
    {
        Decal = GetComponent<DecalProjector>();
    }

    public void StopFade()
    {
        Decal.fadeFactor = 1;
        _timer = 0;
    }

    private void Update()
    {
        if(Decal.fadeFactor >= 0)
        {
            _timer += Time.deltaTime;
            Decal.fadeFactor = Mathf.Lerp(1, 0, _timer/_maxTime);
        }
    }

}
