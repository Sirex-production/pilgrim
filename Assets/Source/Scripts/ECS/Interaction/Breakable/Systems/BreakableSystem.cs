using System.Collections;
using System.Collections.Generic;
using Ingame.Interaction.Common;
using Leopotam.Ecs;
using UnityEngine;

namespace Ingame.Breakable
{
    public sealed class BreakableSystem : IEcsRunSystem
    {
        private EcsFilter<BreakableModel,BreakableShouldBeDestroyedTag>.Exclude<BreakableDestroyedTag> _breakableFilter;
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
        }
    }
}