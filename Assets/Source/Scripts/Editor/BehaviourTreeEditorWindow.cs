using System;
using Ingame.Behaviour;
using Ingame.Enemy;
using Leopotam.Ecs;
using PlasticGui.WorkspaceWindow.CodeReview;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEngine.ProBuilder.MeshOperations;

namespace Ingame.Editor
{
    
    public class BehaviourTreeEditorWindow : EditorWindow
    {
        private BehaviourTreePanelView _treePanelView;
        private BehaviourPanelInspectorView _inspectorPanelView;
        private BehaviourPanelStateView _stateView;
        
        [MenuItem("Editor/Behaviour/BehaviourTreeEditorWindow")]
        public static void Init()
        {
            BehaviourTreeEditorWindow wnd = GetWindow<BehaviourTreeEditorWindow>();
            wnd.titleContent = new GUIContent("BehaviourTreeEditorWindow");
        }

        public void CreateGUI()
        {
            VisualElement root = rootVisualElement;

            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Source/Scripts/Editor/BehaviourTreeEditorWindow.uxml"); 
            visualTree.CloneTree(root);

            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Source/Scripts/Editor/BehaviourTreeEditorWindow.uss");
       
            root.styleSheets.Add(styleSheet);

            _treePanelView = root.Q<BehaviourTreePanelView>();
            _inspectorPanelView = root.Q<BehaviourPanelInspectorView>();
            _stateView = root.Q<BehaviourPanelStateView>();
            _treePanelView.OnNodeSelected = OnNodeSelectionChange;
            
            OnSelectionChange();
        }

        private void OnEnable()
        {
            EditorApplication.playModeStateChanged -= ChangeModeDependingOnEditorStateChange;
            EditorApplication.playModeStateChanged += ChangeModeDependingOnEditorStateChange;
        }

        private void OnDisable()
        {
            EditorApplication.playModeStateChanged -= ChangeModeDependingOnEditorStateChange;
        }

        private void OnInspectorUpdate()
        {
            _treePanelView?.UpdateNodesState();
            _stateView.UpdateEntityInfo();
        }

   

        private void ChangeModeDependingOnEditorStateChange(PlayModeStateChange state)
        {
            switch (state)
            {
                case PlayModeStateChange.EnteredEditMode:
                case  PlayModeStateChange.EnteredPlayMode:
                    OnSelectionChange();
                    break;
                
                default:
                    break;
            }
        }
        
        private void OnSelectionChange()
        {
            
            var tree = Selection.activeObject as BehaviourTree;
            //Get Current behaviour tree from agent
            if (!tree)
            {
                try
                {
                    //running
                    if (Selection.activeObject && Selection.activeGameObject.TryGetComponent<EntityReference>(out var entity) && Application.isPlaying)
                    {
                        if (entity.Entity !=null && entity.Entity.Has<BehaviourAgentModel>())
                        {
                            tree = entity.Entity.Get<BehaviourAgentModel>().Tree;
                            _stateView.UpdateEntityInfo(tree);
                        }
                    }
                    else if(Selection.activeObject && Selection.activeGameObject.TryGetComponent<BehaviourAgentModelProvider>(out var treeModel))
                    {
                        var treeProvider = treeModel.GetTree().OriginalTree;
                        if (treeProvider !=null)
                        {
                            tree =  treeProvider;
                        }
                    }
                }
                catch (Exception)
                {
                    //DO NOTHING    
                }
            }

            if (!tree) return;
            
            //Create a Panel
            if (Application.isPlaying && _treePanelView!=null)
            {
                _treePanelView.PopulateView(tree);
            }
            else if(AssetDatabase.CanOpenAssetInEditor(tree.GetInstanceID()))
            {
                _treePanelView.PopulateView(tree);
            }
        }

        private void OnNodeSelectionChange(NodeView nodeView)
        {
            _inspectorPanelView.UpdateSelection(nodeView);
        }

        [OnOpenAsset]
        public static bool OnOpenAsset(int id, int line)
        {
            if (Selection.activeObject is not BehaviourTree) return false;
            Init();
            return true;

        }

        private void OnDestroy()
        {
           SaveAll();
        }

        private void SaveAll()
        {
            if (_treePanelView.Tree == null)
            {
                #if UNITY_EDITOR
                    UnityEngine.Debug.LogError("Tree can not be saved, ensure such a tree exists!!!");
                #endif
                return;
            }
            _treePanelView.Tree.Nodes.ForEach(EditorUtility.SetDirty);
            EditorUtility.SetDirty(_treePanelView.Tree);
            AssetDatabase.SaveAssets();
        }
    }
}