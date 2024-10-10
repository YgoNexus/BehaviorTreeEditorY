//------------------------------------------------------------
//        File:  ActionSkill.cs
//       Brief:  ActionSkill
//
//      Author:  Saroce, Saroce233@163.com
//
//    Modified:  2024-06-21
//============================================================

using BTCore.Runtime;
using BTCore.Runtime.Actions;
using UnityEngine;

public class TestMoveNode : Action
{
    public int ID;
    public int TargetID;
    Entity Target;
    Entity Self;
    private float _startTime;
    private float _duration = 9000;

    public float Speed = 2;

    protected override void OnStart()
    {
        base.OnStart();
        Debug.Log("==============移动开始=============");
        _startTime = Time.time;
        Self = EntityManager.Get(ID);
        if (Self == null)
        {
            Debug.LogError($"self is null");
        }
        Target = EntityManager.Get(TargetID);
        if (Target == null)
        {
            Debug.LogError($"Target is null");
        }
    }

    protected override NodeState OnUpdate()
    {
        Self.Position = Vector3.MoveTowards(Self.Position, Target.Position, Speed * Time.deltaTime);

        Debug.Log($"{Self.Position} =>  {Target.Position}");

        if (Vector3.Distance(Self.Position, Target.Position) < 10)
        {
            return NodeState.Success;
        }
        return NodeState.Running;
    }

    protected override void OnStop()
    {
        Debug.Log("==============移动结束=============");

        // State = NodeState.Inactive;
    }
}