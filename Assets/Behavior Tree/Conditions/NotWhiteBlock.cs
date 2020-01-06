using UnityEngine;
using System.Collections;
[System.Serializable]
public class NotWhiteBlock : Conditional {
    /* Child node to evaluate */
    /* The constructor requires the child node that this inverter decorator 
     * wraps*/

    /* Reports a success if the child fails and 
     * a failure if the child succeeds. Running will report 
     * as running */
    public override NodeState Evaluate() {
        if (!RobotController.main.lightsensor().Contains(RobotController.Colors.white)) {
            m_nodeState = NodeState.S;
        } else m_nodeState = NodeState.F;

        return m_nodeState;
    }
}