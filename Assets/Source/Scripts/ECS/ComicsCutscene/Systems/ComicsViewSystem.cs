using System;
using Ingame.Comics;
using Ingame.ECS;
using Leopotam.Ecs;
using UnityEngine;

namespace Ingame.ComicsCutscene
{
    public class ComicsViewSystem : IEcsInitSystem,IEcsRunSystem
    {
        private readonly EcsFilter<UiComicsViewModel> _viewFilter;
        private readonly EcsFilter<ComicsContainerModel,CurrentComicsComponent, ComicsHasChangedEvent> _comicsModelFilter;
        private ComicsService _comicsService;
        public void Init()
        { 
            if ( _viewFilter.IsEmpty())
                return;
            
            
            ref var uiView = ref _viewFilter.Get1(0);
            uiView.BackButton.onClick.AddListener(()=>_comicsService.Back());
            uiView.NextButton.onClick.AddListener(()=>_comicsService.Next());
            uiView.SkipButton.onClick.AddListener(()=>_comicsService.Skip());

            uiView.ComicsBackgroundImage.gameObject.SetActive(false);
            
        }

        public void Run()
        {
            OnComicsChange();
        }

        private void OnComicsChange()
        {
            if(_comicsModelFilter.IsEmpty())
                return;
            
            if ( _viewFilter.IsEmpty())
                return;
            
            ref var comicsModel = ref _comicsModelFilter.Get1(0);
            ref var currentComicsComponent = ref _comicsModelFilter.Get2(0);
            ref var entity = ref _comicsModelFilter.GetEntity(0);
            ref var uiView = ref _viewFilter.Get1(0);
            
            if (string.IsNullOrWhiteSpace(currentComicsComponent.id))
            {
                uiView.ComicsBackgroundImage.gameObject.SetActive(false);
                entity.Del<ComicsHasChangedEvent>();
                return;
            }
            
            uiView.ComicsBackgroundImage.gameObject.SetActive(true);
            
            uiView.BackButton.gameObject.SetActive(currentComicsComponent.page>0);
            if (currentComicsComponent.page >= comicsModel.comics[currentComicsComponent.id].Pages.Count)
            {
                uiView.ComicsBackgroundImage.gameObject.SetActive(false);
                entity.Del<ComicsHasChangedEvent>();
                return;
            }
            var currentPage = comicsModel.comics[currentComicsComponent.id].Pages[currentComicsComponent.page];
            uiView.ComicsBackgroundImage.sprite = currentPage.Page;

            entity.Del<ComicsHasChangedEvent>();
            
        }
        
    }
}