using System.Collections.Generic;
using UnityEngine;
public abstract class Control : Node {
    public int nodeRunning;
    //public static Random random = new Random();

    /** The child nodes for this selector */
    public List<Node> m_nodes = new List<Node>();
    public override List<Action> GetAllActions() {
        List<Action> output = new List<Action> { };
        foreach (Node node in m_nodes) {
            output.AddRange(node.GetAllActions());
        }
        return output;
    }

    public override Node GetReplaceNode(ref int n, Node newnode) {
        if (n < m_nodes.Count) {
            Node temp = m_nodes[n];
            m_nodes[n] = newnode;
            n = -1;
            return temp;
        }
        n -= m_nodes.Count;
        foreach (Node node in m_nodes) {
            Node temp = node.GetReplaceNode(ref n, newnode);
            if (n == -1) {
                return temp;
            }
        }
        return this;
    }

    public override void ReplaceNode(ref int n, Node newnode) {
        if (n < m_nodes.Count) {
            m_nodes[n] = newnode;
            n = -1;
            return;
        }
        n -= m_nodes.Count;
        foreach (Node node in m_nodes) {
            node.ReplaceNode(ref n, newnode);
            if (n == -1) {
                return;
            }
        }
    }

    public override void MutateNode(ref int n) {
        if (n < m_nodes.Count) {
            Node node = m_nodes[n];
            switch (Random.Range(0, 3)) {
                case 0:
                    //Replace the Node with a random Node of the same type
                    if (node.m_type == Type.Control) {
                        Control cNode = (Control)node;
                        List<Node> temp = cNode.m_nodes;
                        m_nodes[n] = NewRandomNode(node.m_type, temp);
                    } else {
                        m_nodes[n] = NewRandomNode(node.m_type);
                    }
                    break;
                case 1:
                    // Delete the Node
                    m_nodes.RemoveAt(n);
                    break;
                case 2:
                    // Insert random condition or action node
                    if (Random.value > 0.5) {
                        m_nodes.Insert(n, NewRandomNode(Type.Action));
                    } else {
                        m_nodes.Insert(n, NewRandomNode(Type.Conditional));
                    }
                    break;
            }
            n = -1;
            return;
        }
        n -= m_nodes.Count;
        foreach (Node node in m_nodes) {
            node.MutateNode(ref n);
            if (n == -1) {
                return;
            }
        }

    }
    public override Node GetNode(ref int n) {
        if (n < m_nodes.Count) {
            Node temp = m_nodes[n];
            n = -1;
            return temp;
        }
        n -= m_nodes.Count;
        foreach (Node node in m_nodes) {
            Node temp = node.GetNode(ref n);
            if (n == -1) {
                return temp;
            }
        }
        return this;
    }


    public override int NodeCount() {
        int total = 1;
        foreach (Node node in m_nodes) {
            total += node.NodeCount();
        }
        return total;
    }
    /** The constructor requires a lsit of child nodes to be  
     * passed in*/
    public Control(List<Node> nodes) {
        m_nodes = nodes;
        m_type = Type.Control;
    }

    //Copy constructor
    public override Node Clone() {
        Control other = (Control)this.MemberwiseClone();
        other.m_nodes = new List<Node>();
        foreach (Node node in m_nodes) {
            other.m_nodes.Add(node.Clone());
        }
        other.m_nodeState = NodeState.F;
        other.nodeRunning = 0;
        return other;
    }

    public Control() {
        m_type = Type.Control;
    }

    /* If any of the children reports a success, the selector will 
     * immediately report a success upwards. If all children fail, 
     * it will report a failure instead.*/
}