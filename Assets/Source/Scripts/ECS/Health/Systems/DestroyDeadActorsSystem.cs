using Ingame.Movement;
using Ingame.Player;
using Ingame.SupportCommunication;
using Leopotam.Ecs;
using UnityEngine;

namespace Ingame.Health
{
    public class DestroyDeadActorsSystem : IEcsRunSystem
    {
        private readonly EcsWorld _world;
        private readonly EcsFilter<HealthComponent, TransformModel, DeathTag> _deadActorsFilter;

        public void Run()
        {
            foreach (var i in _deadActorsFilter)
            {
                ref var entity = ref _deadActorsFilter.GetEntity(i);
                ref var transformModel = ref _deadActorsFilter.Get2(i);

                if (entity.Has<PlayerModel>())
                {
                    _world.NewEntity().Get<LevelEndRequest>().isVictory = false;
                    continue;
                }

                Object.Destroy(transformModel.transform.gameObject);
                entity.Destroy();
            }
        }
    }
}