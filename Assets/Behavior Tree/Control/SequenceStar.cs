using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class SequenceStar : Control {
    public SequenceStar() {
        nodeRunning = 0;
    }

    public SequenceStar(List<Node> nodes) : base(nodes) {
        nodeRunning = 0;
    }

    /* If any child node returns a failure, the entire node fails. When all  
     * nodes return a success, the node reports a success. */
    public override NodeState Evaluate() {
        for (int i = nodeRunning; i < m_nodes.Count; i++) {
            Node node = m_nodes[i];
            switch (node.Evaluate()) {
                case NodeState.F:
                    m_nodeState = NodeState.F;
                    nodeRunning = 0;
                    return m_nodeState;
                case NodeState.S:
                    continue;
                case NodeState.R:
                    m_nodeState = NodeState.R;
                    nodeRunning = i;
                    return m_nodeState;
                default:
                    continue;
            }
        }
        m_nodeState = NodeState.R;
        nodeRunning = 0;
        return m_nodeState;
    }


    public override string ToS() {
        string total = "Sequence* {";
        foreach (Node node in m_nodes) {
            total += ", " + node.ToS();
        }
        total += "}: " + m_nodeState.ToString();
        return total;
    }
}