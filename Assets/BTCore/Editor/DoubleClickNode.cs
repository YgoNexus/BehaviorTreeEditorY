//------------------------------------------------------------
//        File:  DoubleClickNode.cs
//       Brief:  双击节点，打开对应文件
//
//      Author:  Saroce, Saroce233@163.com
//
//    Modified:  2024-06-29
//============================================================

using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace BTCore.Editor
{
    public class DoubleClickNode : MouseManipulator
    {
        private double _time;
        private double _clickInterval = 0.3f;

        public DoubleClickNode()
        {
            _time = EditorApplication.timeSinceStartup;
        }

        protected override void RegisterCallbacksOnTarget()
        {
            target.RegisterCallback<MouseDownEvent>(OnMouseDown);
        }

        protected override void UnregisterCallbacksFromTarget()
        {
            target.UnregisterCallback<MouseDownEvent>(OnMouseDown);
        }

        private void OnMouseDown(MouseDownEvent evt)
        {
            if (!CanStopManipulation(evt))
                return;

            if (evt.target is not VisualElement visualElement)
            {
                return;
            }

            var nodeView = visualElement.GetFirstAncestorOfType<BTNodeView>();
            if (nodeView == null)
            {
                return;
            }

            double duration = EditorApplication.timeSinceStartup - _time;
            if (duration < _clickInterval)
            {
                OnDoubleClick(evt, nodeView);
            }

            _time = EditorApplication.timeSinceStartup;
        }

        void OpenScriptForNode(MouseDownEvent evt, BTNodeView clickedElement)
        {
            var nodeName = clickedElement.Node.GetType().Name;
            var assetGuids = AssetDatabase.FindAssets($"t:Script {nodeName}");
            if(assetGuids.Length == 0)
            {
                Debug.LogError($"Can't find script for node {nodeName}");
                return;
            }
            // open first script
            var path = AssetDatabase.GUIDToAssetPath(assetGuids[0]);
            var script = AssetDatabase.LoadAssetAtPath<Object>(path);
            AssetDatabase.OpenAsset(script);

            BTEditorWindow.Instance.BTView.RemoveFromSelection(clickedElement);
        }

        private void OnDoubleClick(MouseDownEvent evt, BTNodeView clickedElement)
        {
            OpenScriptForNode(evt, clickedElement);
        }
    }
}