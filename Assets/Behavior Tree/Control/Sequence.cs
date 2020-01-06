using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Sequence : Control {
    public Sequence() {
    }

    public Sequence(List<Node> nodes) : base(nodes) {
    }

    /* If any child node returns a failure, the entire node fails. When all  
     * nodes return a success, the node reports a success. */
    public override NodeState Evaluate() {


        foreach (Node node in m_nodes) {
            switch (node.Evaluate()) {
                case NodeState.F:
                    m_nodeState = NodeState.F;
                    return m_nodeState;
                case NodeState.S:
                    continue;
                case NodeState.R:
                    m_nodeState = NodeState.R;
                    return m_nodeState;
            }
        }
        m_nodeState = NodeState.S;
        return m_nodeState;
    }

    public override string ToS() {
        string total = "Sequence {";
        foreach (Node node in m_nodes) {
            total += ", " + node.ToS();
        }
        total += "}: " + m_nodeState.ToString();
        return total;
    }
}