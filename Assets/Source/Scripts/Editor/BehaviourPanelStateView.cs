using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using Ingame.Behaviour;
using Ingame.Enemy;
using Leopotam.Ecs;
using UnityEngine.UIElements;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Ingame.Editor
{
    public class BehaviourPanelStateView: VisualElement
    {
        public new class UxmlFactory : UxmlFactory<BehaviourPanelStateView,VisualElement.UxmlTraits>{}

        private UnityEditor.Editor _editor;
        private TextElement text;
        private BehaviourTree _tree;
        public BehaviourPanelStateView()
        {
        }

        internal void UpdateEntityInfo()
        {
            if (_tree == null)
            {
                return;
            }
            UpdateEntityInfo(_tree);
        }
        //todo refactor it
        internal void UpdateEntityInfo(BehaviourTree tree)
        {
            if (!tree.Entity.Has<EnemyStateModel>()) return;

            _tree = tree;
            ref var enemy = ref tree.Entity.Get<EnemyStateModel>();
            
            Clear();
            Object.DestroyImmediate(_editor);
            _editor = UnityEditor.Editor.CreateEditor( tree);
           
            var container = new IMGUIContainer();
            var text = UnboxInfo( enemy.GetType().GetFields(), ref enemy);
            
            var textElem = new TextElement();
            textElem.text = text;
            
            container.Add(textElem);
            Add(container);
        }
//todo refactor it
        private string UnboxInfo(FieldInfo[] infos,ref EnemyStateModel e)
        {
            var str = "";
            foreach (var info in infos)
            {
                str += $"\n{info.Name}:";
               //Transform
                if (info.FieldType == typeof(Transform))
                {
                    var trans = (Transform)info.GetValue(e);
                    if (trans ==null)
                    {
                        str += "  null";
                    }else {
                        str += $"   [{trans.position.x}; {trans.position.y}; {trans.position.z}]  "; //+
                        //$" [{trans.rotation.x}; {trans.rotation.y}; {trans.rotation.z}]"; 
                    }
                }else if (info.FieldType == typeof(Vector3))
                {
                    var position = (Vector3)info.GetValue(e);
                    if (position ==null)
                    {
                        str += "  null";
                    }
                    else
                    {
                        str += $"   [{position.x}; {position.y}; {position.z}]  ";
                    }
                }
                //Collections
                else if (info.FieldType.GetInterfaces().Contains(typeof(IEnumerable)))
                {
                    var enumer =info.GetValue(e);
                    if (enumer == null)
                    {
                        str += "   {}"; 
                        continue;
                    }
                    //non-empty Collection
                    foreach (var item in (IEnumerable)enumer)
                    {
                        if (item.GetType() == typeof(Transform))
                        {
                            var trans = (Transform) item;
                            str += $"\n \t [{trans.position.x}; {trans.position.y}; {trans.position.z}]  "; //+
                            // $" [{trans.rotation.x}; {trans.rotation.y}; {trans.rotation.z}]";
                        }
                        else if (item is Vector3 vector3)
                        {
                            if (vector3 ==null)
                            {
                                str += "  null";
                            }
                            else
                            {
                                str += $"   [{vector3.x}; {vector3.y}; {vector3.z}]  ";
                            }
                        }
                        else
                        {
                            str += $"\n \t {item}";
                        }
                        
                    }
                }
                else
                {
                    var value = info.GetValue(e);
                    str += $"   {value??"null"}";
                }


                str += "\n";

                //nested
                /*if (info.FieldType.IsNested)
                {
                    var val = info.GetValue(new Object());
                    UnboxInfo( info.GetType().GetFields(), ref e);
                }
                else
                {
                    str += info.GetValue(e);
                }*/
            }

            return str;
        }
    }
}