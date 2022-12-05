using Ingame.Interaction.Common;
using Ingame.Movement;
using Leopotam.Ecs;
using UnityEngine;

namespace Ingame.Interaction.DraggableObject
{
    public class PickUpDraggableObjectSystem : IEcsRunSystem
    {
        private readonly EcsFilter<RigidbodyModel, DraggableObjectModel, InteractiveTag, PerformInteractionTag>.Exclude<ObjectIsBeingDraggedTag> _dragObjectFilter;

        public void Run()
        {
            foreach (var i in _dragObjectFilter)
            {
                ref var draggableObjectEntity = ref _dragObjectFilter.GetEntity(i);
                ref var rigidBodyModel = ref _dragObjectFilter.Get1(i);

                rigidBodyModel.rigidbody.useGravity = false;
                rigidBodyModel.rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
                draggableObjectEntity.Get<ObjectIsBeingDraggedTag>();
                draggableObjectEntity.Del<PerformInteractionTag>();
            }
        }
    }
}