using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FadeOnTrigger : MonoBehaviour
{
    public DecalProjector Decal;
    void Awake()
    {
        Decal = GetComponent<DecalProjector>();
    }

    public void TriggerFade() 
    {
        InvokeRepeating("Fade", 0.05f, 0.05f);
    }

    public void StopFade()
    {
        CancelInvoke();
        Decal.fadeFactor = 1;
    }

    private void Fade()
    {
        Decal.fadeFactor -= 0.1f;
        if(Decal.fadeFactor <= 0)
        {
            CancelInvoke();
        }
    }

}
