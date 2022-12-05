using System;
using System.Collections.Generic;
using UnityEngine;


namespace Ingame.Debug
{
    #if UNITY_EDITOR
    public class WaypointGizmo: MonoBehaviour
    {
        private void OnDrawGizmos()
        { 
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(transform.position,1);
        }
    }
   #endif
}
