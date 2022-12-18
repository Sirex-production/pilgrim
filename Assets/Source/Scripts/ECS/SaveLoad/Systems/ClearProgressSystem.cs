using Leopotam.Ecs;
using UnityEngine;

namespace Ingame.SaveLoad
{
    public sealed class ClearProgressSystem : IEcsRunSystem
    {
        private readonly EcsFilter<ClearProgressEvent> _clearFilter;
        public void Run()
        {
            if (_clearFilter.IsEmpty())
            {
                return;
            }
            PlayerPrefs.SetString(SaveDataContainer.LEVEL_PERSISTANCE_NAME,null);
            PlayerPrefs.SetString(SaveDataContainer.PLAYER_PERSISTANCE_NAME,null);
            PlayerPrefs.Save();
            _clearFilter.GetEntity(0).Destroy();
        }
    }
}