using UnityEngine;

namespace Ingame.Data.Interaction.Explosive
{
    [CreateAssetMenu(menuName = "Ingame/Interaction/Explosive/HandGrenade", fileName = "HandGrenade")]
    public sealed class HandGrenadeData : ScriptableObject
    {
        [SerializeField][Min(0)] private float _range;

        [SerializeField][Min(0)] private float _damageOnBlast;

        [SerializeField][Min(0)]private float _knockbackForce;

        [SerializeField][Min(0)] private float _timeToExplode;

        public float Range => _range;

        public float DamageOnBlast => _damageOnBlast;

        public float KnockbackForce => _knockbackForce;

        public float TimeToExplode => _timeToExplode;
    }
}