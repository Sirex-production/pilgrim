using Ingame.CameraWork;
using Ingame.Input;
using Ingame.Interaction.Common;
using Ingame.Interaction.DraggableObject;
using Ingame.Interaction.Explosive;
using Ingame.Inventory;
using Ingame.Movement;
using Ingame.Player;
using Leopotam.Ecs;
using UnityEngine;

namespace Ingame.Interaction.Throwable
{
    public sealed class ThrowObjectSystem : IEcsRunSystem
    {
        private readonly EcsFilter<RigidbodyModel,InteractiveTag, ThrowableTag,PerformInteractionTag>  _throwableObjectFilter;
        private readonly EcsFilter<CameraModel, MainCameraTag> _mainCameraFilter;
        
        private readonly EcsFilter<InteractiveTag,RigidbodyModel, DraggableObjectModel, ObjectIsBeingDraggedTag, HandGrenadeModel> _interactionWithGrenadeFilter;
        private const float STRONG_THROW_FORCE = 25;
        private const float WEAK_THROW_FORCE = 15;
        
        //input
        private readonly EcsFilter<AimInputEvent> _aimEventFilter;
        private EcsFilter<ShootInputEvent> shoot;

        private EcsWorld _world;
        //blocking

        private EcsFilter<BlockShootingRequest> _blockFilter;
        
        public void Run()
        {
            if (_mainCameraFilter.IsEmpty()) return;
            ref var pos = ref _mainCameraFilter.Get1(0);
            
            
            //throw objects
            foreach (var i in _throwableObjectFilter)
            {
                ref var throwableObjectEntity = ref _throwableObjectFilter.GetEntity(i);
                ref var rigidBodyModel = ref _throwableObjectFilter.Get1(i);
                
                rigidBodyModel.rigidbody.useGravity = true;
                rigidBodyModel.rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
                rigidBodyModel.rigidbody.AddForce(pos.camera.transform.forward*25,ForceMode.Impulse);
 
                throwableObjectEntity.Del<ObjectIsBeingDraggedTag>();
                throwableObjectEntity.Del<PerformInteractionTag>();
            }
            
            
             
                if ((!_aimEventFilter.IsEmpty()|| !shoot.IsEmpty()) && !_interactionWithGrenadeFilter.IsEmpty() )
                {
                    ref var grenadeEntity = ref _interactionWithGrenadeFilter.GetEntity(0);
                    ref var rigidBodyModel = ref _interactionWithGrenadeFilter.Get2(0);
                    rigidBodyModel.rigidbody.useGravity = true;
                    rigidBodyModel.rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
                    var force = shoot.IsEmpty() ? WEAK_THROW_FORCE : STRONG_THROW_FORCE;
                    rigidBodyModel.rigidbody.AddForce(pos.camera.transform.forward*force, ForceMode.Impulse);

            
                    if (!_aimEventFilter.IsEmpty())
                    {
                        
                        
                        _aimEventFilter.GetEntity(0).Destroy();
                    }

                    if (!shoot.IsEmpty())
                    {
                        shoot.GetEntity(0).Destroy();
                    }

                    _world.NewEntity().Get<BlockShootingRequest>();
                    
                    
                    grenadeEntity.Get<GrenadeTriggered>();
                    grenadeEntity.Del<ObjectIsBeingDraggedTag>();
                }

                foreach (var i in _blockFilter)
                {
                    ref var time = ref _blockFilter.Get1(i);
                    if (time.TimeLeft>=1)
                    {
                        _blockFilter.GetEntity(i).Destroy();
                        return;
                    }

                    time.TimeLeft += Time.deltaTime;
                }
        }
    }
}