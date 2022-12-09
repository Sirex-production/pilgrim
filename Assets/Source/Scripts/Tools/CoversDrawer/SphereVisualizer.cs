using System;
using UnityEngine;

namespace Ingame.CoversDrawer
{
    #if UNITY_EDITOR
    public sealed class SphereVisualizer : MonoBehaviour
    {
        public Color Color = Color.white;
        public float Radius = 1;

 
        private void OnDrawGizmos()
        {
            Gizmos.color = Color;
            Gizmos.DrawSphere(this.transform.position,Radius);
        }
    }
    #endif
}