using UnityEngine;
using Voody.UniLeo;
using Zenject;

namespace Ingame.Movement
{
    [RequireComponent(typeof(Collider))]
    public sealed class ColliderModelProvider : MonoProvider<ColliderModel>
    {
        [Inject]
        private void Construct()
        {
            value = new ColliderModel
            {
                collider = GetComponent<Collider>()
            };
        }
    }
}