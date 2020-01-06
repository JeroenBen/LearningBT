using UnityEngine;
using System.Collections;

public class Wheelscript : MonoBehaviour {
    public Vector3 size;
    // Use this for initialization
    void Start () {

        size = GetComponent<Renderer>().bounds.size;

    }
	

}
