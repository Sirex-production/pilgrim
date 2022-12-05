using Ingame.Dialog;
using LeoEcsPhysics;
using Leopotam.Ecs;

namespace Client {
    public sealed class DialogCutDownDialogSystem : IEcsRunSystem {
        private readonly EcsFilter<OnTriggerExitEvent>  _triggerExitFilter;
        public void Run()
        {
            //add tag
            foreach (var i in _triggerExitFilter)
            {
                if (!_triggerExitFilter.Get1(i).senderGameObject.TryGetComponent(out EntityReference entityRef)) return;
                entityRef.Entity.Get<DialogCutDownDialogRequest>();
            }
        }
    }
}