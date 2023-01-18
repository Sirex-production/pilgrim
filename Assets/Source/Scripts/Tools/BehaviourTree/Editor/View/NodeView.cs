using System;
using System.Collections;
using System.Collections.Generic;
using Ingame.Behaviour;
using UnityEditor;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using Node = Ingame.Behaviour.Node;

namespace Ingame.Editor
{
    public class NodeView : UnityEditor.Experimental.GraphView.Node
    {
        public Action<NodeView> OnNodeSelected;
        public Node Node;
        public Port Input;
        public Port Output;

        private string _nameOfDescription = "description";
        
        public NodeView(Node node) : base("Assets/Source/Scripts/Tools/BehaviourTree/Editor/View/NodeView.uxml")
        {
            Node = node;
            this.title = node.name;
            this.viewDataKey = node.guid;
            
            style.left = node.position.x;
            style.top = node.position.y;

            CreateInputPorts();
            CreateOutputPorts();
            AdjustNodeWithNodeViewBinding();

        }

        private void AdjustNodeWithNodeViewBinding()
        {
            var description = this.Q<Label>(_nameOfDescription);
            description.bindingPath = _nameOfDescription;
            description.Bind(new SerializedObject(Node));
        }
        
        public override void SetPosition(Rect newPos)
        {
            base.SetPosition(newPos);
           
            Node.position.x = newPos.xMin;
            Node.position.y = newPos.yMin;
        }

        public override void OnSelected()
        {
            base.OnSelected();
            OnNodeSelected?.Invoke(this);
        }
        
        private void CreateInputPorts()
        {
            if (Node is RootNode)
            {
                //no input
            }

            if (Node is DecoratorNode or CompositeNode or ActionNode)
            {
                Input = InstantiatePort(Orientation.Vertical, Direction.Input,Port.Capacity.Single,typeof(bool));
            }

            if (Input == null) return;
            Input.portName = "".ToString();
            Input.style.flexDirection = FlexDirection.Column;
            inputContainer.Add(Input);
        }
        private void CreateOutputPorts()
        {
       
            if (Node is CompositeNode )
            {
                Output = InstantiatePort(Orientation.Vertical, Direction.Output,Port.Capacity.Multi,typeof(bool));
            }

            if (Node is DecoratorNode or RootNode)
            {
                Output = InstantiatePort(Orientation.Vertical, Direction.Output,Port.Capacity.Single,typeof(bool));
            }

            if (Output == null) return;
            
            Output.portName = "".ToString();
            Output.style.flexDirection = FlexDirection.ColumnReverse;
            outputContainer.Add(Output);
        }

        public void SortNodes()
        {
            var comp = Node as CompositeNode;
            if (comp)
            {
                comp.Children.Sort((a,b)=> a.position.x < b.position.x ? -1 : 1);
            }
        }
        public void UpdateState()
        {
            if (!Application.isPlaying) return;
            //removing previous states
            RemoveFromClassList("running");
            RemoveFromClassList("success");
            RemoveFromClassList("failure");
            RemoveFromClassList("abandon");
            //updating state
            
            switch (Node.CurrentState)
            {
                case Behaviour.Node.State.Running when Node.IsRunning:
                    AddToClassList("running");
                    break;
                case Behaviour.Node.State.Success:
                    AddToClassList("success");
                    break;
                case Behaviour.Node.State.Failure:
                    AddToClassList("failure");
                    break;
                case Behaviour.Node.State.Abandon:
                    AddToClassList("abandon");
                    break;
            }  
        }
        
    }
}