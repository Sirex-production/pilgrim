using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Ingame.Comics
{
    [CreateAssetMenu(menuName = "Ingame/Comics/ComicsHolderContainer",fileName = "ComicsHolderContainer")]
    public sealed class ComicsHolderContainer : ScriptableObject
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

        public Sprite Page => page;
    }
}