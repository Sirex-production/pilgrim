using UnityEngine;
using Voody.UniLeo;
using Zenject;

namespace Ingame.Health
{
    public class HealthComponentProvider : MonoProvider<HealthComponent>
    {
        [SerializeField] private float initialActorHealth;

        [Inject]
        private void Construct()
        {
            value = new HealthComponent
            {
                initialHealth = initialActorHealth,
                currentHealth = initialActorHealth
            };
        }
    }
}