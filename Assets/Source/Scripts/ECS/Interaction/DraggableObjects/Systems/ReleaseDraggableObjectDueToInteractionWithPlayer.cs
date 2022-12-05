using Ingame.Interaction.Common;
using Ingame.Movement;
using Ingame.Player;
using LeoEcsPhysics;
using Leopotam.Ecs;

namespace Ingame.Interaction.DraggableObject
{
    public class ReleaseDraggableObjectDueToInteractionWithPlayer : IEcsRunSystem
    {
        private readonly EcsFilter<OnTriggerStayEvent> _triggerStayFilter;
        private readonly EcsFilter<RigidbodyModel, DraggableObjectModel, ObjectIsBeingDraggedTag> _draggableObjectFilter;

        public void Run()
        {
            if(_triggerStayFilter.IsEmpty())
                return;

            foreach (var i in _draggableObjectFilter)
            {
                ref var draggableObjectEntity = ref _draggableObjectFilter.GetEntity(i);
                var draggableObjectGameObject = _draggableObjectFilter.Get1(i).rigidbody.gameObject;
                
                foreach (var j in _triggerStayFilter)
                {
                    ref var onTriggerStayEvent = ref _triggerStayFilter.Get1(j);

                    if (onTriggerStayEvent.senderGameObject == draggableObjectGameObject)
                        if (onTriggerStayEvent.collider.TryGetComponent(out EntityReference entityReference))
                            if (entityReference.Entity.Has<PlayerModel>())
                                draggableObjectEntity.Get<PerformInteractionTag>();
                }
            }
        }
    }
}