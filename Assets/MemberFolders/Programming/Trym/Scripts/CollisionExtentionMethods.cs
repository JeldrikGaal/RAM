using UnityEngine;


namespace NorrunLib
{
    public static class CollisionExtentionMethods
    {

        public static float GetLossyRadius(this SphereCollider sphereCollider)
        {
            Vector3 lossyScale = sphereCollider.transform.lossyScale;


            return (Mathf.Max(lossyScale.x, Mathf.Max(lossyScale.y, lossyScale.z))) * sphereCollider.radius;


        }
    }
}