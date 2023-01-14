using Ingame.Utils;
using Leopotam.Ecs;
using UnityEngine;

namespace Ingame.Breakable
{
    public sealed class BreakableSystem : IEcsRunSystem
    {
        private EcsFilter<BreakableModel,BreakableShouldBeDestroyedTag>.Exclude<BreakableDestroyedTag> _breakableFilter;
        private EcsFilter<BreakableModel,BreakableDestroyedTag, GlassTag> _brokenGlassFilter;

        private float _time = 2f;
        public void Run()
        {
            foreach (var i in _breakableFilter)
            {
                ref var entity = ref _breakableFilter.GetEntity(i); 
               ref var breakable = ref _breakableFilter.Get1(i);
               
               breakable.AfterBreakObject.SetActive(true);
               breakable.BeforeBreakObject.SetActive(false);
               
               entity.Get<BreakableDestroyedTag>();
               entity.Del<BreakableShouldBeDestroyedTag>();
            }

            foreach (var glass in _brokenGlassFilter)
            {
                ref var glassEntity = ref _brokenGlassFilter.GetEntity(glass);
                ref var timer = ref glassEntity.Get<TimerComponent>();

                if (!(timer.timePassed >= _time)) continue;
                
                ref var breakableModel = ref _brokenGlassFilter.Get1(glass);
                breakableModel.AfterBreakObject.gameObject.SetActive(false);
                glassEntity.Del<BreakableDestroyedTag>();
            }
        }
        
    }
}