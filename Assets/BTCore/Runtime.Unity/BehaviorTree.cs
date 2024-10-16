﻿//------------------------------------------------------------
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

namespace BTCore.Runtime.Unity
{
    public class BehaviorTree : MonoBehaviour
    {
        [SerializeField]
        private TextAsset _btAsset;
        public Dictionary<int, BTree> TreeMap = new();
        public BTree BTree { get; private set; }
        public bool flag = true;


        public void CreateBTree(int entityID = 0)
        {
            if (_btAsset == null)
            {
                Debug.LogError("Please assign bt asset file.");
                return;
            }
            //导入数据就行 节点状态显示是打开编辑器会实时刷新的 运行时
            try
            {
                BTree = JsonConvert.DeserializeObject<BTree>(_btAsset.text, BTDef.SerializerSettingsAuto);
                BTree?.RebuildTree();
            }
            catch (Exception e)
            {
                Debug.LogError($"BT data deserialize failed, please check bt asset file!\n{e}");
            }
            if (entityID > 0)
            {
                TreeMap.Add(entityID, BTree);
            }
        }
        public BTree CreateBTree(string assetText, int entityID = 0)
        {
            if (assetText == null)
            {
                throw new NullReferenceException("assetText is null");
            }
            //导入数据就行 节点状态显示是打开编辑器会实时刷新的 运行时
            BTree tree = null;
            try
            {
                tree = JsonConvert.DeserializeObject<BTree>(assetText, BTDef.SerializerSettingsAuto);
                tree?.RebuildTree();
                if (entityID > 0)
                    TreeMap.Add(entityID, tree);
            }
            catch (Exception e)
            {
                Debug.LogError($"BT data deserialize failed, please check bt asset file!\n{e}");
            }
            return tree;
        }

        private void Update()
        {
            return;
            if (Input.GetKeyDown(KeyCode.A))
            {
                if (flag)
                {
                    flag = false;
                    BTLogger.OnLogReceived += OnLogReceived;
                    CreateBTree();
                    BTree?.Enable();
                }
            }
            if (flag == false)
            {
                BTree?.Update();
                foreach (var item in TreeMap)
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
