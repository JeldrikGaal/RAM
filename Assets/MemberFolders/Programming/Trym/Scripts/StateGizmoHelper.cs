using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
public class StateGizmoHelper : MonoBehaviour
{
    [SerializeField] StateBlock[] _stateBlocks;

    private void OnDrawGizmos()
    {
        foreach (var stateBlock in _stateBlocks)
        {
            var block = stateBlock as IStateBlockGizmo;
            Gizmos.DrawSphere(block.GizPos, block.GizRad);
        }
    }

}
#endif

public interface IStateBlockGizmo
{
    public Vector3 GizPos { get;}
    public float GizRad { get; }

}