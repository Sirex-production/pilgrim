using System;
using System.Collections.Generic;
using System.Linq;
using Ingame.QuestInventory;
using UnityEditor;
using UnityEngine;

namespace Ingame.InventoryItems
{
    [CustomEditor(typeof(InventoryConfig))]
    public sealed class InventoryConfigEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            DisplayFeedbackAboutInventoryConfig();
        }
    
        
        private void DisplayFeedbackAboutInventoryConfig()
        {
            var config = target as InventoryConfig;
            
            bool isNumberOfItemsValid =IsNumberOfItemsValid(config);
            bool isNumberOfNamesValid = IsNumberOfNamesValid(config);
            
            using (new GUILayout.HorizontalScope())
            {
                GUI.contentColor = isNumberOfItemsValid&&isNumberOfNamesValid? Color.green: Color.red;
                
                string message = !isNumberOfItemsValid ? "The element is repeated" :
                    !isNumberOfNamesValid ? "Two or more items have the same itemName" : "List is valid";
                GUILayout.Label(message);
            }
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
        
        private void OnValidate()
        {
            DisplayFeedbackAboutInventoryConfig();
        }
    }
}