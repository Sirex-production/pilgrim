using UnityEngine.UIElements;
using UnityEngine;
namespace Ingame.Editor
{
    public class BehaviourPanelInspectorView : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<BehaviourPanelInspectorView,VisualElement.UxmlTraits>{}

        private UnityEditor.Editor _editor;
        public BehaviourPanelInspectorView()
        {
        }

        internal void UpdateSelection(NodeView nodeView)
        {
            Clear();
            Object.DestroyImmediate(_editor);
            _editor = UnityEditor.Editor.CreateEditor(nodeView.Node);
            IMGUIContainer container = new IMGUIContainer(()=>_editor.OnInspectorGUI());
            Add(container);
        }
    }
}