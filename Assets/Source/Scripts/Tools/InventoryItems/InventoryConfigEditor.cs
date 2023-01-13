using System;
using System.Collections.Generic;
using System.Linq;
using Ingame.QuestInventory;
using UnityEditor;
using UnityEngine;

namespace Ingame.InventoryItems
{
#if UNITY_EDITOR
    [CustomEditor(typeof(InventoryConfig),true)]
    public sealed class InventoryConfigEditor : Editor
    {
        private string _feedback = "Empty";
        private Color _colorOfFeedback = Color.black;
        private InventoryConfig _config;
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            using (new GUILayout.HorizontalScope())
            {
                GUI.contentColor = _colorOfFeedback;
                GUILayout.Label(_feedback);
            }
        }

        private void OnEnable()
        {
            _config = target as InventoryConfig;

            _config.onValidateData -= AdjustFeedbackAboutInventoryConfig;
            _config.onValidateData += AdjustFeedbackAboutInventoryConfig;

            AdjustFeedbackAboutInventoryConfig();
        }
        
        private void OnValidate()
        {
            if (_config != null)
            {
                _config.onValidateData -= AdjustFeedbackAboutInventoryConfig;
            }
            
            _config = target as InventoryConfig;
            
            _config.onValidateData -= AdjustFeedbackAboutInventoryConfig;
            _config.onValidateData += AdjustFeedbackAboutInventoryConfig;
        }
        
        private void OnDestroy()
        {
            if(_config==null)
                return;
            
            _config.onValidateData -= AdjustFeedbackAboutInventoryConfig;
        }


        private void AdjustFeedbackAboutInventoryConfig()
        {
            var config = target as InventoryConfig;
            
            bool isNumberOfItemsValid =IsNumberOfItemsValid(config);
            bool isNumberOfNamesValid = IsNumberOfNamesValid(config);
            
            _colorOfFeedback = isNumberOfItemsValid&&isNumberOfNamesValid? Color.green: Color.red;
            _feedback = !isNumberOfItemsValid ? "The element is repeated" :
                !isNumberOfNamesValid ? "Two or more items have the same itemName" : "List is valid";
        }

        private bool IsNumberOfItemsValid(InventoryConfig config)
        {
            return config.Items.Count == config.Items.Distinct().Count();
        }
        
        private bool IsNumberOfNamesValid(InventoryConfig config)
        {
            HashSet<string> names = new HashSet<string>();
            foreach (var item in config.Items)
            {
                if (names.Contains(item.ItemName))
                {
                    return false;
                }
                names.Add(item.ItemName);
            }

            return true;
        }
    }
#endif
}