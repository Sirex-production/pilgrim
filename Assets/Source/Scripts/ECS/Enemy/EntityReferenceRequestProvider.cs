using UnityEngine;
using Voody.UniLeo;
using Zenject;

namespace Ingame
{
    [RequireComponent(typeof(EntityReference))]
    public sealed class EntityReferenceRequestProvider : MonoProvider<InitializeEntityReferenceRequest>
    {
        [Inject]
        private void Construct()
        {
            value = new InitializeEntityReferenceRequest
            {
                entityReference = GetComponent<EntityReference>()
            };
        }
    }
}
