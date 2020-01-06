using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Linq;
using System.IO;
using System.Globalization;

public struct indexedFitness {
    public float fitness;
    public int index;
}

public class EvolutionManager : MonoBehaviour {
    //Population size of one generation, amount of the fittest individuals that always go to the next generation
    public const int popSize = 50, freepassSize = 6;
    public Vector2 boardSize = new Vector2(1, 1), actualsize;
    public int amountofblocks;
    public float blockradius, robotradius, mutationChance, crossoverChance, controlFitness = 0;
    public List<Vector2> currentPattern;
    public Node curBT;
    public Node[] curGen = new Node[popSize];
    public indexedFitness[] curFitness = new indexedFitness[popSize];
    public int curindex, nGen = 0, gensUntilChange;
    public Text title, BTinfo, fitnessinfo;
    public Slider timeslider;
    public string path;
    bool IscontrolBT = true;

    public List<Vector2> GenerateRandomPos(int amount) {
        actualsize = boardSize - new Vector2(blockradius * 2, blockradius * 2);
        int antiloop = 100;
        Vector2 robot2dpos = new Vector2(0, 0);
        List<Vector2> output = new List<Vector2>();
        for (int i = 0; i < amount; i++) {
            Vector2 candidate = new Vector2(0, 0);
            bool collision = true;
            int cntr = 0;
            while (collision && cntr < antiloop) {
                candidate = new Vector2(Random.value * actualsize.x - actualsize.x / 2, Random.value * actualsize.y - actualsize.y / 2);
                collision = false;
                foreach (Vector2 block in output) {
                    if ((candidate - block).sqrMagnitude < blockradius * blockradius) {
                        collision = true;
                        break;
                    }
                }
                if ((candidate - robot2dpos).sqrMagnitude < robotradius * robotradius) {
                    collision = true;
                }
                cntr++;
            }
            output.Add(candidate);
        }
        return output;
    }

    public string PrintPopulationFitness(indexedFitness[] fitness) {
        string output = "";
        NumberFormatInfo nfi = new NumberFormatInfo();
        nfi.NumberDecimalSeparator = ".";
        for (int i = 0;i<popSize;i++) {
            output += (fitness[i].fitness).ToString(nfi) + ", ";
        }
        return output;
    }

    public string PrintPopulation(Node[] gen, indexedFitness[] fitness) {
        string output = "";
        for (int i = 0; i < popSize; i++) {
            output += gen[i].ToS() + "\n";
        }
        return output;
    }
    public void writeToFile(string data, string path) {
        StreamWriter sw = new StreamWriter( Application.dataPath + path, true);

        sw.WriteLine(data);
        sw.Close();
    }
    public void PrintFitness() {
        fitnessinfo.text = "Fitness = " + Environment.main.GetFitness().ToString();
    }

    public int RandomIndexFitness(double total) {
        double cumulative = 0;
        double a = Mathf.Pow(Random.value, 1.4f);
        for (int i = 0; i < popSize - 1; i++) {
            cumulative += curFitness[i].fitness/total;
            if (cumulative > a) {
                return i;
            }
        }
        return popSize-1;
    }


    public static EvolutionManager main;
    void Awake() {
        if (main == null) {
            main = this;
            DontDestroyOnLoad(gameObject);
            StartEvolution();
            System.DateTime theTime = System.DateTime.Now;
            path = '/'+theTime.ToString("yyyy-MM-dd-HH-mm-ss")+".txt";
            System.IO.File.WriteAllText(Application.dataPath + path, "");


            //print(Node.RandomBT().ToS());
            //currentGen = new List<Node> { new Selector(new List<Node> { new Sequence(new List<Node> { new FaceToAngle(90) }), new ArmDown() }), new Selector(new List<Node> { new ArmUp() }) };


        } else {
            Destroy(gameObject);
        }
    }


