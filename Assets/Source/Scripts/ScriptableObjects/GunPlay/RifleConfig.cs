using UnityEngine;

namespace Ingame.Gunplay
{
    [CreateAssetMenu(menuName = "GunPlay/RifleConfig", fileName = "NewRifleConfig")]
    public sealed class RifleConfig : ScriptableObject
    {
        [SerializeField] [Min(0)] private float pauseBetweenShots;
        
        public float PauseBetweenShots => pauseBetweenShots;
    }
}