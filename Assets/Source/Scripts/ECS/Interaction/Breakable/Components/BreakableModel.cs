using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ingame.Breakable
{
    [Serializable]
    public struct BreakableModel
    {
        public GameObject BeforeBreakObject;
        public GameObject AfterBreakObject;
    }
}