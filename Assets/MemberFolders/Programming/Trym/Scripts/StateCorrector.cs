using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateCorrector : StateBlock
{

    [SerializeField] AI_State[] aI_States;
    public override void OnEnd(EnemyController user, GameObject target)
    {
        
    }

    public override void OnStart(EnemyController user, GameObject target)
    {
        
    }

    public override (AI_State state, List<float> val) OnUpdate(EnemyController user, GameObject target)
    {
        return (aI_States[AI_StageCheck.Check()-1], null);
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
