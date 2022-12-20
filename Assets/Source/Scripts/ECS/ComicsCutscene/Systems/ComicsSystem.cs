﻿using System;
using Leopotam.Ecs;

namespace Ingame.ComicsCutscene
{
    public class ComicsSystem : IEcsRunSystem
    {
        private readonly EcsFilter<ComicsContainerModel,CurrentComicsComponent> _comicsModelFilter;
        private readonly EcsFilter<BackPageEvent> _backPageEventFilter;
        private readonly EcsFilter<NextPageEvent> _nextPageEventModelFilter;
        private readonly EcsFilter<SkipPageEvent> _skipPageEventFilter;
        private readonly EcsFilter<PlayComicsRequest> _playPlayComicsRequestEventFilter;
        private readonly EcsFilter<CloseUiComicsEvent> _closeUiEventFilter;
        public void Run()
        {
            if (_comicsModelFilter.IsEmpty())
                return;
            
            ref var currentComics = ref _comicsModelFilter.Get2(0);
            ref var comicsModel = ref _comicsModelFilter.Get1(0);
            
            if (!_skipPageEventFilter.IsEmpty())
            {
                ref var skipEntity = ref _skipPageEventFilter.GetEntity(0);
                
            }
           
          
 
        }
        private void OnBack(ref ComicsContainerModel comics, ref CurrentComicsComponent currentComics)
        {
            if()
        }
        
        private void OnPlay(ref ComicsContainerModel comics, ref CurrentComicsComponent currentComics)
        {
            if(_playPlayComicsRequestEventFilter.IsEmpty())
                return;
            
        }
        private void OnSkip(ref ComicsContainerModel comics, ref CurrentComicsComponent currentComics)
        {
            if()
        }
    }
}