using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
public class StateGizmoHelper : MonoBehaviour
{
    [SerializeField] bool _on = true;
    [SerializeField] StateBlock[] _stateBlocks;
    [SerializeField] Transform refrenceEntity;
    private void OnDrawGizmos()
    {
        if (_on)
        {
            foreach (var stateBlock in _stateBlocks)
            {
                var block = stateBlock as IStateBlockGizmo;
                Vector3 pos = block.GizPos;
                if (refrenceEntity != null)
                {
                    pos = refrenceEntity.position + (refrenceEntity.rotation * block.GizPos);
                }


                Gizmos.DrawSphere(pos, block.GizRad);
            }
        }
        
    }

}
#endif

public interface IStateBlockGizmo
{
    public Vector3 GizPos { get;}
    public float GizRad { get; }

}