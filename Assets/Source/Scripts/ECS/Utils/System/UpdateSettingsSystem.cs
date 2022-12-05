using Ingame.UI;
using Leopotam.Ecs;
using UnityEngine;

namespace Ingame.Utils
{
    public sealed class UpdateSettingsSystem : IEcsRunSystem
    {
        private readonly EcsFilter<UpdateSettingsRequest> _updateSettingsRequestFilter;
        private readonly EcsFilter<ImageModel, UiAimDotTag> _aimDotFilter;

        public void Run()
        {
            foreach (var i in _updateSettingsRequestFilter)
            {
                ref var updateSettingsReq = ref _updateSettingsRequestFilter.Get1(i);

                //Cursor options
                if (updateSettingsReq.isCursorAvailable)
                {
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;
                }
                else
                {
                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;
                }

                //Aim dot options
                if(_aimDotFilter.IsEmpty())
                    continue;
                
                ref var aimImage = ref _aimDotFilter.Get1(0).image;
                aimImage.gameObject.SetActive(updateSettingsReq.isAimDotVisible);
            }
        }
    }
}