using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Ingame.Behaviour
{
    [Serializable]
    public struct BehaviourAgentModel
    {
        [FormerlySerializedAs("Tree")] [HideInInspector]
        public BehaviourTree tree;

        [SerializeField]
        private BehaviourTree originalTree;

        public BehaviourTree OriginalTree => originalTree;
    }
}