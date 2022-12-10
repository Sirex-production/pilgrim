using System;
using UnityEngine;

namespace Ingame.CoversDrawer
{
   
    public sealed class SphereVisualizer : MonoBehaviour
    {
#if UNITY_EDITOR
        public Color Color = Color.white;
        public float Radius = 1;

 
        private void OnDrawGizmos()
        {
            Gizmos.color = Color;
            Gizmos.DrawSphere(this.transform.position,Radius);
        }
#endif
    }

}