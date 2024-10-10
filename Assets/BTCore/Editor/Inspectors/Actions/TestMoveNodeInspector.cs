//------------------------------------------------------------
//        File:  LogInspector.cs
//       Brief:  LogInspector
//
//      Author:  Saroce, Saroce233@163.com
//
//    Modified:  2023-10-08
//============================================================

using BTCore.Runtime;
using BTCore.Runtime.Actions;
using BTCore.Runtime.Attributes;
using Sirenix.OdinInspector;

namespace BTCore.Editor.Inspectors.Actions
{
    [NodeInspector(typeof(TestMoveNode))]
    public class TestMoveNodeInspector : BTNodeInspector<TestMoveNode>
    {
        //[ShowInInspector]
        //[LabelText("Log")]
        //[LabelWidth(100)]
        //[OnValueChanged("OnFieldValueChanged")]
        //private SharedValueInspector<string> _message = new SharedValueInspector<string>();

        [ShowInInspector]
        [LabelText("Move Speed")]
        [OnValueChanged("OnFieldValueChanged")]
        [LabelWidth(100)]
        public float Speed;
        [ShowInInspector]
        [LabelText("Self ID")]
        [OnValueChanged("OnFieldValueChanged")]
        public int ID;
        [ShowInInspector]
        [LabelText("Target ID")]
        [OnValueChanged("OnFieldValueChanged")]
        [LabelWidth(100)]
        public int TargetID;

        private TestMoveNode node;

        protected override void OnImportData(TestMoveNode data)
        {
            node = data;
            this.ID = node.ID;
            this.Speed = node.Speed;
            this.TargetID = node.TargetID;
            Comment = node.ID.ToString();
        }

        public override BTNode ExportData()
        {
            return node;
        }

        protected override void OnFieldValueChanged()
        {
            // node.Message = _message.ExportData();
            node.ID = ID;
            node.Speed = Speed;
            node.TargetID = TargetID;
            base.OnFieldValueChanged();
        }

        public override void Reset()
        {
            //_message = new SharedValueInspector<string>();
            node = null;
        }
    }
}