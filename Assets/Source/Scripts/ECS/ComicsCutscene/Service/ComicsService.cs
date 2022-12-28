using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace Ingame.Comics
{
    public sealed class ComicsService : MonoBehaviour
    {
        private class CurrentComics
        {
            public ComicsData comicsData;
            public int currentPage = 0;
        }
        
        [SerializeField]
        private ComicsHolderContainer comicsHolderContainer;

        public event Action onPageChanged;
        public event Action onClose;
        public event Action onOpen;
        
        private Dictionary<string, ComicsData> _comics;
        private CurrentComics _currentComics = new CurrentComics();
  
        private void Awake()
        {
            _comics = comicsHolderContainer.Pages.ToDictionary(i => i.Name);
        }

        public void Play(string name)
        {
          if(!_comics.ContainsKey(name))
              return;
          
          _currentComics.comicsData = _comics[name];
          _currentComics.currentPage = 0;
          
          onOpen?.Invoke();
          onPageChanged?.Invoke();
        }
        
        public void Skip()
        {
            if(_currentComics.comicsData == null)
                return;
            
            _currentComics.comicsData = null;
            _currentComics.currentPage = 0;
            onClose?.Invoke();
        }
        
        public void Next()
        { 
            if(_currentComics.comicsData == null)
                return;
            
            if (_currentComics.comicsData.Pages.Count-1 <= _currentComics.currentPage)
            {
                onClose?.Invoke();
                return;
            }

            _currentComics.currentPage++;
            onPageChanged?.Invoke();
        }
        
        public void Back()
        { 
            if(_currentComics.comicsData == null)
                return;
            
            if (_currentComics.currentPage <= 0)
            {
                return;
            }
            
            _currentComics.currentPage--;
            onPageChanged?.Invoke();
        }

        public Sprite GetCurrentPage()
        {
            return _currentComics.comicsData?.Pages[_currentComics.currentPage].Page;
        }
    }
    
   
}