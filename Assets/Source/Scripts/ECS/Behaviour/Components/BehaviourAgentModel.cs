using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ingame.Behaviour
{
    [Serializable]
    public struct BehaviourAgentModel
    {
        [HideInInspector]
        public BehaviourTree Tree;

        [SerializeField]
        private BehaviourTree originalTree;

        public BehaviourTree OriginalTree => originalTree;
    }
}