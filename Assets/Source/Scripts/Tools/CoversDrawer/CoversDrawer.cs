using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
 
using UnityEngine;

namespace Ingame.CoversDrawer
{
    public class CoversDrawer : EditorWindow
    {
        private Color _color = Color.red;
        private float _radius = 1f;
        private List<GameObject> _gameobjects;
        private bool _isEnabled;

        
        [MenuItem("Editor/Covers/CoversDrawer")]
        public static void Init()
        {
            var window = (CoversDrawer)EditorWindow.GetWindow(typeof(CoversDrawer));
            window.Show();
        }

        private void OnEnable()
        {
            _isEnabled = false;
            _gameobjects = new List<GameObject>();
            var list = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects().ToList();
            foreach (var obj in list)
            {
               Travel(obj);
            }
        }

        private void Travel(GameObject obj)
        {
            if (obj.CompareTag("CoverPoint"))
            {
                _gameobjects.Add(obj);
            }
            if(obj.transform.childCount<=0)
                return;
            foreach (Transform i in obj.transform)
            {
                Travel(i.gameObject);
            }
            
        }
        
        private void OnDestroy()
        {
            foreach (var obj in _gameobjects)
            {
                Remove(obj);
            } 
            _gameobjects = new List<GameObject>();
        }
        

        private void OnGUI()
        {
            EditorGUILayout.BeginHorizontal();
            _color =  EditorGUILayout.ColorField("Color", _color);
            _radius = EditorGUILayout.FloatField("Radius",_radius);
            EditorGUILayout.EndHorizontal();
            
            var style = new GUIStyle(GUI.skin.button)
            {
                normal = new GUIStyleState()
                {
                    textColor = !_isEnabled? Color.green : Color.red
                }
            };

            if (GUILayout.Button(!_isEnabled?"Enable":"Disable",style))
            {
                _isEnabled = !_isEnabled;
                if (_isEnabled)
                {
                    foreach (var obj in _gameobjects)
                    {
                        Add(obj);
                    }
                }
                else
                {
                    foreach (var obj in _gameobjects)
                    {
                        Remove(obj);
                    } 
                }
            }
        }

        private void Add(GameObject obj)
        {
            if (!obj.TryGetComponent<SphereVisualizer>(out var vis))
            {
                vis = obj.AddComponent<SphereVisualizer>();
            };

            vis.Color = _color;
            vis.Radius = _radius;
        }

        private void Remove(GameObject obj)
        {
            if (!obj.TryGetComponent<SphereVisualizer>(out var vis))
                return;
            DestroyImmediate(vis);
        }
    }
}