using UnityEngine;
using System.Collections;
using System;
[System.Serializable]
public class FaceToAngle : Action {
    private float endAngle, untilfailure = 3;
    private int begintick = 0;
    public FaceToAngle(float endAngleIn) {
        endAngle = endAngleIn;
    }

    public override NodeState DoAction() {
        if (!RunningConsecutive()) {
            begintick = Root.main.nticks;
            RobotController.main.StopActions();
            RobotController.main.targetAngle = endAngle;
            RobotController.main.rotating = true;
            m_nodeState = NodeState.R;
        } else if (!RobotController.main.rotating) {
            RobotController.main.SetSpeed(0, 0);
            m_nodeState = NodeState.S;
        } else if (begintick + untilfailure / Root.main.ticktime < Root.main.nticks) {

            m_nodeState = NodeState.F;
        }
        return m_nodeState;
    }
}
