using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailEmitter : MonoBehaviour
{
    private ParticleSystem _party;

    private void Start()
    {
        _party = GetComponent<ParticleSystem>();
    }

    public void EmitParticle()
    {
        _party.Emit(6);
    }
}
