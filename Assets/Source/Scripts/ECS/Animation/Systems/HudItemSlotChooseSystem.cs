using System.Runtime.CompilerServices;
using Ingame.Hud;
using Ingame.Input;
using Ingame.Inventory;
using Leopotam.Ecs;

namespace Ingame.Animation
{
    public sealed class HudItemSlotChooseSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsFilter<HudItemModel, InInventoryTag> _hudItemsFilter;
        private readonly EcsFilter<HudPlayerItemContainerComponent, AnimatorModel> _itemsContainerFilter;

        private readonly EcsFilter<InteractWithFirstSlotInputEvent> _interactWithFirstSlotEventFilter;
        private readonly EcsFilter<InteractWithSecondSlotInputEvent> _interactWithSecondSlotEventFilter;
        private readonly EcsFilter<OpenInventoryInputEvent> _openInventoryInputEventFilter;
        private readonly EcsFilter<HideGunInputEvent> _hideGunEventFilter;

        public void Init()
        {
            ref var itemsContainerComponent = ref _itemsContainerFilter.Get1(0);
            ref var itemsContainerAnimator = ref _itemsContainerFilter.Get2(0);

            itemsContainerComponent.isShown = true;
            itemsContainerAnimator.animator.SetBool("IsShown", itemsContainerComponent.isShown);
        }

        public void Run()
        {
            if(_itemsContainerFilter.IsEmpty())
                return;

            ref var itemsContainerComponent = ref _itemsContainerFilter.Get1(0);
            ref var itemsContainerAnimator = ref _itemsContainerFilter.Get2(0);
            bool wereItemsShown = itemsContainerComponent.isShown;

            bool isInteractedWithFirstSlot = !_interactWithFirstSlotEventFilter.IsEmpty();
            bool isInteractedWithSecondSlot = !_interactWithSecondSlotEventFilter.IsEmpty();
            bool isInteractedWithInventory = !_openInventoryInputEventFilter.IsEmpty();
            bool areAllGunsHidden = !_hideGunEventFilter.IsEmpty();

            if(!isInteractedWithFirstSlot && !isInteractedWithSecondSlot && !isInteractedWithInventory && !areAllGunsHidden)
                return;

            if (areAllGunsHidden)
            {
                itemsContainerComponent.isShown = false; 
                HideAllGuns();
            }
            else
            {
                itemsContainerComponent.isShown = true;
                ShowGun(isInteractedWithInventory, isInteractedWithFirstSlot, isInteractedWithSecondSlot);
            }

            itemsContainerAnimator.animator.SetBool("IsShown", itemsContainerComponent.isShown);

            if (wereItemsShown && itemsContainerComponent.isShown)
            {
                itemsContainerAnimator.animator.ResetTrigger("Switch");
                itemsContainerAnimator.animator.SetTrigger("Switch");
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void HideAllGuns()
        {
            foreach (var i in _hudItemsFilter)
            {
                ref var hudItemEntity = ref _hudItemsFilter.GetEntity(i);
                
                hudItemEntity.Get<AwaitsToBeHiddenTag>();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ShowGun(bool isInventory, bool isFirstSlot, bool isSecondSlot)
        {
            foreach (var i in _hudItemsFilter)
            {
                ref var hudItemEntity = ref _hudItemsFilter.GetEntity(i);
                
                if(hudItemEntity.Has<BackpackModel>())
                    if (isInventory)
                        hudItemEntity.Get<AwaitsToBeShownTag>();
                    else
                        hudItemEntity.Get<AwaitsToBeHiddenTag>();
                
                if(hudItemEntity.Has<FirstHudItemSlotTag>())
                    if (isFirstSlot)
                        hudItemEntity.Get<AwaitsToBeShownTag>();
                    else
                        hudItemEntity.Get<AwaitsToBeHiddenTag>();
                
                if(hudItemEntity.Has<SecondHudItemSlotTag>())
                    if (isSecondSlot)
                        hudItemEntity.Get<AwaitsToBeShownTag>();
                    else
                        hudItemEntity.Get<AwaitsToBeHiddenTag>();
            }
        }
    }
}