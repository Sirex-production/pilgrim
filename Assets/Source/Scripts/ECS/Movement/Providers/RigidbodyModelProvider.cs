using UnityEngine;
using Voody.UniLeo;
using Zenject;

namespace Ingame.Movement
{
    [RequireComponent(typeof(Rigidbody))]
    public class RigidbodyModelProvider : MonoProvider<RigidbodyModel>
    {
        [Inject]
        private void Construct()
        {
            value = new RigidbodyModel
            {
                rigidbody = GetComponent<Rigidbody>()
            };
        }
    }
}