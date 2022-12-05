using Leopotam.Ecs;
using UnityEngine;

namespace Ingame.Utils
{
    public sealed class TimeSystem : IEcsRunSystem
    {
        private EcsFilter<TimerComponent> _timerFilter;

        public void Run()
        {
            foreach (var i in _timerFilter)
            {
                ref var timerComponent = ref _timerFilter.Get1(i);

                timerComponent.timePassed += Time.deltaTime;
            }
        }
    }
}