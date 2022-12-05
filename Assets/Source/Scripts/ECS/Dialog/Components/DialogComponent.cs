using System;
using System.Collections;
using System.Collections.Generic;
using Ingame.Data.Dialog;
using UnityEngine;
using UnityEngine.UI;

namespace Ingame.Dialog
{
    [Serializable]
    public struct DialogComponent
    {
        public DialogQuestion DialogQuestion;
        
        public int LineNo;
        public int AnsNo;
    }
}