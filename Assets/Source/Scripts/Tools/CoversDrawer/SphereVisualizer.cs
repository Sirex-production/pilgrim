using System;
using UnityEngine;

namespace Ingame.CoversDrawer
{
   
    public sealed class SphereVisualizer : MonoBehaviour
    {
#if UNITY_EDITOR
        public Color color = Color.white;
        public float radius = 1;

 
        private void OnDrawGizmos()
        {
            Gizmos.color = color;
            Gizmos.DrawSphere(this.transform.position,radius);
        }
#endif
    }

}