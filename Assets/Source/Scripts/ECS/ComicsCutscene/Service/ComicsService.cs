using System;
using System.Collections.Generic;
using System.Linq;
using Support;
using UnityEngine;
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

        public event Action onPageChanged;
        public event Action onTextChanged;
        public event Action onClose;
        public event Action onOpen;
        
        private Dictionary<string, ComicsData> _comics;
        private CurrentComics _currentComics = new CurrentComics();
        
        private void Awake()
        {
            _comics = comicsHolderConfig.Pages.ToDictionary(i => i.Name);
        }
        
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

            onTextChanged?.Invoke();
            return true;
        }
        
        
        public void Play(string name)
        {
          if(!_comics.ContainsKey(name))
              return;
          
          _currentComics.comicsData = _comics[name];
          _currentComics.currentPageIndex = 0;
          _currentComics.currentTextIndex = 0;
          
          onOpen?.Invoke();
          onPageChanged?.Invoke();
          onTextChanged?.Invoke();
        }
        
        public void Skip()
        {
            if(_currentComics.comicsData == null)
                return;
            
            _currentComics.comicsData = null;
            _currentComics.currentPageIndex = 0;
            _currentComics.currentTextIndex = 0;
            
            onClose?.Invoke();
        }
        
        public void Next()
        { 
            if(_currentComics.comicsData == null)
                return;


            if (TryToChangeText(1))
            {
                return;
            }
            
            if (_currentComics.comicsData.Pages.Count-1 <= _currentComics.currentPageIndex)
            {
                onClose?.Invoke();
                return;
            }

            _currentComics.currentPageIndex++;
            _currentComics.currentTextIndex = 0;
            
            onPageChanged?.Invoke();
            onTextChanged?.Invoke();
        }
        
        public void Back()
        { 
            if(_currentComics.comicsData == null)
                return;
            
            if (TryToChangeText(-1))
            {
                return;
            }

            if (_currentComics.currentPageIndex <= 0)
            {
                return;
            }
            
            _currentComics.currentPageIndex--;
            
            var sentences = _currentComics.comicsData?.Pages[_currentComics.currentPageIndex].TextsIntroductions;
            _currentComics.currentTextIndex = (sentences == null || sentences.Count <= 0) ? 0 : sentences.Count - 1;
            
            onPageChanged?.Invoke();
            onTextChanged?.Invoke();
        }

        public Sprite GetCurrentPage()
        {
            return _currentComics.comicsData?.Pages[_currentComics.currentPageIndex].Page;
        }
        
        public string GetCurrentText()
        {
            if (_currentComics.comicsData?.Pages[_currentComics.currentPageIndex].TextsIntroductions == null
                || _currentComics.comicsData?.Pages[_currentComics.currentPageIndex].TextsIntroductions.Count <= 0 )
                return "";

            return _currentComics.comicsData?.Pages[_currentComics.currentPageIndex]
                .TextsIntroductions[_currentComics.currentTextIndex];
        }
    }
}