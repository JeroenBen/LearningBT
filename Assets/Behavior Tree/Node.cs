using UnityEngine;
using System.Collections.Generic;
public enum NodeState { F, S, R };
public enum Type { Action, Conditional, Control };
[System.Serializable]

public abstract class Node {

    public static int GetCountNode(Type type) {
        switch (type) {
            case Type.Action:
                return 12;
            case Type.Conditional:
                return 6;
            default:
                return 4;
        }
    }

    public static void swapSubTrees(ref Node subTree1, ref Node subTree2) {
        Node temp = subTree1;
        subTree1 = subTree2;
        subTree2 = temp;
    }

    public static Node RandomBT() {
        List<Node> children = new List<Node> { NewRandomNode(Type.Action),NewRandomNode(Type.Conditional), NewRandomNode(Type.Action) };
        return NewRandomNode(Type.Control, children);
    }
    public static Node NewRandomNode(Type type) {
        return NewNode(type, Random.Range(0, GetCountNode(type)));
    }


    public static Node NewRandomNode(Type type, List<Node> children) {
        return NewNode(Random.Range(0, GetCountNode(type) ), children);
    }

    public virtual void MutateNode(ref int n) { }

    public static Node NewNode(int n, List<Node> list) {
        switch (n) {
            case 0:
                return new Sequence(list);
            case 1:
                return new Selector(list);
            case 2:
                return new SequenceStar(list);
            default:
                return new SelectorStar(list);
        }
    }
    public static Node NewNode(Type type, int n) {
        switch (type) {
            case Type.Action:
                switch (n) {
                    case 0:
                        return new ArmUp();
                    case 1:
                        return new ArmDown();
                    case 2:
                        return new MoveAmount(90, 1f, 0);
                    case 3:
                        return new MoveAmount(90, 1f, 0.5f);
                    case 4:
                        return new MoveAmount(90, 1f, -0.5f);
                    case 5:
                        return new MoveAmount(90, 1f, 1);
                    case 6:
                        return new MoveAmount(90, 1f, -1);
                    case 7:
                        return new MoveAmount(90, -1f, 0);
                    case 8:
                        return new FaceToAngle(0);
                    case 9:
                        return new FaceToAngle(90);
                    case 10:
                        return new FaceToAngle(180);
                    case 11:
                        return new FaceToAngle(270);
                    default:
                        return new ArmUp();
                }
            case Type.Conditional:
                switch (n) {
                    case 0:
                        return new WhiteBlock();
                    case 1:
                        return new BlackBlock();
                    case 2:
                        return new GrayWall();
                    case 3:
                        return new NotWhiteBlock();
                    case 4:
                        return new NotBlackBlock();
                    case 5:
                        return new NotGrayWall();
                    default:
                        return new WhiteBlock();
                }
            default:
                switch (n) {
                    case 0:
                        return new Sequence(new List<Node> { });
                    default:
                        return new Selector(new List<Node> { });
                }
        }
    }
    public virtual List<Action> GetAllActions() {
        return new List<Action> { };
    }

    public virtual Node GetReplaceNode(ref int n, Node newNode) {
        return this;
    }

    public virtual void ReplaceNode(ref int n, Node newNode) {
    }

    public virtual Node GetNode(ref int n) {
        return this;
    }

    public static void Mutate(Node[] curGen, int BT1) {
        int count = curGen[BT1].NodeCount();
        if (count > 1) {
            int n = Random.Range(0, count - 1);
            curGen[BT1].MutateNode(ref n);
        }
    }

    public static void Crossover(Node[] curGen, int BT1, int BT2) {
        int bt1index = Random.Range(-1, curGen[BT1].NodeCount() - 1);

        Node subtree1;
        int n = bt1index;
        if (n == -1) subtree1 = curGen[BT1];
        else subtree1 = curGen[BT1].GetNode(ref n);

        Node subtree2;
        n = Random.Range(-1, curGen[BT2].NodeCount() - 1);

        if (n == -1) {
            subtree2 = curGen[BT2];
            curGen[BT2] = subtree1;
        } else {
            subtree2 = curGen[BT2].GetReplaceNode(ref n, subtree1);
        }
        n = bt1index;
        if (n == -1) curGen[BT1] = subtree2;
        else curGen[BT1].ReplaceNode(ref n, subtree2);
    }
    public virtual int NodeCount() {
        return 1;
    }

    public Type m_type;

    /* Delegate that returns the state of the node.*/
    public delegate NodeState NodeReturn();

    /* The current state of the node */
    protected NodeState m_nodeState;

    public NodeState nodeState {
        get { return m_nodeState; }
    }

    /* The constructor for the node */
    public Node() { }

    public virtual Node Clone() {
        Node other = (Node)this.MemberwiseClone();
        other.m_nodeState = NodeState.F;
        return other;
    }



    /* Implementing classes use this method to evaluate the desired set of conditions */
    public abstract NodeState Evaluate();

    public virtual string ToS() {
        return this.ToString()+ ": " + m_nodeState.ToString();
    }

}
