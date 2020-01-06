using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[System.Serializable]
public class Selector : Control {
    public Selector(List<Node> nodes) : base(nodes) {
    }


    /* If any of the children reports a success, the selector will 
     * immediately report a success upwards. If all children fail, 
     * it will report a failure instead.*/
    public override NodeState Evaluate() {
        foreach (Node node in m_nodes) {
            switch (node.Evaluate()) {
                case NodeState.F:
                    continue;
                case NodeState.S:
                    m_nodeState = NodeState.S;
                    return m_nodeState;
                case NodeState.R:
                    m_nodeState = NodeState.R;
                    return m_nodeState;
                default:
                    continue;
            }
        }
        m_nodeState = NodeState.F;
        return m_nodeState;
    }

    public override string ToS() {
        string total = "Selector {";
        foreach (Node node in m_nodes) {
            total += ", " + node.ToS();
        }
        total += "}: " + m_nodeState.ToString();
        return total;
    }
}