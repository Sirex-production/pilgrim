using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Support;
using UnityEngine;
using UnityEngine.Video;
using Zenject;


namespace Ingame.Comics
{
    public sealed class ComicsService : MonoBehaviour
    {
        private sealed class CurrentComics
        {
            public ComicsData comicsData;
            public int currentPageIndex = 0;
            public int currentTextIndex = 0;
        }
        
        [SerializeField]
        private ComicsHolderConfig comicsHolderConfig;

        public event Action<Sprite> OnComicsPageChanged;
        public event Action<VideoClip> OnComicsVideoChanged;
        public event Action<string> OnComicsTextChanged;
        public event Action OnComicsClosed;
        public event Action OnComicsOpened;
        
        private Dictionary<string, ComicsData> _comics;
        private CurrentComics _currentComics = new CurrentComics();
        
        private void Awake()
        {
            _comics = comicsHolderConfig.Pages.ToDictionary(i => i.Name);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Sprite GetCurrentPage()
        {
            return _currentComics.comicsData?.Pages[_currentComics.currentPageIndex].Page;
        }
        
        private VideoClip GetCurrenVideo()
        {
            return _currentComics.comicsData?.Pages[_currentComics.currentPageIndex].VideoClip;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private string GetCurrentText()
        {
            if (_currentComics.comicsData?.Pages[_currentComics.currentPageIndex].TextsIntroductions == null
                || _currentComics.comicsData?.Pages[_currentComics.currentPageIndex].TextsIntroductions.Count <= 0 )
                return "";

            return _currentComics.comicsData?.Pages[_currentComics.currentPageIndex]
                .TextsIntroductions[_currentComics.currentTextIndex];
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool TryToChangeText(int pageIncremental)
        {
            if (_currentComics.comicsData.Pages[_currentComics.currentPageIndex].TextsIntroductions == null)
                return false;

            if (_currentComics.comicsData.Pages[_currentComics.currentPageIndex].TextsIntroductions.Count <= 0)
            {
                _currentComics.currentTextIndex = 0;
                return false;
            }

            _currentComics.currentTextIndex += pageIncremental;
            
            if (_currentComics.comicsData.Pages[_currentComics.currentPageIndex].TextsIntroductions.Count <=
                _currentComics.currentTextIndex || _currentComics.currentTextIndex < 0)
            {
                _currentComics.currentTextIndex = 0;
                return false;
            }

            OnComicsTextChanged?.Invoke(GetCurrentText());
            return true;
        }
        
        public void Play(string comicsName)
        {
          if(!_comics.ContainsKey(comicsName))
              return;
          
          _currentComics.comicsData = _comics[comicsName];
          _currentComics.currentPageIndex = 0;
          _currentComics.currentTextIndex = 0;
          
          OnComicsOpened?.Invoke();
          OnComicsPageChanged?.Invoke(GetCurrentPage());
          OnComicsVideoChanged?.Invoke(GetCurrenVideo());
          OnComicsTextChanged?.Invoke(GetCurrentText());
        }
        
        public void Close()
        {
            if(_currentComics.comicsData == null)
                return;
            
            _currentComics.comicsData = null;
            _currentComics.currentPageIndex = 0;
            _currentComics.currentTextIndex = 0;
            
            OnComicsClosed?.Invoke();
        }
        
        public void OpenNextPage()
        { 
            if(_currentComics.comicsData == null)
                return;
            
            /*if (TryToChangeText(1))
                return;*/

            if (_currentComics.comicsData.Pages.Count-1 <= _currentComics.currentPageIndex)
            {
                OnComicsClosed?.Invoke();
                return;
            }

            _currentComics.currentPageIndex++;
            _currentComics.currentTextIndex = 0;
            
            OnComicsPageChanged?.Invoke(GetCurrentPage());
            OnComicsVideoChanged?.Invoke(GetCurrenVideo());
            OnComicsTextChanged?.Invoke(GetCurrentText());
        }
        
        public void OpenPreviousPage()
        { 
            if(_currentComics.comicsData == null)
                return;
            
            if (TryToChangeText(-1))
                return;
            
            if (_currentComics.currentPageIndex <= 0)
                return;
            
            _currentComics.currentPageIndex--;
            
            var sentences = _currentComics.comicsData?.Pages[_currentComics.currentPageIndex].TextsIntroductions;
            _currentComics.currentTextIndex = (sentences == null || sentences.Count <= 0) ? 0 : sentences.Count - 1;
            
            OnComicsPageChanged?.Invoke(GetCurrentPage());
            OnComicsVideoChanged?.Invoke(GetCurrenVideo());
            OnComicsTextChanged?.Invoke(GetCurrentText());
        }
    }
}