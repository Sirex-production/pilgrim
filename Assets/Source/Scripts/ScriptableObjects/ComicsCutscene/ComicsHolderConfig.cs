using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Video;

namespace Ingame.Comics
{
    [CreateAssetMenu(menuName = "Ingame/Comics/ComicsHolderContainer",fileName = "ComicsHolderContainer")]
    public sealed class ComicsHolderConfig : ScriptableObject
    {
        [SerializeField] 
        private List<ComicsData> comics;
        public ReadOnlyCollection<ComicsData> Pages => comics.AsReadOnly();
    }
    
    [Serializable]
    public sealed class ComicsData
    {
        [SerializeField] 
        private string name;
        
        [SerializeField] 
        private List<ComicsPage> pages;
        
        public ReadOnlyCollection<ComicsPage> Pages => pages.AsReadOnly();

        public String Name => name;
    }
    
    [Serializable]
    public sealed class ComicsPage
    {
        [SerializeField]
        private Sprite page;
        
        [SerializeField]
        private VideoClip videoClip;

        [SerializeField] private bool useVideo = true;

        [SerializeField] 
        private List<string> textsIntroductions;

        public ReadOnlyCollection<string> TextsIntroductions => textsIntroductions.AsReadOnly();
        public Sprite Page => page;
        public VideoClip VideoClip => videoClip;
    }
}