    public void StartEvolution() {
        for (int j = 0; j < popSize; j++) {
            if (j < 10)
            {
                // Own Behavior Tree designed to complete the task
                curGen[j] = new Selector(new List<Node> {
                   new SequenceStar(new List<Node> { new BlackBlock(), new Selector(new List<Node> { new ArmDown(), new MoveAmount(360, -1, 0) }),
                        new BlackBlock(), new FaceToAngle(45), new BlackBlock(), new MoveAmount(500, 1, 0), new MoveAmount(100, -0.5f, 0), new ArmUp(), new MoveAmount(400, -0.7f, 0.2f) }),

                   new SequenceStar(new List<Node> { new WhiteBlock(), new Selector(new List<Node> { new ArmDown(), new MoveAmount(360, -1, 0) }),
                        new WhiteBlock(), new FaceToAngle(-135), new WhiteBlock(), new MoveAmount(500, 1, 0),new MoveAmount(100, -0.5f, 0), new ArmUp(), new MoveAmount(1000, -0.7f, 0.2f) }),

                   new SequenceStar(new List<Node>{new GrayWall(),new MoveAmount(400, -0.7f, 0.4f) }),
                   new SequenceStar(new List<Node> { new MoveAmount(100, -0.5f, -0.1f), new MoveAmount(200, 1, -0.7f), new ArmUp(),
                       new MoveAmount(400, 1, 0.05f), new MoveAmount(300, 1, 0.2f), new MoveAmount(100, 1, 0)}),
                   });
            }
            else
            {
                curGen[j] = Node.RandomBT();
            }
        }

        curindex = 0;
        currentPattern = GenerateRandomPos(amountofblocks);
        StartSimulation();
    }

    public void EvolutionStep() {
        for (int i = 0; i < popSize; i++)
        {
            curFitness[i].fitness = Mathf.Max(curFitness[i].fitness - controlFitness + 2, 0);
        }
        print(PrintPopulationFitness(curFitness));
        curFitness = curFitness.OrderByDescending(i => i.fitness).ToArray();
        print(PrintPopulationFitness(curFitness));

        Node[] newGen = new Node[popSize];
        for (int i = 0; i < popSize; i++) {
            if (i < freepassSize) {
                newGen[i] = curGen[curFitness[i].index].Clone();
            } else {
                newGen[i] = curGen[curFitness[RandomIndexFitness(curFitness.Sum(obj => obj.fitness))].index].Clone();
                if (i % 2 == 1 && Random.value < crossoverChance) {
                    Node.Crossover(newGen, i, i - 1);
                }
                if (Random.value < mutationChance) {
                    Node.Mutate(newGen, i);
                }
            }
        }
        nGen++;
        title.text = "Gen " + nGen.ToString() + "\nBest Fitness: " + curFitness[0].fitness.ToString() + "\nAverage Fitness: " + curFitness.Average(a => a.fitness).ToString();
        writeToFile(PrintPopulationFitness(curFitness), path);
        curGen = newGen;
        print(PrintPopulation(curGen, curFitness));
        if (nGen % gensUntilChange == 0)
        {
            currentPattern = GenerateRandomPos(amountofblocks);
            IscontrolBT = true;
        }
        curindex = 0;
        StartSimulation();
    }

    public void StartSimulation() {
        if (IscontrolBT) {
            curBT = new Sequence(new List < Node >{ new BlackBlock() });
        }
        else curBT = curGen[curindex];

        BTinfo.text = "Current BT: " + curindex.ToString() + "\n" + curBT.ToS();
        SceneManager.LoadScene("Simulatie");
        //InvokeRepeating("PrintFitness", 0.5f, 0.5f); 
    }
    public void FitnessCallback(float fitness) {
        if (IscontrolBT)
        {
            controlFitness = fitness;
            IscontrolBT = false;
            fitnessinfo.text = "ControlFitness = " + fitness.ToString();
        }
        else
        {
            curFitness[curindex].fitness = fitness;
            curFitness[curindex].index = curindex;
            fitnessinfo.text = "Fitness = " + fitness.ToString();
            curindex++;
        }
        if (curindex >= popSize) {
            EvolutionStep();
        } else {
            StartSimulation();
        }

    }

    private void Update() {
        Time.timeScale = timeslider.value;
    }

}
