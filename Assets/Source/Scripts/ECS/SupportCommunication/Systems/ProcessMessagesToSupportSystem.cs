using Leopotam.Ecs;
using Support;

namespace Ingame.SupportCommunication
{
    public sealed class ProcessMessagesToSupportSystem : IEcsRunSystem
    {
        private readonly GameController _gameController;
        private readonly EcsFilter<LevelEndRequest> _levelEndRequestFilter;

        public void Run()
        {
            foreach (var i in _levelEndRequestFilter)
            {
                ref var levelEndReq = ref _levelEndRequestFilter.Get1(i);
                
                _gameController.EndLevel(levelEndReq.isVictory);
            }
        }
    }
}