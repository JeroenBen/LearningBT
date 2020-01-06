using UnityEngine;
using System.Collections;

public class ContinuousMove : Node
{
    protected float mlspeed, mrspeed;

    /* Child node to evaluate */
    /* The constructor requires the child node that this inverter decorator 
     * wraps*/

    /* Reports a success if the child fails and 
     * a failure if the child succeeds. Running will report 
     * as running */
    public override NodeState Evaluate()
    {
        RobotController.main.SetSpeed(mlspeed, mrspeed);
        return NodeState.R;
    }
}