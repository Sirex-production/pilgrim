 
using System;
using System.Collections.Generic;
using Ingame.Enemy;
using Leopotam.Ecs;
using UnityEditor;
using UnityEngine;


namespace Ingame.Behaviour{
    [Serializable]
    public abstract class BehaviourTree : ScriptableObject
    {
        [SerializeField] private Node root;
        
        [SerializeField] private Node.State state = Node.State.Running;
        
        [SerializeField]
        [HideInInspector]
        public List<Node> Nodes  = new();

        public EcsEntity Entity;

        public Node Root
        {
            get => root;
            set => root = value;
        }

        public Node.State State
        {
            get => state;
            set => state = value;
        }

        public abstract bool Init();
        public Node.State Tick()
        {
            return state==Node.State.Running ? root.Tick() : state;
        }
#if UNITY_EDITOR
        public Node CreateNode(Type typeOfNode)
        {
            var node = ScriptableObject.CreateInstance(typeOfNode) as Node;
            
            node.name = typeOfNode.Name;
            node.Guid = GUID.Generate().ToString();
            Nodes.Add(node);
            
            if (!Application.isPlaying)
            {
                AssetDatabase.AddObjectToAsset(node,this);
            }

            AssetDatabase.SaveAssets();
            return node;
        }
#endif
        public void RemoveNode(Node node)
        {
            Nodes.Remove(node);
            AssetDatabase.RemoveObjectFromAsset(node);
            AssetDatabase.SaveAssets();
        }

        public void AddChild(Node parent, Node child)
        {
            //decoration
            var decoratorNode = parent as DecoratorNode;
            if (decoratorNode)
            {
                decoratorNode.Child = child;
            }
            //root node
            var rootNode = parent as RootNode;
            if (rootNode)
            {
                rootNode.Child = child;
            }
            //composite
            var composite = parent as CompositeNode;
            if (composite)
            {
                composite.Children.Add(child);
            }
        }
        
        public void RemoveChild(Node parent, Node child)
        {
            //decoration node
            var decoratorNode = parent as DecoratorNode;
            if (decoratorNode)
            {
                decoratorNode.Child = null;
            }
            //root node
            var rootNode = parent as RootNode;
            if (rootNode)
            {
                rootNode.Child = null;
            }
            //composite node
            var composite = parent as CompositeNode;
            if (composite)
            {
                composite.Children.Remove(child);
            }
        }
        
        public static List<Node> GetChildren(Node parent)
        {
            List<Node> children = new();
            //decoration node
            var decoratorNode = parent as DecoratorNode;
            if (decoratorNode && decoratorNode.Child != null)
            {
                children.Add(decoratorNode.Child);
            }
            //root node
            var rootNode = parent as RootNode;
            if (rootNode&& rootNode.Child!=null)
            {
                children.Add(rootNode.Child);
            }
            //composite node
            var composite = parent as CompositeNode;
            if (composite)
            {
                return composite.Children;
            }

            return children;
        }

        public BehaviourTree Clone()
        {
            var tree = Instantiate(this);
            tree.root = tree.root.Clone();
            tree.Nodes = new List<Node>();
            Traverse(tree.Root, (e) =>
            {
                tree.Nodes.Add(e);
            });
            return tree;

        }
        public static void Traverse(Node node, System.Action<Node> visiter)
        {
            if (!node) return;
            visiter.Invoke(node);
            var children = GetChildren(node);
            children.ForEach((e) => Traverse(e, visiter));
        }

        public void InjectEntity(EcsEntity entity)
        {
            Traverse(root,(e)=>e.Entity = entity);
        }

        public void InjectWorld(EcsWorld world)
        {
            Traverse(root,(e)=>e.World = world);
        }
    }
}