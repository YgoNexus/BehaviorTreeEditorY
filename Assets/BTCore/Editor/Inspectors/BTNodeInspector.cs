//------------------------------------------------------------
//        File:  BTNodeInspector.cs
//       Brief:  BTNodeInspector
//
//      Author:  Saroce, Saroce233@163.com
//
//    Modified:  2023-10-08
//============================================================

using System;
using BTCore.Runtime;
using UnityEngine;

namespace BTCore.Editor.Inspectors
{
    public abstract class BTNodeInspector : InspectorBase, IDataSerializable<BTNode>
    {
        [HideInInspector]
        public Action OnNodeViewUpdate;
        
        public abstract void ImportData(BTNode data);
        public abstract BTNode ExportData();
        
        public void OnBlackboardValueChanged() {
            foreach (var fieldInfo in GetType().GetFields(BTEditorDef.BindValueFlags)) {
                if (fieldInfo.FieldType.IsSubclassOf(typeof(SharedValueInspector))) {
                    var svInspector = fieldInfo.GetValue(this) as SharedValueInspector;
                    svInspector?.RebindValueName();
                }
            }
        }
    }
    
    public abstract class BTNodeInspector<T> : BTNodeInspector where T : BTNode
    {
        public override void ImportData(BTNode data) {
            OnImportData(data as T);
        }

        protected abstract void OnImportData(T data);
    }
}