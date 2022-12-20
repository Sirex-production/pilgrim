using System;
using System.Collections.Generic;
using Ingame.Comics;
using Ingame.Movement;
using NaughtyAttributes;
using UnityEngine;
using Zenject;

namespace Ingame.ComicsCutscene
{
    [Serializable]
    public struct ComicsContainerModel
    {
        [Required]
        public ComicsHolderContainer comicsHolderContainer;
        
        [NonSerialized]
        public Dictionary<string, ComicsData> comics;
  
    }
}