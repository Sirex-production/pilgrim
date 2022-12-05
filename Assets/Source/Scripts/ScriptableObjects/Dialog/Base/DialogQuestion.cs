using System;
using System.Collections;
using System.Collections.Generic;
using Ingame.Dialog;
using NaughtyAttributes;
using UnityEngine;

namespace Ingame.Data.Dialog{
    [CreateAssetMenu(menuName = "Ingame/Dialog/Question", fileName = "question")]
    public class DialogQuestion : ScriptableObject
    {
        [SerializeField] [Expandable] [ReorderableList]
        public List<DialogAnswer> PossibleAnswers;
        
        
        
        [SerializeField] [ReorderableList]
        public List<string> Lines; 
        
    }
}