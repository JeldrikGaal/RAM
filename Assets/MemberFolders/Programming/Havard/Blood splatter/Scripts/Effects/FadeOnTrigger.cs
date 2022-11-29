using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FadeOnTrigger : MonoBehaviour
{
    public DecalProjector Decal;
    public bool Fade = false;

    // Start is called before the first frame update
    void Awake()
    {
        Decal = GetComponent<DecalProjector>();
    }

    private void FixedUpdate()
    {
        if (Fade)
        {
            Decal.fadeFactor -= 0.1f;
            if(Decal.fadeFactor <= 0)
            {
                Fade = false;
            }
        }
    }

}
