using UnityEngine;
using System.Collections;
using System;
[System.Serializable]
public class ArmUp : Action {
    private int begintick = 0;
    double untilfailure = 1.5;


    public override NodeState DoAction() {
        GripperScript.main.SetSpeed(1);
        if (GripperScript.main.IsUp()) {
            m_nodeState = NodeState.S;
        } else if (RunningConsecutive()) {
            if (begintick + untilfailure / Root.main.ticktime < Root.main.nticks) {

                m_nodeState = NodeState.F;
            }
        } else {
            begintick = Root.main.nticks;
            RobotController.main.StopActions();
            m_nodeState = NodeState.R;
        }
        return m_nodeState;
    }
}
