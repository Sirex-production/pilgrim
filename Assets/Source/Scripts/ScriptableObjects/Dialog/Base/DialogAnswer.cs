using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ingame.Data.Dialog
{
    [CreateAssetMenu(menuName = "Ingame/Dialog/Answer", fileName = "answer")]
    public class DialogAnswer : ScriptableObject
    {
        [SerializeField] public DialogQuestion question;
        [SerializeField] public string response;
    }
}