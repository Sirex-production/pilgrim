using DG.Tweening;
using Ingame.Interaction.Common;
using Ingame.Movement;
using Leopotam.Ecs;

namespace Ingame.Interaction.Doors
{
    public class DoorRotationSystem : IEcsRunSystem
    {
        private EcsFilter<DoorModel, TransformModel, InteractiveTag, PerformInteractionTag> _interactedDoorFilter;

        public void Run()
        {
            foreach (var i in _interactedDoorFilter)
            {
                ref var doorEntity = ref _interactedDoorFilter.GetEntity(i);
                ref var doorModel = ref _interactedDoorFilter.Get1(i);
                ref var transformModel = ref _interactedDoorFilter.Get2(i);
                
                transformModel.transform.DOKill();
                
                if (doorEntity.Has<OpenedDoorTag>())
                {
                    transformModel.transform
                        .DOLocalRotate(transformModel.initialLocalRotation.eulerAngles, doorModel.openAnimationDuration)
                        .SetEase(doorModel.animationEase);
                    
                    doorEntity.Del<OpenedDoorTag>();
                }
                else
                {
                    transformModel.transform
                        .DOLocalRotate(doorModel.rotationWhenOpened, doorModel.openAnimationDuration)
                        .SetEase(doorModel.animationEase);

                    doorEntity.Get<OpenedDoorTag>();
                }
                
                doorEntity.Del<PerformInteractionTag>();
            }
        }
    }
}