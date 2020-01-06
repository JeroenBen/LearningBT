using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class Environment : MonoBehaviour {
    public static Environment main;
    public Vector2 boardSize = new Vector2(1, 1);
    public int antiloop;
    public GameObject blackBox, whiteBox, shapes;
    public float kES, kV, kR, kNodes;
    public List<Vector2> blocks;
    public List<GameObject> blackBlocks, whiteBlocks;
    public Vector2 actualsize;

    IEnumerator EvaluateFitnessAfter(float time) {
        yield return new WaitForSeconds(time);
        EvolutionManager.main.FitnessCallback(GetFitness());
    }


    public float GetFitness() {
        Vector3 whitemiddle, blackmiddle;
        // Get the average (middle) coordinate of the white and black blocks.
        whitemiddle.x = whiteBlocks.Average<GameObject>((arg) => { return arg.transform.position.x; });
        whitemiddle.y = whiteBlocks.Average<GameObject>((arg) => { return arg.transform.position.y; });
        whitemiddle.z = whiteBlocks.Average<GameObject>((arg) => { return arg.transform.position.z; });
        blackmiddle.x = blackBlocks.Average<GameObject>((arg) => { return arg.transform.position.x; });
        blackmiddle.y = blackBlocks.Average<GameObject>((arg) => { return arg.transform.position.y; });
        blackmiddle.z = blackBlocks.Average<GameObject>((arg) => { return arg.transform.position.z; });

        float whiteDistanceVariance = 0;
        foreach (GameObject block in whiteBlocks) {
            whiteDistanceVariance += (block.transform.position - whitemiddle).magnitude;
        }
        whiteDistanceVariance /= (whiteBlocks.Count());
        float blackDistanceVariance = 0;
        foreach (GameObject block in blackBlocks) {
            blackDistanceVariance += (block.transform.position - blackmiddle).magnitude;
        }
        blackDistanceVariance /= (blackBlocks.Count());
        float effectSize = 2 * (whitemiddle - blackmiddle).magnitude / (whiteDistanceVariance + blackDistanceVariance);
        Vector3 robotpos = RobotController.main.transform.position;
        float minimalRobotDistance = Mathf.Min(whiteBlocks.Min(i => (robotpos-i.transform.position).magnitude), blackBlocks.Min(i => (robotpos - i.transform.position).magnitude));
        int nodeCount = Root.main.FirstNode.NodeCount();
        if (nodeCount == 1) return 0;
        float fitness = 0.5f + kES * effectSize - kV * (whiteDistanceVariance + blackDistanceVariance - Mathf.Sqrt(2)) - kR * (minimalRobotDistance-1/Mathf.Sqrt(2)) - kNodes*Root.main.FirstNode.NodeCount();
        
        return Mathf.Max(fitness,0);
    }

    void Awake() {
        main = this;
    }

    void Start() {
        blocks = EvolutionManager.main.currentPattern;
        for (int i = 0; i < blocks.Count; i++) {
            Vector3 pos = new Vector3(blocks[i].x, 0.1f, blocks[i].y);
            if (i % 2 == 0) {
                blackBlocks.Add(Instantiate(blackBox, pos, Quaternion.identity, shapes.transform));
            } else {
                whiteBlocks.Add(Instantiate(whiteBox, pos, Quaternion.identity, shapes.transform));
            }
        }
        StartCoroutine(EvaluateFitnessAfter(80));
    }

}
