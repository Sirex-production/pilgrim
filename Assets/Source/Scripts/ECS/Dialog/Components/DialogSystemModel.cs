using System;
using System.Collections;
using System.Collections.Generic;
using Ingame.Data.Dialog;
using TMPro;
using UnityEngine;

namespace Ingame.Dialog
{
    [Serializable]
    public struct DialogSystemModel
    {
        public TextMeshProUGUI QuestionHolder;
        public List<TextMeshProUGUI> AnswerHolder;
        
        public DialogQuestion CurrentLine;
    }
}