//------------------------------------------------------------
//        File:  BehaviorTree.cs
//       Brief:  BehaviorTree
//
//      Author:  Saroce, Saroce233@163.com
//
//    Modified:  2023-10-01
//============================================================

using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Pool;
namespace BTCore.Runtime.Unity
{
    public class BehaviorTreeManager : MonoBehaviour
    {
        public Dictionary<int, BTree> TreesMap = new();
        public Dictionary<int, BTree> PausedTreeMap = new();
        public string TestTreeName = "TestMoveNode";
        public bool flag = true;
        ObjectPool<BTree> pool;
        public BTree CreateBTree(TextAsset tAsset, int entityID) => CreateBTree(tAsset.text, entityID);

        public BTree CreateBTree(string assetText, int entityID)
        {
            if (entityID < 0)
                throw new ArgumentOutOfRangeException(nameof(entityID), "entityID must be greater than or equal to 0.");
            if (assetText == null)
                throw new NullReferenceException("assetText is null");
            //导入数据就行 节点状态显示是打开编辑器会实时刷新的 运行时
            BTree tree = null;
            try
            {
                tree = JsonConvert.DeserializeObject<BTree>(assetText, BTDef.SerializerSettingsAuto);
                tree?.RebuildTree();
                tree.Blackboard.SetValue("SelfID", entityID);

                TreesMap.Add(entityID, tree);
            }
            catch (Exception e)
            {
                Debug.LogError($"BT data deserialize failed, please check bt asset file!\n{e}");
            }
            return tree;
        }

        // LL todo  object pool
        public void ReleaseBTree(int entityID)
        {
            TreesMap.Remove(entityID);
        }

        private void Awake()
        {
            BTLogger.OnLogReceived += OnLogReceived;
            //   pool = new ObjectPool<BTree>(
            //createFunc: () => Instantiate(prefab),
            //actionOnGet: obj => obj.SetActive(true),
            //actionOnRelease: obj => obj.SetActive(false),
            //defaultCapacity: 10);
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                if (flag)
                {
                    flag = false;
                    var tree = CreateBTree(Resources.Load<TextAsset>($"BTreeData/{TestTreeName}"), 1);
                    tree?.Enable();
                }
            }
            if (flag == false)
            {
                foreach (var item in TreesMap)
                {
                    item.Value.Update();
                }
            }

            if (Input.GetKeyDown(KeyCode.V))
            {
                new Entity() { Position = new Vector3(22, 22, 22) };
                new Entity();
            }
        }

        private void OnLogReceived(string message, BTLogType logType)
        {
            switch (logType)
            {
                case BTLogType.Debug:
                    Debug.Log(message);
                    break;
                case BTLogType.Warning:
                    Debug.LogWarning(message);
                    break;
                case BTLogType.Error:
                    Debug.LogError(message);
                    break;
            }
        }
    }
}
