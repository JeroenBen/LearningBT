using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[System.Serializable]
public class SelectorStar : Control {
    public SelectorStar(List<Node> nodes) : base(nodes) {
        nodeRunning = 0;
    }


    /* If any of the children reports a success, the selector will 
     * immediately report a success upwards. If all children fail, 
     * it will report a failure instead.*/
    public override NodeState Evaluate() {
        for(int i=nodeRunning;i<m_nodes.Count; i++) {
            Node node = m_nodes[i];
            switch (node.Evaluate()) {
                case NodeState.F:
                    continue;
                case NodeState.S:
                    m_nodeState = NodeState.S;
                    nodeRunning = 0;
                    return m_nodeState;
                case NodeState.R:
                    m_nodeState = NodeState.R;
                    nodeRunning = i;
                    return m_nodeState;
                default:
                    continue;
            }
        }
        m_nodeState = NodeState.F;
        nodeRunning = 0;
        return m_nodeState;
    }

    public override string ToS() {
        string total = "Selector* {";
        foreach (Node node in m_nodes) {
            total += ", " + node.ToS();
        }
        total += "}: " + m_nodeState.ToString();
        return total;
    }
}