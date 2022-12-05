using Ingame.Interaction.Common;
using Ingame.Movement;
using Leopotam.Ecs;
using UnityEngine;

namespace Ingame.Interaction.DraggableObject
{
    public class ReleaseDraggableObjectSystem : IEcsRunSystem
    {
        private readonly EcsFilter<RigidbodyModel, DraggableObjectModel, ObjectIsBeingDraggedTag, PerformInteractionTag> _draggableObjectFilter;

        public void Run()
        {
            foreach (var i in _draggableObjectFilter)
            {
                ref var draggableObjectEntity = ref _draggableObjectFilter.GetEntity(i);
                ref var rigidBodyModel = ref _draggableObjectFilter.Get1(i);

                rigidBodyModel.rigidbody.useGravity = true;
                rigidBodyModel.rigidbody.collisionDetectionMode = CollisionDetectionMode.Discrete;
                draggableObjectEntity.Del<ObjectIsBeingDraggedTag>();
                draggableObjectEntity.Del<PerformInteractionTag>();
            }
        }
    }
}