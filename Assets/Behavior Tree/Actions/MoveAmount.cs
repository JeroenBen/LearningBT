using UnityEngine;
using System.Collections;
using System;
[System.Serializable]
public class MoveAmount : Action {
    private float endAngle, speed, steering, untilfailure = 3;
    private int begintick = 0;
    public MoveAmount(float endAngleIn, float speedIn, float steeringIn) {
        endAngle = endAngleIn;
        speed = speedIn;
        steering = steeringIn;

    }

    public override NodeState DoAction() {
        if (!RunningConsecutive()) {
            RobotController.main.StopActions();
            RobotController.main.ResetWheelAngles();
            RobotController.main.SetSpeedSteering(speed, steering);
            begintick = Root.main.nticks;
            m_nodeState = NodeState.R;
        } else if (RobotController.main.IsFinished(endAngle)) {
            RobotController.main.SetSpeed(0, 0);
            m_nodeState = NodeState.S;
        } else if (begintick + untilfailure / Root.main.ticktime < Root.main.nticks) {

            m_nodeState = NodeState.F;
        }
        return m_nodeState;
    }
}
