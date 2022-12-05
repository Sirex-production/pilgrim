using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Ingame.Utils
{

    public sealed class SurfaceDetector : MonoBehaviour
    {
        [SerializeField] [Range(0, 10)] private float surfaceDetectionDistance = .5f;

        private const float GIZMOS_SPHERE_SIZE = .01f;
        private const float MAXIMAL_DISTANCE_TO_THE_SAME_SPOT = .05f;
        
        private LayerMask _layerMask;
        private Vector3 _lastHitPos;
        private bool _wasFacingSurfaceOnPreviousInvoke = false;

        public SurfaceDetectionType SurfaceDetectionType
        {
            get
            {
                if (surfaceDetectionDistance <= 0)
                    return SurfaceDetectionType.Nothing;
                
                var ray = new Ray(transform.position, transform.forward);
                if (Physics.Raycast(ray, out RaycastHit hit, surfaceDetectionDistance, _layerMask, QueryTriggerInteraction.Ignore))
                {
                    if (!_wasFacingSurfaceOnPreviousInvoke)
                    {
                        if (Vector3.Distance(_lastHitPos, transform.position) < MAXIMAL_DISTANCE_TO_THE_SAME_SPOT)
                            return SurfaceDetectionType.SameSpot;

                        _wasFacingSurfaceOnPreviousInvoke = true;
                        _lastHitPos = transform.position;
                        return SurfaceDetectionType.Detection;
                    }
                    
                    return SurfaceDetectionType.Detection;
                }

                _wasFacingSurfaceOnPreviousInvoke = false;
                return SurfaceDetectionType.Nothing;
            }
        }

        private void Awake()
        {
            _layerMask = ~LayerMask.GetMask("Ignore Raycast", "HUD", "PlayerStatic", "Weapon");
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            var position = transform.position;

            Gizmos.color = Color.red;
            Handles.color = Color.red;
            Gizmos.DrawWireSphere(position, GIZMOS_SPHERE_SIZE);
            Handles.DrawLine(position, position + transform.forward * surfaceDetectionDistance, 4);
        }
#endif
    }
    
    public enum SurfaceDetectionType
    {
        Nothing, //If raycast detects nothing
        SameSpot, //If raycast detected this point previously
        Detection //If raycast detects surface
    }
}