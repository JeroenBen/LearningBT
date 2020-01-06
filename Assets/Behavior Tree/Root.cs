using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Root : MonoBehaviour {
    public static Root main;
    public int nticks = 0;
    public float ticktime;
    public NodeState nodestate;
    public Node FirstNode;


    IEnumerator RunBehaviorTree() {
        while (true) {
            nodestate = FirstNode.Evaluate();

            //print(FirstNode.ToS());
            nticks++;
            yield return new WaitForSeconds(ticktime);
        }
    }
    void Awake() {
        main = this;
    }
    // Use this for initialization
    void Start() {
        FirstNode = EvolutionManager.main.curBT;
        StartCoroutine("RunBehaviorTree");
    }

}
