using System;
using System.Linq;
using Ingame.Comics;
using Leopotam.Ecs;

namespace Ingame.ComicsCutscene
{
    public class ComicsSystem : IEcsRunSystem
    {
        private readonly EcsFilter<ComicsContainerModel,CurrentComicsComponent> _comicsModelFilter;
        private readonly EcsFilter<BackPageEvent> _backPageEventFilter;
        private readonly EcsFilter<NextPageEvent> _nextPageEventModelFilter;
        private readonly EcsFilter<SkipComicsEvent> _skipPageEventFilter;
        private readonly EcsFilter<PlayComicsRequest> _playPlayComicsRequestEventFilter;

        private EcsWorld _world;
        private ComicsService _comicsService;
   
        public void Run()
        {
            if (_comicsModelFilter.IsEmpty())
                return;

            ref var comicsEntity = ref _comicsModelFilter.GetEntity(0);
            ref var currentComics = ref _comicsModelFilter.Get2(0);
            ref var comicsModel = ref _comicsModelFilter.Get1(0);
            
            if (!_playPlayComicsRequestEventFilter.IsEmpty())
            {
                OnPlay(ref comicsEntity,ref comicsModel,ref currentComics);
               _playPlayComicsRequestEventFilter.GetEntity(0).Destroy();
            }
            
            if (!_backPageEventFilter.IsEmpty())
            {
                OnBack(ref comicsEntity, ref currentComics);
                _backPageEventFilter.GetEntity(0).Destroy();
            }
            
            if (!_nextPageEventModelFilter.IsEmpty())
            {
                OnNext(ref comicsEntity,ref comicsModel,ref currentComics);
                _nextPageEventModelFilter.GetEntity(0).Destroy();
            }
            
            if (!_skipPageEventFilter.IsEmpty())
            {
                OnSkip(ref comicsEntity ,ref currentComics);
                _skipPageEventFilter.GetEntity(0).Destroy();
            }
        }
        private void OnBack(ref EcsEntity entity, ref CurrentComicsComponent currentComics)
        {
            if(_backPageEventFilter.IsEmpty())
                return;
            
            if(string.IsNullOrWhiteSpace(currentComics.id))
                return;
          
            if(currentComics.page <=0)
                return;
            
            currentComics.page--;
            entity.Get<ComicsHasChangedEvent>();
        }
        
        private void OnPlay(ref EcsEntity entity,ref ComicsContainerModel comics, ref CurrentComicsComponent currentComics)
        {
            if(_playPlayComicsRequestEventFilter.IsEmpty())
                return;
            
            ref var request = ref _playPlayComicsRequestEventFilter.Get1(0);
            
            if(!comics.comics.ContainsKey(request.id))
                return;
          
            currentComics.page = 0;
            currentComics.id = request.id;
            entity.Get<ComicsHasChangedEvent>();
            
        }
        private void OnSkip(ref EcsEntity entity, ref CurrentComicsComponent currentComics)
        {
            if(_skipPageEventFilter.IsEmpty())
                return;
            currentComics.id = null;
            currentComics.page = 0;
            entity.Get<ComicsHasChangedEvent>();
            
        }
        
        private void OnNext(ref EcsEntity entity,ref ComicsContainerModel comics, ref CurrentComicsComponent currentComics)
        {
            if(_nextPageEventModelFilter.IsEmpty())
                return;
            
            if(string.IsNullOrWhiteSpace(currentComics.id))
                return;
            
            if(!comics.comics.ContainsKey(currentComics.id))
                return;

            if (currentComics.page >= comics.comics[currentComics.id].Pages.Count)
            {
                currentComics.id = null;
                currentComics.page = 0;
                entity.Get<ComicsHasChangedEvent>();
                return;
                
            }
            ++currentComics.page;
            entity.Get<ComicsHasChangedEvent>();
        }
    }
}