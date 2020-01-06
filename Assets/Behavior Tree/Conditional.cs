using UnityEngine;
using System.Collections;

public abstract class Conditional : Node {
    /* Child node to evaluate */
    /* The constructor requires the child node that this inverter decorator 
     * wraps*/

    /* Reports a success if the child fails and 
     * a failure if the child succeeds. Running will report 
     * as running */
    public Conditional() {
        m_type = Type.Conditional;
    }

}