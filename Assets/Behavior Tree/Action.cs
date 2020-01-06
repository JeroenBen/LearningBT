using UnityEngine;
using System.Collections.Generic;
using System;

public abstract class Action : Node {
    private int prevtick = 0;
    public bool finished = false;

    public override List<Action> GetAllActions() {
        return new List<Action> { this };
    }

    public Action() {
        m_type = Type.Action;
        //Root.main.actionList.Add(this);
    }

    public Action(Node node) {
        m_type = Type.Action;
        //Root.main.actionList.Add(this);
    }



    public bool RunningConsecutive() {
        return (m_nodeState == NodeState.R && Root.main.nticks == prevtick + 1);
    }

    public override NodeState Evaluate() {

        DoAction();
        prevtick = Root.main.nticks;
        return m_nodeState;
    }
    public abstract NodeState DoAction();

    //public override string ToS() {
    //    return base.ToS() + ", finish: " + finished.ToString();
    //}
}