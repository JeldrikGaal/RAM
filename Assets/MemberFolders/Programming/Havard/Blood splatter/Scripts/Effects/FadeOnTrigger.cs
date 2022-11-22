using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FadeOnTrigger : MonoBehaviour
{
    private DecalProjector _decal;
    public bool Fade = false;

    // Start is called before the first frame update
    void Start()
    {
        _decal = GetComponent<DecalProjector>();
    }

    private void FixedUpdate()
    {
        if (Fade)
        {
            _decal.fadeFactor -= 0.1f;
        }
    }

}
